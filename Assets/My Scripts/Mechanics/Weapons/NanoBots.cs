using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using unciphering.Characters;

namespace unciphering.Mechanics
{
    public class NanoBots : MonoBehaviour
    {
        // Utility Params
        GameObject targetedEnemy;
        Enemy currentEnemy;

        float floatingTime = 2;
        float initialTime;
        float initialTimeForDestruction;

        bool destructionActivated;
        bool isNanoLaunched;

        // From Gun.cs
        public float damage{get;set;}
        public int ammount{get;set;}

        // Visual Params
        Light myLight;
        bool hasFxPlayed = false;
        
        // Audio Params
        AudioSource audioSource;
        [SerializeField] AudioClip[] audioClips;
        
        void Start()
        {
            initialTime = Time.timeSinceLevelLoad;
            myLight = GetComponent<Light>();
            audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioClips[0]);
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
                    currentEnemy.ProcessDamage(damage);
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
                    audioSource.PlayOneShot(audioClips[1], 0.5f);
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
                destructionActivated = true;
            }
            
        }
    }
}

