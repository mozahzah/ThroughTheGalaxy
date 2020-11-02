using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ThroughtTheGalaxy.Mechanics
{

    public class Enemy : MonoBehaviour
    {
        [SerializeField] GameObject explosion;

        public bool isSelected;
        public float health = 100;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
            if (isSelected){
                GetComponent<Outline>().enabled = true;
            }
            else
            {
                GetComponent<Outline>().enabled = false;
            }


            if (health <= 0)
            {
                Instantiate(explosion, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
