using Bindito.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Timberborn.Buildings;
using Timberborn.ConstructibleSystem;
using Timberborn.Persistence;
using Timberborn.WeatherSystem;
using UnityEngine;

namespace TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.EntityAction
{
    public class PowerWheelMonoBehaviour : MonoBehaviour, IPersistentEntity, IFinishedStateListener
    {
        private static readonly ComponentKey PowerWheelKey = new ComponentKey(nameof(PowerWheelMonoBehaviour));

        private static readonly ListKey<PowerWheelGravityBatteryLink> PowerWheelLinksKey = new ListKey<PowerWheelGravityBatteryLink>(nameof(PowerWheelLinks));

        private PowerWheelGravityBatteryLinkSerializer _linkSerializer;

        private readonly List<PowerWheelGravityBatteryLink> _powerWheelLinks = new List<PowerWheelGravityBatteryLink>();
        public ReadOnlyCollection<PowerWheelGravityBatteryLink> PowerWheelLinks { get; private set; }

        public int MaxGravityBatteryLinks = 1;

        [Inject]
        public void InjectDependencies(
            PowerWheelGravityBatteryLinkSerializer linkSerializer)
        {
            _linkSerializer = linkSerializer;
        }

        public void Awake()
        {
            PowerWheelLinks = _powerWheelLinks.AsReadOnly();
        }

        public void Save(IEntitySaver entitySaver)
        {
            IObjectSaver component = entitySaver.GetComponent(PowerWheelKey);

            component.Set(PowerWheelLinksKey, PowerWheelLinks, _linkSerializer);
        }

        public void Load(IEntityLoader entityLoader)
        {
            if (!entityLoader.HasComponent(PowerWheelKey))
            {
                return;
            }
            IObjectLoader component = entityLoader.GetComponent(PowerWheelKey);
            if (component.Has(PowerWheelLinksKey))
            {
                _powerWheelLinks.AddRange(component.Get(PowerWheelLinksKey, _linkSerializer));

                foreach (var link in PowerWheelLinks)
                {
                    PostAttachLink(link);
                }
            }
        }

        public void OnEnterFinishedState()
        {
        }

        public void OnExitFinishedState()
        {
            DetachAllLinks();
        }

        public void AttachLink(PowerWheelMonoBehaviour powerWheel,
                               GravityBatteryMonoBehaviour gravityBattery)
        {

            var link = new PowerWheelGravityBatteryLink(powerWheel, gravityBattery);
            _powerWheelLinks.Add(link);
            PostAttachLink(link);
        }

        public void PostAttachLink(PowerWheelGravityBatteryLink link)
        {
            link.GravityBattery.AttachPowerWheel(link);
        }

        public void DetachAllLinks()
        {
            foreach (var link in _powerWheelLinks)
            {
                PostDetachLink(link);
            }
            _powerWheelLinks.Clear();
        }
        public void DetachLink(PowerWheelGravityBatteryLink link)
        {
            if (!_powerWheelLinks.Remove(link))
            {
                throw new InvalidOperationException($"Coudln't remove {link} from {this}, it wasn't added.");
            }
            PostDetachLink(link);
        }

        private void PostDetachLink(PowerWheelGravityBatteryLink link)
        {
            link.GravityBattery.DetachPowerWheel(link);
        }

        public void PauseBuilding()
        {
            var pausable = GetComponent<PausableBuilding>();
            if (!pausable.Paused)
            {
                pausable.Pause();
            }
        }

        public void ResumeBuilding()
        {
            var pausable = GetComponent<PausableBuilding>();
            if (pausable.Paused)
            {
                pausable.Resume();
            }
        }
    }
}
