using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ThroughtTheGalaxy.Mechanics
{
public class MissileProjectile : MonoBehaviour
{

    bool isMissileLaunched;
    GameObject targetedEnemy;
    [SerializeField] ParticleSystem _particleSystem;

    bool hasFxPlayed = false;

    float floatingTime = 1;
    float initialTime;
    // Start is called before the first frame update
    void Start()
    {
        initialTime = Time.timeSinceLevelLoad;
    }

    // Update is called once per frame
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
            other.gameObject.GetComponent<Enemy>().health = 0;
            Destroy(gameObject);
        }
        
    }
}
}
