using CAVS.ProjectOrganizer.Controls;
using System;
using UnityEngine;
using VRTK;

namespace CAVS.ProjectOrganizer.Scenes.Showcase
{
    internal class DummyControl : PlayerControl
    {
          public override Action Build(VRTK_ControllerEvents hand)
        {        

            return delegate ()
            {
            };
        }

         public override Texture2D GetIcon()
        {
            return Resources.Load<Texture2D>("PlayerControl/placeholder");
        }

    }
}