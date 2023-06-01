using System;
using System.Collections.Generic;
using System.Text;
using TimberApi.EntityLinkerSystem;
using TimberApi.UiBuilderSystem;
using Timberborn.EntityPanelSystem;
using Timberborn.Localization;
using Timberborn.PrefabSystem;
using Timberborn.SelectionSystem;
using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.ObjectModel;
using static UnityEngine.UIElements.Length.Unit;
using Timberborn.CoreUI;
using Timberborn.PowerStorage;
using Timberborn.Stockpiles;
using TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.EntityAction;
using static UnityEngine.UIElements.UIR.Implementation.UIRStylePainter;
using Timberborn.BaseComponentSystem;

namespace TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.UI
{
    public class LinkerFragment<T> : IEntityPanelFragment
    {
        protected readonly UIBuilder _builder;
        protected VisualElement _root;
        protected EntityLinker _entityLinker;

        protected static string LinkContainerName = "LinkContainer";
        internal static string NewLinkButtonName = "NewLinkButton";

        protected VisualElement _linksContainer;

        protected StartLinkingButton _startLinkButton;

        protected EntityLinkViewFactory _entityLinkViewFactory;
        protected readonly EntitySelectionService _selectionManager;
        protected readonly ILoc _loc;

        private int _maxLinks = 1;

        private T _component;
        private BeaverPoweredGeneratorService _beaverPoweredComponent;
        private GoodPoweredGeneratorService _goodPoweredComponent;
        private GravityBattery _gravityBattery;

        private Tuple<Label, Slider, Label, Slider, Label> _settings = null;

        public LinkerFragment(
            UIBuilder builder,
            EntityLinkViewFactory entityLinkViewFactory,
            StartLinkingButton startLinkButton,
            EntitySelectionService selectionManager,
            ILoc loc)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _entityLinkViewFactory = entityLinkViewFactory;
            _startLinkButton = startLinkButton;
            _selectionManager = selectionManager;
            _loc = loc;
        }

