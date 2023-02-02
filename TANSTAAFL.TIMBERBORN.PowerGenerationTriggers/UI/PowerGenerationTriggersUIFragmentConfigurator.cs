using Bindito.Core;
using System;
using System.Collections.Generic;
using System.Text;
using TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.EntityAction;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;
using Timberborn.Buildings;
using Timberborn.EntityPanelSystem;
using Timberborn.PowerGenerating;
using Timberborn.TemplateSystem;

namespace TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.UI
{
    [Configurator(SceneEntrypoint.InGame)]
    public class PowerGenerationTriggersUIFragmentConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<LinkerFragment<BeaverPoweredGenerator>>().AsSingleton();
            containerDefinition.Bind<LinkerFragment<GoodPoweredGenerator>>().AsSingleton();
            containerDefinition.Bind<EntityLinkViewFactory>().AsSingleton();
            containerDefinition.Bind<StartLinkingButton>().AsTransient();
            containerDefinition.Bind<LinkViewFactory>().AsSingleton();
            containerDefinition.Bind<GravityBatteryLinksFragment<GravityBatteryRegisteredComponent>>().AsSingleton();
            containerDefinition.MultiBind<EntityPanelModule>().ToProvider<EntityPanelModuleProvider>().AsSingleton();
        }

        /// <summary>
        /// This magic class somehow adds our UI fragment into the game
        /// </summary>
        private class EntityPanelModuleProvider : IProvider<EntityPanelModule>
        {
            private readonly LinkerFragment<BeaverPoweredGenerator> _linkerFragment;
            private readonly LinkerFragment<GoodPoweredGenerator> _linkerFragment2;
            private readonly GravityBatteryLinksFragment<GravityBatteryRegisteredComponent> _gravityBatteryFragment;

            public EntityPanelModuleProvider(LinkerFragment<BeaverPoweredGenerator> linkerFragment, LinkerFragment<GoodPoweredGenerator> linkerFragment2, GravityBatteryLinksFragment<GravityBatteryRegisteredComponent> gravityBatteryFragment)
            {
                _linkerFragment = linkerFragment;
                _linkerFragment2 = linkerFragment2;
                _gravityBatteryFragment = gravityBatteryFragment;
            }

            public EntityPanelModule Get()
            {
                EntityPanelModule.Builder builder = new EntityPanelModule.Builder();
                builder.AddBottomFragment(_linkerFragment);
                builder.AddBottomFragment(_linkerFragment2);
                builder.AddBottomFragment(_gravityBatteryFragment);
                return builder.Build();
            }
        }
    }
}
