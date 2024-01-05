using Galacticus.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Galacticus
{
    public class Health : MonoBehaviour, IDamageable
    {
        public event UnityAction OnKill;

        [SerializeField]
        float maxHealth;

        [SerializeField]
        float currentHealth;

        private void Awake()
        {
            if (currentHealth > maxHealth)
                currentHealth = maxHealth; // Just to be sure
        }

        public void ApplyDamage(float amount)
        {
            currentHealth = Mathf.Max(0f, currentHealth - amount);
            if (currentHealth == 0)
                OnKill?.Invoke();
        }

        public bool TryHeal(float amount)
        {
            if (currentHealth == maxHealth || currentHealth <= 0)
                return false;

            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
            return true;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
