using Bindito.Core;
using Bindito.Unity;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;
using TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.EntityAction;
using TimberApi.ConfiguratorSystem;
using TimberApi.DependencyContainerSystem;
using TimberApi.SceneSystem;
using Timberborn.EntitySystem;
using Timberborn.IrrigationSystem;
using Timberborn.PowerStorage;
using Timberborn.TemplateSystem;
using Timberborn.WaterBuildings;
using UnityEngine;

namespace TANSTAAFL.TIMBERBORN.PowerGenerationTriggers
{
    [Configurator(SceneEntrypoint.InGame)]
    public class PowerGenerationTriggersConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<PowerWheelGravityBatteryLinkSerializer>().AsSingleton();
            containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
        }

        private static TemplateModule ProvideTemplateModule()
        {
            TemplateModule.Builder builder = new TemplateModule.Builder();
            builder.AddDecorator<GravityBattery, GravityBatteryMonoBehaviour>();
            return builder.Build();
        }
    }

    [HarmonyPatch(typeof(EntityService), "Instantiate", typeof(GameObject), typeof(Guid))]
    class AddPowerWheelMonoBehaviourPatch
    {
        public static void Postfix(GameObject __result)
        {
            if ((__result.GetComponent<Timberborn.PowerGenerating.BeaverPoweredGenerator>())
                && __result.name.ToLower().Contains("powerwheel"))
            {
                var instantiator = DependencyContainer.GetInstance<IInstantiator>();
                instantiator.AddComponent<PowerWheelMonoBehaviour>(__result);
            }
        }
    }
}
