using System;
using System.Collections.Generic;
using System.Text;

namespace TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.EntityAction
{
    public class PowerWheelGravityBatteryLink
    {
        public PowerWheelMonoBehaviour PowerWheel { get; }

        public GravityBatteryMonoBehaviour GravityBattery { get; }

        public PowerWheelGravityBatteryLink(
            PowerWheelMonoBehaviour powerWheel,
            GravityBatteryMonoBehaviour gravityBattery)
        {
            PowerWheel = powerWheel;
            GravityBattery = gravityBattery;
        }
    }
}
