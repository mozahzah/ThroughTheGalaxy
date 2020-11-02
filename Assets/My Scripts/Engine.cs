using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace ThroughtTheGalaxy.Controller{
    public class Engine : MonoBehaviour
    {
        [SerializeField] GameObject target;
        [Header("General")]
        [SerializeField] float sensitivity = 1f;

       

        private void Start() 
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        void Update()
        {
            transform.position += target.transform.forward * Time.deltaTime * CrossPlatformInputManager.GetAxis("Vertical") *4;
            ProcessLook();
        }

        private void ProcessLook()
        {
            float pitch = transform.localEulerAngles.x + CrossPlatformInputManager.GetAxis("Mouse Y") * -sensitivity * Time.deltaTime;
            float yaw  =  transform.localEulerAngles.y + CrossPlatformInputManager.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float roll =   transform.localEulerAngles.z;
            transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
        }
    }
}