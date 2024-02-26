#define SHIP

#if ROBOT
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Galacticus
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        float acceleration = 10;

        [SerializeField]
        float deceleration = 10;

        [SerializeField]
        float maxSpeed = 8;

        [SerializeField]
        float rotationSpeed = 120f;

        Vector3 moveDirection;
        bool moving;

        CharacterController cc;
        float horizontalSpeed;

        float verticalSpeed = 0;

        private void Awake()
        {
            cc = GetComponent<CharacterController>();
        }

        private void Update()
        {
            CheckInput();

            Move();
        }

        void CheckInput()
        {
            // Get input
            Vector3 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Vector3 aimInput = new Vector2(Input.GetAxis("AimHorizontal"), Input.GetAxis("AimVertical"));

            // Moving
            if (moveInput.magnitude > 0)
            {
                moving = true;
                // Set target direction and throttle
                moveDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

            }
            else
            {
                // Reset throttle
                moving = false;
                moveDirection = Vector3.zero;
            }

        }

        void Move()
        {
            if (moving)
            {
                horizontalSpeed = Mathf.MoveTowards(horizontalSpeed, maxSpeed, acceleration * Time.deltaTime);
            }
            else
            {
                horizontalSpeed = Mathf.MoveTowards(horizontalSpeed, 0, deceleration * Time.deltaTime);
            }
            Debug.Log($"CC.IsGrounded:{cc.isGrounded}");
            Debug.Log($"speed:{horizontalSpeed}");

            if (!cc.isGrounded)
            {
                verticalSpeed += Physics.gravity.y * Time.deltaTime;
            }
            else
            {
                verticalSpeed = 0;
            }

            cc.Move(moveDirection*horizontalSpeed*Time.deltaTime + Vector3.up*verticalSpeed*Time.deltaTime);
        }
    }
}
#endif


#if SHIP
using Galacticus.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galacticus
{
    public class PlayerController : MonoBehaviour, IStrafer
    {
        [Header("Mover")]
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

        [Header("Shooter")]
        [SerializeField]
        GameObject bulletPrefab;

        [SerializeField]
        Transform firePoint;

        [SerializeField]
        float fireRate;

        [SerializeField]
        float firePower = 10;

        Rigidbody rb;
        Collider coll;

       
        Vector3 moveDirection;
        Vector3 aimDirection;

        bool aiming = false;
        bool moving = false;
        System.DateTime lastShotTime;

        //Vector3 moveInput, aimInput;

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
            Vector3 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Vector3 aimInput = new Vector2(Input.GetAxis("AimHorizontal"), Input.GetAxis("AimVertical"));

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
            float mul = 1;
            if (aiming)
                mul = .5f;
            float angle = Vector3.SignedAngle(transform.forward, aimDirection.normalized, Vector3.up);
            angle = Mathf.MoveTowardsAngle(0f, angle, rotationSpeed * mul * Time.deltaTime);
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
                if ((System.DateTime.Now - lastShotTime).TotalSeconds > fireRate)
                {
                    lastShotTime = System.DateTime.Now;
                    // Create new bullet
                    GameObject bullet = Instantiate(bulletPrefab);
                    bullet.transform.position = firePoint.position;
                    bullet.transform.rotation = firePoint.rotation;
                    Rigidbody brb = bullet.GetComponent<Rigidbody>();
                    // No collision with the shooter
                    Physics.IgnoreCollision(coll, bullet.GetComponent<Collider>(), true);
                    // Apply force
                    //brb.AddForce(transform.forward * firePower, ForceMode.VelocityChange);
                }
            }


        }

        //void CheckInput()
        //{
        //    // Get input
        //    Vector3 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //    Vector3 aimInput = new Vector2(Input.GetAxis("AimHorizontal"), Input.GetAxis("AimVertical"));

        //    // Moving
        //    if (moveInput.magnitude > 0)
        //    {
        //        moving = true;
        //        // Set target direction and throttle
        //        moveDirection = new Vector3(moveInput.normalized.x, 0f, moveInput.normalized.y);

        //    }
        //    else
        //    {
        //        // Reset throttle
        //        moving = false;
        //        moveDirection = Vector3.zero;
        //    }



        //    // Aiming

        //    if (aimInput.magnitude > 0)
        //    {
        //        aiming = true;
        //        aimDirection = new Vector3(aimInput.x, 0f, aimInput.y).normalized;
        //    }
        //    else
        //    {
        //        aiming = false;
        //        aimDirection = moveDirection;
        //    }
        //    // Get the angle between the current aiming direction and the target one
        //    float mul = 1;
        //    if (aiming)
        //        mul = .5f;
        //    float angle = Vector3.SignedAngle(transform.forward, aimDirection.normalized, Vector3.up);
        //    angle = Mathf.MoveTowardsAngle(0f, angle, rotationSpeed * mul * Time.deltaTime);
        //    // Apply rotation
        //    transform.forward = Quaternion.AngleAxis(angle, Vector3.up) * transform.forward;


        //    // Setting drag
        //    if (moving)
        //        rb.drag = movingDrag;
        //    else
        //        rb.drag = notMovingDrag;

        //    // Shoot
        //    float angleTollerance = 3;
        //    angle = Vector3.SignedAngle(transform.forward, aimDirection.normalized, Vector3.up); // Update the angle
        //    if (aiming && Mathf.Abs(angle) < angleTollerance)
        //    {
        //        if((System.DateTime.Now - lastShotTime).TotalSeconds > fireRate)
        //        {
        //            lastShotTime = System.DateTime.Now;
        //            // Create new bullet
        //            GameObject bullet = Instantiate(bulletPrefab);
        //            bullet.transform.position = firePoint.position;
        //            bullet.transform.rotation = firePoint.rotation;
        //            Rigidbody brb = bullet.GetComponent<Rigidbody>();
        //            // No collision with the shooter
        //            Physics.IgnoreCollision(coll, bullet.GetComponent<Collider>(), true);
        //            // Apply force
        //            //brb.AddForce(transform.forward * firePower, ForceMode.VelocityChange);
        //        }
        //    }


        //}

        void Move()
        {
            Vector3 target = aiming ? moveDirection : transform.forward;
            //target = moveDirection;

            float mul = 1f;
            if (!aiming)
            {
                if (Vector3.Dot(moveDirection, Vector3.ProjectOnPlane(transform.forward, Vector3.up)) < 0f)
                    mul = .5f;
            }

            if(moving)
            {
                bool applyForce = true;

                // If we are not aiming we apply force only if the angle between the forward direction and the move direction is less than 90 degrees
                if (!aiming)
                {
                    // Get the angle between the current velocity and the target direction
                    Vector3 currDirProj = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
                    Vector3 targetDirProj = Vector3.ProjectOnPlane(moveDirection, Vector3.up);
                    if(Vector3.Dot(currDirProj, targetDirProj) < 0f)
                        applyForce = false;
                }


                // Apply force
                if (applyForce)
                    rb.AddForce(target * accelerationForce * mul * (aiming ? accelerationForceAimingMul : 1f), ForceMode.Acceleration);
            }

          
            
        }

        #region IRollable implementation
        public bool IsStrafing()
        {
            return aiming && moving;
        }

       
        #endregion
    }

}
#endif