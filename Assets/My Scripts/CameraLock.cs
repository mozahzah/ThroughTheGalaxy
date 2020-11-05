using System.Collections;
using System.Collections.Generic;
using UnityEngine;


 public class CameraLock : MonoBehaviour 
{
  void Start() 
  {
    transform.localPosition = (new Vector3(0,0.3f,-2));
  }
  void Update()
  {
      if (gameObject.tag == "MainCamera")
      {
        transform.localPosition = Vector3.Lerp (transform.localPosition, new Vector3(0,0.3f,-2), 0.5f * Time.deltaTime);
        transform.rotation = new Quaternion(0,0,0,0);
      }
      if (gameObject.tag == "SecondaryCamera")
      {
        transform.localPosition = Vector3.Lerp (transform.localPosition, new Vector3(0,0.3f,0.05f), 1f * Time.deltaTime);
      }
  }
}