using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button startButton;
    public Button controlsButton;
    public Button exitButton;
    public Text levelText;
    public Slider levelSlider;
    public Canvas controlsCanvas;
    public Text controlsText;
    public Image controlsImage;

    public Sprite[] controlsSprites;
    public string[] controlsStrings = { "controller", "player 1", "player 2" };
    public int selectedControlsIndex = 0;

    private static bool openMenu = true;
    public static UIManager instance;

    void Awake()
    {
        gameObject.SetActive(openMenu);
        Debug.Assert(instance == null);
        instance = this;
        controlsCanvas.gameObject.SetActive(false);
    }
	
    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            Time.timeScale = 0.0f;
//            Camera.main.Set

        }
    }

    public void StartButtonClicked()
    {
        openMenu = false;
        Time.timeScale = 1.0f;
        GameHandler.ChangeLevel((int)levelSlider.value);
        gameObject.SetActive(false);
    }

    public void ExitButtonClicked()
    {
        Application.Quit();
    }

    public void ControlsButtonClicked()
    {
        startButton.interactable = false;
        controlsButton.interactable = false;
        exitButton.interactable = false;
        levelSlider.interactable = false;
        controlsCanvas.gameObject.SetActive(true);
        controlsCanvas.GetComponentInChildren<Button>().Select();
    }

    public void OnLevelSliderChange()
    {
        levelText.text = "level " + levelSlider.value;
    }

    public void MenuButtonPressed()
    {
        Time.timeScale = (1.0f - Time.timeScale);
        openMenu = !openMenu;
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void OnControlsBack()
    {
        startButton.interactable = true;
        controlsButton.interactable = true;
        exitButton.interactable = true;
        levelSlider.interactable = true;
        controlsButton.Select();
        controlsCanvas.gameObject.SetActive(false);
    }

    public void OnControlsLeft()
    {
        selectedControlsIndex = (selectedControlsIndex - 1);
        if (selectedControlsIndex < 0)
            selectedControlsIndex = controlsSprites.Length - 1;
        UpdateOntrolsSprite();
    }

    public void OnControlsRight()
    {
        selectedControlsIndex = (selectedControlsIndex + 1) % controlsSprites.Length;
        UpdateOntrolsSprite();
    }

    public void UpdateOntrolsSprite()
    {
        controlsImage.sprite = controlsSprites[selectedControlsIndex];
        controlsText.text = controlsStrings[selectedControlsIndex];
    }

    public void OnLastLevelDone()
    {
        MenuButtonPressed();
    }
}
