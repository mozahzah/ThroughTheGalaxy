
using UnityEngine;
using unciphering.Mechanics;


// Controls Canvas and Camera Management

namespace unciphering.Controller
{
    public class DroneController : MonoBehaviour
    {
        [SerializeField] public GameObject[] canvas;
        [SerializeField] public ShipController MotherShip;
        [SerializeField] Camera MainCamera;
        [SerializeField] Camera DroneCamera;
        DroneBot droneBot;


        void Start()
        {
            droneBot = transform.parent.GetComponent<DroneBot>();
            MainCamera = Camera.main;
            MainCamera.enabled = false;
            DroneCamera.enabled = true;
            SwitchCanvases();
        }

        void Update()
        {
            ReturnToMotherShip();
        }

        private void SwitchCanvases() 
        {
            canvas[0].SetActive(false);
            canvas[1].SetActive(true);   
        }

        private void UpdateCanvas() 
        {
            //canvas[1]
        }

        private void ReturnToMotherShip()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                MotherShip.transform.parent.GetComponent<Engine>().enabled = true;
                MotherShip.GetComponent<ShipController>().enabled = true;
                MotherShip.GetComponentInChildren<Gun>().enabled = true;
                MainCamera.enabled = true;
                canvas[0].SetActive(true);
                canvas[1].SetActive(false); 
                Destroy(droneBot.gameObject);

            }
        }   
    }
}