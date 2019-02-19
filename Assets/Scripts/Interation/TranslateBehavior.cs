using UnityEngine;

namespace CAVS.ProjectOrganizer.Interation
{
    public class TranslateBehavior : MonoBehaviour, ISelectable
    {
        private GameObject moveArrows = null;

        public void Select(GameObject caller)
        {

            if (moveArrows != null)
            {
                Destroy(moveArrows);
            }
            else
            {
                moveArrows = Instantiate(Resources.Load<GameObject>("Move Arrows"));
                foreach (var arrow in moveArrows.GetComponentsInChildren<MoveArrowsInteraction>())
                {
                    arrow.SetObjectToControl(gameObject);
                }
                moveArrows.transform.position = transform.position;

            }

        }

        public void UnSelect(GameObject caller)
        {

        }
    }

}