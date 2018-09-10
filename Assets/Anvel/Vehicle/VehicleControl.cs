using UnityEngine;

using Thrift.Protocol;
using Thrift.Transport;
using AnvelApi;
namespace CAVS.Anvel.Vehicle
{
    public class VehicleControl : MonoBehaviour
    {

        [SerializeField]
        private string vehicleName;
        [SerializeField]
        private int port;
        private AnvelControlService.Client client;
        private ObjectDescriptor vehicle;

        private VehicleInputRecord vehicleInput = new VehicleInputRecord();

        VRTK.VRTK_ControllerEvents controllerInput;

        void Start()
        {
            //open a connect to ANVEL on the local host
            TTransport transport = new TSocket("localhost", port);
            TProtocol protocol = new TBinaryProtocol(transport);
            client = new AnvelControlService.Client(protocol);
            transport.Open();

            //Steering 0 = No Left/Right Movement ; 1.0 -> 0 = Right ; 0 -> -1.0 = Left
            vehicleInput.Steering = 0.0;
            //Throttle 0 = No Movement ; -1.0 -> 0 = Reverse ; 0 -> 1.0 = Forward
            vehicleInput.Throttle = 0.0;
            //Brake 0 = No Braking ; 1.0 = Full breaks
            vehicleInput.Brake = 0.0;

            vehicle = client.GetObjectDescriptorByName(vehicleName);

            //Set ANVEL to run
            //client.SetSimulationState(SimulationState.RUNNING_REALTIME);

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


