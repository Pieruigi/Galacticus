using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galacticus
{
    public class ShipRoller : MonoBehaviour
    {
        [SerializeField]
        float minSideSpeed = 3;

        [SerializeField]
        float maxSideSpeed = 6;

        [SerializeField]
        float maxRoll = 30f;

        [SerializeField]
        Transform target;

        Vector3 lastPosition;

        // Start is called before the first frame update
        void Start()
        {
            lastPosition = transform.position;                
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            // Get current velocity
            Vector3 vel = (transform.position - lastPosition) / Time.fixedDeltaTime;

            // Compute side speed
            Vector3 sideVel = Vector3.Project(vel, transform.right);

            if(sideVel.magnitude > .5f)
                Debug.Log("SideVel:" + sideVel.magnitude);

            //target.transform.rotation *= Quaternion.Euler(0f, 0f, Mathf.Lerp(0, maxRoll, sideVel.magnitude / maxSideSpeed) * Vector3.Dot(transform.right, sideVel));
            target.transform.localEulerAngles = Vector3.forward * Mathf.Lerp(0, maxRoll, sideVel.magnitude / maxSideSpeed);// * Vector3.Dot(transform.right, sideVel);

            // Update position
            lastPosition = transform.position;
            

        }
    }

}
