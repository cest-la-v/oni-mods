using System.Reflection;
using Harmony;
using UnityEngine;

namespace MightyVincent
{
    public class HyperReservoir : KMonoBehaviour
    {
        private float _endMass;
        private MeterController _meter;
        private float _startMass;
        [MyCmpGet] private Storage _storage;
        [MyCmpGet] private EnergyConsumer _energyConsumer;
        [MyCmpGet] private ConduitConsumer _conduitConsumer;
        [MyCmpGet] private ConduitDispenser _conduitDispenser;
        [MyCmpGet] private BuildingEnabledButton _buildingEnabledButton;
        [MyCmpGet] private LogicOperationalController _logicOperationalController;

        [MyCmpReq] public Operational operational;
        private readonly FieldInfo _logicOperationalFlagGetter = AccessTools.Field(typeof(LogicOperationalController), "LogicOperationalFlag");

        protected override void OnSpawn()
        {
            base.OnSpawn();
            _meter = new MeterController(GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, "meter_fill", "meter_OL");
            Subscribe((int) GameHashes.OnStorageChange, OnStorageChange);
            Subscribe((int) Hashes.OnConduitConsumerUpdateStart, OnConduitConsumerUpdateStart);
            Subscribe((int) Hashes.OnConduitConsumerUpdateEnd, OnConduitConsumerUpdateEnd);
            Subscribe((int) Hashes.OnConduitDispenserUpdateStart, OnConduitDispenserUpdateStart);
            OnStorageChange(null);
        }

        private void OnStorageChange(object data)
        {
            _meter.SetPositionPercent(Mathf.Clamp01(_storage.MassStored() / _storage.capacityKg));
        }

        private void OnConduitConsumerUpdateStart(object o)
        {
            _conduitConsumer.alwaysConsume = _energyConsumer.IsPowered;
            if (!(o is Storage storage)) return;
            _startMass = storage.MassStored();
        }

        private void OnConduitConsumerUpdateEnd(object o)
        {
            if (!(o is Storage storage)) return;
            _endMass = storage.MassStored();
            operational.SetActive(_startMass < _endMass);
        }

        private void OnConduitDispenserUpdateStart(object o)
        {
            var logicOperationalFlag = (Operational.Flag) _logicOperationalFlagGetter.GetValue(_logicOperationalController);
            _conduitDispenser.alwaysDispense = _buildingEnabledButton.IsEnabled && operational.GetFlag(logicOperationalFlag);
        }

    }
}