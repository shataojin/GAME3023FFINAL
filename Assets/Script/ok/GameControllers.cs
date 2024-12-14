using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//inspired by Joss Lab Codes made a game stats for change between the battle screen and world screen
// taojin sha
//2024.10.18
public enum GameState { Walk, Battle }

public class GameControllers : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField]  battleSystem battleSystems;
    [SerializeField]  Camera worldCamera;
    [SerializeField] AudioManager audioManager;
    GameState state;

    public static GameControllers Instance { get; private set; }

    private void Awake()
    {
       
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject); 
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

    
    }

    void Start()
    {
        playerController.OnEncountered += StartBattle;
        battleSystems.OnBattleOver += EndBattle;
        PlayAudioTrack("NormalSound");
    }

    private void OnDestroy()
    {
        playerController.OnEncountered -= StartBattle;
        battleSystems.OnBattleOver -= EndBattle;

    }

    void StartBattle()
    {
        state = GameState.Battle;
        battleSystems.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);
        PlayAudioTrack("FightSound");

    }

    void EndBattle()
    {
        state = GameState.Walk;
        battleSystems.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
        playerController.EndEncounter = true;
        PlayAudioTrack("NormalSound");
    }

    public void SetNewGame(bool newGame)
    {
        PlayerPrefs.SetInt("NewGame", newGame ? 1 : 0);
        PlayerPrefs.Save();
    }

    void Update()
    {
        if (state == GameState.Walk)
        {
            playerController.NeedUpdate();
        }
        else if (state == GameState.Battle)
        {
            battleSystems.NeedUpdate();
        }
       
    }

    private void PlayAudioTrack(string trackName)
    {
        if (audioManager != null)
        {
            audioManager.PlayTrack(trackName);
        }
        else
        {
            Debug.LogError("AudioManager not found!");
        }
    }

}
