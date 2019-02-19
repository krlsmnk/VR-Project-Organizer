using System.Collections.Generic;
using UnityEngine;
using System;
using VRTK;

namespace CAVS.ProjectOrganizer.Controls
{

    public class ControllerConfig
    {
        private readonly List<PlayerControl> controls;

        public ControllerConfig()
        {
            controls = new List<PlayerControl>();
        }

        public ControllerConfig(List<PlayerControl> controls)
        {
            if (controls == null)
            {
                throw new ArgumentException("Controls can not be null");
            }
            this.controls = controls;
        }

        public ControllerConfig AddWieldable(PlayerControl wieldable)
        {
            List<PlayerControl> newWieldables = new List<PlayerControl>(controls)
            {
                wieldable
            };
            return new ControllerConfig(newWieldables);
        }

        public bool HasWieldable(PlayerControl wieldable)
        {
            return controls.Contains(wieldable);
        }

        public void Build(VRTK_ControllerEvents hand)
        {
            if (hand == null)
            {
                throw new Exception("VRTK Hand can not be null when building!");
            }

            var radial = UnityEngine.Object.Instantiate(Resources.Load("PlayerControl/RadialMenu") as GameObject, hand.transform);
            radial.transform.localPosition = new Vector3(0, .2f, 0);

            PlayerControlBehavior.Initialize(hand, controls);
        }

    }

}