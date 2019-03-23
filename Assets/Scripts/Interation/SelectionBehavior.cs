using UnityEngine;

namespace CAVS.ProjectOrganizer.Interation
{
	public class SelectionBehavior : MonoBehaviour, ISelectable
	{
        public GameObject prefab;
        private GameObject ghostClone = null;
		public GameObject controller;
		private Transform offsetTransform;

		public void Select(GameObject caller)
		{
            //Clone the selected object
            //Place the clone next to the controller, but lock its rotation
            offsetTransform = controller.transform;
			Vector3 newpos = offsetTransform.position;
            newpos.x += 5.0f;
            offsetTransform.position = newpos;            

            ghostClone = Instantiate (prefab, offsetTransform.position, this.transform.rotation, controller.transform);


		}//end of Select()

		public void UnSelect(GameObject caller)
		{

		}//end of Unselect()


		//The ghost object should always keep the same orientation as the selected Object
		void LateUpdate()
		{
			if(ghostClone!=null) ghostClone.transform.rotation = this.transform.rotation;
		}
	}



}