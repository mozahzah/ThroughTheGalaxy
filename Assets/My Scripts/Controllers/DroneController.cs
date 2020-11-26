
using UnityEngine;
using unciphering.Mechanics;
using UnityStandardAssets.CrossPlatformInput;


// Controls Canvas and Camera Management

namespace unciphering.Controller
{
    public class DroneController : MonoBehaviour
    {
        [SerializeField] public GameObject[] canvas;
        [SerializeField] Engine engine;
        [SerializeField] Camera MainCamera;
        [SerializeField] Camera DroneCamera;
        
        [SerializeField] public ShipController MotherShip;
        DroneBot droneBot;

        [SerializeField] float lifeTime;


        void Start()
        {
            droneBot = transform.parent.GetComponent<DroneBot>();
            MainCamera = Camera.main;
            MainCamera.enabled = false;
            DroneCamera.enabled = true;
            droneBot.cam = DroneCamera;
            SwitchCanvases();
        }

        void Update()
        {
            ProcessMovement();
            TagEnemy();
            ScanEnemies();

            if (Input.GetKeyDown(KeyCode.G))
            {
                ReturnToMotherShip();
            }

            lifeTime += Time.deltaTime;
            if (lifeTime > 10)
            {
                ReturnToMotherShip();
            }
        }

        private void ProcessMovement()
        {
            engine.ProcessLook(CrossPlatformInputManager.GetAxis("Mouse Y"), CrossPlatformInputManager.GetAxis("Mouse X"));
            engine.BasicMouvement(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"));
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

        private void ScanEnemies()
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                droneBot.ScanEnemies();
            }
        }

        private void TagEnemy()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                droneBot.TagEnemy();
            }
        }

        
        private void ReturnToMotherShip()
        {
            
                MotherShip.transform.parent.GetComponent<Engine>().enabled = true;
                MotherShip.GetComponent<ShipController>().enabled = true;
                MotherShip.GetComponentInChildren<Gun>().enabled = true;
                MainCamera.enabled = true;
                canvas[0].SetActive(true);
                canvas[1].SetActive(false); 
                Destroy(engine.gameObject);

            
        }   
    }
}