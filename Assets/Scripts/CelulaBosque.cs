using UnityEngine;

public class CelulaBosque : MonoBehaviour
{
    public EstadoCelula currentState;
    public Renderer render;

    private void Awake()
    {
        render = GetComponent<Renderer>();
    }
    public void SetState(EstadoCelula state)
    {
        currentState = state;

        switch (state)
        {
            case EstadoCelula.Empty:
                render.material.color = Color.black;
                break;
            case EstadoCelula.Tree:
                render.material.color = Color.green;
                break;
            case EstadoCelula.Burning:
                render.material.color = Color.red;
                break;
            case EstadoCelula.Ash:
                render.material.color = Color.gray;
                break;
            
        }   
    }

}
