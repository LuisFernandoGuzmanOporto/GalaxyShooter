using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public float timeBtwSpawn = 1.5f;
    float timer = 0;
    public Transform leftPoint;
    public Transform rightPoint;
    public List<GameObject> enemyPrefabs;
    public int score = 0;
    public Text scoreText;
    public Text waveText;
    public Text enemiesInWaveText;
    public static Spawner instance;

    public int enemiesToSpawnPerWave = 1; 
    private int enemiesSpawned = 0; 
    private int enemiesAlive = 0; 
    private bool waveInProgress = false;
    private int waveCount = 0;

    public GameObject finalBossPrefab; 
    private bool bossSpawned = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        scoreText.text = "SCORE: " + score;
        waveText.text = "START";
        enemiesInWaveText.text = "Enemies left: " + (enemiesToSpawnPerWave - enemiesSpawned);
        StartCoroutine(StartNewWave()); 
    }

    void Update()
    {
        scoreText.text = "SCORE: " + score;
        waveText.text = "WAVE: " + waveCount;
        enemiesInWaveText.text = "Enemies left: " + (enemiesToSpawnPerWave - enemiesSpawned);
        //SpawnEnemy();
        
        if (waveInProgress)
        {
            SpawnEnemy();
        }
        
        if (enemiesAlive <= 0 && enemiesSpawned == enemiesToSpawnPerWave && waveInProgress)
        {
            waveInProgress = false; 
            if (waveCount < 3)
            {
                StartCoroutine(NextWave()); 
            }
            else if (!bossSpawned)
            {
                
                SpawnFinalBoss();
            }
        }
    }


    void SpawnEnemy()
    {
        if (enemiesSpawned < enemiesToSpawnPerWave)
        {
            if (timer < timeBtwSpawn)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
                int enemy = Random.Range(0, enemyPrefabs.Count);//0 1 2 3
                float x = Random.Range(leftPoint.position.x, rightPoint.position.x);
                Vector3 newPos = new Vector3(x, transform.position.y, transform.position.z);
                Instantiate(enemyPrefabs[enemy], newPos, Quaternion.Euler(0, 180, 0));
                enemiesSpawned++; 
                enemiesAlive++; 
                
            }
        }  
    }

    public void AddScore()
    {
        score ++;
        scoreText.text = "SCORE: " + score;        
    }
    public void AddScore(int points)
    {
        score+= points;
        scoreText.text = "SCORE: " + score;       
    }

    public void EnemyDestroyed()
    {
        enemiesAlive--; 
    }

    IEnumerator NextWave()
    {
        yield return new WaitForSeconds(3f); 
        StartCoroutine(StartNewWave()); 
    }

    IEnumerator StartNewWave()
    {
        yield return new WaitForSeconds(2f);

        enemiesSpawned = 0;
        enemiesAlive = 0;
        enemiesToSpawnPerWave += 2;

        waveInProgress = true;
        waveCount++;
    }

    void SpawnFinalBoss()
    {
        
        float centerX = (leftPoint.position.x + rightPoint.position.x) / 2;
        Vector3 bossSpawnPosition = new Vector3(centerX, transform.position.y, 0);

       
        Instantiate(finalBossPrefab, bossSpawnPosition, transform.rotation);

        bossSpawned = true; 
        waveText.text = "FINAL BOSS!";
        enemiesInWaveText.text = ""; 
    }


}