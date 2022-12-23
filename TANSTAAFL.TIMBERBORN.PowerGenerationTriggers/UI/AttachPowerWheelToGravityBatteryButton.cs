using System;
using System.Collections.Generic;
using System.Text;
using TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.EntityAction;
using Timberborn.Localization;
using Timberborn.PickObjectToolSystem;
using Timberborn.PowerStorage;
using Timberborn.SelectionSystem;
using Timberborn.ToolSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.UI
{
    public class AttachPowerWheelToGravityBatteryButton
    {
        private static readonly string PickGravityBatteryTipLocKey = "PowerGenerationTriggers.PickGravityBatteryTip";
        private static readonly string PickGravityBatteryTitleLocKey = "PowerGenerationTriggers.PickGravityBatteryTitle";
        private static readonly string AttachToGravityBatteryLocKey = "PowerGenerationTriggers.AttachToGravityBattery";

        private readonly ILoc _loc;
        private readonly PickObjectTool _pickObjectTool;
        private readonly SelectionManager _selectionManager;
        private readonly ToolManager _toolManager;
        private Button _button;

        public AttachPowerWheelToGravityBatteryButton(ILoc loc,
                                         PickObjectTool pickObjectTool,
                                         SelectionManager selectionManager,
                                         ToolManager toolManager)
        {
            _loc = loc;
            _pickObjectTool = pickObjectTool;
            _selectionManager = selectionManager;
            _toolManager = toolManager;
        }

        public void Initialize(VisualElement root, Func<PowerWheelMonoBehaviour> powerWheelProvider, Action createdRouteCallback)
        {
            _button = root.Q<Button>("NewGravityBatteryButton");
            _button.clicked += delegate
            {
                StartAttachGravityBattery(powerWheelProvider(), createdRouteCallback);
            };
        }

        /// <summary>
        /// If selection is cancelled, opt out of the 
        /// object picking tool
        /// </summary>
        public void StopGravityBatteryAttachment()
        {
            if (_toolManager.ActiveTool == _pickObjectTool)
            {
                _toolManager.SwitchToDefaultTool();
            }
        }

        /// <summary>
        /// Fire up the object picking tool when the button is clicked
        /// </summary>
        /// <param name="powerWheel"></param>
        /// <param name="createdLinkCallback"></param>
        private void StartAttachGravityBattery(PowerWheelMonoBehaviour powerWheel, Action createdLinkCallback)
        {
            _pickObjectTool.StartPicking<GravityBatteryMonoBehaviour>(
                _loc.T(PickGravityBatteryTitleLocKey),
                _loc.T(PickGravityBatteryTipLocKey),
                (GameObject gameObject) => ValidateGravityBattery(powerWheel, gameObject),
                delegate (GameObject gravityBattery)
                {
                    FinishGravityBatterySelection(powerWheel, gravityBattery, createdLinkCallback);
                });

            var powerWhellGameObject = powerWheel.gameObject;

            _selectionManager.Select(powerWhellGameObject);
        }

        /// <summary>
        /// This is basically useless as of now
        /// </summary>
        /// <param name="powerWheel"></param>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        private string ValidateGravityBattery(PowerWheelMonoBehaviour powerWheel, GameObject gameObject)
        {
            GravityBatteryMonoBehaviour gravityBatteryComponent = gameObject.GetComponent<GravityBatteryMonoBehaviour>();
            return "";
        }

        /// <summary>
        /// Link the water pump and Battery when a Battery
        /// is selected
        /// </summary>
        /// <param name="floodgate"></param>
        /// <param name="gravityBattery"></param>
        /// <param name="attachedGravityBatteryCallback"></param>
        private void FinishGravityBatterySelection(
            PowerWheelMonoBehaviour powerWheel,
            GameObject gravityBattery,
            Action attachedGravityBatteryCallback)
        {
            GravityBatteryMonoBehaviour gravityBatteryComponent = gravityBattery.GetComponent<GravityBatteryMonoBehaviour>();
            powerWheel.AttachLink(powerWheel, gravityBatteryComponent);
            attachedGravityBatteryCallback();
        }

        /// <summary>
        /// Update the text on the button
        /// </summary>
        /// <param name="currentLinks"></param>
        /// <param name="maxLinks"></param>
        public void UpdateRemainingSlots(int currentLinks, int maxLinks)
        {
            _button.text = $"{_loc.T(AttachToGravityBatteryLocKey)} ({currentLinks}/{maxLinks})";
            _button.SetEnabled(currentLinks < maxLinks);
        }
    }
}
