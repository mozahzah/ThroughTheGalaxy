using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ThroughtTheGalaxy.Mechanics
{
    public class MissileProjectile : MonoBehaviour
    {
        // Utility Params
        GameObject targetedEnemy;
        bool isMissileLaunched;

        float floatingTime = 1.5f;
        float initialTime;

        // For Gun.cs
        public float damage{get;set;}
        public int ammount{get;set;}

        // Visual Params
        [SerializeField] ParticleSystem _particleSystem;
        bool hasFxPlayed = false;

        // Audio Params
        AudioSource audioSource;
        [SerializeField] AudioClip[] audioClips;

        void Start()
        {
            initialTime = Time.timeSinceLevelLoad;
            audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioClips[0]);
        }

        
        void Update()
        {
            if (isMissileLaunched)
            {
                OnMissileLaunch();
            }
        }

        public void OnMissileLaunch()
        {
            if (Time.timeSinceLevelLoad - initialTime < floatingTime)
            {
                GetComponent<Rigidbody>().AddForce(transform.up * 50 * Time.deltaTime);
            }
            else
            {
                if (!hasFxPlayed)
                {
                    _particleSystem.Play();
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                    transform.rotation = Quaternion.LookRotation(targetedEnemy.transform.position - transform.position);
                    audioSource.PlayOneShot(audioClips[1]);
                    hasFxPlayed = true;
                }
                else 
                {
                    Debug.Log(targetedEnemy.name);
                    
                    //GetComponent<Rigidbody>().AddForce((targetedEnemy.transform.position - transform.position).normalized * 50 * Time.deltaTime);
                    GetComponent<Rigidbody>().AddForce(transform.forward * 5000 * Time.deltaTime);
                }
                
            } 
        }

        public void ActivateMissile(GameObject enemy)
        {
            isMissileLaunched = true;
            targetedEnemy = enemy;
        }

        private void OnCollisionEnter(Collision other) 
        {
            if (other.gameObject.GetComponent<Enemy>())
            {
                other.gameObject.GetComponent<Enemy>().ProcessDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
