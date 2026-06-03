using UnityEngine;
using System.Collections;

public class GameOfLife : MonoBehaviour {

    public int width = 30;
    public int height = 30;

    public GameObject celulaPrefab;

    private Celula[,] grid;
    private bool[,] nextState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        grid = new Celula[width, height];
        nextState = new bool[width, height];

        GenerateGrid();
        StartCoroutine(SimulationLoop());
    }

    void GenerateGrid() {
        for(int x = 0; x<width; x++) {
            for(int y = 0; y < height; y++) {
                GameObject obj = Instantiate(celulaPrefab);
                obj.transform.position = new Vector3(x,0,y);
                obj.transform.parent = this.transform;

                Celula cel = obj.GetComponent<Celula>();
                bool alive = Random.value > 0.7;
                cel.SetState(alive);
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

                if(nx >= 0 && nx < width && ny >= 0 && ny < height) {
                    if (grid[nx, ny].isAlive) count++;
                }
            }
        }
        return count;
    }

    void Simulate() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                int neighbors = CountNeighbors(x,y);

                bool alive = grid[x, y].isAlive;

                if (alive) {
                    nextState[x, y] = neighbors == 2 || neighbors == 3;
                } else {
                    nextState[x, y] = neighbors == 3;
                }
            }
        }
        ApplyNextState();
    }

    void ApplyNextState() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                grid[x, y].SetState(nextState[x,y]);
            }
        }
    }

    IEnumerator SimulationLoop() {
        while (true) {
            Simulate();

            yield return new WaitForSeconds(0.3f);
        }
    }
}
