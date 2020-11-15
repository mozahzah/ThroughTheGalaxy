using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] Text title;
    Color color;

    void Start()
    {
       StartCoroutine(LoadAsyncScene());
       color = new Color32 (255,255,255,255);
    }

    private void Update() 
    {
        color.a = 1*(Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad)));
        title.color = color;
    }

    IEnumerator LoadAsyncScene()
    {
        AsyncOperation gameAsynLoad = SceneManager.LoadSceneAsync(2);

        while (!gameAsynLoad.isDone)
        {
            yield return null;
        }
    }
}