        public virtual VisualElement InitializeFragment()
        {
            _root = _builder.CreateFragmentBuilder()
                            .ModifyWrapper(builder => builder.SetFlexDirection(FlexDirection.Row)
                                                             .SetFlexWrap(Wrap.Wrap)
                                                             .SetJustifyContent(Justify.Center))
                            .AddComponent(
                                _builder.CreateComponentBuilder()
                                        .CreateVisualElement()
                                        .SetName(LinkContainerName)
                                        .BuildAndInitialize())
                            .AddComponent(
                                _builder.CreateComponentBuilder()
                                        .CreateButton()
                                        .AddClass("entity-fragment__button--green")
                                        .SetName(NewLinkButtonName)
                                        .SetColor(new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)))
                                        .SetFontSize(new Length(13, Pixel))
                                        .SetFontStyle(FontStyle.Normal)
                                        .SetHeight(new Length(29, Pixel))
                                        .SetWidth(new Length(290, Pixel))
                                        .Build())
                            .BuildAndInitialize();

            _linksContainer = _root.Q<VisualElement>(LinkContainerName);

            _startLinkButton.Initialize<GravityBatteryRegisteredComponent>(_root, () => _entityLinker, delegate
            {
                foreach (var link in _entityLinker.EntityLinks)
                {
                    _gravityBattery = link.Linkee.GetComponentFast<GravityBattery>();
                    if (_gravityBattery != null)
                    {
                        break;
                    }
                }

                RemoveAllLinkViews();
                ShowFragment(_entityLinker.GameObjectFast.GetComponent<BaseComponent>());
            });

            _root.ToggleDisplayStyle(false);
            return _root;
        }

        public virtual void ShowFragment(BaseComponent entity)
        {
            _entityLinker = entity.GetComponentFast<EntityLinker>();
            _component = entity.GetComponentFast<T>();
            _beaverPoweredComponent = entity.GetComponentFast<BeaverPoweredGeneratorService>();
            _goodPoweredComponent = entity.GetComponentFast<GoodPoweredGeneratorService>();

            if ((bool)_entityLinker && _component != null)
            {
                foreach (var link in _entityLinker.EntityLinks)
                {
                    _gravityBattery = link.Linkee.GetComponentFast<GravityBattery>();
                    if (_gravityBattery != null)
                    {
                        break;
                    }

                    _gravityBattery = link.Linker.GetComponentFast<GravityBattery>();
                    if (_gravityBattery != null)
                    {
                        break;
                    }
                }

                AddLinkView();

                if (_settings != null)
                {
                    if (_beaverPoweredComponent != null)
                    {
                        _settings.Item2.SetValueWithoutNotify(_beaverPoweredComponent.MinValue);
                        _settings.Item4.SetValueWithoutNotify(_beaverPoweredComponent.MaxValue);
                    }

                    if (_goodPoweredComponent != null)
                    {
                        _settings.Item2.SetValueWithoutNotify(_goodPoweredComponent.MinValue);
                        _settings.Item4.SetValueWithoutNotify(_goodPoweredComponent.MaxValue);
                    }
                }
            }
        }

        public virtual void ClearFragment()
        {
            _entityLinker = null;
            //_beaverPoweredComponent = null;
            //_goodPoweredComponent = null;
            _root.ToggleDisplayStyle(false);
            RemoveAllLinkViews();
        }

        public virtual void UpdateFragment()
        {
            if (_entityLinker != null && _component != null)
            {
                if (_settings != null)
                {
                    _settings.Item1.text = $"Enable when charge is below: {_settings.Item2.value*100:##0.0}%";
                    _settings.Item3.text = $"Disable when charge is above: {_settings.Item4.value*100:##0.0}%";

                    if (_gravityBattery != null)
                    {
                        var currChargePercentage = _gravityBattery.Charge / _gravityBattery.Capacity;
                        _settings.Item5.text = $" {currChargePercentage*100:##0.0}%";
                    }
                }

                _root.ToggleDisplayStyle(true);
            }
            else
            {
                _root.ToggleDisplayStyle(false);
            }
        }

        /// <summary>
        /// Loops through and adds a view for all existing Links
        /// </summary>
        public virtual void AddLinkView()
        {
            ReadOnlyCollection<EntityLink> links = (ReadOnlyCollection<EntityLink>)_entityLinker.EntityLinks;

            _startLinkButton.UpdateRemainingSlots(links.Count, _maxLinks);

            if (links.Count == 0)
            {
                return;
            }

            var link = links[0];

            var linkee = link.Linker == _entityLinker
                ? link.Linkee
                : link.Linker;

            var linkeeGameObject = linkee.GameObjectFast;

            var prefab = linkeeGameObject.GetComponent<LabeledPrefab>();
            var sprite = prefab.Image;

            var view = _entityLinkViewFactory.Create(_loc.T(prefab.DisplayNameLocKey));

            var imageContainer = view.Q<VisualElement>("ImageContainer");
            var img = new Image();
            img.sprite = sprite;
            imageContainer.Add(img);

            var targetButton = view.Q<Button>("Target");
            targetButton.clicked += delegate
            {
                _selectionManager.FocusOnSelectable(linkeeGameObject.GetComponent<SelectableObject>());
            };
            view.Q<Button>("RemoveLinkButton").clicked += delegate
            {
                link.Linker.DeleteLink(link);
                ResetLinks();
            };

            var threshold1Label = view.Q<Label>("Threshold1Label");
            var threshold1Slider = view.Q<Slider>("Threshold1Slider");
            threshold1Slider.RegisterValueChangedCallback((@event) => ChangeThresholdSlider(@event, 0));
            var threshold2Label = view.Q<Label>("Threshold2Label");
            var threshold2Slider = view.Q<Slider>("Threshold2Slider");
            threshold2Slider.RegisterValueChangedCallback((@event) => ChangeThresholdSlider(@event, 1));
            var gaugeHeightLabel = view.Q<Label>("GravityBatteryCharge");

            _settings = new Tuple<Label, Slider, Label, Slider, Label>(threshold1Label, threshold1Slider, threshold2Label, threshold2Slider, gaugeHeightLabel);

            _linksContainer.Add(view);
        }

        public void ChangeThresholdSlider(ChangeEvent<float> changeEvent, int sliderIndex)
        {
            Slider slider;
            if (sliderIndex == 0)
            {
                slider = _settings.Item2;
                if (_beaverPoweredComponent != null)
                {
                    _beaverPoweredComponent.MinValue = changeEvent.newValue;

                    if (_beaverPoweredComponent.MinValue > _beaverPoweredComponent.MaxValue)
                    {
                        _beaverPoweredComponent.MaxValue = changeEvent.newValue;
                        _settings.Item4.SetValueWithoutNotify(changeEvent.newValue);
                    }
                }

                if (_goodPoweredComponent != null)
                {
                    _goodPoweredComponent.MinValue = changeEvent.newValue;

                    if (_goodPoweredComponent.MinValue > _goodPoweredComponent.MaxValue)
                    {
                        _goodPoweredComponent.MaxValue = changeEvent.newValue;
                        _settings.Item4.SetValueWithoutNotify(changeEvent.newValue);
                    }
                }
            }
            else
            {
                slider = _settings.Item4;
                if (_beaverPoweredComponent != null)
                {
                    _beaverPoweredComponent.MaxValue = changeEvent.newValue;

                    if (_beaverPoweredComponent.MaxValue < _beaverPoweredComponent.MinValue)
                    {
                        _beaverPoweredComponent.MinValue = changeEvent.newValue;
                        _settings.Item2.SetValueWithoutNotify(changeEvent.newValue);
                    }
                }

                if (_goodPoweredComponent != null)
                {
                    _goodPoweredComponent.MaxValue = changeEvent.newValue;

                    if (_goodPoweredComponent.MaxValue < _goodPoweredComponent.MinValue)
                    {
                        _goodPoweredComponent.MinValue = changeEvent.newValue;
                        _settings.Item2.SetValueWithoutNotify(changeEvent.newValue);
                    }
                }
            }

            slider.SetValueWithoutNotify(changeEvent.newValue);
        }

        /// <summary>
        /// Resets the link views. 
        /// Used for example when a new Link is added
        /// </summary>
        public virtual void ResetLinks()
        {
            RemoveAllLinkViews();
            AddLinkView();
            UpdateFragment();
        }

        /// <summary>
        /// Removes all existing Link from an entity.
        /// Used for example when the entity is destroyed
        /// </summary>
        public virtual void RemoveAllLinkViews()
        {
            _linksContainer.Clear();
        }
    }
}
