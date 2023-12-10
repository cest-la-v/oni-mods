using System;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using KSerialization;
using UnityEngine;
using Random = UnityEngine.Random;

#pragma warning disable 649

namespace AsLimc.LaserTurret {
    [SerializationConfig(MemberSerialization.OptIn)]
    public class LaserTurret : StateMachineComponent<LaserTurret.Instance> {
        [SerializeField] public Color noFilterTint = (Color32) new Color(0.5019608f, 0.5019608f, 0.5019608f, 1f);
        [SerializeField] public Color filterTint = (Color32) Color.white;

        [MyCmpReq] private Operational operational;
//        [MyCmpGet] private KSelectable _selectable;

        // self
        public int rangeX;
        public int rangeY;
        public int rangeWidth;
        public int rangeHeight;
        private Rect rangeRect;
        private Vector2I selfXY;
        private int selfCell;

        // target
        private KPrefabID target;
        private int targetCell;

        // effect
        private static readonly HashedString _HASH_ROTATION = (HashedString) "rotation";
        private string rotateSoundName = "AutoMiner_rotate";
        [MyCmpGet] private Rotatable rotatable;
        private const float TURN_RATE = 180f;
        private float _armRot = -45f;
        private GameObject _armGo;
        private KBatchedAnimController armAnimCtrl;
        private KAnimLink _link;
        private LoopingSounds _loopingSounds;
        private bool _rotateSoundPlaying;
        private GameObject _hitEffectPrefab;
        private GameObject _hitEffect;

        private int getTargetCell => Grid.PosToCell(target);
        private bool isTargetMoved => targetCell != getTargetCell;
        private Vector3 getTargetPosCcc => Grid.CellToPosCCC(getTargetCell, Grid.SceneLayer.FXFront2);

        private Vector3 getTargetDirection {
            get {
                var posCcc = getTargetPosCcc;
                posCcc.z = 0.0f;
                var position = _armGo.transform.position;
                position.z = 0.0f;
                return posCcc - position;
            }
        }

        protected override void OnPrefabInit() {
            base.OnPrefabInit();
            simRenderLoadBalance = true;
            GetComponent<TreeFilterable>().OnFilterChanged = OnFilterChanged;
        }

        protected override void OnSpawn() {
            base.OnSpawn();
            // self position
            var position = transform.position;
            selfXY = Grid.PosToXY(position);
            selfCell = Grid.PosToCell(position);

            // range detector
            var anchorMinRotatedOffset = rotatable.GetRotatedCellOffset(new CellOffset(rangeX, rangeY));
            var anchorMinRotated = Grid.CellToPos2D(Grid.OffsetCell(selfCell, anchorMinRotatedOffset));
            var sizeRotatedOffset = rotatable.GetRotatedCellOffset(new CellOffset(rangeWidth - 1, rangeHeight - 1));
            rangeRect = new Rect(anchorMinRotated, sizeRotatedOffset.ToVector3());

            // anim
            _hitEffectPrefab = Assets.GetPrefab((Tag) EffectConfigs.AttackSplashId);
            var animCtrl = GetComponent<KBatchedAnimController>();
            var armName = animCtrl.name + ".gun";
            _armGo = new GameObject(armName);
            _armGo.SetActive(false);
            _armGo.transform.parent = animCtrl.transform;
            _loopingSounds = _armGo.AddComponent<LoopingSounds>();
            rotateSoundName = GlobalAssets.GetSound(rotateSoundName);
            _armGo.AddComponent<KPrefabID>().PrefabTag = new Tag(armName);
            armAnimCtrl = _armGo.AddComponent<KBatchedAnimController>();
            armAnimCtrl.AnimFiles = new[] {animCtrl.AnimFiles[0]};
            armAnimCtrl.initialAnim = "gun";
            armAnimCtrl.isMovable = true;
            armAnimCtrl.sceneLayer = Grid.SceneLayer.TransferArm;
            animCtrl.SetSymbolVisiblity((KAnimHashedString) "gun_target", false);
            Vector3 column = animCtrl.GetSymbolTransform(new HashedString("gun_target"), out _).GetColumn(3);
            column.z = Grid.GetLayerZ(Grid.SceneLayer.TransferArm);
            _armGo.transform.SetPosition(column);
            _armGo.SetActive(true);
            _link = new KAnimLink(animCtrl, armAnimCtrl);

            // events
            Subscribe((int) GameHashes.CopySettings, OnCopySettings);

            // init
            //RotateArm(0f, 0f);
            //ClearTarget();
            smi.StartSM();
        }

