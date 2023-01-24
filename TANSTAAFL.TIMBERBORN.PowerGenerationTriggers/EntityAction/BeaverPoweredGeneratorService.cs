using MonoMod.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using TimberApi.EntityLinkerSystem;
using Timberborn.Buildings;
using Timberborn.PowerGenerating;
using Timberborn.PowerStorage;
using Timberborn.TickSystem;

namespace TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.EntityAction
{
    public class BeaverPoweredGeneratorService : TickableComponent
    {
        private PausableBuilding _beaverPoweredGeneratorPausable;
        private EntityLinker _linker;

        private void Awake()
        {
            _beaverPoweredGeneratorPausable = this.GetComponent<PausableBuilding>();
            _linker = GetComponent<EntityLinker>();
        }

        public override void Tick()
        {
            if (!enabled)
            {
                return;
            }

            foreach (var link in _linker.EntityLinks)
            {
                var gravityBattery = link.Linker == _linker
                    ? link.Linkee.GetComponent<GravityBattery>()
                    : link.Linker.GetComponent<GravityBattery>();

                var currChargePercentage = gravityBattery.Charge / gravityBattery.Capacity;

                if (currChargePercentage <= 0.25f && _beaverPoweredGeneratorPausable.Paused)
                {
                    _beaverPoweredGeneratorPausable.Resume();
                    continue;
                }

                if (currChargePercentage > 0.75 && !_beaverPoweredGeneratorPausable.Paused)
                {
                    _beaverPoweredGeneratorPausable.Pause();
                }
            }
        }
    }
}
