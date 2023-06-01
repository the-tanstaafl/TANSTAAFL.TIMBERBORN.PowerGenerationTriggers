using MonoMod.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using TimberApi.EntityLinkerSystem;
using Timberborn.BlockSystem;
using Timberborn.Buildings;
using Timberborn.BuildingsBlocking;
using Timberborn.MechanicalSystem;
using Timberborn.Persistence;
using Timberborn.PowerStorage;
using Timberborn.TickSystem;
using UnityEngine;

namespace TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.EntityAction
{
    public class PowerShaftPausable : TickableComponent, IPausableComponent
    {
        private bool Paused = false;
        private MechanicalNode _mechanicalNode;
        private PausableBuilding _pausableBuilding;
        private BlockObject _blockObject;

        private void Awake()
        {
            _mechanicalNode = GetComponentFast<MechanicalNode>();
            _pausableBuilding = GetComponentFast<PausableBuilding>();
            _blockObject = GetComponentFast<BlockObject>();
        }

        public override void Tick()
        {
            if (!_mechanicalNode.IsShaft || !_blockObject.Finished)
            {
                return;
            }

            if (Paused != _pausableBuilding.Paused)
            {
                Paused = _pausableBuilding.Paused;
                if (_pausableBuilding.Paused)
                {
                    _mechanicalNode._mechanicalGraphManager.RemoveNode(_mechanicalNode);
                }
                else
                {
                    _mechanicalNode._mechanicalGraphManager.AddNode(_mechanicalNode);
                }
            }
        }
    }
}
