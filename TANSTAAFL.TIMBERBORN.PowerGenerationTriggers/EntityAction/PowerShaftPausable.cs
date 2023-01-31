using MonoMod.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using TimberApi.EntityLinkerSystem;
using Timberborn.Buildings;
using Timberborn.MechanicalSystem;
using Timberborn.Persistence;
using Timberborn.PowerStorage;
using Timberborn.TickSystem;
using UnityEngine;

namespace TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.EntityAction
{
    public class PowerShaftPausable : TickableComponent, IPausableComponent
    {
        private bool? Paused = null;
        private MechanicalNode _mechanicalNode;
        private PausableBuilding _pausableBuilding;

        private void Awake()
        {
            _mechanicalNode = GetComponent<MechanicalNode>();
            _pausableBuilding = GetComponent<PausableBuilding>();
        }

        public override void Tick()
        {
            if (!_mechanicalNode.IsShaft)
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
