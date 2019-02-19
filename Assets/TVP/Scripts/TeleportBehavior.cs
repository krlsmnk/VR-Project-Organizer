using UnityEngine;
using VRTK;

namespace KarlSmink.Teleporting
{

    [RequireComponent(typeof(VRTK_HeadsetCollision))]
    public class TeleportBehavior : MonoBehaviour
    {
        private Transform headset;

        private Transform playArea;

        [SerializeField]
        private AudioClip teleportSound;

        private Vector3 headsetPosition;

        private Quaternion headsetRotation;

        [SerializeField]
        private GameObject portal;

        [SerializeField]
        private float height;

        [SerializeField]
        private Transform cameraBeingControlled;

        private VRTK_HeadsetCollision headsetCollision;

        public static TeleportBehavior Initialize(VRTK_HeadsetCollision headsetCollision, float height, Transform cameraBeingControlled, GameObject portal)
        {
            var script = headsetCollision.gameObject.AddComponent<TeleportBehavior>();

            script.teleportSound = Resources.Load<AudioClip>("teleport");
            script.headsetCollision = headsetCollision;
            script.height = height;
            script.portal = portal;
            script.cameraBeingControlled = cameraBeingControlled;

            script.headsetCollision.HeadsetCollisionDetect += script.OnHeadsetCollisionDetect;
            script.headsetCollision.HeadsetCollisionEnded += script.OnHeadsetCollisionEnded;

            return script;
        }

        void OnDestroy()
        {
            headsetCollision.HeadsetCollisionDetect -= new HeadsetCollisionEventHandler(OnHeadsetCollisionDetect);
            headsetCollision.HeadsetCollisionEnded -= new HeadsetCollisionEventHandler(OnHeadsetCollisionEnded);
        }

        private void OnHeadsetCollisionDetect(object sender, HeadsetCollisionEventArgs e)
        {
            playArea = VRTK_DeviceFinder.PlayAreaTransform();
            headset = VRTK_DeviceFinder.HeadsetTransform();

            headsetPosition = headset.position;
            headsetRotation = headset.rotation;

            playArea.position = cameraBeingControlled.position + cameraBeingControlled.forward;
            playArea.position -= new Vector3(0f, 1.2f, 0f);
            headset.position = playArea.position;

            portal.transform.LookAt(headset);
            portal.transform.position = new Vector3(portal.transform.position.x, headset.position.y + (4 * height), portal.transform.position.z);
            portal.transform.Rotate(90, 0, 180);

            Util.PlaySoundEffect(teleportSound, transform.position);
        }

        private void OnHeadsetCollisionEnded(object sender, HeadsetCollisionEventArgs e)
        {
            headset = VRTK_DeviceFinder.HeadsetTransform();
            cameraBeingControlled.position = headsetPosition;
            cameraBeingControlled.rotation = headsetRotation;

            Transform tempPos = headset.transform;
            tempPos.rotation = headset.rotation;
            tempPos.Translate(Vector3.back * 0.1f);
            tempPos.Rotate(0, 180, 0);

            portal.transform.position = tempPos.position;
            portal.transform.rotation = tempPos.rotation;
        }

        

    }
}