using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Galacticus
{
    public class TurretFx : FxHandler
    {
        [SerializeField]
        List<GameObject> shooters;

        protected override void Awake()
        {
            base.Awake();

            GetComponent<Health>().OnKill += HandleOnKillFx;
        }

        void HandleOnKillFx()
        {
            Animator?.SetTrigger("Explode");
            
        }

        public async void TossShooters()
        {
            List<int> indices = new List<int>();
            for (int i = 0; i < shooters.Count; i++)
                indices.Add(i);

            for(int i=0; i<shooters.Count; i++)
            {
                int index = indices[Random.Range(0, indices.Count)];
                indices.Remove(index);
                Rigidbody rb = shooters[index].AddComponent<Rigidbody>();
                rb.useGravity = true;
                rb.isKinematic = false;
                rb.AddForce(new Vector3(Random.Range(-.3f, .3f), 1f, Random.Range(-.3f, .3f)) * Random.Range(4, 7), ForceMode.VelocityChange);
                rb.AddTorque(new Vector3(Random.Range(-60f, 60f), Random.Range(-60f, 60f), Random.Range(-60f, 60f)));
                Destroy(shooters[index], 5);
                await Task.Delay(System.TimeSpan.FromSeconds(Random.Range(0.05f, .2f)));
            }

           
            

        }
    }

}
