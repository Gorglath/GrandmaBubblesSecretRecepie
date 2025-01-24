using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameLoader
{
    public void LoadGame((int, InputDevice)[] players)
    {
        SceneManager.LoadScene(1);
        SceneManager.sceneLoaded += (_, _) => OnSceneLoaded(players);
    }

    private void OnSceneLoaded((int, InputDevice)[] players)
    {
        Debug.Log("Why?");
        var gameManager = Object.FindAnyObjectByType<GameManager>();
        gameManager.StartGame(players);
    }
}
