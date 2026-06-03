using UnityEngine;

public class Celula : MonoBehaviour {

    public bool isAlive;
    Renderer rend;

    private void Awake() {
        rend = GetComponent<Renderer>();
    }

    public void SetState(bool alive) {
        isAlive = alive;
        rend.material.color = alive ? Color.white : Color.black;
    }
}
