using System.Collections;
using System.Collections.Generic;
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

            //Explode();

            Destroy(gameObject, 1f);
        }

        
    }

}
