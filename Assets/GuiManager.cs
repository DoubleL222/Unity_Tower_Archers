using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour {

    [Header("Public REFERENCES")]
    public GameObject LogoPanel;
    public GenericCircularMenu MainMenu;
    public GameObject MainMenuPanel;
    public GenericCircularMenu UpgradeMenu;
    public GameObject UpgradeMenuPanel;
    public GenericCircularMenu Ending;
    public GameObject EndingPanel;
    public GameObject WinImage;
    public GameObject LoseImage;
    public GameObject GameRunningPanel;

    public Text livesLeftText;

    private GenericCircularMenu CurrentPanel;

    private static GuiManager _instance;
    public static GuiManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GuiManager>();
            }
            return _instance;
        }
    }

    public void ShowMainMenuPanel(bool _show)
    {
        MainMenuPanel.SetActive(_show);
        CurrentPanel = MainMenu;
    }

    public void ShowEndingPanel(bool _show, bool _won)
    {
        EndingPanel.SetActive(_show);
        CurrentPanel = Ending;

        if (_won)
        {
            WinImage.SetActive(true);
        }
        else
        {
            LoseImage.SetActive(true);
        }
    }

    public void HideEndingPanel()
    {
        EndingPanel.SetActive(false);
        CurrentPanel = Ending;
        WinImage.SetActive(false);
        LoseImage.SetActive(false);
    }

    public void ShowUpgradeMenu(bool _show)
    {
        UpgradeMenuPanel.SetActive(_show);
        CurrentPanel = UpgradeMenu;
    }


    public void HandleAButton()
    {
        CurrentPanel.HandleButtonA();
    }

    public void HandleJoystickChange(AirConsolePlayer player, bool pressed, Vector2 pos)
    {
        if (player.holdinJoystick)
        {
            if (pressed)
            {
                
                CurrentPanel.MovePointer(player, pos);
                
            }
            else
            {
                CurrentPanel.ResetPointer();
                // Debug.Log("let go with joystick position "+ pos); 
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

    public void ShowLogoPanel(bool _show)
    {
        LogoPanel.SetActive(_show);
    }

    public void HideLogoAfter(float _seconds)
    {
        StartCoroutine(HideLogoPanelAfter(1.0f));
    }

    public void ShowGameRunningPanel(bool _show)
    {
        GameRunningPanel.SetActive(_show);
    }

    IEnumerator HideLogoPanelAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Debug.Log("HIDE PANEL");
        ManagerFSM.InvokeEvent(enumEvents.OnIntroEnd);
        yield return null;
    }

    public void UpdateLivesText(int _lives)
    {
        livesLeftText.text = _lives.ToString();
    }

    // Use this for initialization
    void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
