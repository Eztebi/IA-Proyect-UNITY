using UnityEngine;

public class CelulaCueva : MonoBehaviour
{
    public bool isWall;
    Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }
    public void SetState(bool ALIVE)
    {
        isWall = ALIVE;
        rend.material.color = ALIVE ? Color.white : Color.black;
        if (isWall)
        {
            rend.enabled = true;
            rend.material.color = Color.white;
        }
        else
        {
            rend.enabled = false;   
        }
    }

}
