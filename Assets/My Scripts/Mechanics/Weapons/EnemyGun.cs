using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace unciphering.Mechanics
{
    public class EnemyGun : MonoBehaviour
    {
        ParticleSystem machineGun;
        public bool isFiring;

        // Start is called before the first frame update
        void Start()
        {
            machineGun = GetComponent<ParticleSystem>();
        }

        public void Fire()
        {
            if (!isFiring)
            {
                machineGun.Play();
                isFiring = true;
            }
        }

        public void StopFire()
        {
            machineGun.Stop();
            isFiring = false;
        }
    }
}
