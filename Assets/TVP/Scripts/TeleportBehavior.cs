using CAVS.ProjectOrganizer.Scenes.Showcase;
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

        private Vector3 oldHeadsetPosition;

        private Quaternion oldHeadsetRotation;

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

            return script;
        }

        void OnDestroy()
        {
            headsetCollision.HeadsetCollisionDetect -= new HeadsetCollisionEventHandler(OnHeadsetCollisionDetect);
        }

        private void OnHeadsetCollisionDetect(object sender, HeadsetCollisionEventArgs e)
        {   
            if(e.collider.gameObject.tag == "Portal") { 
                playArea = VRTK_DeviceFinder.PlayAreaTransform();
                headset = VRTK_DeviceFinder.HeadsetTransform();

                Vector3 playareaHeadsetOffset = headset.transform.position - playArea.transform.position;

                var headForwardCleaned = headset.forward;
                headForwardCleaned.y = 0;
                portal.transform.position = cameraBeingControlled.position - (headForwardCleaned.normalized);
                portal.transform.LookAt(cameraBeingControlled.position - (headForwardCleaned.normalized * 4)); // 4 means nothing, we just have to go further than it's position

                Vector3 newHeadsetPosition = cameraBeingControlled.position + cameraBeingControlled.forward;
                //If we're not allowing height adjust, make sure the user doesn't end up in or above the floor
                if(!GameObject.FindObjectOfType<SceneManagerBehavior>().allowHeightAdjustTVP)newHeadsetPosition = new Vector3(newHeadsetPosition.x, GameObject.FindObjectOfType<SceneManagerBehavior>().userHeight, newHeadsetPosition.z);

                cameraBeingControlled.position = headset.position;
                cameraBeingControlled.rotation = headset.rotation;

                playArea.position = newHeadsetPosition - playareaHeadsetOffset;
                headset.position = newHeadsetPosition;

                Util.PlaySoundEffect(teleportSound, transform.position);
            }
        }

    }
}