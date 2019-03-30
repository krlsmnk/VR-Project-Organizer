 using UnityEngine;
 using System.Collections;
 using System;
 using UnityEngine.Events;
 using UnityEngine.EventSystems;
 
 
 public class MyClickTrigger : MonoBehaviour , IPointerClickHandler
 {
     #region IPointerClickHandler implementation
 
     public void OnPointerClick (PointerEventData eventData)
     {
         MyOwnEventTriggered ();
     }
 
     #endregion
 
     //my event
     [Serializable]
     public class MyOwnEvent : UnityEvent { }
 
     [SerializeField]
     private MyOwnEvent myOwnEvent = new MyOwnEvent();
     public MyOwnEvent onMyOwnEvent { get { return myOwnEvent; } set { myOwnEvent = value; } }
 
     public void MyOwnEventTriggered()
     {
         onMyOwnEvent.Invoke();

        Debug.Log("MyOwnEventTriggered");
     }
 
 }