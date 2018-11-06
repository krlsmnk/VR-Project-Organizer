using UnityEngine;
using CAVS.Anvel;
using AnvelApi;

namespace CAVS.ProjectOrganizer.Scenes.Showcase
{
    public class VehicleControl : MonoBehaviour
    {

        private AnvelControlService.Client client;

        private ObjectDescriptor vehicle;

        private VehicleInputRecord vehicleInput;

        VRTK.VRTK_ControllerEvents controllerInput;

        public static VehicleControl Build(GameObject parent, ClientConnectionToken connectionToken, ObjectDescriptor vehicle)
        {
            VehicleControl controller = parent.AddComponent<VehicleControl>();
            controller.client = ConnectionFactory.CreateConnection(connectionToken);

            controller.vehicle = vehicle;
            controller.vehicleInput = new VehicleInputRecord
            {
                //Steering 0 = No Left/Right Movement ; 1.0 -> 0 = Right ; 0 -> -1.0 = Left
                Steering = 0.0,
                
                //Throttle 0 = No Movement ; -1.0 -> 0 = Reverse ; 0 -> 1.0 = Forward
                Throttle = 0.0,
                
                //Brake 0 = No Braking ; 1.0 = Full breaks
                Brake = 0.0
            };

            controller.controllerInput = parent.GetComponent<VRTK.VRTK_ControllerEvents>();
            return controller;
        }

        void Update()
        {
            if (controllerInput == null)
            {
                return;
            }
            if (controllerInput.touchpadTouched || controllerInput.touchpadPressed)
            {
                vehicleInput.Steering = controllerInput.GetTouchpadAxis().x;
            }
            else
            {
                vehicleInput.Steering = 0.0f;
            }

            if (controllerInput.triggerTouched)
            {
                //defaults to accelerating forward, back if touchpad indicates that direction.
                if (controllerInput.GetTouchpadAxis().y < 0)
                {
                    vehicleInput.Throttle = controllerInput.GetTriggerAxis() * -1;
                }
                else
                {
                    vehicleInput.Throttle = controllerInput.GetTriggerAxis();
                }
            }
            else
            {
                vehicleInput.Throttle = 0.0f;
            }
            SendControls(vehicleInput);
        }

        void SendControls(VehicleInputRecord vehicleInput)
        {
            client.SetVehicleInput(vehicle.ObjectKey, vehicleInput);
        }

    }
}


