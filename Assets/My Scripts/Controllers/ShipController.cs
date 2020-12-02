
using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using unciphering.Mechanics;
using unciphering.UI;
using UnityEngine.SceneManagement;


namespace unciphering.Controller
{
    public class ShipController : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] Engine engine;
        [SerializeField] GameObject[] canvas;


        [Header("Guns")]
        [SerializeField] Gun gun;
        Gun.WeaponType caseSwitch = 0;

        [Header("Gadgets")]
        [SerializeField] DroneBot droneBot;


        float cachedTime1;
        float cachedTime2;
        float cachedTime3;
        int i = 0;

        private void Start() 
        {
            UpdateCanvas();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            cachedTime1 = Time.timeSinceLevelLoad;
            cachedTime2 = Time.timeSinceLevelLoad;
            cachedTime3 = Time.timeSinceLevelLoad;
        }

        void Update()
        {
            //Debug.Log(transform.position);


            ProcessMovement();
            SwitchWeapon();
            ReleaseDroneBot();
            caseSwitch = gun.currentWeapon;
            switch (caseSwitch)
            {
                case Gun.WeaponType.MG:
                    gun.hasOpenedMGFire = Shooting();
                    break;
                case Gun.WeaponType.MSL:
                    LaunchMissile();
                    break;
                case Gun.WeaponType.NB:
                    ReleaseNanoBots();
                    break;
                default:
                    gun.hasOpenedMGFire = false;
                    break;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(2);
            }


        }

        private void ProcessMovement()
        {
            engine.ProcessLook(CrossPlatformInputManager.GetAxis("Mouse Y"), CrossPlatformInputManager.GetAxis("Mouse X"));
            engine.BasicMouvement(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"));
        }

        // Gun Activation
        private bool Shooting()
        {
            if (CrossPlatformInputManager.GetAxis("Fire1") == 1)
            {
                return true;
            }
            else return false;
        }

        private void LaunchMissile()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && Time.timeSinceLevelLoad - cachedTime2 > gun.MSL_WeaponStats.reloadTime)
            {
                if (gun.ReleaseMissile())
                {   
                    cachedTime2 = Time.timeSinceLevelLoad;
                    canvas[0].GetComponent<InGameCanvas>().cachedTime2 = Time.timeSinceLevelLoad;
                    canvas[0].GetComponent<InGameCanvas>().MSL_isReloading = true;
                }
            }
            else
            {
                return;
            }
        }

        private void ReleaseNanoBots()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && Time.timeSinceLevelLoad - cachedTime3 > gun.NB_WeaponStats.reloadTime)
            {
                if (gun.ReleaseNanoBots())
                {
                    cachedTime3 = Time.timeSinceLevelLoad;
                    canvas[0].GetComponent<InGameCanvas>().cachedTime3 = Time.timeSinceLevelLoad;
                    canvas[0].GetComponent<InGameCanvas>().NB_isReloading = true;
                }
            }
            else
            {
                return;
            }
        }

         private void SwitchWeapon()
        {   
           if (Input.GetKeyDown(KeyCode.Q))
           {
               int count = Enum.GetValues(typeof(Gun.WeaponType)).Length;
               if (i >= count - 1)
               {
                   i = 0;
               }
               else
               {
                    i ++;
               }
                i = Mathf.Clamp(i,0,count);
                gun.SwitchWeapon(i);
                UpdateCanvas();
           }
        }


        // Gadget Activation
        private void ReleaseDroneBot()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                var currentDroneBot = Instantiate(droneBot, transform.position, transform.rotation);
                currentDroneBot.GetComponentInChildren<DroneController>().canvas = canvas;
                currentDroneBot.GetComponentInChildren<DroneController>().MotherShip = this;
                gun.enabled = false;
                transform.parent.GetComponent<Engine>().enabled = false;
                GetComponent<ShipController>().enabled = false;
            }
            else
            {
                return;
            }
        }

        private void UpdateCanvas()
        {   
            canvas[0].SetActive(true);
            canvas[1].SetActive(false);
            
            canvas[0].GetComponent<InGameCanvas>().MG_reloadTime = gun.MG_WeaponStats.reloadTime;
            canvas[0].GetComponent<InGameCanvas>().MSL_reloadTime = gun.MSL_WeaponStats.reloadTime;
            canvas[0].GetComponent<InGameCanvas>().NB_reloadTime = gun.NB_WeaponStats.reloadTime;


            caseSwitch = gun.currentWeapon;
            switch (caseSwitch)
            {
                case Gun.WeaponType.MG:
                canvas[0].GetComponent<InGameCanvas>().selectedWeapon = 0;
                break;
                case Gun.WeaponType.MSL:
                canvas[0].GetComponent<InGameCanvas>().selectedWeapon = 1;
                break;
                case Gun.WeaponType.NB:
                canvas[0].GetComponent<InGameCanvas>().selectedWeapon = 2;
                break;
            }
        }

        public void OnDeath()
        {
            SceneManager.LoadScene(2);
        }

        private void OnCollisionEnter(Collision other) {
            OnDeath();
        }

        
    }
}