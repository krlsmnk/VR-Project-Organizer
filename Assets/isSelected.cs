namespace VRTK
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.EventSystems;

	public class isSelected : MonoBehaviour,  IPointerClickHandler{

		public bool isSelectedBool;
		private isSelected[] allSelectables;

		// Use this for initialization
		void Start () {
			isSelectedBool = false;
			allSelectables = GameObject.FindObjectsOfType<isSelected> ();
		}
		
		// Update is called once per frame
		void Update () {
		}

		public void OnPointerClick (PointerEventData eventData){
			//make every possible inputField not selected
			foreach(isSelected item in allSelectables){
				item.setBool (false);				
			}
			//make current one selected
			isSelectedBool = true;
			Debug.Log (this.name + " is selected = true");
		}

		void OnTriggerEnter(Collider collider)
		{
			var pointerCheck = collider.GetComponentInParent<VRTK_UIPointer>();
			if (pointerCheck) {
				isSelectedBool = true;
				Debug.Log (this.name + " is selected = true");
			}
		}

		public void setBool (bool setThis){
			isSelectedBool = setThis;
			Debug.Log (this.name + " is selected = " + setThis);
			
		}



	}//end of class
}//end of namespace