using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerInputManager playerInputManager;

    private int numberOfPlayers;
    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += PlayerJoined;
        playerInputManager.onPlayerLeft += PlayerLeft;
        numberOfPlayers = 0;
    }

    private void PlayerLeft(PlayerInput input)
    {
        Debug.Log("PlayerLeft");
        // Remove player controller.
        if (numberOfPlayers == 0)
        {
            return;
        }
        numberOfPlayers--;
    }

    private void PlayerJoined(PlayerInput input)
    {
        Debug.Log("PlayerJoined");
        if(numberOfPlayers >= playerInputManager.maxPlayerCount)
        {
            return;
        }

        // Add player controller with the given input.
        numberOfPlayers++;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= PlayerJoined;
        playerInputManager.onPlayerLeft -= PlayerLeft;
    }

}
