using System;
using UnityEngine;
using VRTK;

namespace CAVS.ProjectOrganizer.Controls
{

    public class GrabPlayerControl : PlayerControl
    {

        public override Action Build(VRTK_ControllerEvents hand)
        {
            var touch = hand.gameObject.AddComponent<VRTK_InteractTouch>();
            var grab = GrabControlBehavior.Initialize(hand);


            return delegate ()
            {
                UnityEngine.Object.Destroy(grab);
                UnityEngine.Object.Destroy(touch);
            };
        }

        public override Texture2D GetIcon()
        {
            return Resources.Load<Texture2D>("PlayerControl/Grab-icon");
        }
    }

}