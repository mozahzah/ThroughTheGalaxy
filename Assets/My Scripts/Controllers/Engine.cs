using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace unciphering.Controller
{
   
    public class Engine : MonoBehaviour
    {
        [SerializeField] GameObject ship;
        [Header("General")]
        [SerializeField] float sensitivity = 1f;

        [Header("Throw Control")]
        [SerializeField] float controlRollFactor = 0f;
        float xThrow, yThrow, zThrow;

        [Tooltip("In ms")] [SerializeField] float xSpeed = 5f;
        [Tooltip("In ms")] [SerializeField] float zSpeed = 5f;


        private void Start() 
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        void Update()
        {
            ProcessLook();
            BasicMouvement();
            ProcessRotation();
        }

        private void ProcessLook()
        {
            float pitch = transform.localRotation.eulerAngles.x + CrossPlatformInputManager.GetAxis("Mouse Y") * -sensitivity;
            float yaw  = transform.localRotation.eulerAngles.y + CrossPlatformInputManager.GetAxis("Mouse X") * sensitivity;
            transform.localRotation = Quaternion.Euler(pitch, yaw, 0);
        }

        private void ProcessRotation()
        {
            float roll= xThrow * controlRollFactor;
            ship.transform.localRotation = Quaternion.Euler(ship.transform.localRotation.eulerAngles.x, 
            ship.transform.localRotation.eulerAngles.y, roll);
        }

        private void BasicMouvement()
        {
            xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
            float xOffset = xThrow * xSpeed * Time.deltaTime;
            transform.Translate(Vector3.right * xOffset);

            zThrow = CrossPlatformInputManager.GetAxis("Vertical");
            float zOffset = zThrow * zSpeed * Time.deltaTime;
            transform.Translate(Vector3.forward * zOffset);
        }
    }
}