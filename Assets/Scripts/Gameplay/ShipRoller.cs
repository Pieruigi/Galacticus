using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galacticus
{
    public class ShipRoller : MonoBehaviour
    {
        [SerializeField]
        float minYawSpeed = 20;

        [SerializeField]
        float maxYawSpeed = 180;

        [SerializeField]
        float minSideSpeed = 2;

        [SerializeField]
        float maxSideSpeed = 4;

        [SerializeField]
        float maxRollAngle = 30f;


        [SerializeField]
        Transform target;

        Vector3 lastForward;
        float currentRollAngle;
        Vector3 lastPosition;


        // Start is called before the first frame update
        void Start()
        {
            lastForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
            lastPosition = transform.position;
        }

        
        void LateUpdate()
        {
            //// Get the current yaw
            //Vector3 fwd = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
            //// Compute the angle
            //float angle = Vector3.SignedAngle(fwd, lastForward, Vector3.up);
            
            //// Compute rotation speed
            //float rotSpeed = Mathf.Abs(angle) / Time.deltaTime;
            
            //float rollAngle = 0;
            //if (rotSpeed > minYawSpeed && rotSpeed < maxYawSpeed)
            //    rollAngle = Mathf.Lerp(0f, maxRollAngle, 1f - ((maxYawSpeed - rotSpeed) / (maxYawSpeed - minYawSpeed)));
            //else if (rotSpeed >= maxYawSpeed)
            //    rollAngle = maxRollAngle;

            //if (rollAngle > 0)
            //    rollAngle *= Mathf.Sign(angle);

         

            //currentRollAngle = Mathf.MoveTowards(currentRollAngle, rollAngle, 80f * Time.deltaTime);

            // Roll
            transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, currentRollAngle);

            //Debug.Log($"RollAngle:{currentRollAngle}");
            //lastForward = fwd;
            
        }

        private void FixedUpdate()
        {
            // Compute side speed
            Vector3 pos = transform.position;
            Vector3 dir = Vector3.ProjectOnPlane(pos - lastPosition, Vector3.up);
            // Get the component along the right axis
            Vector3 rgt = Vector3.ProjectOnPlane(transform.right, Vector3.up);
            Vector3 sideVel = Vector3.Project(dir, rgt);
            
            float sign = Mathf.Sign(Vector3.Dot(dir, rgt));
            float sideSpeed = sideVel.magnitude / Time.fixedDeltaTime;
            //Debug.Log($"SideSpeed:{sideSpeed}");
            float rollAngle = 0;
            if (sideSpeed > minSideSpeed && sideSpeed < maxSideSpeed)
                rollAngle = sign * Mathf.Lerp(0f, maxRollAngle, 1f - ((maxSideSpeed - sideSpeed) / (maxSideSpeed - minSideSpeed)));
            else if (sideSpeed >= maxSideSpeed)
                rollAngle = sign * maxRollAngle;

            Debug.Log($"currentRollAngle:{currentRollAngle}");
            currentRollAngle = Mathf.MoveTowards(currentRollAngle, rollAngle, 80f * Time.fixedDeltaTime);

            // Roll
            transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, currentRollAngle);

            lastPosition = pos;
        }
    }

}
