using UnityEngine;

using AnvelApi;
namespace CAVS.ProjectOrganizer.Scenes.Showcase
{
    public class VehicleControl : MonoBehaviour
    {
        private string vehicleName;

        private AnvelControlService.Client client;

        private ObjectDescriptor vehicle;

        private VehicleInputRecord vehicleInput;

        VRTK.VRTK_ControllerEvents controllerInput;

        public void Initialize(AnvelControlService.Client connection, string vehicleName)
        {
            this.client = connection;
            this.vehicleName = vehicleName;

            vehicleInput = new VehicleInputRecord
            {
                //Steering 0 = No Left/Right Movement ; 1.0 -> 0 = Right ; 0 -> -1.0 = Left
                Steering = 0.0,
                
                //Throttle 0 = No Movement ; -1.0 -> 0 = Reverse ; 0 -> 1.0 = Forward
                Throttle = 0.0,
                
                //Brake 0 = No Braking ; 1.0 = Full breaks
                Brake = 0.0
            };

            vehicle = client.GetObjectDescriptorByName(vehicleName);

            controllerInput = GetComponent<VRTK.VRTK_ControllerEvents>();
        }

        void Update()
        {

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

        public void setVehicleName(string newVehicleName)
        {
            vehicleName = newVehicleName;
            vehicle = client.GetObjectDescriptorByName(vehicleName);
        }
    }
}


