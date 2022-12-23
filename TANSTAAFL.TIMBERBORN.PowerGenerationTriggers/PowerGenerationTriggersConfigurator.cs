using Bindito.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;
using TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.EntityAction;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;
using Timberborn.PowerStorage;
using Timberborn.TemplateSystem;
using Timberborn.WaterBuildings;

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
}
