using UnityEngine;

public class PlayerClassifier : MonoBehaviour
{
    [SerializeField] private DatasetLoader datasetLoader;

    public string ClassifyPlayer(int kills, float damage, float survival, float accuarcy)
    {
        PlayerData nearest = null;

        float nearestDistance = float.MaxValue;

        foreach (PlayerData sample in datasetLoader.DataSet)
        {
            float distance = CalculateDistance(kills, damage, survival, accuarcy, sample);

            if(distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = sample;
            }
        }
        Debug.Log($"Jugador Clasificado como {nearest.playerType} +\nDistancia: {nearestDistance}");
        return nearest.playerType;
    }
    float CalculateDistance(int kills, float damage, float survival, float accuarcy, PlayerData sample)
    {
        float dKills = Mathf.Pow(kills - sample.enemiesKilled, 2);
        float dSurvival = Mathf.Pow(survival - sample.survivalTime, 2);
        float dDamage = Mathf.Pow(damage - sample.damageTaken, 2);
        float dAccuarcy = Mathf.Pow(accuarcy - sample.accuarcy, 2);

        return Mathf.Sqrt(dKills + dDamage + dSurvival + dAccuarcy);
    }
}
