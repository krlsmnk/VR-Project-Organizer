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

        private static GameObject footstepOffset;

        public static CameraBehavior Initialize(GameObject cameraObj, float cameraSpeed, GameObject portal)
        {
            var script = cameraObj.AddComponent<CameraBehavior>();

            script.theFollowScript = portal.AddComponent<VRTK_TransformFollow>();            
            script.theFollowScript.gameObjectToChange = portal;
            if (VRTK_DeviceFinder.HeadsetTransform().gameObject == null) script.theFollowScript.gameObjectToFollow = GameObject.FindObjectOfType<SceneManagerBehavior>().headsetNullfix.gameObject;
            else script.theFollowScript.gameObjectToFollow = VRTK_DeviceFinder.HeadsetTransform().gameObject;            
            script.theFollowScript.followsRotation = true;
            script.theFollowScript.followsPosition = true;
            
            script.rotatorScript = cameraObj.AddComponent<VRTK_TransformFollow>();
            script.rotatorScript.gameObjectToChange = cameraObj;
            script.rotatorScript.gameObjectToFollow = script.theFollowScript.gameObjectToFollow;
            script.rotatorScript.followsRotation = true;
            script.rotatorScript.followsPosition = false;

            //CNG
            footstepOffset = new GameObject("FootstepOffset");
            cameraObj.gameObject.transform.parent = footstepOffset.transform;
            footstepOffsetScript = footstepOffset.AddComponent<VRTK_TransformFollow>();
            footstepOffsetScript.gameObjectToFollow = script.theFollowScript.gameObjectToFollow;
            footstepOffsetScript.gameObjectToChange = footstepOffsetScript.gameObject;
            footstepOffsetScript.followsRotation = false;
            footstepOffsetScript.followsPosition = true;


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

        public void Move(Vector3 direction, Space relativeSpace)
        {
            cameraSpeed = originalCameraSpeed;
            moveDirection = direction;
            this.relativeSpace = relativeSpace;
        }
        public void Move(Vector3 direction, Space relativeSpace, float newSpeed)
        {
            cameraSpeed = newSpeed;
            moveDirection = direction;
            this.relativeSpace = relativeSpace;
        }

        void Update()
        {
            transform.Translate(moveDirection.normalized * cameraSpeed * Time.deltaTime, relativeSpace);
        }

    }

}