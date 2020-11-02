using System.Collections;
using System.Collections.Generic;
using UnityEngine;


 public class CameraLock : MonoBehaviour 
{
  [SerializeField] Transform target;

  void Update()
  {
    if (target)
    {
      transform.localPosition = Vector3.Lerp (transform.localPosition, target.localPosition +  new Vector3(0,0.3f,-2), 50 * Time.deltaTime); 
    }
  }
}