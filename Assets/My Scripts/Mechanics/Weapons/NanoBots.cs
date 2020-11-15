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

        [SerializeField] float speed = 30;
        float floatingTime = 2;
        float initialTime;
        float initialTimeForDestruction;
        float lifeTime;

        bool destructionActivated;
        bool isNanoLaunched;

        // From Gun.cs
        public float damage{get;set;}
        public int ammount{get;set;}
        public float reloadTime{get;set;}

        // Visual Params
        Light myLight;
        bool hasFxPlayed = false;
        
        // Audio Params
        AudioSource audioSource;
        [SerializeField] AudioClip[] audioClips;
        
        void Start()
        {
            initialTime = Time.timeSinceLevelLoad;
            lifeTime = Time.timeSinceLevelLoad;
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
                if (Time.timeSinceLevelLoad - initialTimeForDestruction > 2)
                {
                    currentEnemy.ProcessDamage(damage);
                    Destroy(gameObject);
                }
            }


            if (Time.timeSinceLevelLoad - lifeTime > 10)
            {
                Destroy(gameObject);
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
                GetComponent<Rigidbody>().
                AddForce((transform.up + new Vector3(Random.Range(-5,5),0,Random.Range(-5,5)).normalized) * 1 * Time.deltaTime, ForceMode.Impulse);
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
                    if (targetedEnemy == null) return;
                    if (Vector3.Distance(transform.position, targetedEnemy.transform.position) < 10)
                    {
                        GetComponent<Rigidbody>().velocity = Vector3.zero;
                        transform.position = targetedEnemy.transform.position;
                    }
                    else 
                    {
                        transform.LookAt(targetedEnemy.transform.position);
                        transform.position = Vector3.MoveTowards(transform.position, targetedEnemy.transform.position, speed*Time.deltaTime);
                    }
                    
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

