using Bindito.Unity;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.EntityAction;
using TimberApi.ConfigSystem;
using TimberApi.ConsoleSystem;
using TimberApi.DependencyContainerSystem;
using TimberApi.ModSystem;
using Timberborn.BuildingsNavigation;
using Timberborn.CoreUI;
using Timberborn.EntitySystem;
using Timberborn.IrrigationSystem;
using Timberborn.MechanicalSystem;
using Timberborn.MechanicalSystemUI;
using Timberborn.WaterBuildings;
using UnityEngine;

namespace TANSTAAFL.TIMBERBORN.PowerGenerationTriggers
{
    [HarmonyPatch]
    public class PowerGenerationTriggersPlugin : IModEntrypoint
    {
        internal static IConsoleWriter Log;

        public void Entry(IMod mod, IConsoleWriter consoleWriter)
        {
            Log = consoleWriter;
            var harmony = new Harmony("tanstaafl.plugins.powergenerationtriggers");
            harmony.PatchAll();

            consoleWriter.LogInfo("PowerGenerationTriggers is loaded.");
        }

        [HarmonyPatch(typeof(NetworkFragmentService), nameof(NetworkFragmentService.Update))]
        public static bool Prefix(NetworkFragmentService __instance, ref bool __result, MechanicalNode mechanicalNode)
        {
            bool flag = NetworkFragmentService.ShouldShowNetworkStatistics(mechanicalNode) && mechanicalNode.Graph != null;
            if (flag)
            {
                MechanicalGraphPower currentPower = mechanicalNode.Graph.CurrentPower;
                __instance._label.text = __instance._loc.T(NetworkFragmentService.NetworkPowerLocKey, currentPower.PowerSupply, $"{currentPower.PowerDemand} {__instance._loc.T(NetworkFragmentService.PowerSymbolLocKey)}");
            }
            __instance._label.ToggleDisplayStyle(flag);
            __result = flag;

            return false;
        }
    }
}