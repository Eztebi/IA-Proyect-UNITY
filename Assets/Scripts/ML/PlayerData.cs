using System;

[Serializable]
public class PlayerData
{
    public int enemiesKilled;
    public float damageTaken;
    public float survivalTime;
    public float accuarcy;

    public string playerType;

    public PlayerData(int kills, float damage, float surviveTime, float accuarcy, string type)
    {
        enemiesKilled = kills;
        damageTaken = damage;
        survivalTime = surviveTime;
        this.accuarcy = accuarcy;
        playerType = type;
    }
}
