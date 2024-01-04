using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Galacticus
{
    public class Shooter : MonoBehaviour
    {
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

        System.DateTime lastShotTime;

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
            if ((System.DateTime.Now - lastShotTime).TotalSeconds > fireRate)
            {
                lastShotTime = System.DateTime.Now;

                await Task.Delay(System.TimeSpan.FromSeconds(fireDelay));

                int count = burstCount;
                float time = burstTime / burstCount;
                while(count > 0)
                {
                    // Create bullet
                    GameObject bullet = Instantiate(bulletPrefab);

                    bullet.transform.position = firePoint.position;
                    bullet.GetComponent<Rigidbody>().AddForce(firePower * firePoint.forward, ForceMode.VelocityChange);
                    // Update count
                    count--;
                    if (count > 0)
                        await Task.Delay(System.TimeSpan.FromSeconds(time));
                }

                
                //return true;
            }

            //return false;
        }
    }

}
