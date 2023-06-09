using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{

    public int GameTimer;
    public static Game Instance { get; private set; }
    public GameState State {  get; private set; }
    [field: SerializeField] public LayerMask PlayerMask { get; private set; }
    public HumanPlayer Player { get; private set; }
    public float PlayerOFFTime { get; private set; }
    [field: SerializeField] public int OffStrikeBonusScore { get; private set; }
    private PlayerData _playerData;

    public static event Action<int> TimerTicked;
    public static event Action GameStarted;
    public static event Action GameOvered;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        _playerData = SaveProvider.Instace.CurrentPlayer;
        PlayerOFFTime = _playerData.OFFTime;
        State = GameState.Init;
        StartCoroutine(InitGame());
    }


    private IEnumerator InitGame()
    {
        InitPlayer();
        yield return null;
        InitDrinks();
        StartCoroutine(StartGame());
    }


    private void InitPlayer()
    {
        Player = FindObjectOfType<HumanPlayer>();
        Player.Init(_playerData);
    }
    private void InitDrinks()
    {
        foreach (var drink in FindObjectsByType<Drink>(FindObjectsSortMode.None))
        {
            drink.Init(this);
        }
    }

    private IEnumerator StartGame()
    {
        State = GameState.Play;
        GameStarted?.Invoke();
        
        TimerTicked?.Invoke(GameTimer);
        while (GameTimer > 0)
        {
            yield return new WaitForSeconds(1f);
            GameTimer--;
            TimerTicked?.Invoke(GameTimer);
        }
        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        State = GameState.GameOver;
        GameOvered?.Invoke();
        yield return null;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene(1);
        }
    }

}



public enum GameState
{
    Init,
    Play,
    GameOver
}
