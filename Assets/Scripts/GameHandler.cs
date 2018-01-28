using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System.IO;

public class GameHandler: MonoBehaviour
{
    private static GameHandler instance;

    private static bool running = false;

    private static int nextLevel = int.MinValue;

    public int firstLevel = 0;

    public static int maxLevel = 2;

    public float difficulty = 1;

    void Awake()
    {
        Debug.Assert(instance == null);
        instance = this;

        int numLevels = getNumLevelsFromFiles();

        if (nextLevel == int.MinValue)
            nextLevel = firstLevel;
        maxLevel = numLevels - 1;
        UIManager.instance.levelSlider.maxValue = maxLevel;
        Time.timeScale = difficulty;
    }

    void Start()
    {
        TiledJsonImporter.LoadLevel(nextLevel);
        instance.StartCoroutine(StartCoro());
    }

    private int getNumLevelsFromFiles()
    {
        DirectoryInfo info = new DirectoryInfo(TiledJsonImporter.instance.levelPath);
        FileInfo[] fileInfos = info.GetFiles();
        int sumLevels = 0;
        List<int> levelNumbers = new List<int>();
        Regex r = new Regex(@"^level(\d+)\.json$");
        foreach (FileInfo fi in fileInfos)
        {
            GroupCollection groups = r.Match(fi.Name).Groups;
            Match match = Regex.Match(fi.Name, @"^level(\d+)\.json$");
            if (match.Success)
            {
                levelNumbers.Add(int.Parse(match.Groups[1].Value));
            }
        }

        for (int i = 0; i < levelNumbers.Count; i++)
        {
            if (levelNumbers.Contains(i))
            {
                sumLevels++;
            }
            else
            {
                return sumLevels;
            }
        }

        return sumLevels;
    }

    void Update()
    {
        if (Input.GetButtonDown("Menu"))
        {
            Debug.Log(Time.frameCount);
            UIManager.instance.MenuButtonPressed();
        }

        if (Input.GetButtonDown("Screenshot"))
        {
            var antialiasing = QualitySettings.antiAliasing;
            QualitySettings.antiAliasing = 0;
            ScreenCapture.CaptureScreenshot(string.Format("screenshots/{0:dd-MM-yy_HH-mm-ss}.png", System.DateTime.Now), 2);
            QualitySettings.antiAliasing = antialiasing;
        }
    }

    private static IEnumerator StartCoro()
    {
        yield return new WaitForSecondsRealtime(2);
        running = true;
    }

    public static void RestartGame()
    {
        instance.StartCoroutine(RestartCoro());
    }

    private static IEnumerator RestartCoro()
    {
        while (Player.player1 == null)
            yield return null;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Player.OnSceneReload();
        yield return new WaitForSecondsRealtime(1);
        running = true;
    }

    public static void GameOver()
    {
        instance.StartCoroutine(GameOverCoro());
    }

    public static bool NextLevel()
    {
        if (nextLevel != maxLevel)
        {
            ChangeLevel(nextLevel + 1);
            return true;
        }
        return false;
    }

    public bool PreviousLevel()
    {
        if (nextLevel > 0)
        {
            ChangeLevel(nextLevel - 1);
            return true;
        }
        return false;
    }

    public static void ChangeLevel(int level)
    {
        running = false;
        nextLevel = level;
        RestartGame();
    }

    private static IEnumerator GameOverCoro()
    {
        running = false;
        yield return new WaitForSecondsRealtime(3);
        RestartGame();
    }

    public static bool IsRunning()
    {
        return running;
    }

    public static void OnPlayerReachedGoal(Player player)
    {
        instance.StartCoroutine(GoalCoro());
    }

    private static IEnumerator GoalCoro()
    {
        running = false;
        yield return new WaitForSecondsRealtime(3);
        if (!NextLevel())
        {
            UIManager.instance.OnLastLevelDone();
        }
    }


}

