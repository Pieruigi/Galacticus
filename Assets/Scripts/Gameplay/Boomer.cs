using Galacticus.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Galacticus
{
    public class Boomer : MonoBehaviour
    {
        public UnityAction OnBoom;

        [SerializeField]
        float damage = 50;

        List<IDamageable> targets = new List<IDamageable>();


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Boom()
        {
            // Apply damage to all damageables in the trigger
            foreach (IDamageable d in targets)
                d.ApplyDamage(damage);


            OnBoom?.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            IDamageable d = other.GetComponent<IDamageable>();
            if (d != null && !targets.Contains(d))
                targets.Add(d);

        }

        private void OnTriggerExit(Collider other)
        {
            IDamageable d = other.GetComponent<IDamageable>();
            if (d != null && targets.Contains(d))
                targets.Remove(d);
        }
    }

}
