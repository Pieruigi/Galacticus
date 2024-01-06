using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Galacticus
{
    public class Shooter : MonoBehaviour
    {
        public UnityAction OnShoot;

        [SerializeField]
        float fireRate = 1f;
     
        [SerializeField]
        float firePower = 10f;

        [SerializeField]
        float fireDelay = 0;

        [SerializeField]
        int burstCount = 1;

        [SerializeField]
        float burstTime = 0f;

        [SerializeField]
        Transform firePoint;

        [SerializeField]
        GameObject bulletPrefab;

        [SerializeField]
        List<Collider> avoiders;

        System.DateTime lastShotTime;
        bool disabled = false;
        public bool Disabled
        {
            get { return disabled; }
            set { disabled = value; }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public async void Shoot()
        {
            if (disabled)
                return;

            if ((System.DateTime.Now - lastShotTime).TotalSeconds > fireRate)
            {
                lastShotTime = System.DateTime.Now;

                await Task.Delay(System.TimeSpan.FromSeconds(fireDelay));

                if (disabled)
                    return;

                int count = burstCount;
                float time = burstTime / burstCount;
                while(count > 0)
                {
                    // Create bullet
                    GameObject bullet = Instantiate(bulletPrefab);

                    // Disable some collisions
                    Collider bColl = bullet.GetComponent<Collider>();
                    foreach (Collider c in avoiders)
                        Physics.IgnoreCollision(bColl, c, true);

                    bullet.transform.position = firePoint.position;
                    bullet.transform.rotation = firePoint.rotation;
                    //bullet.GetComponent<Rigidbody>().AddForce(firePower * firePoint.forward, ForceMode.VelocityChange);
                    // Update count
                    count--;

                    OnShoot?.Invoke();

                    if (count > 0)
                        await Task.Delay(System.TimeSpan.FromSeconds(time));
                }

                
                //return true;
            }

            //return false;
        }
    }

}
