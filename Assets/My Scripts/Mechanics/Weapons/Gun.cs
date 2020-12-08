using System;
using UnityEngine;
using System.Linq;
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
            public float reloadTime;
        }

        // Core Weapon Instances
        [Header("Weaponery")] 
        [SerializeField] public WeaponStats MG_WeaponStats;
        [SerializeField] public WeaponStats MSL_WeaponStats;
        [SerializeField] public WeaponStats NB_WeaponStats;

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
        GameObject targetedEnemyCache;
        Int32 layerMask; /*Bit Shift to Channel 11 (targets)*/

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
            //SetMGWeaponStats();
            layerMask = 1 << 11;
        }

        private void FixedUpdate() 
        {
            SetWeaponRayCast();
        }

        void Update()
        {
            CrossairAim();
            ProcessMGFire();
        }

        // Aiming and Targeting
        private void CrossairAim()
        {
            Vector3 crossairLocation = Camera.main.
            ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;
            //Debug.DrawLine(crossairLocation, Camera.main.transform.TransformDirection(Vector3.forward) * 10000, Color.red);
            if (Physics.Linecast(crossairLocation, Camera.main.transform.TransformDirection(Vector3.forward) * 10000, out hit, layerMask))
            {
                transform.LookAt(hit.point);
            }
        }
        private void SetWeaponRayCast()
        {
            if (currentWeapon == WeaponType.MSL)
            {
                RayCastForTracking(300, 50);
            }
            else if (currentWeapon == WeaponType.NB)
            {
                RayCastForTracking(1,100);
            }
            else
            {
                TurnOffTargeting();
            }
        }

        private void RayCastForTracking(int distance, int radius)
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, radius, transform.TransformDirection(Vector3.forward), distance, layerMask);
            targetedEnemies = Array.FindAll(hits, e => e.collider.gameObject.GetComponent<Enemy>());

            // Array Sorting by Distance
            for (int i = 0; i < targetedEnemies.Length - 1; i++)
            {
                for (int j = i + 1; j < targetedEnemies.Length; j++)
                {
                    if (Vector3.Distance(targetedEnemies[i].collider.transform.position,
                        Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)) +
                        Camera.main.transform.TransformDirection(Vector3.forward) * 
                        Vector3.Distance(((targetedEnemies[i].collider.transform.position+targetedEnemies[j].collider.transform.position)/2)
                        , Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)))) >

                        Vector3.Distance(targetedEnemies[j].collider.transform.position,
                        Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)) +
                        Camera.main.transform.TransformDirection(Vector3.forward) * 
                        Vector3.Distance(((targetedEnemies[i].collider.transform.position+targetedEnemies[j].collider.transform.position)/2), 
                        Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)))))
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
                if (targetedEnemies != null)
                {
                    targetedEnemies[0].collider.gameObject.GetComponent<Enemy>().isSelected = true;
                }
                if (Vector3.Distance(transform.position, targetedEnemies[0].transform.position) > distance - 2)
                {
                    TurnOffTargeting();
                }
            }
            else if (currentWeapon == WeaponType.NB)
            {
                if (targetedEnemies != null)
                {   
                    for (int i = 0; i <= Mathf.Clamp(targetedEnemies.Length, 0, 5) - 1; ++i)
                    {
                        targetedEnemies[i].collider.gameObject.GetComponent<Enemy>().isSelected = true;
                    }
                }
                if (Vector3.Distance(transform.position, targetedEnemies[0].transform.position) > radius - 2)
                {
                    TurnOffTargeting();
                } 
            }
            
        }       
        private void TurnOffTargeting()
        {
            foreach (var targetedEnemy in targetedEnemies)
            {
                targetedEnemy.collider.gameObject.GetComponent<Enemy>().isSelected = false;
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
        public bool ReleaseMissile()
        { 
            if (targetedEnemies.Length > 0)
            {
                MissileProjectile currentMissile = Instantiate(MSL_MissileProjectile, transform.position, 
                transform.rotation);

                currentMissile.ammount = MSL_WeaponStats.ammoAmount;
                currentMissile.damage = MSL_WeaponStats.damage;
                
                currentMissile.ActivateMissile(targetedEnemies[0].collider.gameObject);
                Array.Clear(targetedEnemies,0, targetedEnemies.Length);
                return true;
            }
            else {
                return false;
            }
        }
        public bool ReleaseNanoBots()
        {
            if (targetedEnemies.Length > 0)
            {
                for (int i = 0; i <= Mathf.Clamp(targetedEnemies.Length,0,5) - 1; ++i)
                {
                    NanoBots currentNanoBot = Instantiate(NB_NanoBots, transform.position, 
                    transform.rotation);

                    currentNanoBot.ammount = NB_WeaponStats.ammoAmount;
                    currentNanoBot.damage = NB_WeaponStats.damage;

                    currentNanoBot.ActivateNanoBot(targetedEnemies[i].collider.gameObject);
                }
                Array.Clear(targetedEnemies,0, targetedEnemies.Length);
                return true;
            }
            else{
                return false;
            }
        }

        // Weapon Management
        public void SwitchWeapon(int i)
        {
            if (targetedEnemies == null) {TurnOffTargeting();}

                currentWeapon = (WeaponType)(Enum.GetValues(typeof(WeaponType)).GetValue(i));
                if (currentWeapon == WeaponType.MG)
                {
                    audioSource.PlayOneShot(MG_WeaponStats.onLoadWeaponSound);
                } 
                if (currentWeapon == WeaponType.MSL)
                {
                    CloseMGFire();
                    hasOpenedMGFire = false;
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
            MG_WeaponStats.damage = 1;
            MG_WeaponStats.reloadTime = 0;
        }
        private void ProcessMGFire()
        {
            if (hasOpenedMGFire)
            {
                if (isFireOpen == false)
                {
                    OpenMGFire();
                    GetComponent<AudioSource>().PlayOneShot(MG_ShotSound);
                    cachedTime = Time.timeSinceLevelLoad;
                }
                if (Time.timeSinceLevelLoad - cachedTime > 0.1)
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
                targetedEnemies = targetedEnemies.Where(x => 0 != 0).ToArray();
                other.GetComponent<Enemy>().ProcessDamage(MG_WeaponStats.damage);
            }
        }
    }
}