using Bindito.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Timberborn.ConstructibleSystem;
using Timberborn.EntitySystem;
using Timberborn.PowerStorage;
using Timberborn.TickSystem;
using UnityEngine;

namespace TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.EntityAction
{
    public class GravityBatteryRegisteredComponent : MonoBehaviour, IRegisteredComponent, IFinishedStateListener
    {
        private EntityComponentRegistry _entityComponentRegistry;

        public GravityBattery GravityBattery;

        [Inject]
        public void InjectDependencies(EntityComponentRegistry entityComponentRegistry)
        {
            _entityComponentRegistry = entityComponentRegistry;
        }

        public void OnEnterFinishedState()
        {
            _entityComponentRegistry.Register(this);
            GravityBattery = this.GetComponent<GravityBattery>();
        }

        public void OnExitFinishedState()
        {
            _entityComponentRegistry.Unregister(this);
            GravityBattery = null;
        }
    }
}
