using System;
using System.Collections.Generic;
using System.Text;
using TimberApi.UiBuilderSystem;
using Timberborn.EntityPanelSystem;
using UnityEngine.UIElements;
using UnityEngine;
using TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.EntityAction;
using Timberborn.SelectionSystem;
using System.Linq;
using Timberborn.CoreUI;
using Timberborn.PrefabSystem;
using System.Collections.ObjectModel;
using Timberborn.Common;

namespace TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.UI
{
    public class GravityBatteryLinksFragment : IEntityPanelFragment
    {
        private readonly UIBuilder _builder;

        private VisualElement _root;
        private ScrollView _links;
        private Label _noLinks;

        private GravityBatteryMonoBehaviour _gravityBatteryMonoBehaviour;

        private VisualElement _linksView;
        private Sprite _powerWheelSprite;

        LinkViewFactory _linkViewFactory;
        private readonly SelectionManager _selectionManager;

        public GravityBatteryLinksFragment(
            UIBuilder builder,
            LinkViewFactory linkViewFactory,
            SelectionManager selectionManager)
        {
            _builder = builder;
            _linkViewFactory = linkViewFactory;
            _selectionManager = selectionManager;
        }
        public VisualElement InitializeFragment()
        {
            _powerWheelSprite = (Sprite)Resources.LoadAll("Buildings", typeof(Sprite))
                                                .Where(x => x.name.StartsWith("PowerWheel"))
                                                .FirstOrDefault();

            var rootbuilder =
                _builder.CreateFragmentBuilder()
                        .AddComponent(
                            _builder.CreateComponentBuilder()
                                    .CreateVisualElement()
                                    .SetJustifyContent(Justify.Center)
                                    .SetName("ObjectsContainer")
                                    .AddComponent(
                                        _builder.CreateComponentBuilder()
                                                .CreateLabel()
                                                .AddPreset(
                                                    factory => factory.Labels()
                                                                      .GameTextBig(locKey: "PowerGenerationTriggers.LinkedObjects",
                                                                                   builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)))
                                                .BuildAndInitialize())
                                    .AddComponent(
                                        _builder.CreateComponentBuilder()
                                                .CreateScrollView()
                                                .AddPreset(
                                                    factory => factory.ScrollViews()
                                                                      .MainScrollView(name: "GravityBatteryLinks"))
                                                .BuildAndInitialize())
                                    .BuildAndInitialize());

            _root = rootbuilder.BuildAndInitialize();
            _links = _root.Q<ScrollView>("GravityBatteryLinks");

            _noLinks = _builder.CreateComponentBuilder()
                               .CreateLabel()
                               .AddPreset(factory => factory.Labels()
                                                            .GameTextBig(name: "NoLinksLabel",
                                                                         locKey: "Floodgates.Triggers.NoFloodgateLinks",
                                                                         builder: builder =>
                                                                            builder.SetStyle(style =>
                                                                                style.alignSelf = Align.Center)))
                               .BuildAndInitialize();

            _root.ToggleDisplayStyle(visible: false);
            return _root;
        }

        public void ShowFragment(GameObject entity)
        {
            _gravityBatteryMonoBehaviour = entity.GetComponent<GravityBatteryMonoBehaviour>();
            if ((bool)_gravityBatteryMonoBehaviour)
            {
                UpdateLinks();
            }
        }
        public void ClearFragment()
        {
            _gravityBatteryMonoBehaviour = null;
            _root.ToggleDisplayStyle(visible: false);
        }

        public void UpdateFragment()
        {
            if ((bool)_gravityBatteryMonoBehaviour)
            {
                _root.ToggleDisplayStyle(visible: true);
            }
        }

        /// <summary>
        /// Creates view for all existing floodgate links
        /// </summary>
        public void UpdateLinks()
        {
            ReadOnlyCollection<PowerWheelGravityBatteryLink> links = _gravityBatteryMonoBehaviour.PowerWheelLinks;

            _links.Clear();

            foreach (var link in links)
            {
                var powerWheel = link.PowerWheel.gameObject;
                var labeledPrefab = powerWheel.GetComponent<LabeledPrefab>();
                var view = _linkViewFactory.CreateViewForGravityBattery(labeledPrefab.DisplayNameLocKey);

                var imageContainer = view.Q<VisualElement>("ImageContainer");
                var img = new Image();
                img.sprite = _powerWheelSprite;
                imageContainer.Add(img);

                var targetButton = view.Q<Button>("Target");
                targetButton.clicked += delegate
                {
                    _selectionManager.FocusOn(powerWheel);
                };

                view.Q<Button>("DetachLinkButton").clicked += delegate
                {
                    link.PowerWheel.DetachLink(link);
                    UpdateLinks();
                };

                _links.Add(view);
            }
            if (links.IsEmpty())
            {
                _links.Add(_noLinks);
            }
            UpdateFragment();
        }

        /// <summary>
        /// Removes all existing floodagate link views
        /// </summary>
        public void RemoveAllStreamGaugeViews()
        {
            _linksView.Clear();
        }
    }
}