        // --------------------------------- actions --------------------------------------

        private KPrefabID GetNextTarget() {
            var cavityInfo = Game.Instance.roomProber.GetCavityForCell(selfCell);
            if (cavityInfo == null)
                return null;

            var filterTags = GetComponent<TreeFilterable>().GetTags();
            var creatures = cavityInfo.creatures;
            var filteredCreatures = new List<KPrefabID>();
            // 过滤无效对象
            foreach (var creature in creatures) {
                if (creature == null)
                    // void
                    continue;

                var health = creature.GetComponent<Health>();
                if (health != null && health.IsDefeated())
                    // something is still dying, exit
                    return null;

                if (!creature.HasAnyTags(filterTags.ToArray()))
                    // filtered
                    continue;

                if (!IsAbleToAttack(creature))
                    continue;

                filteredCreatures.Add(creature);
            }

            KPrefabID targetCreature = null;
            var targetProfit = float.MaxValue;
            // 搜索剩余价值最低的对象
            // VUtils.Log("==============================================");
            foreach (var creature in filteredCreatures) {
                var profit = CalcProfit(creature);
                if (targetCreature == null || profit < targetProfit) {
                    targetCreature = creature;
                    targetProfit = profit;
                }

                // VUtils.Log($"creature: {creature.name}, profit: {profit}");
                // VUtils.Log($"tags: {JsonConvert.SerializeObject(creature.Tags.Select(tag1 => tag1.Name).ToList())}");
                // foreach (var amount in creature.GetAmounts()) {
                //     amount.hide = false;
                //     VUtils.Log($"{amount.name} {amount.modifier.Id} {amount.modifier.Name} {amount.value} ({amount.GetDelta()}/{amount.GetMin()}/{amount.GetMax()})");
                // }
            }

            // if (targetCreature != null) {
            //     VUtils.Log($"chosen: {targetCreature.name}, profit: {targetProfit}");
            // }
            // VUtils.Log("==============================================");

            return targetCreature;
        }

        /// <summary>
        /// <para>计算剩余价值：繁殖条件相同时，剩余年龄（单位：周期）还能繁殖多少次</para>
        /// <para>无年龄、无繁殖度的对象剩余价值无限大</para>
        /// <para>剩余存活周期 = 总生命周期 - 已存活周期</para>
        /// <para>当前繁殖已消耗周期 = 每次繁殖所需周期 * 当前繁殖度</para>
        /// <para>剩余繁殖次数 = (剩余存活周期 + 当前繁殖次已消耗周期) / 每次繁殖所需周期</para>
        /// </summary>
        private static float CalcProfit(KPrefabID creature) {
            var ageAmount = Db.Get().Amounts.Age.Lookup(creature);
            var fertilityAmount = Db.Get().Amounts.Fertility.Lookup(creature);
            if (ageAmount == null || fertilityAmount == null)
                return float.MaxValue;
            var maxAge = ageAmount.GetMax();
            var currentAge = ageAmount.value;
            // 剩余年龄
            var leftAge = maxAge - currentAge;
            // 每次繁殖所需周期
            var cyclesPerFertility = creature.gameObject.GetDef<FertilityMonitor.Def>()?.baseFertileCycles ?? 0;
            // 当前繁殖度
            var currentFertilityRate = fertilityAmount.value;
            // 当前繁殖已消耗周期
            var currentFertilityCycles = cyclesPerFertility * currentFertilityRate;
            // 剩余繁殖次数
            var fertilityTimes = (leftAge + currentFertilityCycles) / cyclesPerFertility;

            // VUtils.Log($"baseFertileCycles: {cyclesPerFertility}");
            // VUtils.Log($"{ageAmount.name} {ageAmount.modifier.Id} {ageAmount.modifier.Name} {ageAmount.value} ({ageAmount.GetDelta()}/{ageAmount.GetMin()}/{ageAmount.GetMax()})");
            // VUtils.Log($"{fertilityAmount.name} {fertilityAmount.modifier.Id} {fertilityAmount.modifier.Name} {fertilityAmount.value} ({fertilityAmount.GetDelta()}/{fertilityAmount.GetMin()}/{fertilityAmount.GetMax()})");

            return fertilityTimes;
        }

