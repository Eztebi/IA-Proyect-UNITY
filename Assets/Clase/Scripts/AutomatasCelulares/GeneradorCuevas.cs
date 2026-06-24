using UnityEngine;
using System.Collections;

public class GeneradorCuevas : MonoBehaviour {

    public int width = 100;
    public int height = 100;

    [Range(0f, 1f)]
    public float initialWallChance = 0.45f;

    public int simulationSteps = 5;

    public GameObject celulaPrefab;

    private CelulaCueva[,] grid;
    private bool[,] nextState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        grid = new CelulaCueva[width, height];
        nextState = new bool[width, height];

        GenerateGrid();
        StartCoroutine(GenerateCave());
    }

    void GenerateGrid() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                GameObject obj = Instantiate(celulaPrefab);
                obj.transform.position = new Vector3(x * 0.5f, 0, y * 0.5f);
                obj.transform.parent = this.transform;

                CelulaCueva cel = obj.GetComponent<CelulaCueva>();

                bool isBorder = x == 0 || y == 0 || x == width - 1 || y == height - 1;
                bool wall = isBorder || Random.value < initialWallChance;

                cel.SetState(wall);
                grid[x, y] = cel;
            }
        }
    }

    int CountNeighbors(int x, int y) {
        int count = 0;

        for (int dx = -1; dx <= 1; dx++) {
            for (int dy = -1; dy <= 1; dy++) {
                if (dx == 0 && dy == 0)
                    continue;

                int nx = x + dx;
                int ny = y + dy;

                if (nx < 0 || ny < 0 || nx >= width || ny >= height) {
                    count++;
                } else if (grid[nx, ny].isWall) {
                    count++;
                }
            }
        }
        return count;
    }

    void Simulate() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                int wallNeighbors = CountNeighbors(x, y);

                bool currentWall = grid[x, y].isWall;

                if (currentWall) {
                    nextState[x, y] = wallNeighbors >= 4;
                } else {
                    nextState[x, y] = wallNeighbors >= 5;
                }
            }
        }
        ApplyNextState();
    }

    void ApplyNextState() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                grid[x, y].SetState(nextState[x, y]);
            }
        }
    }

    IEnumerator GenerateCave() {
        for (int i = 0; i < simulationSteps; i++) {
            Simulate();
            yield return new WaitForSeconds(1f);
        }
    }

    void Regenerate() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        GenerateGrid();
        StopAllCoroutines();
        StartCoroutine(GenerateCave());
    }
}
