using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public event Action OnLevelCopleted;

    public static LevelController Instance { get; private set; }

    private VictoryArea victoryArea;
    private BlackScreenUI blackScreenUI;
    private List<EnemyBase> enemyList = new List<EnemyBase>();

    private void Awake()
    {
        InitializeSingleton();

        victoryArea = FindFirstObjectByType<VictoryArea>();
        victoryArea.gameObject.SetActive(false);

        EnemyBase.OnAnyEnemySpawned += EnemyBase_OnAnyEnemySpawned;
        EnemyBase.OnAnyEnemyDead += EnemyBase_OnAnyEnemyDead;
    }

    private void InitializeSingleton()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one " + GetType().Name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        blackScreenUI = FindFirstObjectByType<BlackScreenUI>();
        blackScreenUI.ToBlack();
        StartCoroutine(FromBlackScreenAfterTime(0.1f));

        PlayerController.Instance.OnPlayerDead += PlayerController_OnPlayerDead;
    }

    private void PlayerController_OnPlayerDead()
    {
        TimeScaler.SetIsLoading(true);
        PauseUIManager.Instance.UpdateUI(PauseUIManager.PauseWindowType.Death);
    }

    private IEnumerator FromBlackScreenAfterTime(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        blackScreenUI.FromBlackOverTime(5);
    }

    private void EnemyBase_OnAnyEnemySpawned(EnemyBase spawnedEnemy)
    {
        if (enemyList.Contains(spawnedEnemy))
            return;
        enemyList.Add(spawnedEnemy);
    }

    private void EnemyBase_OnAnyEnemyDead(EnemyBase deadEnemy)
    {
        if (enemyList.Contains(deadEnemy))
        {
            enemyList.Remove(deadEnemy);
            CheckForVictory();
        }
    }

    private void CheckForVictory()
    {
        if (enemyList.Count == 0)
        {
            victoryArea.gameObject.SetActive(true);
            OnLevelCopleted?.Invoke();

        }
    }
    private void OnDestroy()
    {
        EnemyBase.OnAnyEnemySpawned -= EnemyBase_OnAnyEnemySpawned;
        EnemyBase.OnAnyEnemyDead -= EnemyBase_OnAnyEnemyDead;
        TimeScaler.SetIsLoading(false);
    }
}
