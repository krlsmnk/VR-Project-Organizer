using VRTK;
using System;
using UnityEngine;

namespace CAVS.ProjectOrganizer.Scenes.Showcase
{
    public abstract class PlayerControl
    {

        public abstract Action Build(VRTK_ControllerEvents hand);

        public Texture2D GetIcon()
        {
            return Resources.Load<Texture2D>("PlayerControl/teleport-icon");
        }

    }

}