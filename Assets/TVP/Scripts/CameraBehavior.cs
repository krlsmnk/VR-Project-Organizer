using UnityEngine;
using VRTK;

namespace KarlSmink.Teleporting
{

    public class CameraBehavior : MonoBehaviour
    {
        [SerializeField]
        private float cameraSpeed;

        [SerializeField]
        private AudioClip lockSound, unlockSound, summonCamera;

        [SerializeField]
        private VRTK_TransformFollow theFollowScript;

        private VRTK_TransformFollow rotatorScript;

        private Transform headset;

        public static CameraBehavior Initialize(GameObject cameraObj, float cameraSpeed, GameObject portal)
        {
            var script = cameraObj.AddComponent<CameraBehavior>();

            script.theFollowScript = portal.AddComponent<VRTK_TransformFollow>();
            script.theFollowScript.gameObjectToChange = portal;
            script.theFollowScript.gameObjectToFollow = VRTK_DeviceFinder.HeadsetTransform().gameObject;
            script.theFollowScript.followsRotation = true;
            script.theFollowScript.followsPosition = true;

            script.rotatorScript = cameraObj.AddComponent<VRTK_TransformFollow>();
            script.rotatorScript.gameObjectToChange = cameraObj;
            script.rotatorScript.gameObjectToFollow = script.theFollowScript.gameObjectToFollow;
            script.rotatorScript.followsRotation = true;
            script.rotatorScript.followsPosition = false;

            script.cameraSpeed = cameraSpeed;
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
            rotatorScript.enabled = !theFollowScript.enabled;
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
            moveDirection = direction;
            this.relativeSpace = relativeSpace;
        }

        void Update()
        {
            transform.Translate(moveDirection.normalized * cameraSpeed * Time.deltaTime, relativeSpace);
        }
    }

}