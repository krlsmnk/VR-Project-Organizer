using VRTK;
using System;
using UnityEngine;

namespace CAVS.ProjectOrganizer.Controls
{
    public abstract class PlayerControl : MonoBehaviour
    {

        void Awake()
        {
            VRTK_SDKManager.instance.AddBehaviourToToggleOnLoadedSetupChange(this);
        }
        void OnDestroy()
        {
            VRTK_SDKManager.instance.RemoveBehaviourToToggleOnLoadedSetupChange(this);
        }

        public abstract Action Build(VRTK_ControllerEvents hand);

        public abstract Texture2D GetIcon();

    }

}