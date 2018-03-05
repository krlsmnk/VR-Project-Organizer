using System;
using System.Collections.Generic;
using UnityEngine;
using CAVS.ProjectOrganizer.Project;


namespace CAVS.ProjectOrganizer.Project
{

    public class ItemBehaviour : MonoBehaviour
    {

        private List<Action<Item, Collider>> onExamineCallbacks;

        private Item item;

        public void SetItem(Item item)
        {
            this.item = item;
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
        }

        public Item ToItem()
        {
            return null;
        }

        protected virtual void OnExamineStart()
        {
            Debug.Log("Examine Started");
        }

        protected virtual void OnExamineStop()
        {
            Debug.Log("Examine Stopped");
        }

        public void AddExamineEvent(Action<Item, Collider> callback)
        {
            if (callback == null)
            {
                return;
            }
            if (onExamineCallbacks == null)
            {
                onExamineCallbacks = new List<Action<Item, Collider>>();
            }
            onExamineCallbacks.Add(callback);
        }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            if (onExamineCallbacks == null)
            {
                return;
            }
            foreach (Action<Item, Collider> callback in onExamineCallbacks)
            {
                if (callback != null)
                {
                    callback(this.item, other);
                }
            }
        }

    }

}