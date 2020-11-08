using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using unciphering.Characters;

namespace unciphering.Mechanics
{
    //[ExecuteInEditMode]
    public class Gun : MonoBehaviour
    {
        // Core Weapon Settings
        public enum WeaponType {MG, MSL, NB}
        public WeaponType currentWeapon;
        [System.Serializable]
        public struct WeaponStats
        {
            public int ammoAmount;
            public float damage;
            public AudioClip onLoadWeaponSound;
        }

        // Core Weapon Instances
        [Header("Weaponery")] 
        [SerializeField] WeaponStats MG_WeaponStats;
        [SerializeField] WeaponStats MSL_WeaponStats;
        [SerializeField] WeaponStats NB_WeaponStats;

        // PreFab Settings
        [Header("External GameObject Settings")] 
        [SerializeField] ParticleSystem MG_BulletParticleSystem;
        [SerializeField] MissileProjectile MSL_MissileProjectile;
        [SerializeField] NanoBots NB_NanoBots;
        [SerializeField] bool isFireOpen;

        // Audio Params
        AudioSource audioSource;

        // RayCasting Params
        RaycastHit[] targetedEnemies;

        // MG Params
        [Header("Specific to MG Weapon")]
        [SerializeField] AudioClip MG_ShotSound;
        public bool hasOpenedMGFire {get; set;}
        float cachedTime;

        void Start()
        {
            targetedEnemies = new RaycastHit[5];
            audioSource = GetComponent<AudioSource>();
            MG_BulletParticleSystem = GetComponent<ParticleSystem>();
            SetMGWeaponStats();
            RayCastForTracking();
        }

        void Update()
        {
            CrossairAim();
            ProcessMGFire();
            SetWeaponRayCast();
        }

        // Aiming and Targeting
        private void CrossairAim()
        {
            transform.rotation = Quaternion.LookRotation(Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)) +
                           Camera.main.transform.forward * 25 -
                            transform.position);
            Debug.DrawLine(transform.position,
            Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)) + Camera.main.transform.forward * 25, Color.red);
        }
        private void SetWeaponRayCast()
        {
            if (currentWeapon == WeaponType.MSL || currentWeapon == WeaponType.NB)
            {
                RayCastForTracking();
            }
            else
            {
                TurnOffTargeting();
            }
        }
        private void RayCastForTracking()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, 50, transform.TransformDirection(Vector3.forward), Mathf.Infinity);
            targetedEnemies = Array.FindAll(hits, e => e.collider.gameObject.GetComponent<Enemy>());

            // Array Sorting by Distance
            for (int i = 0; i < targetedEnemies.Length - 1; i++)
            {
                for (int j = i + 1; j < targetedEnemies.Length; j++)
                {
                    if (Vector3.Distance(targetedEnemies[i].collider.transform.position,
                        Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)) +
                        Camera.main.transform.forward * Vector3.Distance(targetedEnemies[i].collider.transform.position, Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)))) >

                        Vector3.Distance(targetedEnemies[j].collider.transform.position,
                        Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)) +
                        Camera.main.transform.forward * Vector3.Distance(targetedEnemies[i].collider.transform.position, Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)))))
                    {
                        var temp = targetedEnemies[i];
                        targetedEnemies[i] = targetedEnemies[j];
                        targetedEnemies[j] = temp;
                    }
                }
            }


            TurnOffTargeting();
            // Weapon Specific Targetting Methods
            if (currentWeapon == WeaponType.MSL)
            {
                if (targetedEnemies != null || targetedEnemies.Length != 0)
                {
                    targetedEnemies[0].collider.gameObject.GetComponent<Enemy>().isSelected = true;
                }
            }
            if (currentWeapon == WeaponType.NB)
            {
                if (targetedEnemies != null || targetedEnemies.Length != 0)
                {   
                    for (int i = 0; i <= Mathf.Clamp(targetedEnemies.Length, 0, 5) - 1; ++i)
                    {
                        targetedEnemies[i].collider.gameObject.GetComponent<Enemy>().isSelected = true;
                    }
                } 
            }
        }       
        private void TurnOffTargeting()
        {
            foreach (var e in targetedEnemies)
            {
                e.collider.gameObject.GetComponent<Enemy>().isSelected = false;
            }
            
        }

        // Firing Methods
        void OpenMGFire()
        {
            MG_BulletParticleSystem.Play();
            isFireOpen = true;
        }
        void CloseMGFire()
        {
            MG_BulletParticleSystem.Stop();
            isFireOpen = false;
        }
        public void ReleaseMissile()
        { 
            if (targetedEnemies.Length > 0)
            {
                MissileProjectile currentMissile = Instantiate(MSL_MissileProjectile, transform.position, 
                transform.rotation);

                currentMissile.ammount = MSL_WeaponStats.ammoAmount;
                currentMissile.damage = MSL_WeaponStats.damage;

                currentMissile.ActivateMissile(targetedEnemies[0].collider.gameObject);
            }
        }
        public void ReleaseNanoBots()
        {
            for (int i = 0; i <= Mathf.Clamp(targetedEnemies.Length,0,5) - 1; ++i)
            {
                NanoBots currentNanoBot = Instantiate(NB_NanoBots, transform.position, 
                transform.rotation);

                currentNanoBot.ammount = NB_WeaponStats.ammoAmount;
                currentNanoBot.damage = NB_WeaponStats.damage;

                currentNanoBot.ActivateNanoBot(targetedEnemies[i].collider.gameObject);
            }
        }

        // Weapon Management
        public void SwitchWeapon(int i)
        {
           currentWeapon = (WeaponType)(Enum.GetValues(typeof(WeaponType)).GetValue(i));
           if (currentWeapon == WeaponType.MG)
           {
                audioSource.PlayOneShot(MG_WeaponStats.onLoadWeaponSound);
           } 
           if (currentWeapon == WeaponType.MSL)
           {
               audioSource.PlayOneShot(MSL_WeaponStats.onLoadWeaponSound);
           } 
           if (currentWeapon == WeaponType.NB)
           {
               audioSource.PlayOneShot(NB_WeaponStats.onLoadWeaponSound);
               
           } 
        }
        
        // For Machine Gun
        private void SetMGWeaponStats()
        {
            MG_WeaponStats.ammoAmount = 100;
            MG_WeaponStats.damage = 20;
        }
        private void ProcessMGFire()
        {
            if (hasOpenedMGFire)
            {
                if (isFireOpen == false)
                {
                    OpenMGFire();
                    cachedTime = Time.timeSinceLevelLoad;
                }
                if (Time.timeSinceLevelLoad - cachedTime > 0.2)
                {
                    GetComponent<AudioSource>().PlayOneShot(MG_ShotSound);
                    cachedTime = Time.timeSinceLevelLoad;
                }
            }
            else
            {
                CloseMGFire();
            }
        }
        void OnParticleCollision(GameObject other) 
        {
            if(other.GetComponent<Enemy>())
            {
                other.GetComponent<Enemy>().ProcessDamage(MG_WeaponStats.damage);
            }
        }
    }
}