using UnityEngine;

using VRTK;

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

        public void Start()
        {
            Item[] allItems = ProjectFactory.BuildItemsFromCSV("CarData.csv", 7);
            plot = new ItemPlot(allItems, "Year", "Length (in)", "Width (in)").Build(Vector3.one * 3);
            plot.transform.position = Vector3.up * 2;
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
            }

            if (rightController == null)
            {
                rightController = VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_ControllerEvents>();
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