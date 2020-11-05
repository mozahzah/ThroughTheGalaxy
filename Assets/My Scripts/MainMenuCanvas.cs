using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuCanvas : MonoBehaviour
{   
    [SerializeField] Button playButton;

    void Start()
    {
        playButton.onClick.AddListener(LoadLevel);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void LoadLevel()
    {
        SceneManager.LoadScene(1);
    }
}
