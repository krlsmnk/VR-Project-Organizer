namespace VRTK
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.EventSystems;

	public class IsSelected : MonoBehaviour,  IPointerClickHandler{

		public bool isSelectedBool;
		private IsSelected[] allSelectables;

		// Use this for initialization
		void Start () {
			isSelectedBool = false;
			allSelectables = GameObject.FindObjectsOfType<IsSelected> ();
		}
		
		// Update is called once per frame
		void Update () {
		}

		public void OnPointerClick (PointerEventData eventData){
			//make every possible inputField not selected
			foreach(IsSelected item in allSelectables){
				item.SetBool (false);				
			}
			//make current one selected
			isSelectedBool = true;
		}

		void OnTriggerEnter(Collider collider)
		{
			var pointerCheck = collider.GetComponentInParent<VRTK_UIPointer>();
			if (pointerCheck) {
				isSelectedBool = true;
			}
		}

		public void SetBool (bool setThis){
			isSelectedBool = setThis;
		}

	}

}