using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using ThroughtTheGalaxy.Mechanics;
using ThroughtTheGalaxy.UI;


namespace ThroughtTheGalaxy.Controller{

    public class ShipController : MonoBehaviour
    {
        [Header("General")]
        [Tooltip("In ms")] [SerializeField] float xSpeed = 15f;
        [Tooltip("In ms")] [SerializeField] float ySpeed = 15f;
        [SerializeField] Canvas inGameCanvas;

        [Header("Throw Control")]
        [SerializeField] float controlPitchFactor = -25f;
        [SerializeField] float controlRollFactor = -25f;
        float yThrow, xThrow;

        [Header("Guns")]
        [SerializeField] Gun gun;
        Gun.WeaponType caseSwitch = 0;

        int i = 0;

        private void Start() 
        {
            gun.SwitchWeapon(i);
            UpdateCanvas();
        }

        void Update()
        {
            HorizontalMouvement();
            ProcessRotation();
            SwitchWeapon();
            

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
  
        }

        // Mouvement
        private void ProcessRotation()
        {
            float pitch = yThrow * controlPitchFactor;
            float yaw =  transform.localEulerAngles.y;
            float roll= Mathf.Lerp(xThrow * controlRollFactor, 0, 0.1f * Time.deltaTime);
            transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
        }

        private void HorizontalMouvement()
        {
            xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
            float xOffset = xThrow * xSpeed * Time.deltaTime;
            float rawNewXpos = transform.localPosition.x + xOffset;
            transform.localPosition = new Vector3(rawNewXpos, transform.localPosition.y, transform.localPosition.z);
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
               if (i == count - 1)
               {
                   i = 0;
               }
               else
               {
                    i ++;
               }
                gun.SwitchWeapon(i);
                UpdateCanvas();
           }
        }

        private void UpdateCanvas()
        {
            caseSwitch = gun.currentWeapon;
            switch (caseSwitch)
            {
                case Gun.WeaponType.MG:
                inGameCanvas.GetComponent<InGameCanvas>().selectedWeapon = 0;
                break;
                case Gun.WeaponType.MSL:
                inGameCanvas.GetComponent<InGameCanvas>().selectedWeapon = 1;
                break;
                case Gun.WeaponType.NB:
                inGameCanvas.GetComponent<InGameCanvas>().selectedWeapon = 2;
                break;
            }   
        }
    }
}