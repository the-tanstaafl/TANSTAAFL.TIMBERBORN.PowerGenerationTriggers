using System;
using System.Collections.Generic;
using System.Text;
using TimberApi.UiBuilderSystem;
using UnityEngine.UIElements;
using UnityEngine;
using static UnityEngine.UIElements.Length.Unit;

namespace TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.UI
{
    public class EntityLinkViewFactory
    {
        protected readonly UIBuilder _builder;

        public EntityLinkViewFactory(
            UIBuilder builder)
        {
            _builder = builder;
        }

        /// <summary>
        /// Creates a basic link view with a image, label and delete button
        /// </summary>
        /// <param name="buttonLabelText"></param>
        /// <returns></returns>
        public virtual VisualElement Create(string buttonLabelText)
        {
            var rootBuilder =
                    _builder.CreateComponentBuilder()
                            .CreateVisualElement()
                            .AddComponent(
                                _builder.CreateComponentBuilder()
                                        .CreateButton()
                                        .SetName("Target")
                                        .AddClass("entity-fragment__button--green")
                                        .SetColor(new StyleColor(new Color(0.8f, 0.8f, 0.8f, 1f)))
                                        .SetFontSize(new Length(14, Pixel))
                                        .SetFontStyle(FontStyle.Normal)
                                        .SetHeight(new Length(30, Pixel))
                                        .SetWidth(new Length(290, Pixel))
                                        .SetPadding(new Padding(0, 0, 0, 0))
                                        .SetMargin(new Margin(new Length(2, Pixel), 0, new Length(2, Pixel), 0))
                                        .AddComponent(
                                            _builder.CreateComponentBuilder()
                                                    .CreateVisualElement()
                                                    .SetFlexWrap(Wrap.Wrap)
                                                    .SetFlexDirection(FlexDirection.Row)
                                                    .SetJustifyContent(Justify.SpaceBetween)
                                                    .AddComponent(CreateLinkButton(buttonLabelText))
                                                    .AddComponent(CreateRemoveButton())
                                                    .Build())
                                        .Build())
                            .AddComponent(
                                _builder.CreateComponentBuilder()
                                    .CreateVisualElement()
                                    .AddPreset(factory => factory.Labels()
                                        .GameTextBig(name: "Threshold1Label",
                                            builder: builder => builder.SetMargin(new Margin(0, 0, 0, new Length(2, Pixel)))))
                                    .AddPreset(
                                        factory => factory.Sliders()
                                            .SliderCircle(0f, 1f, name: "Threshold1Slider",
                                                        builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
                                                                                                .SetPadding(new Padding(new Length(21, Pixel), 0))))
                                    .AddPreset(factory => factory.Labels()
                                        .GameTextBig(name: "Threshold2Label",
                                            builder: builder => builder.SetMargin(new Margin(0, 0, 0, new Length(2, Pixel)))))
                                    .AddPreset(factory => factory.Sliders()
                                        .SliderCircle(0f, 1f, name: "Threshold2Slider",
                                            builder: sliderBuilder => sliderBuilder.SetStyle(style => style.flexGrow = 1f)
                                                .SetPadding(new Padding(new Length(21, Pixel), 0))))
                                    .Build());

            var root = rootBuilder.BuildAndInitialize();

            return root;
        }

        private VisualElement CreateLinkButton(string buttonLabelText)
        {
            return _builder.CreateComponentBuilder()
                           .CreateVisualElement()
                           .SetFlexWrap(Wrap.Wrap)
                           .SetFlexDirection(FlexDirection.Row)
                           .SetJustifyContent(Justify.FlexStart)
                           .AddComponent(
                               _builder.CreateComponentBuilder()
                                       .CreateVisualElement()
                                       .SetName("ImageContainer")
                                       .SetWidth(new Length(28, Pixel))
                                       .SetHeight(new Length(28, Pixel))
                                       .SetMargin(new Margin(new Length(1, Pixel), 0, 0, new Length(6, Pixel)))
                                       .Build())
                           .AddPreset(factory => factory.Labels()
                                .GameTextBig(text: buttonLabelText))
                           .AddPreset(factory => factory.Labels()
                                .GameTextBig(name: "GravityBatteryCharge",
                                    builder: builder => builder.SetMargin(new Margin(0, 0, 0, new Length(2, Pixel)))))
                           .Build();
        }

        private VisualElement CreateRemoveButton()
        {
            return _builder.CreateComponentBuilder()
                           .CreateButton()
                           .AddClass("entity-panel__button--red")
                           .AddClass("distribution-route__icon-wrapper")
                           .SetName("RemoveLinkButton")
                           .SetMargin(new Margin(new Length(1, Pixel), new Length(2, Pixel), 0, 0))
                           .AddComponent(_builder.CreateComponentBuilder()
                                                 .CreateVisualElement()
                                                 .AddClass("entity-panel__button")
                                                 .AddClass("delete-building__icon")
                                                 .Build())
                           .Build();
        }
    }
}
