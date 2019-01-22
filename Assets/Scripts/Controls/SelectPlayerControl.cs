using System;
using UnityEngine;
using VRTK;

namespace CAVS.ProjectOrganizer.Controls
{

    public class SelectPlayerControl : PlayerControl
    {
        public override Action Build(VRTK_ControllerEvents hand)
        {
            var behavior = SelectBehavior.Initialize(hand);
            var uipoint = hand.gameObject.AddComponent<VRTK_UIPointer>();

            uipoint.activationMode = VRTK_UIPointer.ActivationMethods.AlwaysOn;

            return delegate ()
            {
                UnityEngine.Object.Destroy(behavior);
                UnityEngine.Object.Destroy(uipoint);
            };
        }

        public override Texture2D GetIcon()
        {
            return Resources.Load<Texture2D>("PlayerControl/Gear-icon");
        }
    }

}