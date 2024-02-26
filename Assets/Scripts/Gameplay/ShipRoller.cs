using Galacticus.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galacticus
{
    public class ShipRoller : MonoBehaviour
    {
        [SerializeField]
        float minRotSpeed = 0;

        [SerializeField]
        float maxRotSpeed = 30;

        [SerializeField]
        float minSideSpeed = 1;

        [SerializeField]
        float maxSideSpeed = 3;

        [SerializeField]
        float minStrafeSpeed = 3;

        [SerializeField]
        float maxStrafeSpeed = 6;

        [SerializeField]
        float minRollAngle = 20f;

        [SerializeField]
        float maxRollAngle = 40f;

        [SerializeField]
        float rollSpeed = 80f;

        [SerializeField]
        bool useRigidbody;

        float currentRollAngle;
        Vector3 lastPosition;
        IStrafer strafeable;
        Rigidbody rb;
        Vector3 lastForward;

        private void Awake()
        {
            strafeable = GetComponent<IStrafer>();
            if (useRigidbody)
                rb = GetComponent<Rigidbody>();
        }

        // Start is called before the first frame update
        void Start()
        {
            lastPosition = transform.position;
        }


        void LateUpdate()
        {
            if (!useRigidbody)
                ComputeRollingAngle();

           
            // Roll in the late update also is we are using the rigidbody
            transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, currentRollAngle);

        }


        private void FixedUpdate()
        {
            if (useRigidbody)
                ComputeRollingAngle();
           
        }

        void ComputeRollingAngle()
        {
            float min, max;

            if (strafeable.IsStrafing())
            {
                // Compute side speed
                Vector3 pos = useRigidbody ? rb.position : transform.position;
                Vector3 dir = Vector3.ProjectOnPlane(pos - lastPosition, Vector3.up);
                // Get the component along the right axis
                Vector3 rgt = Vector3.ProjectOnPlane(transform.right, Vector3.up);
                Vector3 sideVel = Vector3.Project(dir, rgt);

                // Sign
                float sign = -1f * Mathf.Sign(Vector3.Dot(dir, rgt));
                // Side speed
                float sideSpeed = sideVel.magnitude / (useRigidbody ? Time.fixedDeltaTime : Time.deltaTime);

                min = minStrafeSpeed;
                max = maxStrafeSpeed;

                float rollAngle = 0;
                if (sideSpeed > min && sideSpeed < max)
                    rollAngle = sign * Mathf.Lerp(minRollAngle, maxRollAngle, 1f - ((max - sideSpeed) / (max - min)));
                else if (sideSpeed >= max)
                    rollAngle = sign * maxRollAngle;

                currentRollAngle = Mathf.MoveTowards(currentRollAngle, rollAngle, rollSpeed * (useRigidbody ? Time.fixedDeltaTime : Time.deltaTime));

                lastPosition = pos;
            }
            else
            {
                // Get the current forward direction
                Vector3 fwd = transform.forward;
                fwd = Vector3.ProjectOnPlane(fwd, Vector3.up);

                // Get angle between fwd and last fwd
                float angle = Vector3.SignedAngle(fwd, lastForward, Vector3.up);
                float rotSpeed = Mathf.Abs(angle) / (useRigidbody ? Time.fixedDeltaTime : Time.deltaTime);

                

                // Compute side speed
                Vector3 pos = useRigidbody ? rb.position : transform.position;
                Vector3 dir = Vector3.ProjectOnPlane(pos - lastPosition, Vector3.up);
                // Get the component along the right axis
                Vector3 rgt = Vector3.ProjectOnPlane(transform.right, Vector3.up);
                Vector3 sideVel = Vector3.Project(dir, rgt);
                // Sign
                float sign = -1f * Mathf.Sign(Vector3.Dot(dir, rgt));
                // Side speed
                float sideSpeed = sideVel.magnitude / (useRigidbody ? Time.fixedDeltaTime : Time.deltaTime);

                bool useRot = ((maxRotSpeed - rotSpeed) / (maxRotSpeed - minRotSpeed)) < ((maxSideSpeed - sideSpeed) / (maxSideSpeed - minSideSpeed));
                //useRot = true;
                min = useRot ? minRotSpeed : minSideSpeed;
                max = useRot ? maxRotSpeed : maxSideSpeed;

                Debug.Log($"UsingRot:{useRot}");

                float rollAngle = 0;
                if (rotSpeed > min && rotSpeed < max)
                    rollAngle = Mathf.Sign(angle) * Mathf.Lerp(minRollAngle, maxRollAngle, 1f - ((max - rotSpeed) / (max - min)));
                else if (rotSpeed >= max)
                    rollAngle = Mathf.Sign(angle) * maxRollAngle;

                currentRollAngle = Mathf.MoveTowards(currentRollAngle, rollAngle, rollSpeed * (useRigidbody ? Time.fixedDeltaTime : Time.deltaTime));

                lastForward = fwd;
            }            


            //// Compute side speed
            //Vector3 pos = useRigidbody ? rb.position : transform.position;
            //Vector3 dir = Vector3.ProjectOnPlane(pos - lastPosition, Vector3.up);
            //// Get the component along the right axis
            //Vector3 rgt = Vector3.ProjectOnPlane(transform.right, Vector3.up);
            //Vector3 sideVel = Vector3.Project(dir, rgt);

            //// Sign
            //float sign = Mathf.Sign(Vector3.Dot(dir, rgt)) * (strafeable.IsStrafing() ? -1f : 1f);
            //// Side speed
            //float sideSpeed = sideVel.magnitude / (useRigidbody ? Time.fixedDeltaTime : Time.deltaTime);

            //// Get minimum and maximum for interpolation 
            //float min = strafeable.IsStrafing() ? minStrafeSpeed : minSideSpeed;
            //float max = strafeable.IsStrafing() ? maxStrafeSpeed : maxSideSpeed;



            //float rollAngle = 0;
            //if (sideSpeed > min && sideSpeed < max)
            //    rollAngle = sign * Mathf.Lerp(minRollAngle, maxRollAngle, 1f - ((max - sideSpeed) / (max - min)));
            //else if (sideSpeed >= max)
            //    rollAngle = sign * maxRollAngle;

            //currentRollAngle = Mathf.MoveTowards(currentRollAngle, rollAngle, rollSpeed * (useRigidbody ? Time.fixedDeltaTime : Time.deltaTime));


            

        }

        //void ComputeRollingAngle()
        //{
        //    // Compute side speed
        //    Vector3 pos = useRigidbody ? rb.position : transform.position;
        //    Vector3 dir = Vector3.ProjectOnPlane(pos - lastPosition, Vector3.up);
        //    // Get the component along the right axis
        //    Vector3 rgt = Vector3.ProjectOnPlane(transform.right, Vector3.up);
        //    Vector3 sideVel = Vector3.Project(dir, rgt);

        //    // Sign
        //    float sign = Mathf.Sign(Vector3.Dot(dir, rgt)) * (strafeable.IsStrafing() ? -1f : 1f);
        //    // Side speed
        //    float sideSpeed = sideVel.magnitude / (useRigidbody ? Time.fixedDeltaTime : Time.deltaTime);

        //    // Get minimum and maximum for interpolation 
        //    float min = strafeable.IsStrafing() ? minStrafeSpeed : minSideSpeed;
        //    float max = strafeable.IsStrafing() ? maxStrafeSpeed : maxSideSpeed;



        //    float rollAngle = 0;
        //    if (sideSpeed > min && sideSpeed < max)
        //        rollAngle = sign * Mathf.Lerp(minRollAngle, maxRollAngle, 1f - ((max - sideSpeed) / (max - min)));
        //    else if (sideSpeed >= max)
        //        rollAngle = sign * maxRollAngle;

        //    currentRollAngle = Mathf.MoveTowards(currentRollAngle, rollAngle, rollSpeed * (useRigidbody ? Time.fixedDeltaTime : Time.deltaTime));


        //    lastPosition = pos;

        //}


    }

}
