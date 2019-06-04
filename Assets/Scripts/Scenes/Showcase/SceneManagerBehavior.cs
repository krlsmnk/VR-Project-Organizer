using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using CAVS.ProjectOrganizer.Interation;
using CAVS.ProjectOrganizer.Project;
using CAVS.ProjectOrganizer.Netowrking;
using CAVS.ProjectOrganizer.Controls;

using CAVS.ProjectOrganizer.SourceControl;

using VRTK;


namespace CAVS.ProjectOrganizer.Scenes.Showcase
{
    
    /// <summary>
    /// Meant managing the entire carshowcase scene.
    /// </summary>
    public class SceneManagerBehavior : MonoBehaviour
    {
        public bool skipShowcase = false;
        public bool BezierTeleport, TVP, Select, Grab = false, allowHeightAdjustTVP;
        public Transform headsetNullfix;

        [SerializeField]
        private VRTK_ControllerEvents rightHand;

        [SerializeField]
        private VRTK_ControllerEvents leftHand;

        /// <summary>
        /// The screens we're going to render the current car to
        /// </summary>
        [SerializeField]
        private RawImage[] carImageScreens;

        /// <summary>
        /// All text displays that will list the car's name currently being displayed
        /// </summary>
        [SerializeField]
        private Text[] carNameDisplays;


        [SerializeField]
        private GameObject extraInfoContent;


        [SerializeField]
        private GameObject extraInfoContentEntry;


        [SerializeField]
        private ButtonBehavior nextButton;


        [SerializeField]
        private ButtonBehavior previousButton;


        /// <summary>
        /// Used for displaying the other players in the room
        /// </summary>
        private RoomDisplayBehavior roomDisplay;


        /// <summary>
        /// The current 3d model in the scene which is the car that players can interact with in
        /// different ways
        /// </summary>
        private GameObject currentCarGameObject;


        /// <summary>
        /// The quality we want to render the cars at, set in accordance to the device that is running
        /// the showcase.
        /// </summary>
        private CarQuality qualityToRender;


        /// <summary>
        /// The lift model that will make it look like the car is being raised
        /// </summary>
        [SerializeField]
        private GameObject liftCarPlacement;

        [SerializeField]
        private PlotControl graphControl;

        [SerializeField]
        private string roomUUID;

        [SerializeField]
        private bool createRoomOnStartup;

        private INetworkRoom sceneReference;

        GameObject player;        

        /// <summary>
        /// The socket exists outside the main thread, and therefor can't 
        /// change stuff like transform. We set this as a flag if things
        /// have changed. -1 represents nothing has changed, anything else
        /// it is the index to the car we want to display
        /// </summary>
        int carChangeFromUpdate = -1;

        private void Awake()
        {
            if(!skipShowcase)roomDisplay = gameObject.GetComponent<RoomDisplayBehavior>();
        }

        void Start()
        {
            if (!skipShowcase) { 
            var prosignServer = new ProsignAdapterNetworkRoom("videogamedev.club", 3000);
            Netowrking.RoomPanel.Builder.Build(new Vector3(3.5f, 2.1f, 5.5f), Quaternion.Euler(14.7f, 27.6f, 3.1f), prosignServer);
            if(createRoomOnStartup)
            {
                prosignServer.CreateRoom("Auto Room", delegate(string id)
                {
                    Debug.LogFormat("Room Id: {0}", id);
                });
            } else if (roomUUID != null && roomUUID != "")
            {
                prosignServer.JoinRoom(roomUUID, delegate() { });
            }            

            sceneReference = prosignServer;
            sceneReference.SubscribeToUpdates(OnSceneUpdate);

            nextButton.Subscribe(DisplayNextCar);
            previousButton.Subscribe(DisplayPreviousCar);

            CarManager
                .Instance()
                .SetGarage(ProjectFactory.BuildItemsFromCSV("Assets/Car_Dataset.csv", 7).SubArray(0, 25));

            CarManager
                .Instance()
                .OnMainCarChange += DisplayCar;

            DisplayNextCar();
            new MiniCarSelectionBuilder(CarManager.Instance())
                .Build(new Vector3(3, 0.77f, 4.5f), Vector3.zero);

            StartCoroutine(UpdatePlayerTransformOnServer());
                // graphControl.Initialize(this.PlotPointBuilder, cars);
            }

            BuildRadialConfig();
        }

