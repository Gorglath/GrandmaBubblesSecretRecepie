using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputSystem_Actions;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private BubblePossesable defaultPossesable;

    [SerializeField]
    private PlayerInput playerInput;

    [SerializeField]
    private float killSelfHoldTime;

    IPossesable activePossesable;
    private Vector2 moveVector;
    private CameraManager cameraManager;
    private DateTime startedKillingTime;
    private GameManager gameManager;

    public int GetPlayerIndex()
    {
        return playerInput.playerIndex;
    }

    private void OnEnable()
    {
        defaultPossesable.RegisterController(this);
        Possess(defaultPossesable);
        var playerInput = GetComponent<PlayerInput>();
        var killSelfAction = playerInput.actions["KillSelf"];

        killSelfAction.started += OnKillSelfStart;
        killSelfAction.canceled += OnKillSelfEnd;

        var inputAction = playerInput.actions["InputAction"];

        inputAction.started += OnInputActionDown;
        inputAction.canceled += OnInputActionUp;
    }

    private void OnInputActionDown(InputAction.CallbackContext context)
    {
        activePossesable.OnActionDown();
    }

    private void OnInputActionUp(InputAction.CallbackContext context)
    {
        activePossesable.OnActionUp();
    }

    private void OnKillSelfEnd(InputAction.CallbackContext context)
    {
        var holdDuration = DateTime.Now - startedKillingTime;
        if(killSelfHoldTime > holdDuration.TotalSeconds)
        {
            return;
        }

        Die();
    }

    public void RegisterGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    private void OnKillSelfStart(InputAction.CallbackContext context)
    {
        this.startedKillingTime = DateTime.Now;
    }

    public void RegisterCameraManager(CameraManager cameraManager)
    {
        this.cameraManager = cameraManager;
    }

    public void OnMove(InputValue inputValue)
    {
        moveVector = inputValue.Get<Vector2>();
    }

    public void OnInputAction(InputValue inputValue)
    {
        activePossesable.OnAction();
    }

    private void FixedUpdate()
    {
        if (activePossesable != null) {
            var newMoveVector = !IsLeavingCameraRect() ? moveVector : Vector2.zero;
            activePossesable.OnMove(newMoveVector);
        }
    }

    private bool IsLeavingCameraRect()
    {
        var predictedLocation = activePossesable.GetPredictedPosition(moveVector);
        return !cameraManager.IsWithinBounds(predictedLocation);
    }

    public void Possess(IPossesable possesable)
    {
        activePossesable?.OnDeath();
        activePossesable = possesable;
        activePossesable.OnPossessed(this);
    }

    public void Die(bool didAddToRecipe = false)
    {
        if (didAddToRecipe)
        {
            gameManager.DeliverIngredient(activePossesable);
        }
        defaultPossesable.SetPosition(activePossesable.GetCenterPosition());
        activePossesable.OnDeath();
        activePossesable = defaultPossesable;
        defaultPossesable.OnPossessed(this);
    }

    public Vector3 GetPossessedPosition()
    {
        return activePossesable.GetCenterPosition();
    }

    public Color GetColorByPlayerIndex()
    {
        switch (playerInput.playerIndex)
        {
            case 0:
                ColorUtility.TryParseHtmlString("#c23551", out var redColor);
                return redColor;
            case 1:
                ColorUtility.TryParseHtmlString("#c2a537", out var yellowColor);
                return yellowColor;
            case 2:
                ColorUtility.TryParseHtmlString("#3590c4 ", out var blueColor);
                return blueColor;
            case 3:
            default:
                ColorUtility.TryParseHtmlString("#c23551", out var greenColor);
                return greenColor;
        }
    }
}
