using System;
using VRTK;

namespace CAVS.ProjectOrganizer.Controls
{

    public class SelectPlayerControl : PlayerControl
    {
        public override Action Build(VRTK_ControllerEvents hand)
        {
            var behavior = SelectBehavior.Initialize(hand);

            return delegate ()
            {
                UnityEngine.Object.Destroy(behavior);
            };

        }

    }

}