        private bool IsAbleToAttack(KPrefabID creature) {
            return creature != null
                   // Trussed 捆绑
                   && !creature.HasTag(GameTags.Creatures.Bagged)
                   // Being Wrangled 被捕捉
                   && !creature.HasTag(GameTags.Creatures.Stunned)
                   && IsReachable(creature.transform.position);
        }

        private bool IsReachable(Vector3 creaturePos) {
            var creatureCell = Grid.PosToCell(creaturePos);
            if (!Grid.IsValidCell(creatureCell))
                return false;
            if (!rangeRect.Contains(new Vector3(Convert.ToSingle(Math.Floor(creaturePos.x)), Convert.ToSingle(Math.Floor(creaturePos.y)), creaturePos.z), true))
                return false;
            var creatureXY = Grid.CellToXY(creatureCell);
            return Grid.TestLineOfSight(selfXY.x, selfXY.y, creatureXY.x, creatureXY.y, RangeBlockingCallback);
        }

        public static bool RangeBlockingCallback(int cell) {
            return Grid.Foundation[cell] || Grid.HasDoor[cell] || Grid.Solid[cell];
        }

        private static void Attack(KPrefabID creature) {
            var health = creature.GetComponent<Health>();
            if (health == null)
                return;
            health.Damage(Random.Range(0f, 1f) * creature.GetComponent<AttackableBase>().GetDamageMultiplier());
        }

        private bool IsAimed(out float deltaAngle) {
            deltaAngle = MathUtil.Wrap(-180f, 180f, MathUtil.AngleSigned(Vector3.up, Vector3.Normalize(getTargetDirection), Vector3.forward) - _armRot);
            return Mathf.Approximately(deltaAngle, 0.0f);
        }

        private void StartAttackEffect() {
            // gun effect
            var animName = (HashedString) "gun_digging";
            if (armAnimCtrl.CurrentAnim.hash != animName) {
                var armPosition = _armGo.transform.position;
                armAnimCtrl.GetBatchInstanceData().SetClipRadius(armPosition.x, armPosition.y, Mathf.Clamp(getTargetDirection.sqrMagnitude, 2f, float.MaxValue), true);
                armAnimCtrl.Play(animName, KAnim.PlayMode.Loop);
            }

            // hit effect
            if (_hitEffectPrefab == null)
                return;
            if (_hitEffect != null) {
                if (target.transform.hasChanged) {
                    // move hit effect
                    _hitEffect.transform.SetPositionAndRotation(target.transform.position, _hitEffect.transform.rotation);
                }

                return;
            }

            // create hit effect
            _hitEffect = GameUtil.KInstantiate(_hitEffectPrefab, getTargetPosCcc, Grid.SceneLayer.FXFront2);
            _hitEffect.SetActive(true);
            var component = _hitEffect.GetComponent<KBatchedAnimController>();
            component.sceneLayer = Grid.SceneLayer.FXFront2;
            component.initialMode = KAnim.PlayMode.Loop;
            component.enabled = true;
        }

        private void StopAttackEffect() {
            // gun effect
            var animName = (HashedString) "gun";
            if (armAnimCtrl.CurrentAnim.hash != animName)
                armAnimCtrl.Play(animName, KAnim.PlayMode.Loop);

            // hit effect
            if (_hitEffectPrefab == null || _hitEffect == null)
                return;
            _hitEffect.DeleteObject();
            _hitEffect = null;
        }

