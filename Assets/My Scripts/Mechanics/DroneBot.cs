using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using unciphering.Characters;

namespace unciphering.Mechanics
{
    public class DroneBot : MonoBehaviour
    {
        // These are passed through by the mother ship on release
        


        RaycastHit[] targetedEnemies;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        
        void Update()
        {
            
            ScanEnemies();
        }

        private void RayCastForTracking()
        {
                RaycastHit[] hits = Physics.SphereCastAll(transform.position, 50, transform.TransformDirection(Vector3.forward), Mathf.Infinity);
                targetedEnemies = Array.FindAll(hits, e => e.collider.gameObject.GetComponent<Enemy>());


                if (targetedEnemies != null){
                    Debug.Log("Array is not empty");
                }

                // Array Sorting by Distance
                // for (int i = 0; i < targetedEnemies.Length - 1; i++)
                // {
                //     for (int j = i + 1; j < targetedEnemies.Length; j++)
                //     {
                //         if (Vector3.Distance(targetedEnemies[i].collider.transform.position,
                //             Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)) +
                //             Camera.main.transform.forward * Vector3.Distance(targetedEnemies[i].collider.transform.position, Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)))) >

                //             Vector3.Distance(targetedEnemies[j].collider.transform.position,
                //             Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)) +
                //             Camera.main.transform.forward * Vector3.Distance(targetedEnemies[i].collider.transform.position, Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)))))
                //         {
                //             var temp = targetedEnemies[i];
                //             targetedEnemies[i] = targetedEnemies[j];
                //             targetedEnemies[j] = temp;
                //         }
                //     }
                // }

        }
        public void ScanEnemies()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                RayCastForTracking();
                foreach (var e in targetedEnemies)
                {
                    e.collider.gameObject.GetComponent<Enemy>().TagEnemy();
                }
            }
        }




    }

}
