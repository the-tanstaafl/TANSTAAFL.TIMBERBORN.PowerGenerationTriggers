﻿using System;
using System.Collections.Generic;
using System.Text;
using TimberApi.EntityLinkerSystem;
using Timberborn.EntitySystem;
using Timberborn.Localization;
using Timberborn.SelectionSystem;
using Timberborn.ToolSystem;
using UnityEngine.UIElements;
using UnityEngine;
using Timberborn.CoreUI;
using TimberApi.ObjectSelectionSystem;
using Timberborn.BaseComponentSystem;

namespace TANSTAAFL.TIMBERBORN.PowerGenerationTriggers.UI
{
    public class StartLinkingButton
    {
        protected static readonly string StartLinkingTipLocKey = "PowerManagement.Entitylink.StartLinkingTip";
        protected static readonly string StartLinkingTitleLocKey = "PowerManagement.Entitylink.StartLinkingTitle";
        protected static readonly string StartLinkLocKey = "PowerManagement.Entitylink.StartLink";

        protected readonly ILoc _loc;
        protected readonly PickObjectTool _pickObjectTool;
        protected readonly EntitySelectionService _selectionManager;
        protected readonly ToolManager _toolManager;
        protected Button _button;

        public StartLinkingButton(
            ILoc loc,
            PickObjectTool pickObjectTool,
            EntitySelectionService selectionManager,
            ToolManager toolManager)
        {
            _loc = loc;
            _pickObjectTool = pickObjectTool;
            _selectionManager = selectionManager;
            _toolManager = toolManager;
        }

        public virtual void Initialize<T>(VisualElement root,
                                       Func<EntityLinker> linkerProvider,
                                       Action createdLinkCallback)
            where T : BaseComponent, IRegisteredComponent
        {
            _button = root.Q<Button>(LinkerFragment<T>.NewLinkButtonName);
            _button.clicked += delegate
            {
                StartLinkEntities<T>(linkerProvider(), createdLinkCallback);
            };
        }

        /// <summary>
        /// Fires up the object picker tool to select the linkee.
        /// Called when the button is pressed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="linker"></param>
        /// <param name="createdLinkCallback"></param>
        protected virtual void StartLinkEntities<T>(EntityLinker linker, Action createdLinkCallback)
            where T : BaseComponent, IRegisteredComponent
        {
            _pickObjectTool.StartPicking<T>(
                _loc.T(StartLinkingTitleLocKey),
                _loc.T(StartLinkingTipLocKey),
                (GameObject gameobject) => ValidateLinkee(linker, gameobject),
                delegate (GameObject linkee)
                {
                    FinishLinkSelection(linker, linkee, createdLinkCallback);
                });
            _selectionManager.Select(linker.GameObjectFast.GetComponent<BaseComponent>());
        }

        /// <summary>
        /// Validation logic for the linkee. Return empty string if valid.
        /// Used for example if the entities need to be connected with a path. 
        /// </summary>
        /// <param name="linker"></param>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        protected virtual string ValidateLinkee(
            EntityLinker linker,
            GameObject gameObject)
        {
            return "";
        }

        /// <summary>
        /// Creates a link between entities after the selection
        /// </summary>
        /// <param name="linker"></param>
        /// <param name="linkee"></param>
        /// <param name="createdLinkCallback"></param>
        protected virtual void FinishLinkSelection(
            EntityLinker linker,
            GameObject linkee,
            Action createdLinkCallback)
        {
            EntityLinker linkeeComponent = linkee.GetComponent<EntityLinker>();
            linker.CreateLink(linkeeComponent);
            createdLinkCallback();
        }

        /// <summary>
        /// Updates the label on the linking button
        /// </summary>
        /// <param name="currentLinks"></param>
        /// <param name="maxLinks"></param>
        public virtual void UpdateRemainingSlots(int currentLinks, int maxLinks)
        {
            _button.text = $"{_loc.T(StartLinkLocKey)} ({currentLinks}/{maxLinks})";
            _button.SetEnabled(currentLinks < maxLinks);
        }
    }
}
