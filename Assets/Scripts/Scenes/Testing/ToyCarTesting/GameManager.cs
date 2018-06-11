using UnityEngine;
using CAVS.ProjectOrganizer.Project;

using CAVS.ProjectOrganizer.Scenes.Showcase;

namespace CAVS.ProjectOrganizer.Scenes.Testing.ToyCarTesting
{

    public class GameManager : MonoBehaviour
    {

        private float width = 14;

        // Use this for initialization
        void Start()
        {
            var cars = ProjectFactory.BuildItemsFromCSV("CarData.csv");
            for (int i = 0; i < cars.Length; i ++)
            {
                CarFactory.MakeToyCar(cars[i], (Vector3.right * (i% width)) + Vector3.forward * 2 * Mathf.Floor(i/ width), Quaternion.identity);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}