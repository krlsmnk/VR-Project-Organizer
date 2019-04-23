using UnityEngine;
using VRTK;


namespace CAVS.ProjectOrganizer.Interation
{

    public abstract class RestrainedInteractableObject : VRTK_InteractableObject
    {

        /// <summary>
        /// Given a position, return the closest possible position the 
        /// restained object is allowed to occupy
        /// </summary>
        /// <param name="desired"></param>
        /// <returns>available</returns>
        public abstract Vector3 GetAvailablePositionFromDesired(Vector3 desiredPosition, Quaternion desiredRotation);


        public abstract Quaternion GetAvailableRotationFromDesired(Vector3 desiredPosition, Quaternion desiredRotation);

    }

}