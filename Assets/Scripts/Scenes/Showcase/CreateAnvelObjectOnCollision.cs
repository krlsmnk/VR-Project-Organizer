using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnvelApi;
using CAVS.Anvel;
namespace CAVS.ProjectOrganizer.Scenes.Showcase
{
    public class CreateAnvelObjectOnCollision : MonoBehaviour
    {

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.transform.name == "Big Car")
            {
                var connection = ConnectionFactory.CreateConnection(new ClientConnectionToken());
                string name = "Some Lidar - " + Random.Range(0, 10000);
                AnvelObject.CreateObject(connection, name, "API 3D Lidar");
                Object.FindObjectsOfType<LiveDisplayBehavior>()[0].AddLidar(name, Color.blue);
                Destroy(gameObject);
            }
        }
    }

}