using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CAVS.Anvel;
using CAVS.Anvel.Lidar;
using CAVS.Anvel.Vehicle;


namespace CAVS.ProjectOrganizer.Scenes.Showcase
{
    public class AnvelBehavior : MonoBehaviour
    {

        enum DisplayType
        {
            Networked,
            File
        }

        [SerializeField]
        private DisplayType startup;

        [SerializeField]
        private string anvelLidarSensorName;

        [SerializeField]
        private string anvelVehicleName;

        [SerializeField]
        private string anvelIP;

        [SerializeField]
        private int anvelPort;

        // Use this for initialization
        void Start()
        {
            switch (startup)
            {
                case DisplayType.File:
                    gameObject.AddComponent<FileDisplayBehavior>().Initialize(
                        LidarSerialization.Load("360 Lidar-11.pcrp"),
                        VehicleLoader.LoadVehicleData("vehicle1_pos_2.vprp")
                    );
                    break;

                case DisplayType.Networked:
                    gameObject.AddComponent<LiveDisplayBehavior>().Initialize(
                        ConnectionFactory.CreateConnection(),
                        anvelLidarSensorName,
                        anvelVehicleName,
                        new Vector3(0.1f, 0.1f, 2.27f),
                        new Vector3(0, 90, 0)
                     );
                    break;
            }

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}