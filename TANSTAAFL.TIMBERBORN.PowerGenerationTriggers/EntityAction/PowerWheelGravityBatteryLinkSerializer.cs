using System;
using System.Collections.Generic;
using System.Text;
using Timberborn.Persistence;

namespace TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.EntityAction
{
    public class PowerWheelGravityBatteryLinkSerializer : IObjectSerializer<PowerWheelGravityBatteryLink>
    {
        private static readonly PropertyKey<PowerWheelMonoBehaviour> PowerWheelKey = new PropertyKey<PowerWheelMonoBehaviour>("PowerWheel");
        private static readonly PropertyKey<GravityBatteryMonoBehaviour> GravityBatteryKey = new PropertyKey<GravityBatteryMonoBehaviour>("GravityBattery");

        public void Serialize(PowerWheelGravityBatteryLink value, IObjectSaver objectSaver)
        {
            objectSaver.Set(PowerWheelKey, value.PowerWheel);
            objectSaver.Set(GravityBatteryKey, value.GravityBattery);
        }

        public Obsoletable<PowerWheelGravityBatteryLink> Deserialize(IObjectLoader objectLoader)
        {
            var link = new PowerWheelGravityBatteryLink(objectLoader.Get(PowerWheelKey),
                                                    objectLoader.Get(GravityBatteryKey))
            {
            };
            return link;
        }
    }
}
