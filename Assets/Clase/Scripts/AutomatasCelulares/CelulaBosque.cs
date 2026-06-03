using UnityEngine;

public class CelulaBosque : MonoBehaviour {

    public EstadoCelula currentState;
    Renderer rend;

    private void Awake() {
        rend = GetComponent<Renderer>();
    }

    public void SetState(EstadoCelula state) {
        currentState = state;

        switch (state) {
            case EstadoCelula.Empty:
                rend.material.color = Color.black;
                break;
            case EstadoCelula.Tree:
                rend.material.color = Color.green;
                break;
            case EstadoCelula.Burning:
                rend.material.color = Color.red;
                break;
            case EstadoCelula.Ash:
                rend.material.color = Color.gray;
                break;
        }
    }
}
