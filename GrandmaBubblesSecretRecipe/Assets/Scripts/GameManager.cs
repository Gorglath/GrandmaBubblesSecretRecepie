using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerInputManager playerInputManager;

    [SerializeField]
    private PlayerInput playerPrefab;

    [SerializeField]
    private CameraManager cameraManager;


    public void StartGame((int, InputDevice)[] playerIndices)
    {
        foreach (var playerIndex in playerIndices)
        {
            var player = playerInputManager.JoinPlayer(playerIndex.Item1, pairWithDevice: playerIndex.Item2);
            PlayerJoined(player);
        }
    }

    private void PlayerJoined(PlayerInput input)
    {
        var controller = input.GetComponent<PlayerController>();
        controller.transform.position = cameraManager.GetSpawnLocation();
        cameraManager.RegisterPlayer(controller);
    }
}
