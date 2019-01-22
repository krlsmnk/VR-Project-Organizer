using System;
using VRTK;

namespace CAVS.ProjectOrganizer.Controls
{

    public class GrabPlayerControl : PlayerControl
    {

        public override Action Build(VRTK_ControllerEvents hand)
        {
            var pointer = hand.gameObject.AddComponent<VRTK_Pointer>();
            var renderer = hand.gameObject.AddComponent<VRTK_StraightPointerRenderer>();
            var touch = hand.gameObject.AddComponent<VRTK_InteractTouch>();
            var grab = hand.gameObject.AddComponent<VRTK_InteractGrab>();

            grab.grabButton = VRTK_ControllerEvents.ButtonAlias.TriggerPress;
            grab.interactTouch = touch;
            pointer.targetListPolicy = hand.GetComponent<VRTK_PolicyList>();
            pointer.interactWithObjects = true;
            pointer.activateOnEnable = true;
            pointer.holdButtonToActivate = false;
            pointer.enableTeleport = false;
            pointer.activationButton = VRTK_ControllerEvents.ButtonAlias.GripPress;
            pointer.selectionButton = VRTK_ControllerEvents.ButtonAlias.TriggerPress;
            pointer.pointerRenderer = renderer;

            return delegate ()
            {
                UnityEngine.Object.Destroy(pointer);
                UnityEngine.Object.Destroy(renderer);
                UnityEngine.Object.Destroy(grab);
                UnityEngine.Object.Destroy(touch);
            };
        }
    }

}