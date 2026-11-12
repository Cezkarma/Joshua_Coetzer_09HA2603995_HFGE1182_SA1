using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void LoadSelectScene(string sceneToLoad)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadSavedGame()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        if (data == null || string.IsNullOrEmpty(data.sceneName))
        {
            Debug.Log("No save file found.");
            return;
        }

        LoadSelectScene(data.sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
