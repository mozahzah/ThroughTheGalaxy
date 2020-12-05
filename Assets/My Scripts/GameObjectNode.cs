using UnityEngine;

/*
    =======================================
    GameObject used specifically to detect
    collision and trigger the cell invalid 
    path. 

    Reason i did this is because the grid 
    and the gridObjects don't inheret from 
    monobehavior therefore cannot instantiate
    gameobjects in the scene using new.
    ========================================
*/

public class GameObjectNode : MonoBehaviour
{
    [SerializeField] public bool isValid = true;

    private void OnTriggerStay(Collider other) 
    {
        if (other.gameObject.transform.tag == "building")
        {
            isValid = false;
            Debug.Log("Not Valid");
        } 
    }
}