        private void RotateArm(float deltaAngle, float dt) {
            deltaAngle = Mathf.Clamp(deltaAngle, -TURN_RATE * dt, TURN_RATE * dt);
            _armRot += deltaAngle;
            _armRot = MathUtil.Wrap(-180f, 180f, _armRot);
            _armGo.transform.rotation = Quaternion.Euler(0.0f, 0.0f, _armRot);
            _loopingSounds.SetParameter(rotateSoundName, _HASH_ROTATION, _armRot);
        }

        private void StartRotateSound() {
            if (_rotateSoundPlaying)
                return;
            _loopingSounds.StartSound(rotateSoundName);
            _rotateSoundPlaying = true;
        }

        private void StopRotateSound() {
            if (!_rotateSoundPlaying)
                return;
            _loopingSounds.StopSound(rotateSoundName);
            _rotateSoundPlaying = false;
        }

        private void LookupTarget() {
            operational.SetActive(true);
            var creature = GetNextTarget();
            if (creature == null)
                return;
            target = creature;
            targetCell = getTargetCell;
            // target.SetTag(GameTags.Trapped, true);
        }

        private void ClearTarget() {
            operational.SetActive(false);
            if (target == null)
                return;
            // target.SetTag(GameTags.Trapped, false);
            target = null;
        }

        // --------------------------------- events --------------------------------------

        private void OnCopySettings(object data) {
            var go = (GameObject) data;
            if (go == null || go.GetComponent<LaserTurret>() == null)
                return;
            ClearTarget();
            LookupTarget();
        }

        private void OnFilterChanged(HashSet<Tag> tags) {
            GetComponent<KBatchedAnimController>().TintColour = tags == null || tags.Count == 0 ? noFilterTint : filterTint;
            ClearTarget();
            LookupTarget();
        }

        public void OnIdleUpdate() {
            LookupTarget();
        }

        public void OnAttackUpdate(float dt) {
            // target
            if (!IsAbleToAttack(target)) {
                StopAttackEffect();
                ClearTarget();
                return;
            }

            // aim
            if (IsAimed(out var deltaAngle)) {
                // aimed
                StopRotateSound();
            }
            else {
                // not aimed
                StopAttackEffect();
                StartRotateSound();
                RotateArm(deltaAngle, dt);
                return;
            }

            // attack
            StartAttackEffect();
            Attack(target);
        }

        public void OnExitAttack() {
            StopAttackEffect();
            StopRotateSound();
            ClearTarget();
        }

        public class Instance : GameStateMachine<States, Instance, LaserTurret, object>.GameInstance {
            public Instance(LaserTurret master)
                : base(master) {
            }
        }

        public class States : GameStateMachine<States, Instance, LaserTurret> {
            public State Off;
            public ReadyStates On;

            public override void InitializeStates(out BaseState baseState) {
                baseState = Off;
                root.DoNothing();
                Off
                    .PlayAnim("off")
                    // .EventTransition(GameHashes.LogicEvent, On,
                    //     smi => smi.GetComponent<Operational>().GetFlag(LogicOperationalController.LogicOperationalFlag))
                    .EventTransition(GameHashes.OperationalChanged, On,
                        smi => smi.GetComponent<Operational>().IsOperational);
                On.DefaultState(On.Idle)
                    // .EventTransition(GameHashes.LogicEvent, Off,
                    //     smi => !smi.GetComponent<Operational>().GetFlag(LogicOperationalController.LogicOperationalFlag))
                    .EventTransition(GameHashes.OperationalChanged, Off,
                        smi => !smi.GetComponent<Operational>().IsOperational);
                On.Idle
                    .PlayAnim("on")
                    .EventTransition(GameHashes.ActiveChanged, On.Attack,
                        smi => smi.GetComponent<Operational>().IsActive)
                    .Update((smi, dt) => smi.master.OnIdleUpdate(), UpdateRate.SIM_1000ms);
                On.Attack
                    .PlayAnim("working")
                    .Exit(smi => smi.master.OnExitAttack())
                    .EventTransition(GameHashes.ActiveChanged, On.Idle,
                        smi => !smi.GetComponent<Operational>().IsActive)
                    .Update((smi, dt) => smi.master.OnAttackUpdate(dt), UpdateRate.SIM_33ms);
            }

            public class ReadyStates : State {
                public State Idle;
                public State Attack;
            }
        }
    }
}