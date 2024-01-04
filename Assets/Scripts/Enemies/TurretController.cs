using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galacticus
{
    public class TurretController : MonoBehaviour
    {
        [SerializeField]
        Transform turret;

        [SerializeField]
        float rotationSpeed = 120f;

        [SerializeField]
        float aimingRange = 10;

        Transform target;
        Collider targetCollider;
        bool disabled = false;

        // Start is called before the first frame update
        void Start()
        {
            target = GameObject.FindGameObjectWithTag(Tags.Player).transform;
            targetCollider = target.GetComponent<Collider>();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (disabled)
                return;


            if(Vector3.Distance(Vector3.ProjectOnPlane(turret.position, Vector3.up), target.position) < aimingRange)
            {
                // Aiming target
                // Check if the turret is aiming the target
                RaycastHit hitInfo;
                Ray ray = new Ray(turret.position, turret.forward);
                bool aim = true;
                if (Physics.Raycast(ray, out hitInfo, aimingRange))
                {
                    if (hitInfo.collider.CompareTag(Tags.Player))
                    {
                        aim = false;
                        Debug.Log("TryShoot()");
                    }
                    
                }

                // Not aiming
                if (aim)
                {
                    // Get the angle
                    float angle = Vector3.SignedAngle(turret.forward, Vector3.ProjectOnPlane(target.position - turret.position, Vector3.up), Vector3.up);
                    angle = Mathf.MoveTowardsAngle(0f, angle, rotationSpeed * Time.deltaTime);
                    Debug.Log($"Angle:{angle}");
                    turret.forward = Quaternion.AngleAxis(angle, Vector3.up) * turret.forward;
                }
            }

            

        }

     
    }

}
