using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject playerObject;
    public GameObject[] enemies;
    private string sceneName;

    public static bool loadSaveOnStart;
    public static bool carryOverOnStart;

    private void Awake()
    {
        // Singleton pattern (optional but useful)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private IEnumerator Start()
    {
        yield return null;

        if (loadSaveOnStart)
        {
            loadSaveOnStart = false;
            LoadGame();
        }
        else if (carryOverOnStart)
        {
            carryOverOnStart = false;
            CarryOverPlayer();
            SaveGame();
        }
    }

    public void SaveGame()
    {
        SavePlayer();
        SaveEnemies();
    }

    public void LoadGame()
    {
        if (SaveSystem.LoadPlayer() != null)
        {
            LoadPlayer();
        }

        if (SaveSystem.LoadEnemy() != null)
        {
            LoadEnemies();
        }
    }

    private void CarryOverPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if (data == null)
        {
            return;
        }

        ShootHandler shootHandler = playerObject.GetComponent<ShootHandler>();
        shootHandler.SetCurrentAmmo(data.currentAmmo);
        shootHandler.SetTotalAmmo(data.totalAmmo);
        playerObject.GetComponent<HealthHandler>().SetHealth(data.health);
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(playerObject.GetComponent<ShootHandler>(), playerObject.GetComponent<HealthHandler>());
    }

    public void SaveEnemies()
    {
        if (Array.Exists(enemies, enemy => enemy == null))
        {
            return;
        }

        SaveSystem.SaveEnemy(enemies);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        playerObject.GetComponent<ShootHandler>().SetCurrentAmmo(data.currentAmmo);
        playerObject.GetComponent<ShootHandler>().SetTotalAmmo(data.totalAmmo);
        playerObject.GetComponent<HealthHandler>().SetHealth(data.health);
        Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2]);
        Rigidbody rb = playerObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.position = position;
        }
        playerObject.transform.position = position;
    }

    public void LoadEnemies()
    {
        EnemySaveData data = SaveSystem.LoadEnemy();

        for (int i = 0; i < enemies.Length && i < data.enemies.Count; i++)
        {
            GameObject enemy = enemies[i];
            if (enemy == null)
            {
                continue;
            }

            EnemyData enemyData = data.enemies[i];
            Vector3 position = new Vector3(enemyData.position[0], enemyData.position[1], enemyData.position[2]);

            enemy.GetComponent<HealthHandler>().SetHealth(enemyData.health);

            NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
            if (agent != null && agent.enabled)
            {
                agent.Warp(position);
            }
            else
            {
                enemy.transform.position = position;
            }

            enemy.SetActive(enemyData.activeInScene);
        }
    }
    
}
