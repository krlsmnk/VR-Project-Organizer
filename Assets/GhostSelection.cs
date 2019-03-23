using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTK.Examples
{ 
public class GhostSelection : VRTK_InteractableObject
{
        private Transform originalItemTransform, offsetTransform;
        private GameObject ghostClone = null;

        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            base.StartUsing(usingObject);
                 
            originalItemTransform = this.gameObject.transform;

            //Clone the selected object, place it with some offset to the original
            offsetTransform = originalItemTransform;
            Vector3 newpos = offsetTransform.position;
            newpos.x += 5.0f;
            offsetTransform.position = newpos;

            ghostClone = Instantiate(this.gameObject, offsetTransform.position, offsetTransform.rotation);            
            
            //Later:
            //Place the clone next to the controller, but lock its rotation
                //offsetTransform = controller.transform;

        }

        public override void StopUsing(VRTK_InteractUse usingObject)
        {
            base.StopUsing(usingObject);
            
        }

        protected void Start()
        {
       
        }

        protected override void Update()
        {
            base.Update();
            //rotator.transform.Rotate(new Vector3(spinSpeed * Time.deltaTime, 0f, 0f));
        }
}

}