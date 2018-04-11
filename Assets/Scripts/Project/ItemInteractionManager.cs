using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAVS.ProjectOrganizer.Project
{

    public class ItemInteractionManager
    {

        private Item lastItemInteractedWith;

        private List<Action<Item>> itemInteractionSubscribers;

        private static ItemInteractionManager instance = null;

        public static ItemInteractionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ItemInteractionManager();
                }
                return instance;
            }
        }

        private ItemInteractionManager()
        {
            lastItemInteractedWith = null;
            itemInteractionSubscribers = new List<Action<Item>>();
        }

        public void UpdateLastNodeInteractedWith(Item item)
        {
            if (item == null)
            {
                return;
            }
            lastItemInteractedWith = item;
            foreach (var callback in itemInteractionSubscribers)
            {
                callback(lastItemInteractedWith);
            }
        }

        public void SubscribeToLatestInteractedItem(Action<Item> callback)
        {
            if (callback == null)
            {
                return;
            }
            itemInteractionSubscribers.Add(callback);
            callback(lastItemInteractedWith);
        }

    }

}