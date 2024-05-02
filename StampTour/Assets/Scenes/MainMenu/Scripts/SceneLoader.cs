using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    //�̷��Ÿ� �̱������� ����ϴ°� �� ������ -> �̰� Ȯ��
    static string nextSceneName;

    [SerializeField]
    Slider progressBar;

    public static void LoadScene(string sceneName)
    {
        LoadScene(sceneName, LoadSceneMode.Additive);
    }
    public static void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Additive)
    {

        nextSceneName = sceneName;
        SceneManager.LoadSceneAsync("LoadingScene", mode);
    }

    IEnumerator LoadSceneProgress()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;

            progressBar.value = asyncLoad.progress;
            
        }
        Debug.Log("Loading complete");

        SceneManager.UnloadSceneAsync("LoadingScene");
    }

    private void Start()
    {
        if(progressBar != null)
        {
            StartCoroutine(LoadSceneProgress());
        }
    }

    public static void RollbackMainScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }
}
