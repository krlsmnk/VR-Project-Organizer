using UnityEngine;

using VRTK;

using CAVS.ProjectOrganizer.Interation;
using CAVS.ProjectOrganizer.Project;
using CAVS.ProjectOrganizer.Project.Aggregations.Plot;

namespace CAVS.ProjectOrganizer.Scenes.GraphExploration
{

    public class GameManager : MonoBehaviour
    {

        enum GameState
        {
            /// <summary>
            /// Not currentely doing any special action
            /// </summary>
            Nothing,

            /// <summary>
            /// Scaling the Data Exploration Space
            /// </summary>
            Scaling,

            /// <summary>
            /// Moving throught the data exploration space
            /// </summary>
            Moving
        }

        private VRTK_ControllerEvents leftController;

        private VRTK_ControllerEvents rightController;

        private GameState lastFramesState = GameState.Nothing;

        private GameObject plot;

        private Vector3 oldPlotScale;

        private Vector3 oldPlotPosition;

        [SerializeField]
        private GameObject plotManagerReference;

        private PlotControl plotManagerInstance;

        public void Start()
        {
            oldPlotScale = Vector3.one;
            oldPlotPosition = new Vector3(-.5f, 1, -1);
            plotManagerInstance = Instantiate(plotManagerReference).GetComponent<PlotControl>();

            plotManagerInstance.Initialize(this.OnNewPlot, ProjectFactory.BuildItemsFromCSV("CarData.csv", 7));

            plotManagerInstance.gameObject.SetActive(false);

        }

        private void OnNewPlot(GameObject newPlot)
        {
            plot = newPlot;
            plot.transform.position = oldPlotPosition;
            plot.transform.localScale = oldPlotScale;
        }

        private GameState GetState()
        {
            if (leftController == null || rightController == null)
            {
                return GameState.Nothing;
            }
            if (leftController.triggerPressed && rightController.triggerPressed)
            {
                return GameState.Scaling;
            }
            if (leftController.triggerPressed || rightController.triggerPressed)
            {
                return GameState.Moving;
            }
            return GameState.Nothing;
        }

        private bool enteredScalingState(GameState last, GameState current)
        {
            return last != GameState.Scaling && current == GameState.Scaling;
        }

        private float controllerDistances()
        {
            return this.leftController == null || this.rightController == null ?
                float.MaxValue :
                Vector3.Distance(this.leftController.transform.position, this.rightController.transform.position);
        }

        private float originalControllerDistance;

        private Vector3 originalScale;

        private void initializeScaling()
        {
            originalControllerDistance = controllerDistances();
            originalScale = plot.transform.localScale;
        }

        private void scalingUpdate()
        {
            plot.transform.localScale = originalScale * (controllerDistances() / originalControllerDistance);
        }

        private bool enteredMovingState(GameState last, GameState current)
        {
            return last != GameState.Moving && current == GameState.Moving;
        }

        Vector3 originalControllerPosition;

        private void initializeMovement()
        {
            if (leftController.triggerPressed)
            {
                originalControllerPosition = this.leftController.transform.position;
            }
            else
            {
                originalControllerPosition = this.rightController.transform.position;
            }
        }

        private void movingUpdate()
        {
            Vector3 pos;
            if (leftController.triggerPressed)
            {
                pos = this.leftController.transform.position;
            }
            else
            {
                pos = this.rightController.transform.position;
            }
            Vector3 translation = (pos - originalControllerPosition) * plot.transform.localScale.magnitude * Time.deltaTime;
            plot.transform.Translate(translation);
            oldPlotPosition = plot.transform.position;
            oldPlotScale = plot.transform.lossyScale;
        }

        void InitializeControllers()
        {
            if (leftController == null || rightController == null)
            {
                return;
            }

            leftController.TouchpadPressed += delegate (object o, ControllerInteractionEventArgs e)
                {
                    plotManagerInstance.gameObject.SetActive(true);
                    plotManagerInstance.transform.parent = leftController.transform;
                    plotManagerInstance.transform.localScale = Vector3.one * 0.3f;
                    plotManagerInstance.transform.localEulerAngles = new Vector3(0, 0, 0);
                    plotManagerInstance.transform.localPosition = ((Vector3.forward + Vector3.up) * .5f);
                    plotManagerInstance.GetComponent<PlotControl>();
                };

            leftController.TouchpadReleased += delegate (object o, ControllerInteractionEventArgs e)
            {
                plotManagerInstance.gameObject.SetActive(false);
            };
        }

        void Update()
        {
            if (plot == null)
            {
                return;
            }

            if (leftController == null)
            {
                leftController = VRTK_DeviceFinder.GetControllerLeftHand().GetComponent<VRTK_ControllerEvents>();
                InitializeControllers();
            }

            if (rightController == null)
            {
                rightController = VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_ControllerEvents>();
                InitializeControllers();
            }

            GameState currentState = GetState();

            if (enteredScalingState(lastFramesState, currentState)) { initializeScaling(); }
            else if (currentState == GameState.Scaling) { scalingUpdate(); }
            else if (enteredMovingState(lastFramesState, currentState)) { initializeMovement(); }
            else if (currentState == GameState.Moving) { movingUpdate(); }

            this.lastFramesState = currentState;
        }
    }

}