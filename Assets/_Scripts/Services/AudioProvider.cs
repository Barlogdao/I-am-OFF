using RB.Services.Audio;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioProvider : MonoBehaviour
{
    private AudioService _audioService;

    public static AudioProvider Instance { get; private set; }

    [SerializeField] private AudioClip _levelOneSobriety;
    [SerializeField] private AudioClip _levelTwoSobriety;
    [SerializeField] private AudioClip _levelThreeSobriety;
    [SerializeField] List<AudioClip> _drinkSounds;
    [SerializeField] private AudioClip _coctalSound;

    [SerializeField] private AudioClip _winSound;
    [SerializeField] private AudioClip _menuButtonSound;
    [SerializeField] private AudioClip _buySound;

    [SerializeField] private AudioClip _othodos;
    [SerializeField] private AudioClip _lightDrunkStage;
    [SerializeField] List<AudioClip> _veryDrunkStageSounds;

    [SerializeField] private AudioClip _backgroundChangedSound;
    [SerializeField] private AudioClip _playerChangedSound;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        _audioService = AudioService.Instance;
    }


    private void OnEnable()
    {
        HumanPlayer.SobrietyChanged += OnSobrietyChanged;
        //SceneManager.sceneLoaded += OnSceneLoaded;
        Game.GameOvered += OnGameOvered;
        HumanPlayer.PlayerMindChanged += OnMindChanged;
        HumanPlayer.CoctailDrinked += OnCoctailDrinked;
        HumanPlayer.PlayerDrinked += OnPlayerDrinked;
        HoveredButton.ButtonHovered += OnButtonHover;
        UnlockablePrefab.ElementPurchased += OnElementPurchased;
        BackGroundProvider.BackgroundChanged += OnBackgroundChanged;
        PlayerVisualPrefab.NewPlayerSelected += OnNewPlayerSelected;

    }

    private void OnNewPlayerSelected()
    {
        _audioService.PlaySound(_playerChangedSound);
    }

    private void OnBackgroundChanged()
    {
        _audioService.PlaySound(_backgroundChangedSound);
    }

    private void OnElementPurchased()
    {
        _audioService.PlaySound(_buySound);
    }

    private void OnButtonHover()
    {
        _audioService.PlaySound(_menuButtonSound);
    }

    private void OnPlayerDrinked()
    {
        _audioService.PlaySound(GetRandomSound(_drinkSounds));
    }

    private void OnCoctailDrinked(CocktailRecipeSO coctail)
    {
        _audioService.PlaySound(_coctalSound);
    }

    private AudioClip GetRandomSound(List<AudioClip> sounds)
    {
        return sounds[UnityEngine.Random.Range(0, sounds.Count)];
    }

    private void OnMindChanged(bool PlayerIsOff)
    {
        if (PlayerIsOff == true)
        {
            _audioService.PlaySound(GetRandomSound(_veryDrunkStageSounds));
        }
        else if (PlayerIsOff == false)
        {
            _audioService.PlaySound(_othodos);
        }
    }



    private void OnGameOvered()
    {
        _audioService.PlaySound(_winSound);
    }

    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    if (scene.buildIndex == 1)
    //    {
    //        _audioService.PlayMusic(_levelOneSobriety);
    //    }
    //    else if (scene.buildIndex == 3)
    //    {
    //        _audioService.PlayMusic(_levelThreeSobriety);
    //    }
    //}

    public void PlayMenuMusic()
    {
        _audioService.PlayMusic(_levelOneSobriety);
    }

    public void PlayCreditMusic()
    {
        _audioService.PlayMusic(_levelThreeSobriety);
    }

    private void OnSobrietyChanged(SobrietyLevel sobrietyLevel)
    {
        switch (sobrietyLevel)
        {
            case SobrietyLevel.Sober:
                _audioService.PlayMusic(_levelOneSobriety);
                break;
            case SobrietyLevel.Drunk:
                _audioService.PlayMusic(_levelTwoSobriety);
                _audioService.PlaySound(_lightDrunkStage);
                break;
            case SobrietyLevel.DrunkAsHell:
                _audioService.PlayMusic(_levelThreeSobriety);
                _audioService.PlaySound(GetRandomSound(_veryDrunkStageSounds));
                break;
        }
    }

    private void OnDisable()
    {
        HumanPlayer.SobrietyChanged -= OnSobrietyChanged;
        //SceneManager.sceneLoaded -= OnSceneLoaded;
        Game.GameOvered -= OnGameOvered;
        HumanPlayer.PlayerMindChanged -= OnMindChanged;
        HumanPlayer.CoctailDrinked -= OnCoctailDrinked;
        HumanPlayer.PlayerDrinked -= OnPlayerDrinked;
        HoveredButton.ButtonHovered -= OnButtonHover;
        UnlockablePrefab.ElementPurchased -= OnElementPurchased;
        BackGroundProvider.BackgroundChanged -= OnBackgroundChanged;
        PlayerVisualPrefab.NewPlayerSelected -= OnNewPlayerSelected;
    }
}

