using System.Collections;
using UnityEngine;


namespace unciphering.Controller
{
   
    public class Engine : MonoBehaviour
    {
        [SerializeField] GameObject ship;
        [Header("General")]
        [SerializeField] float sensitivity = 1f;
        [Tooltip("In m/s")][SerializeField] float speed = 5;

        [Header("Throw Control")]
        [SerializeField] float controlRollFactor = 0f;
        float xThrow, yThrow, zThrow;



        private void Start() 
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        void Update()
        {
            
        }

        public void BasicMouvement()
        {
            // Combat Translation Randomizing Strafing
        }

        public void BasicMouvement(float xThrow, float zThrow)
        {
            ProcessRotation(xThrow);
            float xOffset = xThrow * speed * Time.deltaTime;
            transform.Translate(Vector3.right * xOffset);

            float zOffset = zThrow * speed * Time.deltaTime;
            transform.Translate(Vector3.forward * zOffset);
        }

        


        // Two Look Overloads
        public void ProcessLook(float inputY, float inputX)
        {
            float pitch = transform.localRotation.eulerAngles.x + inputY * -sensitivity;
            float yaw  = transform.localRotation.eulerAngles.y + inputX * sensitivity;
            transform.localRotation = Quaternion.Euler(pitch, yaw, 0);
        }

        public void ProcessLook(Vector3 position)
        {
            var targetRotation = Quaternion.LookRotation(position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 15 * Time.deltaTime);
        }


        




        // Animation Purposes
        private void ProcessRotation(float xThrow)
        {
            float roll= xThrow * controlRollFactor;
            ship.transform.localRotation = Quaternion.Euler(ship.transform.localRotation.eulerAngles.x, 
            ship.transform.localRotation.eulerAngles.y, roll);
        }
    }
}