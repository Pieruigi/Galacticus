using System.Collections;
using UnityEngine;

namespace Galacticus
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        float lifeTime = 2f;

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

            Explode();
        }

        private void OnCollisionEnter(Collision collision)
        {
            Explode(); 
        }

        void Explode()
        {
            GetComponentInChildren<MeshRenderer>().enabled = false;
            Destroy(gameObject, 1f);
        }

    }

}
