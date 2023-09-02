using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DA
{
    public class DamagePlayer : MonoBehaviour
    {
        public int damage = 25;

        private void OnTriggerEnter(Collider other)
        {
            PlayerStats PlayerStats = other.GetComponent<PlayerStats>();
            if(PlayerStats != null)
            {
                PlayerStats.TakeDamage(damage);
            }
        }
    }
}
