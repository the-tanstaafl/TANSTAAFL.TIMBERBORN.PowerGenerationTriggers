using Bindito.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Timberborn.Buildings;
using Timberborn.ConstructibleSystem;
using Timberborn.EntitySystem;
using Timberborn.PowerStorage;
using Timberborn.TickSystem;
using Timberborn.WaterBuildings;
using Timberborn.WeatherSystem;

namespace TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.EntityAction
{
    public class GravityBatteryMonoBehaviour : TickableComponent, IRegisteredComponent, IFinishedStateListener
    {

        private List<PowerWheelGravityBatteryLink> _powerWheelLinks = new List<PowerWheelGravityBatteryLink>();
        public ReadOnlyCollection<PowerWheelGravityBatteryLink> PowerWheelLinks { get; private set; }

        private EntityComponentRegistry _entityComponentRegistry;

        private GravityBattery _gravityBattery;

        [Inject]
        public void InjectDependencies(
            EntityComponentRegistry entityComponentRegistry)
        {
            _entityComponentRegistry = entityComponentRegistry;
        }


        private void Awake()
        {
            PowerWheelLinks = _powerWheelLinks.AsReadOnly();
            base.enabled = false;
        }

        public void AttachPowerWheel(PowerWheelGravityBatteryLink link)
        {
            _powerWheelLinks.Add(link);

        }

        public void DetachPowerWheel(PowerWheelGravityBatteryLink link)
        {
            _powerWheelLinks.Remove(link);
        }

        private void DetachAllPowerWheels()
        {
            for (int i = _powerWheelLinks.Count - 1; i >= 0; i--)
            {
                var link = _powerWheelLinks[i];
                link.PowerWheel.DetachLink(link);
            }
        }

        public void OnEnterFinishedState()
        {
            base.enabled = true;
            _entityComponentRegistry.Register(this);
            _gravityBattery = this.GetComponent<GravityBattery>();
        }

        public void OnExitFinishedState()
        {
            base.enabled = false;
            _entityComponentRegistry.Unregister(this);
            DetachAllPowerWheels();
            _gravityBattery = null;
        }

        /// <summary>
        /// Check every tick if streamgauge is linked to a floodagte
        /// and if the floodgate's height should be altered
        /// </summary>
        public override void Tick()
        {
            if (!enabled)
            {
                return;
            }
            var currChargePercentage = _gravityBattery.Charge / _gravityBattery.Capacity;
            foreach (var link in PowerWheelLinks)
            {
                var pausable = link.PowerWheel.GetComponent<PausableBuilding>();
                if (currChargePercentage <= 0.25f && pausable.Paused)
                {
                    pausable.Resume();
                }
                else if (currChargePercentage > 0.75 && !pausable.Paused)
                {
                    pausable.Pause();
                }
            }
        }
    }
}
