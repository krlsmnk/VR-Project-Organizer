using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using CAVS.ProjectOrganizer.Netowrking;
using CAVS.ProjectOrganizer.Interation;
using CAVS.ProjectOrganizer.Project;
using CAVS.ProjectOrganizer.Project.Aggregations.Plot;

using VRTK;

using Firebase.Database;

namespace CAVS.ProjectOrganizer.Scenes.Showcase
{

    /// <summary>
    /// Meant managing the entire carshowcase scene.
    /// </summary>
    public class SceneManagerBehavior : MonoBehaviour
    {

        [SerializeField]
        private Pedistal pedistal;

        /// <summary>
        /// The screens we're going to render the current car to
        /// </summary>
        [SerializeField]
        private RawImage[] carImageScreens;

        /// <summary>
        /// Table top for displaying mini cars
        /// </summary>
        [SerializeField]
        private MiniCarSelectionBehavior tableTop;

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
        /// All the cars we're going to display information about. (Grabbed from the database)
        /// </summary>
        private PictureItem[] cars;


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
        private string roomToJoin;

        private NetworkRoom sceneReference;

        GameObject player;

        private void Awake()
        {
            roomDisplay = gameObject.GetComponent<RoomDisplayBehavior>();
        }

        void Start()
        {
            if(roomToJoin == null || roomToJoin == "")
            {
                sceneReference = NetworkingManager.Instance.CreateSceneEntry("showcase");
            } else
            {
                sceneReference = NetworkingManager.Instance.JoinScene(roomToJoin);
            }

            sceneReference.SubscribeToNewData(OnSceneUpdate);

            nextButton.Subscribe(DisplayNextCar);
            previousButton.Subscribe(DisplayPreviousCar);
            cars = ProjectFactory.BuildItemsFromCSV("Assets/Car_Dataset.csv", 7);
            DisplayNextCar();
            tableTop.SetCars(cars);
            pedistal.Subscribe(OnPedistalSelection);
            StartCoroutine(UpdatePlayerTransformOnFirebase());
            // graphControl.Initialize(this.PlotPointBuilder, cars);
        }

        private void OnSceneUpdate(Dictionary<string, object> update)
        {
            roomDisplay.UpdatePuppets(new ShowcaseData(update).UsersInScene());
        }

        private void OnPedistalSelection(string selection)
        {
            // ID: 1  - Lexus CT 200h - 4dr Hatchback highdef
            // ID: 44 - Lexus GS GS 300 - Sedan highdef
            // ID: 68 - Lexus GS GS 350 - Sedan highdef (Made up)
            // ID: 4  - Lexus CT 200h Premium - 4dr Hatchback (Made up)
            int j;
            if (int.TryParse(selection, out j))
            {
                carBeingDisplayedIndex = Mathf.Clamp(j - 1, 0, cars.Length);
                DisplayCar(cars[carBeingDisplayedIndex]);
            }
        }

        private GameObject PlotPointBuilder(Item item)
        {
            ItemBehaviour itemObj = item.Build();
            itemObj.AddExamineEvent(this.OnPlotPointExamined);
            itemObj.GetComponent<Rigidbody>().isKinematic = true;
            return itemObj.gameObject;
        }

        private void OnPlotPointExamined(Item point, Collider collider)
        {
            int id = 0;
            if (int.TryParse(point.GetValue("ID"), out id))
            {
                carBeingDisplayedIndex = id - 1;
                DisplayCar(cars[carBeingDisplayedIndex]);
            }
        }

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


        private int carBeingDisplayedIndex = -1;

        public void DisplayPreviousCar()
        {
            // Make sure we even have cars...
            if (cars == null)
            {
                return;
            }

            // Increment Index
            carBeingDisplayedIndex = Mathf.Clamp(carBeingDisplayedIndex - 1, 0, this.cars.Length);

            DisplayCar(cars[carBeingDisplayedIndex]);
        }


        public void DisplayNextCar()
        {
            // Make sure we even have cars...
            if (cars == null)
            {
                return;
            }

            // Increment Index
            carBeingDisplayedIndex = Mathf.Clamp(carBeingDisplayedIndex + 1, 0, this.cars.Length);

            DisplayCar(cars[carBeingDisplayedIndex]);
        }

        private IEnumerator UpdatePlayerTransformOnFirebase()
        {
            while(true)
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
                else
                {
                    sceneReference.Update(new NetworkUpdateBuilder()
                        .AddEntry("position", player.transform.position)
                        .AddEntry("rotation", player.transform.rotation.eulerAngles)
                        .Build());
                }
                yield return new WaitForSeconds(1);
            }
        }

        /// <summary>
        /// Set up the entire scene to be rendering information about the specific
        /// car passed in.
        /// </summary>
        /// <param name="carToDisplay">Car to display info about</param>
        private void DisplayCar(PictureItem carToDisplay)
        {
            sceneReference.SetObjectValue("selectedCar", carToDisplay.ToJson());

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

            currentCarGameObject = CarFactory.MakeCar(carToDisplay, qualityToRender, Vector3.zero, Quaternion.identity);
            currentCarGameObject.transform.parent = liftCarPlacement.transform;
            currentCarGameObject.transform.localPosition = Vector3.zero;
        }


        private void OnApplicationQuit()
        {
            sceneReference.CloseRoom();
        }

        /// <summary>
        /// This will change the quality in which you render the car models.
        /// Meant for rendering on different devices, such as Hololens, which
        /// can't keep up with higher quality models.
        /// </summary>
        /// <param name="newQuality"></param>
        public void SetQuality(CarQuality newQuality)
        {
            // Don't do anything if we're not changing shit
            if (newQuality == qualityToRender)
            {
                return;
            }

            // Update what we're currently rendering.
            qualityToRender = newQuality;
            DisplayCar(cars[carBeingDisplayedIndex]);
        }


    }

}