using Bindito.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Timberborn.BaseComponentSystem;
using Timberborn.ConstructibleSystem;
using Timberborn.EntitySystem;
using Timberborn.PowerStorage;
using Timberborn.TickSystem;
using UnityEngine;

namespace TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.EntityAction
{
    public class GravityBatteryRegisteredComponent : BaseComponent, IRegisteredComponent, IFinishedStateListener
    {
        public void OnEnterFinishedState()
        {
        }

        public void OnExitFinishedState()
        {
        }
    }
}
