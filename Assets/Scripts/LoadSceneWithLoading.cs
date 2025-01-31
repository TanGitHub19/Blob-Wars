using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneWithLoading: MonoBehaviour
{
    public void LoadSceneWithLoadingScreen(string sceneName)
    {
        PlayerPrefs.SetString("NextScene", sceneName);
        PlayerPrefs.Save();

        SceneManager.LoadScene("LoadingScreen");
    }
}
