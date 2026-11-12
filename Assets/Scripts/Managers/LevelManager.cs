using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private bool showCursorOnStart = false;

    private void Start()
    {
        if (showCursorOnStart)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

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

        GameManager.loadSaveOnStart = true;
        LoadSelectScene(data.sceneName);
    }

    public void SaveAndLoadScene(string sceneToLoad)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SaveGame();
        }

        LoadSelectScene(sceneToLoad);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
