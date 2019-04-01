using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAVS.ProjectOrganizer.Interation
{

    public class SnappingZoneBehavior : MonoBehaviour
    {

        [SerializeField]
        private Vector3 dimensions;

		public bool InZone(Vector3 pos)
		{
			if (pos.x < transform.position.x - dimensions.x || pos.x > transform.position.x + dimensions.x)
            {
                return false;
            }

			if (pos.y < transform.position.y - dimensions.y || pos.y > transform.position.y + dimensions.y)
            {
                return false;
            }

			if (pos.z < transform.position.z - dimensions.z || pos.z > transform.position.z + dimensions.z)
            {
                return false;
            }
			return true;
		}

        public Vector3 Discritize(Vector3 pos)
        {
            var precPow = Mathf.Pow(10, 1);
            return new Vector3(
                (Mathf.RoundToInt(pos.x * precPow) / precPow),
                (Mathf.RoundToInt(pos.y * precPow) / precPow),
                (Mathf.RoundToInt(pos.z * precPow) / precPow)
            );
        }

		public Quaternion Discritize(Quaternion pos)
        {
            return Quaternion.LookRotation(transform.forward, Vector3.up);
        }

    }

}