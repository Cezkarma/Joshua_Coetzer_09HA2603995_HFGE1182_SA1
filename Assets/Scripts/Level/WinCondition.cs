using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    public GameObject sceneEnemy;
    
    public string sceneToLoadName;

    public bool deleteSaveOnComplete;
    private bool isLoading;

    private void Update()
    {
        if (!isLoading && sceneEnemy == null)
        {
            isLoading = true;

            if (deleteSaveOnComplete)
            {
                SaveSystem.DeleteSave();
            }
            else if (GameManager.Instance != null)
            {
                GameManager.Instance.SavePlayer();
                GameManager.carryOverOnStart = true;
            }

            SceneManager.LoadScene(sceneToLoadName);
        }
    
    }
    
}
