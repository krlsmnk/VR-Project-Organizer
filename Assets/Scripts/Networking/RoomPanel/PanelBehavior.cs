using EliCDavis.Prosign;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace CAVS.ProjectOrganizer.Netowrking.RoomPanel
{

    public class PanelBehavior : MonoBehaviour
    {

        [SerializeField]
        private GameObject roomUIEntry;

        private GameObject content;

        private List<Room> rooms;

        private bool updated;

        private bool joined;

        private Server server;

        public void Initialize(Server server)
        {
            if (server == null)
            {
                throw new System.Exception("Can not initialize room pannel with null server");
            }
            joined = false;
            this.server = server;
            content = transform.Find("Panel/Scroll View/Viewport/Content").gameObject;
            StartCoroutine(PollRoomListing(server));
        }

        private void Update()
        {
            if (joined)
            {
                Destroy(gameObject);
            }

            if (updated == false)
            {
                return;
            }
            updated = false;


            for (int i = content.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = content.transform.GetChild(i);
                child.SetParent(null);
                Destroy(child.gameObject);
            }

            // Add extra information about car..
            foreach (var entry in rooms)
            {
                GameObject uiEntry = Instantiate(roomUIEntry, content.transform);
                uiEntry.transform.Find("Value").GetComponent<Text>().text = entry.GetName();
                uiEntry.GetComponentInChildren<Button>().onClick = BuildOnClickCallback(entry);
            }
        }

        private void OnRoomJoined()
        {
            joined = true;
        }

        private void OnRoomCreated(string roomID)
        {
            Debug.Log(roomID);
            joined = true;
        }

        private Button.ButtonClickedEvent BuildOnClickCallback(Room r)
        {
            Button.ButtonClickedEvent e = new Button.ButtonClickedEvent();
            e.AddListener(delegate ()
            {
                server.JoinRoom(r.GetID(), OnRoomJoined);
            });
            return e;
        }

        private void OnRoomListUpdate(List<Room> rooms)
        {
            this.rooms = rooms;
            updated = true;
        }

        IEnumerator PollRoomListing(Server server)
        {
            while (true)
            {
                server.GetRooms(OnRoomListUpdate);
                yield return new WaitForSeconds(10);
            }
        }

        public void CreateRoom()
        {
            server.CreateRoom("Showcase", OnRoomCreated);
        }

    }

}