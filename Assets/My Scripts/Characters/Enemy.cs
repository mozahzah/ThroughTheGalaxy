using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace unciphering.Characters
{
    public class Enemy : MonoBehaviour
    {
        // Utility Params
        public bool isSelected;
        public float health = 100;

        // Visual Params
        [SerializeField] GameObject explosionVFX;

        // Audio Params
        /* Managed By the VSX Explosion*/

        void Start()
        {
            GetComponent<EnemyTag>().enabled = false;
        }

        void Update()
        {
            UpdateSelection();

            if (health <= 0)
            {
                Instantiate(explosionVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }

        private void UpdateSelection()
        {
            if (isSelected)
            {
                GetComponent<Outline>().enabled = true;
            }
            else
            {
                GetComponent<Outline>().enabled = false;
            }
        }

        public void TagEnemy()
        {
            GetComponent<EnemyTag>().enabled = true;
        }

        public void ProcessDamage(float damage)
        {
            health -= damage;
        }
    }
}
