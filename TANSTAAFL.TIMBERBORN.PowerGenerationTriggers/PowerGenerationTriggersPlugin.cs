using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using TimberApi.ConsoleSystem;
using TimberApi.ModSystem;

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
    }
}
