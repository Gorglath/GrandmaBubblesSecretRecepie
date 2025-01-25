using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameLoader
{
    public void LoadGame((int, InputDevice)[] players)
    {
        SceneManager.LoadScene(1);
        SceneManager.sceneLoaded += (_, _) => OnSceneLoaded(players);
    }

    private void OnSceneLoaded((int, InputDevice)[] players)
    {
        var gameManager = Object.FindAnyObjectByType<GameManager>();
        gameManager.StartGame(players);
    }
}
