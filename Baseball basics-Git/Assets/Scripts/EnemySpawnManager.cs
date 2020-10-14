using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public int enemyCountMax;
    public int enemyCount;
    public GameObject[] enemies;
    public GameObject[] spawnLocations;
    private int location;

    private void Update()
    {
        GameObject[] aliveEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemyCount = aliveEnemies.Length;

        if (enemyCountMax > enemyCount)
        {
            PickEnemyToSpawn();
        }
    }

    private void PickEnemyToSpawn()
    {
        int enemy = Random.Range(0, enemies.Length);
        PickSpawnLocation();
        Instantiate(enemies[enemy], spawnLocations[location].transform.position, Quaternion.identity);
    }

    private void PickSpawnLocation()
    {
        location = Random.Range(0, spawnLocations.Length);
    }

}
