using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;
//ENUM FOR EVENTS
public enum enumEvents
{
    OnShowIntro,
    OnIntroEnd,
    OnQuickGame,
    OnNextWave,
    OnWaveEnd,
    OnPauseMenu,
    OnContinueUpgrade,
    OnContinueWave,
    OnGameFinish,
    OnGameExit
}
//ENUM FOR STATES
public enum enumStates
{
    ENTRY,
    IntroLogo,
    MainMenu,
    GameIntro,
    Wave,
    UpgradeMenu,
    Pause,
    GameFinished,
    QUIT
}
public class GameController : MonoBehaviour {
    public List<Transform> spawnPoints;
    private int currentSpawnPoint = 0;
    public readonly int startingLives = 15;

    public PlayerController player1;

    [Header("References")]
    public GameObject arrowPrefab;
    public GameObject playerPrefab;

    [Header("Settings")]
    public float arrowSpeed;

    private bool wonQuickGame = false;
    private int livesLeft;

    private static GameController _instance;
    public static GameController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameController>();
            }
            return _instance;
        }
    }

    public void InitFsm()
    {
        ManagerFSM.AddAllStates();
        ManagerFSM.AddEventConsequence(enumStates.ENTRY, enumEvents.OnShowIntro, enumStates.IntroLogo);
        ManagerFSM.AddEventConsequence(enumStates.IntroLogo, enumEvents.OnIntroEnd, enumStates.MainMenu);
        ManagerFSM.AddEventConsequence(enumStates.MainMenu, enumEvents.OnQuickGame, enumStates.GameIntro);
        ManagerFSM.AddEventConsequence(enumStates.GameIntro, enumEvents.OnNextWave, enumStates.Wave);
        ManagerFSM.AddEventConsequence(enumStates.Wave, enumEvents.OnWaveEnd, enumStates.UpgradeMenu);
        ManagerFSM.AddEventConsequence(enumStates.UpgradeMenu, enumEvents.OnNextWave, enumStates.Wave);
        ManagerFSM.AddEventConsequence(enumStates.UpgradeMenu, enumEvents.OnPauseMenu, enumStates.Pause);
        ManagerFSM.AddEventConsequence(enumStates.Wave, enumEvents.OnPauseMenu, enumStates.Pause);
        ManagerFSM.AddEventConsequence(enumStates.Wave, enumEvents.OnGameFinish, enumStates.GameFinished);
        ManagerFSM.AddEventConsequence(enumStates.Wave, enumEvents.OnGameExit, enumStates.MainMenu);
        ManagerFSM.AddEventConsequence(enumStates.GameFinished, enumEvents.OnGameExit, enumStates.MainMenu);
        ManagerFSM.AddEventConsequence(enumStates.GameFinished, enumEvents.OnQuickGame, enumStates.GameIntro);
        ManagerFSM.AddEventConsequence(enumStates.Pause, enumEvents.OnQuickGame, enumStates.GameIntro);
        ManagerFSM.AddEventConsequence(enumStates.MainMenu, enumEvents.OnGameExit, enumStates.QUIT);
        ManagerFSM.AddOnEnterCall(enumStates.MainMenu, OnMainMenuEnter);
        ManagerFSM.AddOnExitCall(enumStates.MainMenu, OnMainMenuExit);
        ManagerFSM.AddOnEnterCall(enumStates.IntroLogo, OnIntroEnter);
        ManagerFSM.AddOnExitCall(enumStates.IntroLogo, OnIntroExit);
        ManagerFSM.AddOnEnterCall(enumStates.GameIntro, OnGameIntroEnter);
        ManagerFSM.AddOnExitCall(enumStates.GameIntro, OnGameIntroExit);
        ManagerFSM.AddOnEnterCall(enumStates.Wave, OnWaveEnter);
        ManagerFSM.AddOnExitCall(enumStates.Wave, OnWaveExit);
        ManagerFSM.AddOnEnterCall(enumStates.UpgradeMenu, OnUpgradeMenuEnter);
        ManagerFSM.AddOnExitCall(enumStates.UpgradeMenu, OnUpgradeMenuExit);
        ManagerFSM.AddOnEnterCall(enumStates.GameFinished, OnGameFinishEnter);
        ManagerFSM.AddOnExitCall(enumStates.GameFinished, OnGameFinishExit);

        ManagerFSM.ForceState(enumStates.ENTRY);
        ManagerFSM.InvokeEvent(enumEvents.OnShowIntro);
    }

    void OnIntroEnter()
    {
        GuiManager.instance.ShowLogoPanel(true);
        GuiManager.instance.HideLogoAfter(1.0f);
    }

    void OnIntroExit()
    {
        GuiManager.instance.ShowLogoPanel(false);
    }

    void OnMainMenuEnter()
    {
        GuiManager.instance.ShowMainMenuPanel(true);
    }

    void OnMainMenuExit()
    {
        GuiManager.instance.ShowMainMenuPanel(false);
    }

    void OnGameIntroEnter()
    {
        livesLeft = startingLives;
        wonQuickGame = false;
        foreach (AirConsolePlayer _player in AirConsolePlayerKeeper.instance.GetAllActivePlayers())
        {
            InstantiateBowmanForPlayer(_player);
        }
        ManagerFSM.InvokeEvent(enumEvents.OnNextWave);
    }

    void OnGameIntroExit()
    {
        UnitSpawnManager.instance.ResetForNewGame();
        GuiManager.instance.ShowGameRunningPanel(true);
    }

    void OnWaveEnter()
    {
        UnitSpawnManager.instance.StartWave();
    }

    void OnWaveExit()
    {
        UnitSpawnManager.instance.EndWave();
    }

    void OnUpgradeMenuEnter()
    {
        GuiManager.instance.ShowUpgradeMenu(true);
    }

    void OnUpgradeMenuExit()
    {
        GuiManager.instance.ShowUpgradeMenu(false);
    }

    void OnGameFinishEnter()
    {
        GuiManager.instance.ShowGameRunningPanel(false);
        GuiManager.instance.ShowEndingPanel(true, wonQuickGame);
       
    }

    void OnGameFinishExit()
    {
        GuiManager.instance.HideEndingPanel();
    }

    void OnPauseMenuEnter()
    {

    }

    void OnPauseMenuExit()
    {

    }

    void OnQUITEnter()
    {

    }



	// Use this for initialization
	void Start () {
        _instance = this;
        InitFsm();
    }

    public void EnemyReachedHeart()
    {
        livesLeft--;
        GuiManager.instance.UpdateLivesText(livesLeft);
        if (livesLeft <= 0)
        {
            wonQuickGame = false;
            ManagerFSM.InvokeEvent(enumEvents.OnGameFinish);
        }
    }

    public void wonGame()
    {
        wonQuickGame = true;
    }

    public void InstantiateBowmanForPlayer(AirConsolePlayer _player)
    {
        if (_player.playerController == null)
        {
            GameObject newPlayer = Instantiate(playerPrefab, spawnPoints[currentSpawnPoint].position, Quaternion.identity) as GameObject;
            _player.playerController = newPlayer.GetComponent<PlayerController>();
            currentSpawnPoint++;
            currentSpawnPoint = currentSpawnPoint % spawnPoints.Count;
        }
    }

    public void HandleButtonA(AirConsolePlayer player)
    {
        UnitSpawnManager.instance.SpawnFriendly();
    }

    void HandleButtonB(AirConsolePlayer player)
    {

    }

    public void HandleJoystickChange(AirConsolePlayer player, bool pressed, Vector2 pos)
    {
        if (player.holdinJoystick)
        {
            if (pressed)
            {
                //Debug.Log("holding");
            }
            else
            {
               // Debug.Log("Previous joystick position " + player.prevjoystickPos);
               // Debug.Log("let go with joystick position "+ pos);
                FireArrow(player, pos);
            }
        }
        else
        {
            if (pressed)
            {
               // Debug.Log("started holding");
            }
            else
            {
               // Debug.Log("What the fuck!?");
            }
        }
        player.holdinJoystick = pressed;
        player.prevjoystickPos = pos;
    }

    void FireArrow(AirConsolePlayer _player, Vector2 _lastJoystickPosition)
    {
        Debug.Log("FIRE ARROW");
        Vector2 dir = new Vector2(-_lastJoystickPosition.x, _lastJoystickPosition.y);
        _player.playerController.FireArrow(arrowPrefab, _player.playerController.arrowSpawn.position, dir, arrowSpeed);
        //player1.FireArrow(arrowPrefab, player1.arrowSpawn.position, dir, arrowSpeed);
    }
}
