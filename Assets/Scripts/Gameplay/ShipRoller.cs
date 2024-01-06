using Galacticus.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galacticus
{
    public class ShipRoller : MonoBehaviour
    {
        [SerializeField]
        float minSideSpeed = 1;

        [SerializeField]
        float maxSideSpeed = 3;

        [SerializeField]
        float minStrafeSpeed = 3;

        [SerializeField]
        float maxStrafeSpeed = 6;

        //[SerializeField]
        //float maxYawSpeed = 30;

        [SerializeField]
        float minRollAngle = 10f;

        [SerializeField]
        float maxRollAngle = 30f;

        [SerializeField]
        float rollSpeed = 80f;

        [SerializeField]
        bool useRigidbody;

        //[SerializeField]
        //Transform target;

        Vector3 lastForward;
        float currentRollAngle;
        Vector3 lastPosition;
        IStrafer strafeable;
        Rigidbody rb;

        private void Awake()
        {
            strafeable = GetComponent<IStrafer>();
            if (useRigidbody)
                rb = GetComponent<Rigidbody>();
        }

        // Start is called before the first frame update
        void Start()
        {
            lastForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
            lastPosition = transform.position;
        }



        //void LateUpdate()
        //{
        //    if (strafeable != null && strafeable.IsStrafing()) // Strafing
        //    {
        //        if (!useRigidbody)
        //        {
        //            Debug.Log($"IsStrafing:{strafeable.IsStrafing()}");
        //            if (strafeable == null || !strafeable.IsStrafing() || !useRigidbody)
        //                return;

        //            // Compute side speed
        //            Vector3 pos = transform.position;
        //            Vector3 dir = Vector3.ProjectOnPlane(pos - lastPosition, Vector3.up);
        //            // Get the component along the right axis
        //            Vector3 rgt = Vector3.ProjectOnPlane(transform.right, Vector3.up);
        //            Vector3 sideVel = Vector3.Project(dir, rgt);

        //            float sign = -Mathf.Sign(Vector3.Dot(dir, rgt));
        //            float sideSpeed = sideVel.magnitude / Time.fixedDeltaTime;
        //            //Debug.Log($"SideSpeed:{sideSpeed}");
        //            float rollAngle = 0;
        //            if (sideSpeed > minSideSpeed && sideSpeed < maxSideSpeed)
        //                rollAngle = sign * Mathf.Lerp(minRollAngle, maxRollAngle, 1f - ((maxSideSpeed - sideSpeed) / (maxSideSpeed - minSideSpeed)));
        //            else if (sideSpeed >= maxSideSpeed)
        //                rollAngle = sign * maxRollAngle;


        //            currentRollAngle = Mathf.MoveTowards(currentRollAngle, rollAngle, rollSpeed * Time.fixedDeltaTime);
        //            Debug.Log($"currentRollAngle:{currentRollAngle}");

        //            lastPosition = pos;

        //        }

        //        // Roll in the late update also is we are using the rigidbody
        //        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, currentRollAngle);

        //    }
        //    else // Not strafing
        //    {
        //        if (useRigidbody)
        //            return;

        //        // Get the current yaw
        //        Vector3 fwd = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        //        // Compute the angle
        //        float angle = Vector3.SignedAngle(fwd, lastForward, Vector3.up);

        //        // Compute rotation speed
        //        float rotSpeed = Mathf.Abs(angle) / Time.deltaTime;
        //        Debug.Log($"RotSpeed:{rotSpeed}");

        //        float rollAngle = 0;
        //        if (rotSpeed > minYawSpeed && rotSpeed < maxYawSpeed)
        //            rollAngle = Mathf.Lerp(minRollAngle, maxRollAngle, 1f - ((maxYawSpeed - rotSpeed) / (maxYawSpeed - minYawSpeed)));
        //        else if (rotSpeed >= maxYawSpeed)
        //            rollAngle = maxRollAngle;

        //        if (rollAngle > 0)
        //            rollAngle *= Mathf.Sign(angle);


        //        currentRollAngle = Mathf.MoveTowards(currentRollAngle, rollAngle, rollSpeed * Time.deltaTime);

        //        // Roll
        //        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, currentRollAngle);
        //        // Update last forward
        //        lastForward = fwd;
        //    }    



        //}

        void LateUpdate()
        {
            if (!useRigidbody)
                ComputeRollingAngle();

            // Roll in the late update also is we are using the rigidbody
            transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, currentRollAngle);

        }

        //bool turningOld = false;
        //System.DateTime startTurningTime;
        private void FixedUpdate()
        {
            if (useRigidbody)
                ComputeRollingAngle();

            //if (turningOld != strafeable.IsTurning())
            //{
            //    if (!turningOld)
            //        startTurningTime = System.DateTime.Now;

            //    turningOld = !turningOld;
            //}


            //if (strafeable.IsTurning())
            //{
            //    currentRollAngle = Mathf.MoveTowards(currentRollAngle, maxRollAngle, rollSpeed  * Time.fixedDeltaTime);
            //}
            //else
            //{
            //    currentRollAngle = Mathf.MoveTowards(currentRollAngle, 0f, rollSpeed * Time.fixedDeltaTime);
            //}
        }

        void ComputeRollingAngle()
        {
            // Compute side speed
            Vector3 pos = useRigidbody ? rb.position : transform.position;
            Vector3 dir = Vector3.ProjectOnPlane(pos - lastPosition, Vector3.up);
            // Get the component along the right axis
            Vector3 rgt = Vector3.ProjectOnPlane(transform.right, Vector3.up);
            Vector3 sideVel = Vector3.Project(dir, rgt);

            // Sign
            float sign = Mathf.Sign(Vector3.Dot(dir, rgt)) * (strafeable.IsStrafing() ? -1f : 1f);
            // Side speed
            float sideSpeed = sideVel.magnitude / (useRigidbody ? Time.fixedDeltaTime : Time.deltaTime);

            // Get minimum and maximum for interpolation 
            float min = strafeable.IsStrafing() ? minStrafeSpeed : minSideSpeed;
            float max = strafeable.IsStrafing() ? maxStrafeSpeed : maxSideSpeed;


            //// Are we turning back ?
            //Vector3 fwd = Vector3.ProjectOnPlane(transform.forward, Vector3.up); // We want to store this value anyway
            //if (!strafeable.IsStrafing() && sideSpeed < minSideSpeed)
            //{

            //    // Compute the angle
            //    float angle = Vector3.SignedAngle(fwd, lastForward, Vector3.up);

            //    // Compute rotation speed
            //    //sideSpeed = Mathf.Abs(angle) / (useRigidbody ? Time.fixedDeltaTime : Time.deltaTime);
            //    sideSpeed = angle != 0 ? maxYawSpeed : 0f;

            //    min = 0;
            //    max = maxYawSpeed;
            //    sign = -Mathf.Sign(angle);

            //    Debug.Log($"SideSpeed:{sideSpeed}");
            //}

            float rollAngle = 0;
            if (sideSpeed > min && sideSpeed < max)
                rollAngle = sign * Mathf.Lerp(minRollAngle, maxRollAngle, 1f - ((max - sideSpeed) / (max - min)));
            else if (sideSpeed >= max)
                rollAngle = sign * maxRollAngle;

            Debug.Log($"Roll:{rollAngle}");
            currentRollAngle = Mathf.MoveTowards(currentRollAngle, rollAngle, rollSpeed * (useRigidbody ? Time.fixedDeltaTime : Time.deltaTime));
           

            // Roll
            //transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, currentRollAngle);

            lastPosition = pos;
            //lastForward = fwd;
        }

        private void _FixedUpdate()
        {
            
            if (strafeable == null || !strafeable.IsStrafing() || !useRigidbody)
                return;

            // Compute side speed
            Vector3 pos = rb.position;
            Vector3 dir = Vector3.ProjectOnPlane(pos - lastPosition, Vector3.up);
            // Get the component along the right axis
            Vector3 rgt = Vector3.ProjectOnPlane(transform.right, Vector3.up);
            Vector3 sideVel = Vector3.Project(dir, rgt);
            
            float sign = -Mathf.Sign(Vector3.Dot(dir, rgt));
            float sideSpeed = sideVel.magnitude / Time.fixedDeltaTime;
            //Debug.Log($"SideSpeed:{sideSpeed}");
            float rollAngle = 0;
            if (sideSpeed > minStrafeSpeed && sideSpeed < maxStrafeSpeed)
                rollAngle = sign * Mathf.Lerp(minRollAngle, maxRollAngle, 1f - ((maxStrafeSpeed - sideSpeed) / (maxStrafeSpeed - minStrafeSpeed)));
            else if (sideSpeed >= maxStrafeSpeed)
                rollAngle = sign * maxRollAngle;

            
            currentRollAngle = Mathf.MoveTowards(currentRollAngle, rollAngle, rollSpeed * Time.fixedDeltaTime);
            

            // Roll
            //transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, currentRollAngle);

            lastPosition = pos;
        }
    }

}
