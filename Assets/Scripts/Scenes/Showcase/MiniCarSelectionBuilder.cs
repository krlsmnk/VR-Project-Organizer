using UnityEngine;

using CAVS.ProjectOrganizer.Project;

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

        Item[] cars;

        public MiniCarSelectionBuilder() {
            cars = new Item[0];
        }

        public MiniCarSelectionBuilder SetCars(Item[] cars){
            this.cars = cars;
            return this;
        }

        public MiniCarSelectionBehavior Build(Vector3 position, Vector3 rotation) {
            var instance = GameObject.Instantiate(GetReference(), position, Quaternion.Euler(rotation));
            if(cars.Length <= 25) {
                GameObject.Destroy(instance.transform.Find("Canvas").gameObject);
            }

            var behavior = instance.GetComponent<MiniCarSelectionBehavior>() ;
            behavior.SetCars(cars);
            return behavior;
        }
    }

}