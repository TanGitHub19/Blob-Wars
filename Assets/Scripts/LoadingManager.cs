using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public Image logo;
    private string sceneToLoad;

    void Start()
    {
        sceneToLoad = PlayerPrefs.GetString("NextScene", "Login");

        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        logo.canvasRenderer.SetAlpha(0.0f);
        logo.CrossFadeAlpha(1.0f, 1.5f, false);

        yield return new WaitForSeconds(3f); 

        SceneManager.LoadScene(sceneToLoad);
    }
}
