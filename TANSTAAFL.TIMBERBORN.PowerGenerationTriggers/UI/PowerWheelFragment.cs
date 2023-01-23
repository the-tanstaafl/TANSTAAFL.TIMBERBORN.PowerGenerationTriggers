using System;
using System.Collections.Generic;
using System.Text;
using TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.EntityAction;
using TimberApi.UiBuilderSystem;
using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.Length.Unit;

namespace TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.UI
{
    public class PowerWheelFragment : IEntityPanelFragment
    {
        private readonly UIBuilder _builder;
        private VisualElement _root;

        private PowerWheelMonoBehaviour _powerWheelMono;

        //private Button _newButton;

        private VisualElement _links;

        private AttachPowerWheelToGravityBatteryFragment _attachPowerWheelToGravityBatteryFragment;

        public PowerWheelFragment(UIBuilder builder, AttachPowerWheelToGravityBatteryFragment attachPowerWheelToGravityBatteryFragment)
        {
            _builder = builder;
            _attachPowerWheelToGravityBatteryFragment = attachPowerWheelToGravityBatteryFragment;
        }

        public VisualElement InitializeFragment()
        {
            var rootBuilder = _builder.CreateFragmentBuilder()
                                      //.ModifyWrapper(builder => builder.SetFlexDirection(FlexDirection.Row)
                                      //                                 .SetFlexWrap(Wrap.Wrap)
                                      //                                 .SetJustifyContent(Justify.Center))                                      
                                      .AddComponent(_builder.CreateComponentBuilder()
                                                            .CreateVisualElement()
                                                            .SetName("Placeholder")
                                                            .BuildAndInitialize())
                                      .AddComponent(_builder.CreateComponentBuilder()
                                                            .CreateButton()
                                                            .AddClass("entity-fragment__button")
                                                            .AddClass("entity-fragment__button--green")
                                                            .SetName("NewGravityBatteryButton")
                                                            .SetColor(new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)))
                                                            .SetFontSize(new Length(13, Pixel))
                                                            .SetFontStyle(FontStyle.Normal)
                                                            .SetHeight(new Length(29, LengthUnit.Pixel))
                                                            .SetWidth(new Length(290, LengthUnit.Pixel))
                                                            .Build());

            _root = rootBuilder.BuildAndInitialize();

            _links = _root.Q<VisualElement>("Placeholder");
            _links.Add(_attachPowerWheelToGravityBatteryFragment.InitiliazeFragment(_root));

            _root.ToggleDisplayStyle(false);

            //_root.Q<Button>("NewGravityBatteryButton").ToggleDisplayStyle(true);

            return _root;
        }

        public void ShowFragment(GameObject entity)
        {
            _powerWheelMono = entity.GetComponent<PowerWheelMonoBehaviour>();
            if ((bool)_powerWheelMono)
            {
                _attachPowerWheelToGravityBatteryFragment.ShowFragment(_powerWheelMono);
                _root.ToggleDisplayStyle(visible: true);
            }
        }

        public void ClearFragment()
        {
            _powerWheelMono = null;
            _attachPowerWheelToGravityBatteryFragment.ClearFragment();
            _root.ToggleDisplayStyle(visible: false);
        }

        public void UpdateFragment()
        {
            if ((bool)_powerWheelMono)
            {
                _attachPowerWheelToGravityBatteryFragment.UpdateFragment();
                _root.ToggleDisplayStyle(visible: true);
            }
        }
    }
}
