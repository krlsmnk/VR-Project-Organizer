using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


using UnityEngine;

using EliCDavis.Prosign;

namespace EliCDavis.Examples.Messaging
{

    public class Test : MonoBehaviour
    {

        enum TestState
        {
            Lobby,
            InRoom
        }

        private List<string> messages;

        private Prosign.Server hotel;

        private string roomId;

        [SerializeField]
        private string prosignServer;

        [SerializeField]
        private int prosignPort;

        void Start()
        {
            messages = new List<string>();
            roomId = "";
            hotel = new Prosign.Server(prosignServer, prosignPort);
        }

        void OnRoomCreated(string roomId)
        {
            Debug.Log(string.Format("Room Id {0}", roomId));
            hotel.SubscribeToRoomUpdates(OnRoomUpdate);
            this.roomId = roomId;
            messages.Add(string.Format("< Created Room {0} >", roomId));
        }

        void OnRoomJoin()
        {
            Debug.Log(string.Format("Room Joined"));
            hotel.SubscribeToRoomUpdates(OnRoomUpdate);
            this.roomId = "joined";
            messages.Add("< Joined Room >");
        }

        void OnRoomUpdate(byte[] update)
        {
            Debug.Log("update " + update);
            messages.Add(string.Format("{0}: {1}", DateTime.Now.ToShortTimeString(), update));
        }

        private void RoomGUIUpdate()
        {
            if (GUI.Button(new Rect(20, 20, 100, 20), "Ping"))
            {
                hotel.UpdateRoom(Encoding.UTF8.GetBytes("Ping"));
                messages.Add("< Sent Ping >");
            }

            for (int i = 0; i < messages.Count; i++)
            {
                GUI.Label(new Rect(20, 50 + (i * 30), Screen.width - 40, 25), messages[i]);
            }
        }

        private string roomIdToConnectTo = "";

        private void LobbyGUIUpdate()
        {
            if (GUI.Button(new Rect(20, 20, 100, 20), "New Room"))
            {
                hotel.CreateRoom("Unity", OnRoomCreated);
            }

            if (GUI.Button(new Rect(20, 60, 100, 20), "Join Room"))
            {
                hotel.JoinRoom(roomIdToConnectTo, OnRoomJoin);
            }

            roomIdToConnectTo = GUI.TextField(new Rect(130, 60, 100, 20), roomIdToConnectTo);
        }

        void OnGUI()
        {
            switch (CurrentState())
            {
                case TestState.InRoom:
                    RoomGUIUpdate();
                    break;

                case TestState.Lobby:
                    LobbyGUIUpdate();
                    break;
            }
        }

        TestState CurrentState()
        {
            return roomId == "" ? TestState.Lobby : TestState.InRoom;
        }

        void OnDestroy()
        {
            hotel.Close();
        }
    }

}
