using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galacticus
{
    public class MineFx : FxHandler
    {
        protected override void Awake()
        {
            base.Awake();
            GetComponentInChildren<Boomer>().OnBoom += HandleOnBoom;
        }
        
        void HandleOnBoom()
        {
            Debug.Log("Boooooom");
            PlayParticle(0);
        }
    }

}
