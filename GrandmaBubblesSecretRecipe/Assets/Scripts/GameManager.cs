using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerInputManager playerInputManager;

    [SerializeField]
    private CameraManager cameraManager;

    private int numberOfPlayers;
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
        var controller = input.GetComponent<PlayerController>();
        cameraManager.DeregisterPlayer(controller);
    }

    private void PlayerJoined(PlayerInput input)
    {
        if(numberOfPlayers >= PlayerInputManager.instance.maxPlayerCount)
        {
            return;
        }

        // Add player controller with the given input.
        numberOfPlayers++;
        var controller = input.GetComponent<PlayerController>();
        controller.transform.position = cameraManager.GetSpawnLocation();
        cameraManager.RegisterPlayer(controller);
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= PlayerJoined;
        playerInputManager.onPlayerLeft -= PlayerLeft;
    }
}
