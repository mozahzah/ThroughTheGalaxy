using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using unciphering.Characters;

namespace unciphering.Mechanics
{
    public class MissileProjectile : MonoBehaviour
    {
        // Utility Params
        GameObject targetedEnemy;
        bool isMissileLaunched;

        float floatingTime = 1.5f;
        float initialTime;
        float lifeTime;

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
            lifeTime = Time.timeSinceLevelLoad;
            audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioClips[0]);
        }

        
        void Update()
        {
            if (isMissileLaunched)
            {
                OnMissileLaunch();
            }

            if (Time.timeSinceLevelLoad - lifeTime > 10)
            {
                Destroy(gameObject);
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
                    if (targetedEnemy == null) return; 
                    transform.rotation = Quaternion.LookRotation(targetedEnemy.transform.position - transform.position);
                    audioSource.PlayOneShot(audioClips[1]);
                    hasFxPlayed = true;
                }
                else 
                {
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
