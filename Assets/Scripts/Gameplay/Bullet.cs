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

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Expire());
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator Expire()
        {
            yield return new WaitForSeconds(lifeTime);

            GetComponentInChildren<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject, 1f);
        }

        private void OnCollisionEnter(Collision collision)
        {
            GetComponentInChildren<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject, 1f);
            // Apply damage
            collision.collider.GetComponent<IDamageable>()?.ApplyDamage(damage);
            


        }

        

    }

}
