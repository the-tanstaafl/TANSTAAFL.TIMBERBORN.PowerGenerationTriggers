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
using TimberApi.EntityLinkerSystem;
using Timberborn.BaseComponentSystem;

namespace TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.UI
{
    public class GravityBatteryLinksFragment<T> : IEntityPanelFragment
    {
        private readonly UIBuilder _builder;

        private VisualElement _root;
        private ScrollView _links;
        private Label _noLinks;

        protected EntityLinker _entityLinker;

        private Sprite _powerWheelSprite;

        LinkViewFactory _linkViewFactory;
        private readonly EntitySelectionService _selectionManager;

        private T _component;

        public GravityBatteryLinksFragment(
            UIBuilder builder,
            LinkViewFactory linkViewFactory,
            EntitySelectionService selectionManager)
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
                                                                      .GameTextBig(locKey: "PowerManagement.Entitylink.LinkedObjects",
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
                                                                         locKey: "PowerManagement.Entitylink.NoLinkedObjects",
                                                                         builder: builder =>
                                                                            builder.SetStyle(style =>
                                                                                style.alignSelf = Align.Center)))
                               .BuildAndInitialize();

            _root.ToggleDisplayStyle(visible: false);
            return _root;
        }

        public void ShowFragment(BaseComponent entity)
        {
            _entityLinker = entity.GetComponentFast<EntityLinker>();
            _component = entity.GetComponentFast<T>();

            if ((bool)_entityLinker && _component != null)
            {
                UpdateLinks();
            }
        }
        public void ClearFragment()
        {
            _entityLinker = null;
            _root.ToggleDisplayStyle(visible: false);
            RemoveAllLinksViews();
        }

        public void UpdateFragment()
        {
            if ((bool)_entityLinker && _component != null)
            {
                _root.ToggleDisplayStyle(visible: true);
            }
        }

        /// <summary>
        /// Creates view for all existing floodgate links
        /// </summary>
        public void UpdateLinks()
        {
            _links.Clear();

            foreach (var link in _entityLinker.EntityLinks)
            {
                var powerWheel = link.Linker.GameObjectFast;
                var labeledPrefab = powerWheel.GetComponent<LabeledPrefab>();
                var view = _linkViewFactory.CreateViewForGravityBattery(labeledPrefab.DisplayNameLocKey);

                var imageContainer = view.Q<VisualElement>("ImageContainer");
                var img = new Image();
                img.sprite = _powerWheelSprite;
                imageContainer.Add(img);

                var targetButton = view.Q<Button>("Target");
                targetButton.clicked += delegate
                {
                    _selectionManager.FocusOnSelectable(powerWheel.GetComponent<SelectableObject>());
                };

                view.Q<Button>("DetachLinkButton").clicked += delegate
                {
                    link.Linker.DeleteLink(link);
                    UpdateLinks();
                };

                _links.Add(view);
            }
            if (_entityLinker.EntityLinks.IsEmpty())
            {
                _links.Add(_noLinks);
            }
            UpdateFragment();
        }

        public void RemoveAllLinksViews()
        {
            _links.Clear();
        }
    }
}
