using System;
using System.Collections;
using System.Collections.Generic;
using _02.Scirpts.Ingame.Entity;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    public AbstractEnemy[] enemyPrefab;
    public GameObject[] spawnPoints;
    
    public float spawnTime = 3.0f;
    public float spawnDelay = 1.0f;


    public Nexus Nexus;
    public void Start()
    {
        if (!Nexus)
            Nexus = FindObjectOfType<Nexus>();
    }

    [ContextMenu("SummonEnemy")]
    public void SummonEnemy() => SummonEnemy(0);
    
    public void SummonEnemy(int index)
    {
        AbstractEnemy enemy = Instantiate(enemyPrefab[index], spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity);
        enemy.target = Nexus;
    }
}
