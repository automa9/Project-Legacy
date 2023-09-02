using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DA
{
    public class PlayerStats : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        public Healthbar healthbar;

        void Start()
        {
            maxHealth = SetMaxFromHealthLevel();
            currentHealth = maxHealth;
            healthbar.SetMaxHealth(maxHealth);
        }

        private int SetMaxFromHealthLevel()
        {
            maxHealth = healthLevel * 5;
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth = currentHealth - damage;
            healthbar.SetCurrentHealth(currentHealth);
        }
    }
}