        private void OnSceneUpdate(Dictionary<string, object> update)
        {
            if (!skipShowcase) { 
            if (update.ContainsKey("carUpdate"))
            {
                carChangeFromUpdate = (int)update["carUpdate"];
            }
            else
            {
                roomDisplay.UpdatePuppets(new ShowcaseData(update).UsersInScene());
            }
            }
        }


        //private GameObject PlotPointBuilder(Item item)
        //{
        //    ItemBehaviour itemObj = item.Build();
        //    itemObj.AddExamineEvent(this.OnPlotPointExamined);
        //    itemObj.GetComponent<Rigidbody>().isKinematic = true;
        //    return itemObj.gameObject;
        //}

        //private void OnPlotPointExamined(Item point, Collider collider)
        //{
        //    int id = 0;
        //    if (int.TryParse(point.GetValue("ID"), out id))
        //    {
        //        carBeingDisplayedIndex = id - 1;
        //        UpdateCar(carBeingDisplayedIndex);
        //    }
        //}

        /// <summary>
        /// Called by buttons placed in the scene
        /// </summary>
        public void OnButtonPress(string buttonName)
        {
            if (buttonName == "Next")
            {
                DisplayNextCar();
            }

            if (buttonName == "Previous")
            {
                DisplayPreviousCar();
            }
        }


