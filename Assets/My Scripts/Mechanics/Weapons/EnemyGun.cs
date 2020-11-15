using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    ParticleSystem machineGun;
    public bool isFiring;
    //List<ParticleCollisionEvent> particleCollisionEvents;

    // Start is called before the first frame update
    void Start()
    {
        //particleCollisionEvents = new List<ParticleCollisionEvent>();
        machineGun = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

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

    private void OnParticleCollision(GameObject other) {
        Debug.Log(other.gameObject.name);
    }
}
