﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhaseOne : MonoBehaviour {

    public static BossPhaseOne instance;
    public BossPhaseTwo bossTwoPrefab;
    public float spawnTimer;
    public int numEnemiesOnScreen;
    public int criticalAreas;
    public int score;

    private bool missileTurretOneAlive = true;
    public void DeactivateMissleOne() { missileTurretOneAlive = false; }
    private bool missileTurretTwoAlive = true;
    public void DeactivateMissleTwo() { missileTurretTwoAlive = false; }
    private bool scatterTurretOneAlive = true;
    public void DeactivateScatterOne() { scatterTurretOneAlive = false; }
    private bool scatterTurretTwoAlive = true;
    public void DeactivateScatterTwo() { scatterTurretTwoAlive = false; }
    private Collider2D hitBox;
    private HangerBaySpawn[] hangers;
    private Player player;

    private float lastSpawn = 0.0f;
    private int enemiesOnScreen;

    // Use this for initialization
    void Start() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        hitBox = gameObject.GetComponent<Collider2D>();
        hangers = gameObject.GetComponentsInChildren<HangerBaySpawn>();
        player = FindObjectOfType<Player>();
        lastSpawn = spawnTimer;
    }

    // Update is called once per frame
    void Update() {
        if (enemiesOnScreen < numEnemiesOnScreen && lastSpawn <= 0.0f)
        {
            HangerBaySpawn currentHander = hangers[Random.Range(0, hangers.Length)];
            currentHander.SpawnEnemy();
            enemiesOnScreen++;
            lastSpawn = spawnTimer;
            player.UpdateScore(-10);
        }

        if (lastSpawn > 0.0f)
            lastSpawn -= Time.deltaTime;

        if (criticalAreas == 0)
            DestroyBoss();
    }

    public void DecrementCriticalAreas()
    {
        player.UpdateScore(200);
        instance.criticalAreas -= 1;
    }

    public void DestroyBoss()
    {
        player.UpdateScore(1000);

        TransitionPhaseTwo();
    }

    private void TransitionPhaseTwo()
    {
        BossPhaseTwo newBossPhase = Instantiate(bossTwoPrefab, transform.position, transform.rotation);
        newBossPhase.InitializePhase(missileTurretOneAlive, missileTurretTwoAlive, scatterTurretOneAlive, scatterTurretTwoAlive, enemiesOnScreen);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().DamagePlayer(8);
        }
    }

    public void KilledEnemy(HealthManager.DamagedByType type)
    {
        enemiesOnScreen--;
        if (enemiesOnScreen < 0)
            enemiesOnScreen = 0;

        lastSpawn = spawnTimer;
    }
}
