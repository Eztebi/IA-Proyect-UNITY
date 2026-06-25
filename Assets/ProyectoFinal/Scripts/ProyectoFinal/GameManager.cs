using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

enum GameState
{
    Menu,
    Playing,
    Win,
    Lose
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    GameState state;
    [SerializeField] UIManager uiManager;
    UnityEvent<GameState> stateEvent;
    public PlayerMovement player;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Menu();
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        state = GameState.Menu;
        //uiManager.OpenMenuPanel();
    }
    public void Lose()
    {
        uiManager.OpenLosePanel();
        state = GameState.Lose;
        player.NotPlaying();
    }
    public void Win()
    {
        uiManager.OpenWinPanel();
        state = GameState.Win;
        player.NotPlaying();
    }
    public void Menu()
    {
        uiManager.OpenMenuPanel();
        state = GameState.Menu;
        player.NotPlaying();
    }
    public void Play()
    {
        player.StartPlaying();
        uiManager.RemoveAllPanels();
        state = GameState.Playing;
    }
}
