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
    public class GoodPoweredGeneratorService : TickableComponent, IPersistentEntity
    {
        private static readonly ComponentKey GoodPoweredGeneratorServiceKey = new (nameof(GoodPoweredGeneratorService));
        private static readonly PropertyKey<float> MinValueKey = new PropertyKey<float>("MinValue");
        private static readonly PropertyKey<float> MaxValueKey = new PropertyKey<float>("MaxValue");

        private PausableBuilding _goodPoweredGeneratorPausable;
        private EntityLinker _linker;

        public float MinValue { get; set; }
        public float MaxValue { get; set; }

        private void Awake()
        {
            _goodPoweredGeneratorPausable = this.GetComponentFast<PausableBuilding>();
            _linker = GetComponentFast<EntityLinker>();
        }

        public void Save(IEntitySaver entitySaver)
        {
            entitySaver.GetComponent(GoodPoweredGeneratorServiceKey).Set(MinValueKey, MinValue);
            entitySaver.GetComponent(GoodPoweredGeneratorServiceKey).Set(MaxValueKey, MaxValue);
        }

        public void Load(IEntityLoader entityLoader)
        {
            if (entityLoader.HasComponent(GoodPoweredGeneratorServiceKey))
            {
                MinValue = entityLoader.GetComponent(GoodPoweredGeneratorServiceKey).Get(MinValueKey);
                MaxValue = entityLoader.GetComponent(GoodPoweredGeneratorServiceKey).Get(MaxValueKey);
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

                if (currChargePercentage < MinValue && _goodPoweredGeneratorPausable.Paused)
                {
                    _goodPoweredGeneratorPausable.Resume();
                    continue;
                }

                if (currChargePercentage >= MaxValue && !_goodPoweredGeneratorPausable.Paused)
                {
                    _goodPoweredGeneratorPausable.Pause();
                }
            }
        }
    }
}
