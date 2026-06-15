using UnityEngine;
using System.Collections;
public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;

    [Header("Player metrics")]
    public int enemiesKilled;
    public float damageTaken;
    public float survivalTime;
    public float accuracy;

    [Header("Performace")]
    [Range(0, 100)]
    public float performanceScore;
    public DifiicultyLevel currentDifficulty;

    [Header("EnemyComposistion")]
    [Range(0, 1)]
    public float aggresiveChance;
    [Range(0, 1)]
    public float tacticChance;

    [SerializeField]PlayerClassifier classifier;
    [SerializeField]string playerProfile;
    private void Awake()
    {
        Instance = this;

    }
    private void Start()
    {
        StartCoroutine(Evaluation());
    }
    private void Update()
    {
        survivalTime += Time.deltaTime;
    }
    public void EvaluatePlayerPerformance()
    {
        CalculatePerformanceScore();
        CalculateDifficulty();
        CalculateEnemyComposition();

        Debug.Log($"Kills: {enemiesKilled} \nDamage: {damageTaken}\nSurvivalTime: {survivalTime}\nAccuarcy: {accuracy}\nPlayerProfile {playerProfile}");
    }

    private void CalculatePerformanceScore()
    {
        //inventar formula
        //float score = (enemiesKilled * 2f) + (survivalTime * 0.05f) + (accuracy * 0.5f) - (damageTaken * .1f);
        //performanceScore = Mathf.Clamp(score, 0f, 100f);

        playerProfile = classifier.ClassifyPlayer(enemiesKilled, damageTaken, survivalTime, accuracy);
    }
    private void CalculateDifficulty()
    {
        //if(performanceScore < 30)
        //{
        //    currentDifficulty = DifiicultyLevel.Easy;
        //}
        //else if(performanceScore < 70)
        //{
        //    currentDifficulty = DifiicultyLevel.Medium;
        //}
        //else
        //{
        //    currentDifficulty = DifiicultyLevel.Hard;
        //}
        if (playerProfile == "Novice")
        {
            currentDifficulty = DifiicultyLevel.Easy;
        }
        else if (playerProfile == "Intermediate")
        {
            currentDifficulty = DifiicultyLevel.Medium;
        }
        else
        {
            currentDifficulty = DifiicultyLevel.Hard;
        }
    }
    public void CalculateEnemyComposition()
    {
        tacticChance = Mathf.Clamp01(performanceScore / 100);

        aggresiveChance = 1f - tacticChance;
    }
    public void RegisterKill()
    {
        enemiesKilled++;
    }
    public void RegisterDamageTaken(float damageTaken)
    {
        this.damageTaken += damageTaken;
    }
    public void RegisterAccuracy(float value)
    {
        accuracy=value;
    }
    public void ResetMetrics()
    {
        enemiesKilled = 0;
        damageTaken = 0;
        accuracy = 0f;
        survivalTime = 0f;
    }
    IEnumerator Evaluation()
    {
        while (true)
        {
            EvaluatePlayerPerformance();
            yield return new WaitForSeconds(1f);
        }
    }
}
