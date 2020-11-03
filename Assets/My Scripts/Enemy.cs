using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ThroughtTheGalaxy.Mechanics
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
            
        }

        void Update()
        {
            
            if (isSelected)
            {
                GetComponent<Outline>().enabled = true;
            }
            else
            {
                GetComponent<Outline>().enabled = false;
            }

            if (health <= 0)
            {
                Instantiate(explosionVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }

        public void ProcessDamage(float damage)
        {
            health -= damage;
        }
    }
}
