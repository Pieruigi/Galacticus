using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Galacticus
{
    public class MineController : MonoBehaviour
    {
        [SerializeField]
        float explosionDelay = 1f;

        [SerializeField]
        Boomer boomer;

        bool exploding = false;
        
       
        // Update is called once per frame
        void Update()
        {

        }

        private async void OnCollisionEnter(Collision collision)
        {
            if (exploding)
                return;

            exploding = true;
            await Task.Delay(System.TimeSpan.FromSeconds(explosionDelay));

            boomer.Boom();

            Destroy(gameObject, 5);

        }
    }

}
