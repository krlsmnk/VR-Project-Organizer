using System;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

namespace CAVS.ProjectOrganizer.Project
{
    [RequireComponent(typeof(VRTK_InteractableObject))]
    public class ItemBehaviour : MonoBehaviour
    {
        VRTK_InteractableObject interactableObject;

        private List<Action<Item, Collider>> onExamineCallbacks;

        private Item item;

        /// <summary>
        /// When we shrink an item whenever a user grabs it, keep the original
        /// size values to appropriately resize when the user is done with it.
        /// </summary>
        private Vector3 originalSize;

        void Start()
        {
            interactableObject = GetComponent<VRTK_InteractableObject>();
            interactableObject.InteractableObjectGrabbed += OnGrab;
            interactableObject.InteractableObjectUngrabbed += OnUnGrab;
        }

        public void SetItem(Item item)
        {
            this.item = item;
        }

        public Item ToItem()
        {
            return item;
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

        private void OnGrab(object sender, InteractableObjectEventArgs e)
        {
            Debug.Log("Grabbed");
            originalSize = transform.localScale;
            transform.localScale *= .5f;
            transform.eulerAngles = Vector3.zero;
        }

        private void OnUnGrab(object sender, InteractableObjectEventArgs e)
        {
            transform.localScale = originalSize;
        }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            OnExamineStart();
            ItemInteractionManager.Instance.UpdateLastNodeInteractedWith(item);
            if (onExamineCallbacks == null)
            {
                return;
            }
            foreach (Action<Item, Collider> callback in onExamineCallbacks)
            {
                if (callback != null)
                {
                    callback(item, other);
                }
            }
        }

    }

}