using UnityEngine;
using EliCDavis.Prosign;

namespace CAVS.ProjectOrganizer.Netowrking.RoomPanel
{
    public static class Builder
    {

        private static GameObject panelReference = null;
        private static GameObject GetPanelReference()
        {
            if (panelReference == null)
            {
                panelReference = Resources.Load<GameObject>("Networking/Room Panel");
            }
            return panelReference;
        }

        public static GameObject Build(Vector3 position, Quaternion rotation, Server server)
        {
            var instance = Object.Instantiate(GetPanelReference());
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.GetComponent<PanelBehavior>().Initialize(server);
            return instance;
        }
        
    }

}