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
using Timberborn.MechanicalSystem;
using Timberborn.PowerGenerating;
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
            containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
        }

        private static TemplateModule ProvideTemplateModule()
        {
            TemplateModule.Builder builder = new TemplateModule.Builder();
            builder.AddDecorator<WalkerPoweredGenerator, BeaverPoweredGeneratorService>();
            builder.AddDecorator<GoodPoweredGenerator, GoodPoweredGeneratorService>();
            builder.AddDecorator<GravityBattery, GravityBatteryRegisteredComponent>();
            builder.AddDecorator<MechanicalNode, PowerShaftPausable>();
            return builder.Build();
        }
    }
}