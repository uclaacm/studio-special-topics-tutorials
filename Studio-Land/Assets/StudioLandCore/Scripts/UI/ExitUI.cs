using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace StudioLand
{
    public class ExitUI : PanelUI
    {
        public override bool CanDeselect => true;

        protected override void Awake()
        {
            base.Awake();
            
            root.Q<Button>("Yes").RegisterCallback<ClickEvent>(ev => Application.Quit());
        }
    }
}

