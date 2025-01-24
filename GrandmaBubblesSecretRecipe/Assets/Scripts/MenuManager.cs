using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private PlayerInputManager playerInputManager;

    [SerializeField]
    private Transform[] playerContainers;

    private List<(int, InputDevice)> players = new List<(int, InputDevice)>();
    private int numberOfPlayers;
    private bool startingGame;
    private void OnEnable()
    {
        numberOfPlayers = 0;
        playerInputManager.onPlayerJoined += PlayerJoined;
        playerInputManager.onPlayerLeft += PlayerLeft;
    }

    private void PlayerLeft(PlayerInput input)
    {
        // Remove player controller.
        if (numberOfPlayers == 0)
        {
            return;
        }
        numberOfPlayers--;
        players.Remove((input.playerIndex, input.devices[0]));
    }

    private void PlayerJoined(PlayerInput input)
    {
        if (numberOfPlayers >= PlayerInputManager.instance.maxPlayerCount)
        {
            return;
        }

        if (numberOfPlayers == 0)
        {
            var startGameAction = input.actions["StartGame"];
            startGameAction.started += StartGame;
        }

        // Add player controller with the given input.
        input.transform.position = playerContainers[numberOfPlayers].position;
        numberOfPlayers++;
        players.Add((input.playerIndex, input.devices[0]));
    }

    private void StartGame(InputAction.CallbackContext context)
    {
        if (startingGame || numberOfPlayers < 2)
        {
            return;
        }

        startingGame = true;
        var loader = new GameLoader();
        loader.LoadGame(players.ToArray());
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= PlayerJoined;
        playerInputManager.onPlayerLeft -= PlayerLeft;
    }
}
