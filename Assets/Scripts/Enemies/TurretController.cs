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

        [SerializeField]
        float fireRate = 1f;

        [SerializeField]
        float firePower = 10f;

        [SerializeField]
        bool shootAllAtOnce = false;

        [SerializeField]
        List<Transform> firePoints;

        [SerializeField]
        GameObject bulletPrefab;

        Transform target;
        Collider targetCollider;
        bool disabled = false;
        int lastBarrelIndex = 0;
        System.DateTime lastShotTime;

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
                        if((System.DateTime.Now - lastShotTime).TotalSeconds > fireRate)
                        {
                            lastShotTime = System.DateTime.Now;
                            Shoot();
                        }
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

        void Shoot()
        {
            if (shootAllAtOnce)
            {
                // Create a bullet for each barrel and shoot all
                //for(int i=0; i<firePoints)
            }
            else
            {
                // Create bullet
                GameObject bullet = Instantiate(bulletPrefab);

                if (firePoints.Count == 1)
                    lastBarrelIndex = 0;
                else
                    lastBarrelIndex = (lastBarrelIndex + 1) % firePoints.Count;

                bullet.transform.position = firePoints[lastBarrelIndex].position;
                bullet.GetComponent<Rigidbody>().AddForce(firePower * firePoints[lastBarrelIndex].forward, ForceMode.VelocityChange);
            }
            


        }
     
    }

}
