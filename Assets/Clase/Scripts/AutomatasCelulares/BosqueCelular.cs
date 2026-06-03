using UnityEngine;
using System.Collections;

public class BosqueCelular : MonoBehaviour {

    public int width = 50;
    public int height = 50;

    public GameObject celulaPrefab;

    private CelulaBosque[,] grid;
    private EstadoCelula[,] nextState;

    public float spreadChance = 0.4f;
    public float spotaneousFireChance = 0.001f;

    void Start() {
        grid = new CelulaBosque[width, height];
        nextState = new EstadoCelula[width, height];

        GenerateGrid();
        StartCoroutine(SimulationLoop());
    }

    void GenerateGrid() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                GameObject obj = Instantiate(celulaPrefab);
                obj.transform.position = new Vector3(x, 0, y);
                obj.transform.parent = this.transform;


                CelulaBosque celula = obj.GetComponent<CelulaBosque>();

                if(Random.value > 0.3) {
                    celula.SetState(EstadoCelula.Tree);
                } else {
                    celula.SetState(EstadoCelula.Empty);
                }

                grid[x, y] = celula;
            }
        }
    }

    int CountNeighbors(int x, int y, EstadoCelula state) {
        int count = 0;

        for (int dx = -1; dx <= 1; dx++) {
            for (int dy = -1; dy <= 1; dy++) {
                if (dx == 0 && dy == 0)
                    continue;

                int nx = x + dx;
                int ny = y + dy;

                if (nx >= 0 && nx < width && ny >= 0 && ny < height) {
                    if (grid[nx, ny].currentState == state) count++;
                }
            }
        }
        return count;
    }

    void Simulate() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                CelulaBosque celula = grid[x, y];

                switch (celula.currentState) {
                    case EstadoCelula.Empty:
                        nextState[x, y] = EstadoCelula.Empty;
                        break;
                    case EstadoCelula.Tree:
                        int burningNeighbors = CountNeighbors(x, y, EstadoCelula.Burning);

                        if(burningNeighbors > 0 && Random.value < spreadChance) {
                            nextState[x, y] = EstadoCelula.Burning;
                        } else if(Random.value < spotaneousFireChance) {
                            nextState[x, y] = EstadoCelula.Burning;
                        } else {
                            nextState[x, y] = EstadoCelula.Tree;
                        }
                        break;
                    case EstadoCelula.Burning:
                        nextState[x, y] = EstadoCelula.Ash;
                        break;
                    case EstadoCelula.Ash:
                        int treeNeighbor = CountNeighbors(x, y, EstadoCelula.Tree);

                        if(treeNeighbor >= 3) {
                            nextState[x, y] = EstadoCelula.Tree;
                        } else {
                            nextState[x, y] = EstadoCelula.Ash;
                        }
                        break;
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

    IEnumerator SimulationLoop() {
        while (true) {
            Simulate();

            yield return new WaitForSeconds(0.3f);
        }
    }
}
