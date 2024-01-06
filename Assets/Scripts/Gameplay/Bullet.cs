using Galacticus.Interfaces;
using System.Collections;
using UnityEngine;

namespace Galacticus
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        float lifeTime = 2f;

        [SerializeField]
        float damage = 10;

        [SerializeField]
        float speed = 20;

        [SerializeField]
        ParticleSystem explosionParticle;

        Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Start is called before the first frame update
        void Start()
        {
            rb.velocity = transform.forward * speed;
            StartCoroutine(Expire());
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator Expire()
        {
            yield return new WaitForSeconds(lifeTime);
            rb.velocity = Vector3.zero;
            GetComponentInChildren<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject, 1f);
        }

        private void OnCollisionEnter(Collision collision)
        {
            rb.velocity = Vector3.zero;
            GetComponentInChildren<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            explosionParticle.Play();
            Destroy(gameObject, 2f);
            // Apply damage
            collision.collider.GetComponent<IDamageable>()?.ApplyDamage(damage);
            


        }

        

    }

}
