using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galacticus
{
    public abstract class FxHandler : MonoBehaviour
    {
        [SerializeField]
        List<AudioSource> sounds;

        [SerializeField]
        List<ParticleSystem> particles;

        Animator animator;
        protected Animator Animator
        {
            get { return animator; }
        }


        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();

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
            if (index < 0 || index >= particles.Count)
            {
                Debug.LogWarning($"{this.name} - No particle for index {index}");
                return;
            }

            particles[index].Play();
        }
    }

}