        private int CarBeingDisplayedIndex()
        {
            if (CarManager.Instance().Garage() != null)
            {
                for (int i = 0; i < CarManager.Instance().Garage().Length; i++)
                {
                    if (CarManager.Instance().GetMainCar() == CarManager.Instance().Garage()[i])
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        public void DisplayPreviousCar()
        {
            var newIndex = Mathf.Clamp(CarBeingDisplayedIndex() - 1, 0, CarManager.Instance().Garage().Length);
            CarManager.Instance().SetMainCar(CarManager.Instance().Garage()[newIndex]);
        }


        public void DisplayNextCar()
        {
            var newIndex = Mathf.Clamp(CarBeingDisplayedIndex() + 1, 0, CarManager.Instance().Garage().Length);
            CarManager.Instance().SetMainCar(CarManager.Instance().Garage()[newIndex]);
        }

        private IEnumerator UpdatePlayerTransformOnServer()
        {
            while (true)
            {
                if (player == null)
                {
                    if (Camera.main == null)
                    {
                        try
                        {
                            player = VRTK_DeviceFinder.HeadsetCamera().gameObject;
                        }
                        catch (System.Exception e) { }
                    }
                    else
                    {
                        player = Camera.main.gameObject;
                    }
                }
                else if (sceneReference != null)
                {
                    var update = new NetworkUpdateBuilder()
                        .AddEntry("head-position", player.transform.position)
                        .AddEntry("head-rotation", player.transform.rotation.eulerAngles);
                        
                    if (leftHand != null)
                    {
                        update
                            .AddEntry("left-position", leftHand.transform.position)
                            .AddEntry("left-rotation", leftHand.transform.rotation.eulerAngles);
                    }

                    if (rightHand != null)
                    {
                        update
                            .AddEntry("right-position", rightHand.transform.position)
                            .AddEntry("right-rotation", rightHand.transform.rotation.eulerAngles);
                    }


                    sceneReference.Update(update.Build());
                }
                yield return new WaitForSeconds(.1f);
            }
        }

        void Update()
        {
            if (carChangeFromUpdate != -1)
            {
                CarManager.Instance().SetMainCar(CarManager.Instance().Garage()[carChangeFromUpdate]);
                carChangeFromUpdate = -1;
            }
        }

        private void UpdateCar(int index)
        {
            if (sceneReference != null)
            {
                sceneReference.Update(new NetworkUpdateBuilder()
                    .AddEntry("selectedCar", index)
                    .Build());
            }
        }

        /// <summary>
        /// Set up the entire scene to be rendering information about the specific
        /// car passed in.
        /// </summary>
        /// <param name="carToDisplay">Car to display info about</param>
        private void DisplayCar(PictureItem carToDisplay)
        {
            if(carToDisplay == null)
            {
                return;
            }

            // Update All The Screens
            if (carImageScreens != null)
            {
                foreach (RawImage screen in carImageScreens)
                {
                    screen.texture = carToDisplay.GetImage();
                }
            }

            // Update all name displays
            if (carNameDisplays != null)
            {
                foreach (Text nameDisplay in carNameDisplays)
                {
                    nameDisplay.text = carToDisplay.ToString();
                }
            }

            // Delete all past car information..
            for (int i = extraInfoContent.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = extraInfoContent.transform.GetChild(i);
                child.SetParent(null);
                Destroy(child.gameObject);
            }

            // Add extra information about car..
            foreach (KeyValuePair<string, string> entry in carToDisplay.GetValues())
            {
                GameObject uiEntry = Instantiate(extraInfoContentEntry, extraInfoContent.transform);
                uiEntry.transform.Find("DataName").GetComponent<Text>().text = entry.Key;
                uiEntry.transform.Find("Value").GetComponent<Text>().text = entry.Value;
            }

            // Delete old car model
            if (currentCarGameObject != null)
            {
                Destroy(currentCarGameObject);
            }

            currentCarGameObject = CarFactory.MakeBigCar(carToDisplay, float.Parse(carToDisplay.GetValue("id")) / (float)CarManager.Instance().GarageSize(), Vector3.zero, Quaternion.Euler(0, 90, 0));
            currentCarGameObject.transform.parent = liftCarPlacement.transform;
            currentCarGameObject.transform.localPosition = Vector3.zero;
        }


        private void OnApplicationQuit()
        {
            if(!skipShowcase) sceneReference.CloseRoom();
        }

        /// <summary>
        /// This will change the quality in which you render the car models.
        /// Meant for rendering on different devices, such as Hololens, which
        /// can't keep up with higher quality models.
        /// </summary>
        /// <param name="newQuality"></param>
        //public void SetQuality(CarQuality newQuality)
        //{
        //    // Don't do anything if we're not changing shit
        //    if (newQuality == qualityToRender)
        //    {
        //        return;
        //    }

        //    // Update what we're currently rendering.
        //    qualityToRender = newQuality;
        //    DisplayCar(cars[carBeingDisplayedIndex]);
        //}

        public void BuildRadialConfig()
        {
            List<PlayerControl> startingControls = new List<PlayerControl>();

            if (Grab) startingControls.Add(new GrabPlayerControl());
            if (BezierTeleport) startingControls.Add(new TeleportPlayerControl());
            if (Select) startingControls.Add(new SelectPlayerControl());
            if (TVP) startingControls.Add(new TVPPlayerControl());

            var config = new ControllerConfig(startingControls);

            config.Build(leftHand);
            config.Build(rightHand);
        }
        public void BuildTVPConfig()
        {
            var config = new ControllerConfig(new List<PlayerControl>()
            {
                new TVPPlayerControl()
            });

            config.Build(leftHand);
            config.Build(rightHand);
        }

        public void BuildPointClickConfig()
        {
            var config = new ControllerConfig(new List<PlayerControl>()
            {
                new TeleportPlayerControl()
            });

            config.Build(leftHand);
            config.Build(rightHand);
        }


    }

}