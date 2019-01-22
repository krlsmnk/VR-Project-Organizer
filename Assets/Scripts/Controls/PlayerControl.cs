using VRTK;
using System;
using UnityEngine;

namespace CAVS.ProjectOrganizer.Controls
{
    public abstract class PlayerControl
    {

        public abstract Action Build(VRTK_ControllerEvents hand);

        public abstract Texture2D GetIcon();

    }

}