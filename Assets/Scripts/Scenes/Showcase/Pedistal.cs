using System;
using System.Collections.Generic;
using UnityEngine;

namespace CAVS.ProjectOrganizer.Scenes.Showcase
{
    public class Pedistal : MonoBehaviour
    {

        void OnCollisionEnter(Collision collision)
        {
            // ID: 1  - Lexus CT 200h - 4dr Hatchback highdef
            // ID: 44 - Lexus GS GS 300 - Sedan highdef
            // ID: 68 - Lexus GS GS 350 - Sedan highdef (Made up)
            // ID: 4  - Lexus CT 200h Premium - 4dr Hatchback (Made up)
            int j;
            if (int.TryParse(collision.transform.name, out j))
            {
                int displayIndex = Mathf.Clamp(j - 1, 0, CarManager.Instance().GarageSize());
                CarManager.Instance().SetMainCar(CarManager.Instance().Garage()[displayIndex]);
            }
        }
    }

}