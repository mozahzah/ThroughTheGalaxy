using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ThroughtTheGalaxy.Controller
{
    public class DroneController : MonoBehaviour
    {
        [SerializeField] public GameObject[] canvas;


        void Start()
        {
            UpdateCanvas();
        }

        void Update()
        {
            
        }

        private void UpdateCanvas() 
        {
            canvas[0].SetActive(false);
            canvas[1].SetActive(true);
            
        }
    }
}