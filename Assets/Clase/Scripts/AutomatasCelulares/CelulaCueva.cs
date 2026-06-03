using UnityEngine;

public class CelulaCueva : MonoBehaviour {

    public bool isWall;
    Renderer rend;

    private void Awake() {
        rend = GetComponent<Renderer>();
    }

    public void SetState(bool wall) {
        isWall = wall;

        if (wall) {
            rend.enabled = true;
            rend.material.color = Color.white;
        } else {
            rend.enabled = false;
        }
    }
}
