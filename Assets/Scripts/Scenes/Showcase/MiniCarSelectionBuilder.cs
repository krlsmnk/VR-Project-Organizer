using UnityEngine;

namespace CAVS.ProjectOrganizer.Scenes.Showcase
{

    class MiniCarSelectionBuilder {

        private static GameObject reference;
        private static GameObject GetReference() {
            if (reference == null) {
                reference = Resources.Load<GameObject>("Mini Car Selection Tabletop");
            }
            return reference;
        }

        CarManager carManager;

        public MiniCarSelectionBuilder(CarManager carManager) {
            this.carManager = carManager;
        }


        public MiniCarSelectionBehavior Build(Vector3 position, Vector3 rotation) {
            var instance = Object.Instantiate(GetReference(), position, Quaternion.Euler(rotation));
            if(carManager.Garage().Length <= 25) {
                Object.Destroy(instance.transform.Find("Canvas").gameObject);
            }

            var behavior = instance.GetComponent<MiniCarSelectionBehavior>() ;
            behavior.SetCars(carManager);
            return behavior;
        }
    }

}