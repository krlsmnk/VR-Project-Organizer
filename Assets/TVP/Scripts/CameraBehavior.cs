using CAVS.ProjectOrganizer.Scenes.Showcase;
using UnityEngine;
using VRTK;

namespace KarlSmink.Teleporting
{

    public class CameraBehavior : MonoBehaviour
    {
        [SerializeField]
        private float cameraSpeed, originalCameraSpeed;

        [SerializeField]
        private AudioClip lockSound, unlockSound, summonCamera;

        [SerializeField]
        private VRTK_TransformFollow theFollowScript;

        private VRTK_TransformFollow rotatorScript;

        private static VRTK_TransformFollow footstepOffsetScript;

        private Transform headset;

        private static GameObject footstepOffset, headsetGameObj;

        private bool allowHeightAdjust, collisionIgnore;

        private static CharacterController myControl;

        public static Rigidbody myRigidBody;

        void Awake()
        {
            VRTK_SDKManager.instance.AddBehaviourToToggleOnLoadedSetupChange(this);
        }
        void OnDestroy()
        {
            VRTK_SDKManager.instance.RemoveBehaviourToToggleOnLoadedSetupChange(this);
        }

        void OnEnable()
        {
            headsetGameObj = VRTK_DeviceFinder.HeadsetTransform().gameObject;
        }

        public static CameraBehavior Initialize(GameObject cameraObj, float cameraSpeed, GameObject portal)
        {
            var script = cameraObj.AddComponent<CameraBehavior>();

            script.theFollowScript = portal.AddComponent<VRTK_TransformFollow>();
            script.theFollowScript.gameObjectToChange = portal;
            script.theFollowScript.gameObjectToFollow = headsetGameObj;
            script.theFollowScript.followsRotation = true;
            script.theFollowScript.followsPosition = true;
            script.theFollowScript.followsScale = false;

            script.rotatorScript = cameraObj.AddComponent<VRTK_TransformFollow>();
            script.rotatorScript.gameObjectToChange = cameraObj;
            script.rotatorScript.gameObjectToFollow = script.theFollowScript.gameObjectToFollow;
            script.rotatorScript.followsRotation = true;
            script.rotatorScript.followsPosition = false;
            script.rotatorScript.followsScale = false;

            footstepOffset = new GameObject("FootstepOffset");
            footstepOffset.layer = 2;
            cameraObj.gameObject.transform.parent = footstepOffset.transform;
            footstepOffsetScript = footstepOffset.AddComponent<VRTK_TransformFollow>();
            footstepOffsetScript.gameObjectToFollow = script.theFollowScript.gameObjectToFollow;
            footstepOffsetScript.gameObjectToChange = footstepOffsetScript.gameObject;
            footstepOffsetScript.followsRotation = false;
            footstepOffsetScript.followsPosition = true;

            myControl = cameraObj.gameObject.AddComponent<CharacterController>();
            myRigidBody = footstepOffset.AddComponent<Rigidbody>();
            myRigidBody.constraints = RigidbodyConstraints.FreezePositionY;
            myRigidBody.useGravity = false;
            VRTK_ControllerEvents[] controllers = FindObjectsOfType<VRTK_ControllerEvents>();
            foreach (VRTK_ControllerEvents thisController in controllers)
            {
                thisController.gameObject.layer = 4; //puts all controllers on the water layer so they ignore collision with the camera
            }


            script.cameraSpeed = cameraSpeed;
            script.originalCameraSpeed = cameraSpeed;
            script.lockSound = Resources.Load<AudioClip>("lock window");
            script.unlockSound = Resources.Load<AudioClip>("unlock window");
            script.summonCamera = Resources.Load<AudioClip>("camera summon");
            return script;
        }

        void Start()
        {
            rotatorScript = GetComponent<VRTK_TransformFollow>();
            allowHeightAdjust = GameObject.FindObjectOfType<SceneManagerBehavior>().allowHeightAdjustTVP;
            collisionIgnore = GameObject.FindObjectOfType<SceneManagerBehavior>().cameraIgnoresCollision;
        }


        public bool ToggleLock()
        {
            Util.PlaySoundEffect(theFollowScript.enabled ? lockSound : unlockSound, transform.position);
            //CNG rotatorScript.enabled = !theFollowScript.enabled;
            theFollowScript.enabled = !theFollowScript.enabled;
            return theFollowScript.enabled;
        }

        public void SummonCamera()
        {
            transform.position = headset.position;
            Util.PlaySoundEffect(summonCamera, transform.position);
        }

        Vector3 moveDirection = Vector3.zero;

        Space relativeSpace = Space.Self;


        /**
         * Changed moveDirection to direction to fix movement of virtual window
         * Changed on 9/5/19 by chris and karl
         * */
        public void Move(Vector3 direction, Space relativeSpace)
        {
            cameraSpeed = originalCameraSpeed;
            if (allowHeightAdjust) moveDirection = direction;
            else moveDirection = new Vector3(direction.x, 0, direction.z);
            //else moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z);
            this.relativeSpace = relativeSpace;
        }

        /**
         * Changed moveDirection to direction to fix movement of virtual window
         * Changed on 9/5/19 by chris and karl
         * */
        public void Move(Vector3 direction, Space relativeSpace, float newSpeed)
        {
            cameraSpeed = newSpeed;
            if (allowHeightAdjust) moveDirection = direction;
            else moveDirection = new Vector3(direction.x, 0, direction.z);
            //else moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z);
            this.relativeSpace = relativeSpace;
        }

        void Update()
        {

            if (!collisionIgnore)
            {
                Vector3 movement = Vector3.zero;

                movement += transform.forward * moveDirection.z * cameraSpeed * Time.deltaTime;
                movement += transform.right * moveDirection.x * cameraSpeed * Time.deltaTime;

                if (allowHeightAdjust) myControl.Move(movement);
                else myControl.Move(new Vector3(movement.x, 0, movement.z));
            }
            else transform.Translate(moveDirection.normalized * cameraSpeed * Time.deltaTime, relativeSpace);
        }
    }

}