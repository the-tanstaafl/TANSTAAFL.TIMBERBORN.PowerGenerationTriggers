using Bindito.Core;
using System;
using System.Collections.Generic;
using System.Text;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;
using Timberborn.EntityPanelSystem;

namespace TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.UI
{
    [Configurator(SceneEntrypoint.InGame)]
    public class PowerGenerationTriggersUIFragmentConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<PowerWheelFragment>().AsSingleton();
            containerDefinition.Bind<AttachPowerWheelToGravityBatteryButton>().AsSingleton();
            containerDefinition.Bind<AttachPowerWheelToGravityBatteryFragment>().AsSingleton();
            //containerDefinition.Bind<TriggersFragment>().AsSingleton();
            containerDefinition.Bind<LinkViewFactory>().AsSingleton();
            containerDefinition.MultiBind<EntityPanelModule>().ToProvider<EntityPanelModuleProvider>().AsSingleton();
        }

        /// <summary>
        /// This magic class somehow adds our UI fragment into the game
        /// </summary>
        private class EntityPanelModuleProvider : IProvider<EntityPanelModule>
        {
            private readonly PowerWheelFragment _powerWheelFragment;

            public EntityPanelModuleProvider(PowerWheelFragment powerWheelFragment)
            {
                _powerWheelFragment = powerWheelFragment;
            }

            public EntityPanelModule Get()
            {
                EntityPanelModule.Builder builder = new EntityPanelModule.Builder();
                builder.AddBottomFragment(_powerWheelFragment);
                return builder.Build();
            }
        }
    }
}
