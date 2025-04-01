using System.Collections;
using UnityEngine;

public class BossDefeatedVictory : MonoBehaviour
{
    private BlackScreenUI blackScreenUI;

    private void Awake()
    {
        EnemyBase.OnAnyEnemyDead += EnemyBase_OnAnyEnemyDead;
    }

    private void EnemyBase_OnAnyEnemyDead(EnemyBase boss)
    {
        blackScreenUI = FindFirstObjectByType<BlackScreenUI>();
        StartCoroutine(ToBlackScreenAfterTime(2f));
    }

    private IEnumerator ToBlackScreenAfterTime(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        blackScreenUI.ToBlackOverTime(5);
        yield return new WaitForSeconds(2f);
        SceneLoader.Load(SceneLoader.Scene.VictoryScene);
    }

    private void OnDestroy()
    {
        EnemyBase.OnAnyEnemyDead -= EnemyBase_OnAnyEnemyDead;
    }
}
