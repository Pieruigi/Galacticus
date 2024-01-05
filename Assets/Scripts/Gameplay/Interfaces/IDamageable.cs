using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Galacticus.Interfaces
{
    public interface IDamageable
    {

        event UnityAction OnKill;

        void ApplyDamage(float amount);
    }

}
