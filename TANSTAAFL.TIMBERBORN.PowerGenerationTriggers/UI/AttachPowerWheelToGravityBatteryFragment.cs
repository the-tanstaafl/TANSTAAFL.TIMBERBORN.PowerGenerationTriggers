using System;
using System.Collections.Generic;
using System.Text;
using TimberApi.UiBuilderSystem;
using Timberborn.Localization;
using Timberborn.SelectionSystem;
using UnityEngine.UIElements;
using UnityEngine;
using TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.EntityAction;
using System.Globalization;
using System.Linq;
using Timberborn.PrefabSystem;
using Timberborn.WaterBuildings;
using UnityEngine.Rendering;
using static UnityEngine.UIElements.Length.Unit;
using Timberborn.PowerStorage;
using System.Collections.ObjectModel;
using Timberborn.Common;

namespace TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.UI
{
    public class AttachPowerWheelToGravityBatteryFragment
    {
        private readonly UIBuilder _builder;
        private readonly ILoc _loc;
        private readonly AttachPowerWheelToGravityBatteryButton _attachPowerWheelToGravityBatteryButton;
        private PowerWheelMonoBehaviour _powerWheelMonoBehaviour;

        private VisualElement _linksScrollView;
        private Label _noLinks;
        private Sprite _gravityBatterySprite;

        private readonly SelectionManager _selectionManager;
        LinkViewFactory _linkViewFactory;

        public AttachPowerWheelToGravityBatteryFragment(
            AttachPowerWheelToGravityBatteryButton attachPowerWheelToGravityBatteryButton,
            UIBuilder builder,
            SelectionManager selectionManager,
            LinkViewFactory linkViewFactory,
            ILoc loc)
        {
            _attachPowerWheelToGravityBatteryButton = attachPowerWheelToGravityBatteryButton;
            _builder = builder;
            _selectionManager = selectionManager;
            _linkViewFactory = linkViewFactory;
            _loc = loc;
        }

        public VisualElement InitiliazeFragment(VisualElement parent)
        {
            _gravityBatterySprite = (Sprite)Resources.LoadAll("Buildings", typeof(Sprite))
                                                  .Where(x => x.name.StartsWith("GravityBattery"))
                                                  .SingleOrDefault();

            var root = _builder.CreateComponentBuilder()
                               .CreateVisualElement()
                               .SetName("LinksScrollView")
                                .SetWidth(new Length(290, Pixel))
                               .SetJustifyContent(Justify.Center)
                               .SetMargin(new Margin(0, 0, new Length(7, Pixel), 0))
                               .BuildAndInitialize();

            _attachPowerWheelToGravityBatteryButton.Initialize(parent, () => _powerWheelMonoBehaviour, delegate
            {
                RemoveAllGravityBatteryViews();
                ShowFragment(_powerWheelMonoBehaviour);
            });

            _noLinks = _builder.CreateComponentBuilder()
                               .CreateLabel()
                               .AddPreset(factory => factory.Labels()
                                                            .GameTextBig(name: "NoLinksLabel",
                                                                         locKey: "PowerGenerationTriggers.NoLinks",
                                                                         builder: builder =>
                                                                            builder.SetStyle(style =>
                                                                                style.alignSelf = Align.Center)))
                               .BuildAndInitialize();

            _linksScrollView = root.Q<VisualElement>("LinksScrollView");

            return root;
        }

        public void ClearFragment()
        {
            _powerWheelMonoBehaviour = null;
            RemoveAllGravityBatteryViews();
        }

        public void ShowFragment(PowerWheelMonoBehaviour powerWheelMonoBehaviour)
        {
            _powerWheelMonoBehaviour = powerWheelMonoBehaviour;
            AddAllGravityBatteryViews();
        }

        public void UpdateFragment()
        {
            if ((bool)_powerWheelMonoBehaviour)
            {
                //var links = _powerWheelMonoBehaviour.PowerWheelLinks;
                //for (int i = 0; i < links.Count(); i++)
                //{
                //    var gauge = links[i].GravityBattery.GetComponent<GravityBattery>();
                //}
            }
        }

        public void AddAllGravityBatteryViews()
        {
            ReadOnlyCollection<PowerWheelGravityBatteryLink> links = _powerWheelMonoBehaviour.PowerWheelLinks;
            for (int i = 0; i < links.Count; i++)
            {
                var j = i;
                var link = links[i];
                var gravityBattery = link.GravityBattery.gameObject;
                var labeledPrefab = gravityBattery.GetComponent<LabeledPrefab>();
                var view = _linkViewFactory.CreateViewForPowerWheel(i, labeledPrefab.DisplayNameLocKey);

                //var gravityBatteryHeightLabel = view.Q<Label>("GravityBatteryHeightLabel");

                var imageContainer = view.Q<VisualElement>("ImageContainer");
                var img = new Image();
                img.sprite = _gravityBatterySprite;
                imageContainer.Add(img);

                var targetButton = view.Q<Button>("Target");
                targetButton.clicked += delegate
                {
                    _selectionManager.FocusOn(gravityBattery);
                };

                view.Q<Button>("DetachLinkButton").clicked += delegate
                {
                    link.PowerWheel.DetachLink(link);
                    ResetLinks();
                };

                _linksScrollView.Add(view);
            }

            _attachPowerWheelToGravityBatteryButton.UpdateRemainingSlots(links.Count, _powerWheelMonoBehaviour.MaxGravityBatteryLinks);
            if (links.IsEmpty())
            {
                _linksScrollView.Add(_noLinks);
            }
        }

        public void ResetLinks()
        {
            RemoveAllGravityBatteryViews();
            AddAllGravityBatteryViews();
            UpdateFragment();
        }

        public void RemoveAllGravityBatteryViews()
        {
            _linksScrollView.Clear();
        }
    }
}
