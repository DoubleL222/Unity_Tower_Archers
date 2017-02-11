using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawnManager : MonoBehaviour {
    private static UnitSpawnManager _instance;
    public static UnitSpawnManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UnitSpawnManager>();
            }
            return _instance;
        }
    }
    public LayerMask friendlyLayerMask;
    public LayerMask enemyLayerMask;
    public WaveScriptableObject[] endlessModeWaves;
    public WaveScriptableObject currentWave;
    public WalkerScriptableObject friendlyWalker;
    int groupIndex, timesPaternRepeated, patternIndex, waveIndex = 0;
    int numberOfEnemiesInWave, enemiesDeadOrDestroyed = 0;
    int friendlySpawns = 0;
    bool waveStarted = false;
    [Header("Spawner Settings")]
    public float timeBetweenGroups = 3;
    public float timeBetweenPatterns = 2;
    public float timeBetweenSpawns = 1;

    EnemyWalkingTrack trackLeft;
    float timeCounter;
    float nextSpawnTime = float.MinValue;
    public SpriteRenderer sr;

    // Use this for initialization
    void Start()
    {
        Debug.Log("sprite renderer position: "+ sr.transform.position);
        trackLeft = new EnemyWalkingTrack(sr.sprite, EnemyWalkingTrackSide.Left, sr.transform);
    }

    public void ResetForNewGame()
    {
        groupIndex = 0;
        timesPaternRepeated = 0;
        patternIndex = 0;
        waveIndex = 0;
        numberOfEnemiesInWave = 0;
        enemiesDeadOrDestroyed = 0;

    }

    public void StartWave()
    {
        if (waveIndex >= 0 && waveIndex < endlessModeWaves.Length)
        {
            friendlySpawns = 0;
            currentWave = endlessModeWaves[waveIndex];
            int enemiesCount = 0;
            foreach (GroupScriptableObject group in currentWave.Groups)
            {
                enemiesCount += group.walkerPattern.Length * group.patternRepeatTime;
            }
            numberOfEnemiesInWave = enemiesCount;
            enemiesDeadOrDestroyed = 0;
            waveIndex++;
            groupIndex = 0;
            timesPaternRepeated = 0;
            patternIndex = 0;
            waveStarted = true;
        }
    }

    public void EnemyDied()
    {
        enemiesDeadOrDestroyed++;
        if (enemiesDeadOrDestroyed == numberOfEnemiesInWave)
        {
            if (waveIndex >= endlessModeWaves.Length)
            {
                StartCoroutine(WaitAndFinishGame());
            }
            else
            {
                StartCoroutine(WaitAndFinishWave());
            }
             
        }
    }

    IEnumerator WaitAndFinishWave()
    {
        yield return new WaitForSeconds(1.0f);
        ManagerFSM.InvokeEvent(enumEvents.OnWaveEnd);
        yield return null;
    }

    IEnumerator WaitAndFinishGame()
    {
        GameController.instance.wonGame();
        yield return new WaitForSeconds(1.0f);
        ManagerFSM.InvokeEvent(enumEvents.OnGameFinish);
        yield return null;
    }

    public void EndWave()
    {
        waveStarted = false;
    }

    public void SpawnFriendly()
    {
        if (friendlySpawns < 3)
        {
            InstantiateAndInitUnit(friendlyWalker);
            friendlySpawns++;
        }
    }

    void InstantiateAndInitUnit(WalkerScriptableObject _walker)
    {
        GameObject enemyInstance;
        GenericTrackWalker gtwScript;
        if (_walker.type == WalkerType.Enemy)
        {

            enemyInstance = Instantiate(_walker.Prefab, UtilityScript.V2toV3(trackLeft.walkingPoints[0]), Quaternion.identity) as GameObject;
            gtwScript = enemyInstance.GetComponent<GenericTrackWalker>();
            gtwScript.InitGenericWalker(_walker.startingHealth, _walker.startingDamage, _walker.startingSpeed, _walker.startingAttackSpeed, _walker.type, trackLeft, friendlyLayerMask);
        }
        else
        {
            enemyInstance = Instantiate(_walker.Prefab, UtilityScript.V2toV3(trackLeft.walkingPoints[trackLeft.Points-1]), Quaternion.identity) as GameObject;
            gtwScript = enemyInstance.GetComponent<GenericTrackWalker>();
            gtwScript.InitGenericWalker(_walker.startingHealth, _walker.startingDamage, _walker.startingSpeed, _walker.startingAttackSpeed, _walker.type, trackLeft, enemyLayerMask);
        }   
        gtwScript.StartFollowTrack();
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("START WAVE");
            StartWave();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SpawnFriendly();
        }
        if (waveStarted)
        {
            if (Time.time >= nextSpawnTime)
            {
                InstantiateAndInitUnit(currentWave.Groups[groupIndex].walkerPattern[patternIndex]);
                patternIndex++;
                // gtwScript.FollowTrack(0, trackLeft, 1);


                nextSpawnTime = Time.time + timeBetweenSpawns;
                //if full patern has bee spawned
                if (patternIndex >= currentWave.Groups[groupIndex].walkerPattern.Length)
                {
                    patternIndex = 0;
                    timesPaternRepeated++;
                    nextSpawnTime = Time.time + timeBetweenPatterns;
                    //if patern has been repeated enough times
                    if (timesPaternRepeated >= currentWave.Groups[groupIndex].patternRepeatTime)
                    {

                        nextSpawnTime = Time.time + timeBetweenGroups;
                        groupIndex++;

                        if (groupIndex >= currentWave.Groups.Length)
                        {
                            EndWave();
                        }
                    }

                }
            }
        }
	}
}
