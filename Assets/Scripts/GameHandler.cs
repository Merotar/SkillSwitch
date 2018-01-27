using System;
using UnityEngine;
using System.Collections;

public class GameHandler: MonoBehaviour
{
    private static GameHandler instance;

    private static bool running = false;

    void Awake()
    {
        Debug.Assert(instance == null);
        instance = this;
        RestartGame();
    }

    public static void RestartGame()
    {
        instance.StartCoroutine(RestartCoro());
    }

    private static IEnumerator RestartCoro()
    {
        while (Player.player1 == null)
            yield return null;
        Player.player1.ResetPosition();
        Player.player2.ResetPosition();
        yield return new WaitForSeconds(1);
        running = true;
    }

    public static void GameOver()
    {
        instance.StartCoroutine(GameOverCoro());
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
}

