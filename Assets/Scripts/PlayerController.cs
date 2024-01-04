using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galacticus
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField]
        float accelerationForce = 4;

        [SerializeField]
        float accelerationForceAimingMul = .7f;

        [SerializeField]
        float notMovingDrag = 4;

        [SerializeField]
        float movingDrag = 2;

        [SerializeField]
        float rotationSpeed = 120f;

        [Header("Fighting")]
        [SerializeField]
        GameObject bulletPrefab;

        [SerializeField]
        Transform firePoint;

        [SerializeField]
        float fireRate;


        Rigidbody rb;
        Collider coll;

       
        Vector3 moveDirection;
        
        bool aiming = false;
        bool moving = false;
        System.DateTime lastShotTime;


        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            coll = GetComponent<Collider>();
            
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            CheckInput();
        }

        private void FixedUpdate()
        {
            Move();
        }

        void CheckInput()
        {
            // Get input
            Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Vector2 aimInput = new Vector2(Input.GetAxis("AimHorizontal"), Input.GetAxis("AimVertical"));

            // Moving
            if (moveInput.magnitude > 0)
            {
                moving = true;
                // Set target direction and throttle
                moveDirection = new Vector3(moveInput.normalized.x, 0f, moveInput.normalized.y);
               
            }
            else
            {
                // Reset throttle
                moving = false;
                moveDirection = Vector3.zero;
            }



            // Aiming
            Vector3 aimDirection;
            if (aimInput.magnitude > 0)
            {
                aiming = true;
                aimDirection = new Vector3(aimInput.x, 0f, aimInput.y).normalized;
            }
            else
            {
                aiming = false;
                aimDirection = moveDirection;
            }
            // Get the angle between the current aiming direction and the target one
            float angle = Vector3.SignedAngle(transform.forward, aimDirection.normalized, Vector3.up);
            angle = Mathf.MoveTowardsAngle(0f, angle, rotationSpeed * Time.deltaTime);
            // Apply rotation
            transform.forward = Quaternion.AngleAxis(angle, Vector3.up) * transform.forward;
           

            // Setting drag
            if (moving)
                rb.drag = movingDrag;
            else
                rb.drag = notMovingDrag;

            // Shoot
            float angleTollerance = 3;
            angle = Vector3.SignedAngle(transform.forward, aimDirection.normalized, Vector3.up); // Update the angle
            if (aiming && Mathf.Abs(angle) < angleTollerance)
            {
                if((System.DateTime.Now - lastShotTime).TotalSeconds > fireRate)
                {
                    lastShotTime = System.DateTime.Now;
                    // Create new bullet
                    GameObject bullet = Instantiate(bulletPrefab);
                    bullet.transform.position = firePoint.position;
                    Rigidbody brb = bullet.GetComponent<Rigidbody>();
                    // No collision with the shooter
                    Physics.IgnoreCollision(coll, bullet.GetComponent<Collider>(), true);
                    // Apply force
                    brb.AddForce(transform.forward * 10, ForceMode.VelocityChange);
                }
            }

            
        }

        void Move()
        {
            Vector3 target = aiming ? moveDirection : transform.forward;
            target = moveDirection;

            if(moving)
            {
                // Apply force
                rb.AddForce(target * accelerationForce * (aiming ? accelerationForceAimingMul : 1f), ForceMode.Acceleration);
            }

          
            
        }
    }

}
