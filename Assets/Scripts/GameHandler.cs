using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameHandler: MonoBehaviour
{
    private static GameHandler instance;

    private static bool running = false;

    private static int nextLevel = 0;

    public static int maxLevel = 2;

    void Awake()
    {
        Debug.Assert(instance == null);
        instance = this;
    }

    void Start()
    {
        TiledJsonImporter.LoadLevel(nextLevel);
        instance.StartCoroutine(StartCoro());
    }

    void Update()
    {
        if (Input.GetButtonDown("Menu"))
        {
            Debug.Log(Time.frameCount);
            UIManager.instance.MenuButtonPressed();
        }
    }

    private static IEnumerator StartCoro()
    {
        yield return new WaitForSeconds(3);
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
        yield return new WaitForSeconds(1);
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
        yield return new WaitForSeconds(1);
        RestartGame();
    }

    public static bool IsRunning()
    {
        return running;
    }

    public static void OnPlayerReachedGoal(Player player)
    {
        if (!NextLevel())
        {
            UIManager.instance.OnLastLevelDone();
        }
    }
}

