using System;
using System.Collections.Generic;
using UnityEngine;

namespace CAVS.ProjectOrganizer.Scenes.Showcase
{
    public class Pedistal : MonoBehaviour
    {

        List<Action<string>> subscribers;

        void Awake()
        {
            subscribers = new List<Action<string>>();
        }

        public void Subscribe(Action<string> cb)
        {
            if(cb != null)
            {
                subscribers.Add(cb);
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            foreach (var cb in subscribers)
            {
                cb(collision.transform.name);
            }
        }
    }

}