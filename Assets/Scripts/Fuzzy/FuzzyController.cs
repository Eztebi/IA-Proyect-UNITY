using UnityEngine;

public class FuzzyController : MonoBehaviour
{
    public Transform player;
    public float health = 100;

    public MovementController movementController;
    private void Start()
    {
        movementController = this.GetComponent<MovementController>();
    }
    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        float rulesAttackStrong = AND(HighHealth(health), CloseDistance(distance));
        float rulesAttackMedium = AND(MediumHelath(health), CloseDistance(distance));
        float rulesFlee = AND(LowHealth(health), CloseDistance(distance));
        float rulesPatrol = AND(FarDistance(distance), HighHealth(health));

        //Defusificacion es dar prioridad a las reglas
        float attack = (rulesAttackStrong * 1.0f) + (rulesAttackMedium * .6f);
        float flee = (rulesFlee * 1.0f);
        float patrol = (rulesPatrol * 1.0f);

        string debugMessage = string.Format("<color=yellow> ValoresDifusos:</color>" + 
                                            "<color=green> Attack:{0:F2}</color>|"+ 
                                            "<color=red> Flee:{0:F2}</color>|"+ 
                                            "<color=cyan> Patrol:{0:F2}</color>|",
                                            attack,flee, patrol);
        Debug.Log(debugMessage);
        if (attack > flee && attack > patrol) Chase();
        else if(flee> attack && flee > patrol) Flee();
        Patrol();
    }
    void Patrol()
    {
        transform.Rotate(0, 50 * Time.deltaTime,0);
    }
    void Chase()
    {
        movementController.MoveTowards(player.position);
    }
    void Flee()
    {
        movementController.MoveAway(player.position);
    }
    //Fuzzification (Conjunto difusos)
    //Hombro Izquierdo
    //<=30 vida | 30 a 60
    float LowHealth(float health)
    {
        if (health <= 30) return 1f;
        if (health >= 60) return 0f;

        return (60 - health) / 30;
    }

    //Triangular
    // 50 es el pico | 30 y 70 sus bajos
    float MediumHelath(float health) {
        if (health <= 30 || health >= 70) return 0f;
        if (health == 50) return 1f;

        if (health < 50) return (health - 30 / 20f);

        return (70 - health) / 20f;
    }

    //Hombro Derecho
    //>= 80 | 50 a 80
    float HighHealth(float health)
    {
        if (health <= 50) return 0;
        if (health >= 80) return 1f;

        return (health - 50) / 30f;
    }

    //Hombro Izquierdo
    //<=5 | 5 a 15
    float CloseDistance(float distance)
    {
        if (distance <= 5) return 1f;
        if (distance >= 15) return 0f;

        return (15 - distance) / 10;
    }

    //HOMBRO IZQUIERDO
    // >=20 | 10 a 20
    float FarDistance(float distance)
    {
        if (distance >= 10) return 0f;
        if (distance >= 20) return 1f;

        return (distance - 10) / 10;    
    }
    ///Operadores Difusass
    float AND(float a, float b)
    {
        return Mathf.Min(a, b);
    }
    float OR(float a, float b)
    {
        return Mathf.Max(a, b);
    }
    float NOT(float a)
    {
        return 1 - a;
    }
}
