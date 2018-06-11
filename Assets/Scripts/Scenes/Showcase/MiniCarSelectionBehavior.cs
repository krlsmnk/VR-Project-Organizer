using UnityEngine;
using UnityEngine.UI;

using CAVS.ProjectOrganizer.Project;

namespace CAVS.ProjectOrganizer.Scenes.Showcase
{

    public class MiniCarSelectionBehavior : MonoBehaviour
    {
        [SerializeField]
        private Text pageDisplay;

        private Item[] allCars;

        private GameObject[] carsBeingRendered;

        private int width = 5;

        private int height = 5;

        private int numberOfPages = 0;

        private int currentPage = 0;

        public void SetCars(Item[] cars)
        {
            if (cars == null)
            {
                throw new System.Exception("Trying to set cars to null");
            }
            carsBeingRendered = new GameObject[width * height];
            currentPage = 0;
            numberOfPages = Mathf.CeilToInt( ((float)cars.Length) / (width * height));
            allCars = cars;
            RenderPage();
        }

        private void ClearCurrentCarsBeingRendered()
        {
            for(var i = 0; i < carsBeingRendered.Length; i ++)
            {
                if (carsBeingRendered[i] != null)
                {
                    Destroy(carsBeingRendered[i]);
                }
            }
        }

        private void RenderPage()
        {
            ClearCurrentCarsBeingRendered();

            int itemsPerPage = width * height;
            int startingIndex = itemsPerPage * currentPage;
            for(int i = startingIndex; i < startingIndex + itemsPerPage && i < allCars.Length; i ++)
            {
                int flatIndex = i % itemsPerPage;

                Vector3 position = (Vector3.right * (i % width)*.3f) + (Vector3.forward * .35f * Mathf.Floor(flatIndex / width)) + transform.position;

                position += (Vector3.up * .25f);
                position += (Vector3.left * .6f);
                position += (Vector3.back * .7f);

                carsBeingRendered[flatIndex] = CarFactory.MakeToyCar(allCars[i], position, Quaternion.identity);
            }
            pageDisplay.text = string.Format("{0} / {1}", currentPage + 1, numberOfPages);
        }

        public void NextPage()
        {
            currentPage = Mathf.Min(currentPage + 1, numberOfPages -1);
            RenderPage();
        }

        public void PreviousPage()
        {
            currentPage = Mathf.Max(currentPage - 1, 0);
            RenderPage();
        }

    }

}