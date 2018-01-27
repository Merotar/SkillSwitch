using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject exitButton;
    public Text levelText;
    public Slider levelSlider;

    private static bool openMenu = true;
    public static UIManager instance;

    void Awake()
    {
        gameObject.SetActive(openMenu);
        Debug.Assert(instance != null);
        instance = this;
    }
	
    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            Debug.Log("TEST");
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

    public void OnLevelSliderChange()
    {
        levelText.text = "level " + levelSlider.value;
    }

    public void MenuButtonPressed()
    {
        Time.timeScale = (1.0f - Time.timeScale);
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
