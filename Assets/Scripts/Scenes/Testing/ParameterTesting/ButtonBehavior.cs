using System;

using UnityEngine;
using UnityEngine.UI;


namespace CAVS.ProjectOrganizer.Scenes.Testing.ParameterTesting
{

    public class ButtonBehavior : MonoBehaviour
    {

        Action<ButtonBehavior> onSelected;

        Action<ButtonBehavior> onUnselected;

        public void OnSelected(Action<ButtonBehavior> onSelected)
        {
            this.onSelected = onSelected;
        }

        public void OnUnselected(Action<ButtonBehavior> onUnselected)
        {
            this.onUnselected = onUnselected;
        }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            GetComponent<Image>().color = Color.cyan;
            if (onSelected != null)
            {
                onSelected(this);
            }
        }


        void OnTriggerExit(Collider other)
        {
            GetComponent<Image>().color = Color.white;
            if (onUnselected != null)
            {
                onUnselected(this);
            }
        }


    }

}