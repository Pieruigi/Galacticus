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

        [SerializeField]
        float minRollAngle = 10f;

        [SerializeField]
        float maxRollAngle = 30f;

        [SerializeField]
        float rollSpeed = 80f;

        [SerializeField]
        bool useRigidbody;

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


         
            float rollAngle = 0;
            if (sideSpeed > min && sideSpeed < max)
                rollAngle = sign * Mathf.Lerp(minRollAngle, maxRollAngle, 1f - ((max - sideSpeed) / (max - min)));
            else if (sideSpeed >= max)
                rollAngle = sign * maxRollAngle;

            currentRollAngle = Mathf.MoveTowards(currentRollAngle, rollAngle, rollSpeed * (useRigidbody ? Time.fixedDeltaTime : Time.deltaTime));
           

            lastPosition = pos;
            
        }

       
    }

}
