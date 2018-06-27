using UnityEngine;

namespace CAVS.ProjectOrganizer.Netowrking
{
    public struct NetworkedObject 
    {

        private Vector3 position;

        private Vector3 rotation;

        private string id;

        public NetworkedObject(string id, Vector3 position, Vector3 rotation)
        {
            this.id = id;
            this.position = position;
            this.rotation = rotation;
        }

        public string GetId()
        {
            return id;
        }

        public Vector3 GetRotation()
        {
            return rotation;
        }

        public Vector3 GetPosition()
        {
            return position;
        }

        public override string ToString()
        {
            return string.Format("{0}: pos{1} rot{2}", id, position, rotation);
        }
    }

}