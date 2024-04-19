using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject[] WeaponCache;
    public GameObject[] EnemyRoster;
    private float spawnLimit = 20.0f;
    private int currentWave = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Instantiate(WeaponCache[0],new Vector3(0, 0.25f, 6), WeaponCache[0].transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0 && gameManager.isGameActive)
        {
            SpawnWave(++currentWave);
        }
    }

    void SpawnWave(int enemyCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            float zPos = Random.Range(-spawnLimit, spawnLimit);
            float xPos = Random.Range(-spawnLimit, spawnLimit);
            Instantiate(EnemyRoster[0], new Vector3(xPos, 0, zPos), EnemyRoster[0].transform.rotation);
        }
    }
}
