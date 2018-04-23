using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MerBehavior : MonoBehaviour
{

    List<GameObject> insideMe;

    List<Action<List<GameObject>>> callbacks;

    void Awake()
    {
        insideMe = new List<GameObject>();
        callbacks = new List<Action<List<GameObject>>>();
        Debug.Log("awoke...");

    }

    void Start()
    {
        Debug.Log("started...");
    }

    public void SubscibeToInside(Action<List<GameObject>> cb)
    {
        if (cb == null)
        {
            return;
        }
        callbacks.Add(cb);
    }

    private void UpdateThoseSubscribed()
    {
        foreach (var cb in callbacks)
        {
            cb(insideMe);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger");
        if (other == null || other.gameObject == null || insideMe.Contains(other.gameObject))
        {
            return;
        }
        insideMe.Add(other.gameObject);
        UpdateThoseSubscribed();
    }

    void OnTriggerExit(Collider other)
    {
        if (other == null || other.gameObject == null || !insideMe.Contains(other.gameObject))
        {
            return;
        }
        insideMe.Remove(other.gameObject);
        UpdateThoseSubscribed();
    }

}

