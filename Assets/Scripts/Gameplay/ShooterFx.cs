using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galacticus
{
    public class ShooterFx : MonoBehaviour
    {
        [SerializeField]
        List<AudioSource> sounds;

        [SerializeField]
        List<ParticleSystem> particles;
        
        Animator animator;
        Shooter shooter;

        private void Awake()
        {
            shooter = GetComponent<Shooter>();
            animator = GetComponent<Animator>();
            shooter.OnShoot += HandleOnShoot;            
        }


        void HandleOnShoot()
        {
            if(animator)
                animator.SetTrigger("Shoot");
        }

        public void PlaySound(int index)
        {
            if (index < 0 || index >= sounds.Count)
            {
                Debug.LogWarning($"{this.name} - No sound for index {index}");
                return;
            }

            sounds[index].Play();

        }

        public void PlayParticle(int index)
        {
            if (index < 0 || index >= sounds.Count)
            {
                Debug.LogWarning($"{this.name} - No particle for index {index}");
                return;
            }

            particles[index].Play();
        }
    }



}
