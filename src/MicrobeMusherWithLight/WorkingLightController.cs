namespace MightyVincent {
    public class WorkingLightController : GameStateMachine<WorkingLightController, WorkingLightController.Instance> {
        public State Off;
        public State On;

        public override void InitializeStates(out BaseState state) {
            state = Off;
            Off
                .PlayAnim("off")
                .Enter(smi => smi._light.enabled = false)
                .EventTransition(GameHashes.ActiveChanged, On, smi => smi.operational.IsActive)
                ;
            On
                .PlayAnim("on")
                .Enter(smi => smi._light.enabled = true)
                .ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight)
                .EventTransition(GameHashes.ActiveChanged, Off, smi => !smi.operational.IsActive)
                ;
        }

        public class Def : BaseDef {
        }

        public new class Instance : GameInstance {
            public readonly Light2D _light;
            public readonly Operational operational;

            public Instance(IStateMachineTarget master, Def def)
                : base(master, def) {
                operational = master.GetComponent<Operational>();
                _light = master.GetComponent<Light2D>();
            }
        }
    }
}