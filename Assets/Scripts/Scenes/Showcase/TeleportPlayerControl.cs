using System;
using VRTK;

namespace CAVS.ProjectOrganizer.Scenes.Showcase
{

    public class TeleportPlayerControl : PlayerControl
    {
        public override Action Build(VRTK_ControllerEvents hand)
        {
            var renderer = hand.gameObject.AddComponent<VRTK_BezierPointerRenderer>();
            var pointer = hand.gameObject.AddComponent<VRTK_Pointer>();
            var teleport = hand.gameObject.AddComponent<VRTK_BasicTeleport>();

            pointer.targetListPolicy = hand.GetComponent<VRTK_PolicyList>();
            pointer.activateOnEnable = true;
            pointer.holdButtonToActivate = false;
            pointer.enableTeleport = true;
            pointer.activationButton = VRTK_ControllerEvents.ButtonAlias.GripPress;
            pointer.selectionButton = VRTK_ControllerEvents.ButtonAlias.TriggerPress;
            pointer.pointerRenderer = renderer;

            return delegate ()
            {
                UnityEngine.Object.Destroy(pointer);
                UnityEngine.Object.Destroy(renderer);
                UnityEngine.Object.Destroy(teleport);
            };
        }
    }

}