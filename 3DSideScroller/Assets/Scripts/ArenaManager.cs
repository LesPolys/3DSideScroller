using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour {

    [System.Serializable]
    public struct SpawnType{
        public GameObject enemyType;
        public int numOfEnemy;
    
    }

    [System.Serializable]
    public struct Wave
    {
      public  SpawnType[] spawnable;
        public float lengthOfWave;
    }

    public Wave[] waves;
    private int currentWaveIndex;


    enum ArenaManagerStates {JOINGING, SPAWNING, FIGHTNING, RESTING };
    ArenaManagerStates currState;

    public Transform[] possibleSpawnPositions;


    private int numEnemies;

    bool hasNotSpawned;

    float timeCounter;
    float startingTimeCounter;

    void Awake()
    {
        //subscribe to to say players are joined

    }

    void Update()
    {

        switch (currState)
        {
            case ArenaManagerStates.JOINGING:
                hasNotSpawned = true;
                break;

            case ArenaManagerStates.SPAWNING:
                if (hasNotSpawned)
                {
                    StartCoroutine(SpawnWave());
                    hasNotSpawned = false;
                }
                break;

            case ArenaManagerStates.FIGHTNING:

                if (Enemy.enemyCount <= 0)
                {
                    StartRestTime();
                }

                break;

            case ArenaManagerStates.RESTING:


                if (timeCounter >= 0)
                {
                    StartSpawning();
                }
                else
                {
                    timeCounter -= Time.deltaTime;
                }

                break;
        }

    }

    public IEnumerator SpawnWave()
    {
        Transform currentSpawnPos;
        int currenSpawnIndex = 0;

        currentSpawnPos = possibleSpawnPositions[currenSpawnIndex];

        foreach (Wave wave in waves)
        {

            foreach (SpawnType spawn in wave.spawnable){
                for(int i =0; i< spawn.numOfEnemy; i++)
                {
                    
                   

                    GameObject gameObject = Instantiate(spawn.enemyType, currentSpawnPos.position, Quaternion.identity);
                    UpdateEnemyCount(1);

                    if (currenSpawnIndex+1 >possibleSpawnPositions .Length )
                    {
                        currenSpawnIndex = 0;
                    }
                    else
                    {
                        currenSpawnIndex++;
                    }

                    currentSpawnPos = possibleSpawnPositions[currenSpawnIndex];

                    yield return new WaitForSeconds(0.5f);
                }
            }
        }

        yield return null;
    }

    void NextWave()
    {
        currentWaveIndex++;
    }

    void SpawnAdHocWave(GameObject enemyType, int numOfEnemies)
    {
        for (int i = 0; i < numOfEnemies; i++)
        {
            GameObject newEnemy = Instantiate(enemyType, transform.position, Quaternion.identity);
            UpdateEnemyCount(1);
        }

    }

    void SpawnAtPoint(GameObject enemyType, Transform spawnPoint)
    {
        GameObject newEnemy = Instantiate(enemyType, spawnPoint.position, Quaternion.identity);
        UpdateEnemyCount(1);
    }

    void UpdateEnemyCount(int i)
    {
        numEnemies += 1;
    }

    public void PlayersAreReady()
    {
        currState = ArenaManagerStates.SPAWNING;
    }

    public void StartFightingState()
    {
       
        currState = ArenaManagerStates.FIGHTNING;
    }

    public void StartRestTime()
    {
        timeCounter = waves[currentWaveIndex].lengthOfWave;
        startingTimeCounter = timeCounter;
        currState = ArenaManagerStates.RESTING;
    }

    public void StartSpawning()
    {

        NextWave();
        hasNotSpawned = true;
        currState = ArenaManagerStates.SPAWNING;
    }



}
