using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


namespace ThroughtTheGalaxy.Mechanics
{
    //[ExecuteInEditMode]
    public class Gun : MonoBehaviour
    {
        public enum WeaponType {MG, MSL, NBL}
        public enum AmmoType {bullets, missile, NanoBots}
        public AudioClip[] audioClips;
        public bool hasOpenedMGFire {get; set;}


        public WeaponType currentWeapon;

        [System.Serializable]
        public struct WeaponStats
        {
            public AmmoType ammoType;
            public int ammoAmount;
            public float fireRate;
            public float damage;
            public AudioClip weaponSound;
        }

        
        [Header("General Settings")] 
        [SerializeField] MissileProjectile MSL_missileProjectile;
        [SerializeField] ParticleSystem MG_BulletParticleSystem;
        [SerializeField] NanoBots NB_nanoBots;
        [SerializeField] bool isFireOpen;


        // Audio
        AudioSource audioSource;
        float cachedTime; 


        [Header("Weaponery")] 
        [SerializeField] WeaponStats activeWeapon;
        
        RaycastHit[] targetedEnemies;
        RaycastHit[] nearestEnemies;
        RaycastHit hit;

        // Start is called before the first frame update
        void Start()
        {
            targetedEnemies = new RaycastHit[5];
            audioSource = GetComponent<AudioSource>();
            MG_BulletParticleSystem = GetComponent<ParticleSystem>();

        }

        // Update is called once per frame
        void Update()
        {
            UpdateWeaponery();
            //RayCastForTracking();
            
           
               transform.rotation = Quaternion.LookRotation(Camera.main.ViewportToWorldPoint(new Vector3(0.5f,0.5f,0f)) + 
               Camera.main.transform.forward * 25 -
                transform.position);
                Debug.DrawLine(transform.position, 
                Camera.main.ViewportToWorldPoint(new Vector3(0.5f,0.5f,0f)) + Camera.main.transform.forward * 25, Color.red);

            
            

            if (hasOpenedMGFire)
            {
                if (isFireOpen == false)
                {
                    OpenMGFire();
                    cachedTime = Time.timeSinceLevelLoad;
                }

                if (Time.timeSinceLevelLoad - cachedTime > 0.1)
                {
                    GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
                    cachedTime = Time.timeSinceLevelLoad;
                }

            }
            else
            {
                CloseMGFire();
            }
        }

        private void RayCastForTracking()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, 50, transform.TransformDirection(Vector3.forward), Mathf.Infinity);
            targetedEnemies = Array.FindAll(hits, e => e.collider.gameObject.GetComponent<Enemy>());

            for (int i = 0; i < targetedEnemies.Length - 1; i++)
            {
                for (int j = i+ 1; j < targetedEnemies.Length; j++)
                {
                    if (Vector3.Distance(targetedEnemies[i].collider.transform.position, 
                        Camera.main.ViewportToWorldPoint(new Vector3(0.5f,0.5f,0f)) + 
                        Camera.main.transform.forward * Vector3.Distance(targetedEnemies[i].collider.transform.position,Camera.main.ViewportToWorldPoint(new Vector3(0.5f,0.5f,0f)))) > 
                        
                        Vector3.Distance(targetedEnemies[j].collider.transform.position, 
                        Camera.main.ViewportToWorldPoint(new Vector3(0.5f,0.5f,0f)) + 
                        Camera.main.transform.forward * Vector3.Distance(targetedEnemies[i].collider.transform.position,Camera.main.ViewportToWorldPoint(new Vector3(0.5f,0.5f,0f)))))
                        {
                            var temp = targetedEnemies[i];
                            targetedEnemies[i] = targetedEnemies[j];
                            targetedEnemies[j] = temp;
                        }
                }
            }

            foreach (var e in targetedEnemies)
            {
                e.collider.gameObject.GetComponent<Enemy>().isSelected = false;
            }

            if (currentWeapon == WeaponType.MSL)
            {
                targetedEnemies[0].collider.gameObject.GetComponent<Enemy>().isSelected = true;
            } 

            if (currentWeapon == WeaponType.NBL)
            {
                for (int i = 0; i <= Mathf.Clamp(targetedEnemies.Length,0,5) - 1; ++i)
                {
                    targetedEnemies[i].collider.gameObject.GetComponent<Enemy>().isSelected = true;
                }
            }       
        }

        private void UpdateWeaponery()
        {
            if (currentWeapon == WeaponType.MG)
            {
                foreach (var e in targetedEnemies)
            {
                e.collider.gameObject.GetComponent<Enemy>().isSelected = false;
            }
                activeWeapon.ammoType = AmmoType.bullets;
                activeWeapon.ammoAmount = 100;
                activeWeapon.damage = 5;
                activeWeapon.fireRate = 20;
                //activeWeapon.weaponSound = 
            }
            if (currentWeapon == WeaponType.MSL)
            {
                RayCastForTracking();
                activeWeapon.ammoType = AmmoType.missile;
                activeWeapon.ammoAmount = 10;
                activeWeapon.damage = 100;
                activeWeapon.fireRate = 2;
                //activeWeapon.weaponSound = 
            }
            if (currentWeapon == WeaponType.NBL)
            {
                RayCastForTracking();
                activeWeapon.ammoType = AmmoType.NanoBots;
                activeWeapon.ammoAmount = 5;
                activeWeapon.damage = 50;
                activeWeapon.fireRate = 5;
                //activeWeapon.weaponSound = 
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

        public void LaunchMissile()
        {
            
            if (targetedEnemies.Length > 0)
            {
                MissileProjectile currentMissile = Instantiate(MSL_missileProjectile, transform.position, 
                transform.rotation);
                currentMissile.ActivateMissile(targetedEnemies[0].collider.gameObject);
            }
            
            // Launch Missile
        }

        public void ReleaseNanoBots()
        {
            for (int i = 0; i <= Mathf.Clamp(targetedEnemies.Length,0,5) - 1; ++i)
            {
                NanoBots currentNanoBot = Instantiate(NB_nanoBots, transform.position, 
                transform.rotation);
                currentNanoBot.ActivateNanoBot(targetedEnemies[i].collider.gameObject);
            }
            
        }

        void OnParticleCollision(GameObject other) 
        {
            if(other.GetComponent<Enemy>())
            {
                other.GetComponent<Enemy>().health -= 10;
            }
        }

        public void SwitchWeapon(int i)
        {
           currentWeapon = (WeaponType)(Enum.GetValues(typeof(WeaponType)).GetValue(i)); 
        }
    }
}