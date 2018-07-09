using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAVS.ProjectOrganizer.Netowrking
{

    public class RoomDisplayBehavior : MonoBehaviour
    {
        [SerializeField]
        private GameObject puppetRepresentation;

        private Dictionary<string, Transform> puppets;

        private Dictionary<string, Vector3> puppetsDesiredPosition;

        private Dictionary<string, Vector3> puppetsDesiredRotation;

        private float timeLastUpdated;

        private float durationBetweenLastTwoUpdates;

        // Use this for initialization
        void Awake()
        {
            timeLastUpdated = Time.time;
            durationBetweenLastTwoUpdates = Time.deltaTime;
            puppets = new Dictionary<string, Transform>();
            puppetsDesiredPosition = new Dictionary<string, Vector3>();
            puppetsDesiredRotation = new Dictionary<string, Vector3>();
        }

        public void UpdatePuppets(IEnumerable<NetworkedObject> incomingPuppets)
        {
            if (incomingPuppets == null)
            {
                throw new System.Exception("Argument can not be null");
            }

            durationBetweenLastTwoUpdates = Time.time - timeLastUpdated;
            timeLastUpdated = Time.time;

            List<string> puppetsUpdated = new List<string>();
            foreach (var puppet in incomingPuppets)
            {
                if (puppets.ContainsKey(puppet.GetId()))
                {
                    UpdatePuppet(puppet);
                } else
                {
                    AddPuppetEntry(puppet);
                }
                puppetsUpdated.Add(puppet.GetId());
            }

            foreach (var keyValPair in puppets)
            {
                if(!puppetsUpdated.Contains(keyValPair.Key))
                {
                    RemovePuppetEntry(keyValPair.Key);
                }
            }
        }

        private void RemovePuppetEntry(string id)
        {
            Destroy(puppets[id].gameObject);
            puppets.Remove(id);
            puppetsDesiredPosition.Remove(id);
            puppetsDesiredRotation.Remove(id);
        }

        private void UpdatePuppet(NetworkedObject puppet)
        {
            puppetsDesiredPosition[puppet.GetId()] = puppet.GetPosition();
            puppetsDesiredRotation[puppet.GetId()] = puppet.GetRotation();
        }

        private void AddPuppetEntry(NetworkedObject puppet)
        {
            GameObject newPuppet = Instantiate(puppetRepresentation);
            newPuppet.transform.name = string.Format("Puppet: {0}", puppet.GetId());
            newPuppet.transform.position = puppet.GetPosition();
            newPuppet.transform.rotation = Quaternion.Euler(puppet.GetRotation());
            puppets.Add(puppet.GetId(), newPuppet.transform);
            puppetsDesiredPosition.Add(puppet.GetId(), puppet.GetPosition());
            puppetsDesiredRotation.Add(puppet.GetId(), puppet.GetRotation());
        }

        // Update is called once per frame
        void Update()
        {
            float percentThroughLerp = (Time.time - timeLastUpdated) / durationBetweenLastTwoUpdates;
            foreach (var keyValPair in puppets)
            {
                keyValPair.Value.position = Vector3.Lerp(keyValPair.Value.position, puppetsDesiredPosition[keyValPair.Key], percentThroughLerp);
                keyValPair.Value.rotation = Quaternion.Euler(Vector3.Lerp(keyValPair.Value.rotation.eulerAngles, puppetsDesiredRotation[keyValPair.Key], percentThroughLerp));
            }
        }
    }

}