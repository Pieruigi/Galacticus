using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Galacticus
{
    public class ShooterFx : FxHandler
    {
        Shooter shooter;

        protected override void Awake()
        {
            base.Awake();
            shooter = GetComponent<Shooter>();
            shooter.OnShoot += HandleOnShoot;            
        }


        void HandleOnShoot()
        {
            if(Animator)
                Animator.SetTrigger("Shoot");
        }

       
    }



}
