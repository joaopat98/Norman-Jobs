using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss Config")]
public class BossConfigs : ScriptableObject
{
    public GameObject bossPrefab;
    public GameObject pathPrefab;
    public float timeBetweenSpawns = 0.5f;
    public float spawnRandomFactor = 0.3f;
    public int numberOfEnemies = 5;
    public float moveSpeed = 2f;

    public GameObject getEnemyPrefab() { return bossPrefab; }

    public List<Transform> getWaypoints()
    {
        var waveWaypoints = new List<Transform>();
        foreach (Transform child in pathPrefab.transform)
        {
            waveWaypoints.Add(child);
        }

        return waveWaypoints;
    }

    public float getTimeBetweenSpawns() { return timeBetweenSpawns; }

    public float getSpawnRandomFactor() { return spawnRandomFactor; }

    public int getNumberOfEnemies() { return numberOfEnemies; }

    public float getMoveSpeed() { return moveSpeed; }


}