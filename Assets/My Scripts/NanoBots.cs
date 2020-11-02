using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ThroughtTheGalaxy.Mechanics
{
    public class NanoBots : MonoBehaviour
    {
        // Utility Params
        bool isNanoLaunched;
        GameObject targetedEnemy;
        float floatingTime = 2;
        float initialTime;
        float initialTimeForDestruction;
        Enemy currentEnemy;
        bool destructionActivated;

        // Visual Params
        [SerializeField] ParticleSystem _particleSystem;
        Light myLight;
        bool hasFxPlayed = false;
        
        // Audio Params
        
        
        void Start()
        {
            initialTime = Time.timeSinceLevelLoad;
            myLight = GetComponent<Light>();
        }

        void Update()
        {
            // Blink Light
            myLight.intensity = Mathf.Sin(10 * Time.timeSinceLevelLoad);

            if (isNanoLaunched)
            {
                OnNanoRelease();
            }
            if (destructionActivated)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                if (Time.timeSinceLevelLoad - initialTimeForDestruction > 2)
                {
                    currentEnemy.health = 0;
                    Destroy(gameObject);
                }
            }
        }


        // Methods
        public void ActivateNanoBot(GameObject enemy)
        {
            isNanoLaunched = true;
            targetedEnemy = enemy;
        }
        public void OnNanoRelease()
        {
            if (Time.timeSinceLevelLoad - initialTime < floatingTime)
            {
                GetComponent<Rigidbody>().AddForce(transform.up * 25 * Time.deltaTime);
            }
            else
            {
                if (!hasFxPlayed)
                {
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                    hasFxPlayed = true;
                }
                else 
                {
                    transform.rotation = Quaternion.LookRotation(targetedEnemy.transform.position - transform.position);
                    GetComponent<Rigidbody>().AddForce(transform.forward * 500 * Time.deltaTime);
                }
                
            } 
        }   
        private void OnCollisionEnter(Collision other) 
        {
            if (other.gameObject.GetComponent<Enemy>())
            {
                currentEnemy = other.gameObject.GetComponent<Enemy>();
                initialTimeForDestruction = Time.timeSinceLevelLoad;
                if (_particleSystem != null)
                {
                    _particleSystem.Play();
                }
                else
                {
                    destructionActivated = true;
                }
            }
            
        }
    }
}

