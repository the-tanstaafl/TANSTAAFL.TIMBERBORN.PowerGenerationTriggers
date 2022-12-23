using TimberApi.UiBuilderSystem;
using Timberborn.Localization;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.Length.Unit;

namespace TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.UI
{
    /// <summary>
    /// Handles the creation of new powerwheel-gravitybattery link views.
    /// </summary>
    public class LinkViewFactory
    {
        private readonly UIBuilder _builder;
        private readonly ILoc _loc;

        public LinkViewFactory(
            UIBuilder builder,
            ILoc loc)
        {
            _builder = builder;
            _loc = loc;
        }

        /// <summary>
        /// Createa a link view that is shown on the StreamGauge's fragment
        /// </summary>
        /// <returns></returns>
        public VisualElement CreateViewForGravityBattery(string objectLocKey)
        {
            var foo = _builder.CreateComponentBuilder()
                                     .CreateButton()
                                     .SetName("Target")
                                     .AddClass("entity-fragment__button")
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
                                                 .AddComponent(
                                                      _builder.CreateComponentBuilder()
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
                                                                                           .GameTextBig(text: _loc.T(objectLocKey),
                                                                                                        builder: builder => builder
                                                                                                                                .SetWidth(new Length(200, Pixel))
                                                                                                                                   .SetStyle(style =>
                                                                                                                                   {
                                                                                                                                       //style.alignContent = Align.FlexStart;
                                                                                                                                       //style.alignSelf = Align.FlexStart;
                                                                                                                                       style.unityTextAlign = TextAnchor.MiddleLeft;
                                                                                                                                       //style.alignItems = Align.FlexStart;
                                                                                                                                   })))
                                                              .Build())
                                                 .AddComponent(
                                                     _builder.CreateComponentBuilder()
                                                           .CreateButton()
                                                           .AddClass("unity-text-element")
                                                           .AddClass("unity-button")
                                                           .AddClass("entity-panel__button")
                                                           .AddClass("entity-panel__button--red")
                                                           .AddClass("distribution-route__icon-wrapper")
                                                           .SetName("DetachLinkButton")
                                                           .SetMargin(new Margin(new Length(1, Pixel), new Length(2, Pixel), 0, 0))
                                                           .AddComponent(_builder.CreateComponentBuilder()
                                                                                 .CreateVisualElement()
                                                                                 .AddClass("entity-panel__button")
                                                                                 .AddClass("delete-building__icon")
                                                                                 .Build())
                                                           .Build())
                                                 .Build()) 
                                     .Build();

            return foo;
        }


        /// <summary>
        /// Create a link view that is shown on the Floodgate's fragment
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public VisualElement CreateViewForPowerWheel(int index, string gravityBatteryLocKey)
        {
            var root = _builder.CreateComponentBuilder()
                              .CreateVisualElement()
                              .AddComponent(
                                _builder.CreateComponentBuilder()
                                        .CreateButton()
                                        .SetName("Target")
                                        .AddClass("entity-fragment__button")
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
                                                    .AddComponent(
                                                         _builder.CreateComponentBuilder()
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
                                                                                              .GameTextBig(text: _loc.T(gravityBatteryLocKey),
                                                                                                           name: "GravityBatteryLabel"))
                                                                 .AddPreset(factory => factory.Labels()
                                                                                              .GameTextBig(name: "GravityBatteryHeightLabel",
                                                                                                           builder: builder => builder.SetMargin(new Margin(0, 0, 0, new Length(2, Pixel)))
                                                                                                           ))
                                                                 .Build())
                                                    .AddComponent(
                                                        _builder.CreateComponentBuilder()
                                                              .CreateButton()
                                                              .AddClass("unity-text-element")
                                                              .AddClass("unity-button")
                                                              .AddClass("entity-panel__button")
                                                              .AddClass("entity-panel__button--red")
                                                              .AddClass("distribution-route__icon-wrapper")
                                                              .SetName("DetachLinkButton")
                                                              .SetMargin(new Margin(new Length(1, Pixel), new Length(2, Pixel), 0, 0))
                                                              .AddComponent(_builder.CreateComponentBuilder()
                                                                                    .CreateVisualElement()
                                                                                    .AddClass("entity-panel__button")
                                                                                    .AddClass("delete-building__icon")
                                                                                    .Build())
                                                              .Build())
                                                    .Build())
                                              .Build())
                                  .BuildAndInitialize();

            return root;
        }
    }
}
