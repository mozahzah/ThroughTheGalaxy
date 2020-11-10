using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using unciphering.Mechanics;
using unciphering.Characters;
using unciphering.UI;


namespace unciphering.Controller
{
    public class ShipController : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] GameObject[] canvas;

        [Header("Guns")]
        [SerializeField] Gun gun;
        Gun.WeaponType caseSwitch = 0;

        [Header("Gadgets")]
        [SerializeField] DroneBot droneBot;

        int i = 0;

        private void Start() 
        {
            UpdateCanvas();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
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

            // Reloading Scene
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //SceneManager.LoadScene(0);
            }
  
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
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                gun.ReleaseMissile();
            }
        }

        private void ReleaseNanoBots()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                gun.ReleaseNanoBots();
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
    }
}