using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isAlive;
    Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();    
    }
    public void SetState(bool ALIVE)
    {
        isAlive = ALIVE;
        rend.material.color = ALIVE ? Color.white : Color.black;
    }
}
