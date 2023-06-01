using MonoMod.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using TimberApi.EntityLinkerSystem;
using Timberborn.Buildings;
using Timberborn.BuildingsBlocking;
using Timberborn.Persistence;
using Timberborn.PowerGenerating;
using Timberborn.PowerStorage;
using Timberborn.TickSystem;

namespace TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.EntityAction
{
    public class BeaverPoweredGeneratorService : TickableComponent, IPersistentEntity
    {
        private static readonly ComponentKey BeaverPoweredGeneratorServiceKey = new (nameof(BeaverPoweredGeneratorService));
        private static readonly PropertyKey<float> MinValueKey = new PropertyKey<float>("MinValue");
        private static readonly PropertyKey<float> MaxValueKey = new PropertyKey<float>("MaxValue");

        private PausableBuilding _beaverPoweredGeneratorPausable;
        private EntityLinker _linker;

        public float MinValue { get; set; }
        public float MaxValue { get; set; }

        private void Awake()
        {
            _beaverPoweredGeneratorPausable = this.GetComponentFast<PausableBuilding>();
            _linker = GetComponentFast<EntityLinker>();
        }

        public void Save(IEntitySaver entitySaver)
        {
            entitySaver.GetComponent(BeaverPoweredGeneratorServiceKey).Set(MinValueKey, MinValue);
            entitySaver.GetComponent(BeaverPoweredGeneratorServiceKey).Set(MaxValueKey, MaxValue);
        }

        public void Load(IEntityLoader entityLoader)
        {
            if (entityLoader.HasComponent(BeaverPoweredGeneratorServiceKey))
            {
                MinValue = entityLoader.GetComponent(BeaverPoweredGeneratorServiceKey).Get(MinValueKey);
                MaxValue = entityLoader.GetComponent(BeaverPoweredGeneratorServiceKey).Get(MaxValueKey);
            }
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
                    ? link.Linkee.GetComponentFast<GravityBattery>()
                    : link.Linker.GetComponentFast<GravityBattery>();

                var currChargePercentage = gravityBattery.Charge / gravityBattery.Capacity;

                if (currChargePercentage < MinValue && _beaverPoweredGeneratorPausable.Paused)
                {
                    _beaverPoweredGeneratorPausable.Resume();
                    continue;
                }

                if (currChargePercentage >= MaxValue && !_beaverPoweredGeneratorPausable.Paused)
                {
                    _beaverPoweredGeneratorPausable.Pause();
                }
            }
        }
    }
}
