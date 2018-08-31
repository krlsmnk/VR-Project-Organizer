using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using EliCDavis.Prosign;

namespace EliCDavis.Examples.Positions
{

    public class GameManager : MonoBehaviour
    {

        enum TestState
        {
            Lobby,
            InRoom
        }

        private float updateRate = .1f;

        private List<string> messages;

        private Server hotel;

        private string roomId;

        [SerializeField]
        private string prosignServer;

        [SerializeField]
        private int prosignPort;

        [SerializeField]
        private Transform transformToSync;

        private Transform puppet;

        private float lastPuppetUpdate;

        private Vector3 lastPuppetPosition;

        private Vector3 lastPuppetRotation;

        private Vector3 newPuppetPosition;

        private Vector3 newPuppetRotation;

        private List<Room> rooms;

        void Start()
        {
            lastPuppetUpdate = 0;
            lastPuppetPosition = Vector3.zero;
            newPuppetPosition = Vector3.zero;
            lastPuppetRotation = Vector3.zero;
            newPuppetRotation = Vector3.zero;

            puppet = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
            messages = new List<string>();
            roomId = "";
            hotel = new Server(prosignServer, prosignPort);
            StartCoroutine(PollRoomListing());
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
            roomId = roomIdToConnectTo;
            messages.Add("< Joined Room >");
        }

        /// <summary>
        /// Some things can't be called out of the main thread (Time.time, transform.poistion, etc)
        /// To signal we got changes from the socket, we flip a boolean. This allows elsewhere to grab
        /// those values for us and do things appropriatly.
        /// </summary>
        bool flipped = false;

        void OnRoomUpdate(byte[] update)
        {
            messages.Add(string.Format("{0}: {1}", DateTime.Now.ToShortTimeString(), update));
            if (update.Length >= 24)
            {
                lastPuppetPosition = newPuppetPosition;
                lastPuppetRotation = newPuppetRotation;
                newPuppetPosition = new Vector3(
                    BitConverter.ToSingle(update, 0),
                    BitConverter.ToSingle(update, 4),
                    BitConverter.ToSingle(update, 8)
                );
                newPuppetRotation = new Vector3(
                    BitConverter.ToSingle(update, 12),
                    BitConverter.ToSingle(update, 16),
                    BitConverter.ToSingle(update, 20)
                );
                flipped = true;
            }
            else
            {
                Debug.Log(string.Format("Bad Update: {0} bytes", update.Length));
            }
        }

        private void RoomGUIUpdate()
        {
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

            if (GUI.Button(new Rect(20, 50, 100, 20), "Join Room"))
            {
                hotel.JoinRoom(roomIdToConnectTo, OnRoomJoin);
            }

            if(rooms == null)
            {
                return;
            }

            for (int i = 0; i < rooms.Count; i++)
            {
                if (GUI.Button(new Rect(20, 80 + (i*30), 100, 20), rooms[i].GetName()))
                {
                    hotel.JoinRoom(rooms[i].GetID(), OnRoomJoin);
                    roomIdToConnectTo = rooms[i].GetID();
                }
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


        bool startedCoroutine = false;

        void Update() {
            if(flipped) {
                lastPuppetUpdate = Time.time;
                flipped = false;
            }

            float percentThroughAnimation = (Time.time - lastPuppetUpdate) / updateRate;
            puppet.position = Vector3.Lerp(lastPuppetPosition, newPuppetPosition, percentThroughAnimation);
            puppet.rotation = Quaternion.Lerp(Quaternion.Euler(lastPuppetRotation), Quaternion.Euler(newPuppetRotation), percentThroughAnimation);

            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * Time.deltaTime * 5f;
            transformToSync.Translate(movement);

            if (!startedCoroutine && CurrentState() == TestState.InRoom){
                StartCoroutine(UpdatePosition());
                startedCoroutine = true;
            }
        }

        private void OnRoomListingUpdate(List<Room> rooms)
        {
            this.rooms = rooms;
        }

        IEnumerator PollRoomListing()
        {
            while (CurrentState() == TestState.Lobby)
            {
                hotel.GetRooms(OnRoomListingUpdate);
                yield return new WaitForSeconds(1);
            }
        }

            IEnumerator UpdatePosition()
        {
            while (CurrentState() == TestState.InRoom)
            {
                var buffer = new List<byte>();
                buffer.AddRange(BitConverter.GetBytes(transformToSync.position.x));
                buffer.AddRange(BitConverter.GetBytes(transformToSync.position.y));
                buffer.AddRange(BitConverter.GetBytes(transformToSync.position.z));
                buffer.AddRange(BitConverter.GetBytes(transformToSync.rotation.eulerAngles.x));
                buffer.AddRange(BitConverter.GetBytes(transformToSync.rotation.eulerAngles.y));
                buffer.AddRange(BitConverter.GetBytes(transformToSync.rotation.eulerAngles.z));

                hotel.UpdateRoom(buffer.ToArray());
                yield return new WaitForSeconds(updateRate);
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
