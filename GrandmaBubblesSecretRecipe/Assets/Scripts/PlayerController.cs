using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputSystem_Actions;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private BubblePossesable defaultPossesable;

    [SerializeField]
    private float killSelfHoldTime;

    IPossesable activePossesable;
    private Vector2 moveVector;
    private CameraManager cameraManager;
    private DateTime startedKillingTime;

    private void OnEnable()
    {
        defaultPossesable.RegisterController(this);
        Possess(defaultPossesable);
        var playerInput = GetComponent<PlayerInput>();
        var killSelfAction = playerInput.actions["KillSelf"];

        killSelfAction.started += OnKillSelfStart;
        killSelfAction.canceled += OnKillSelfEnd;
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

    public void Die()
    {
        defaultPossesable.SetPosition(activePossesable.GetCenterPosition());
        activePossesable.OnDeath();
        activePossesable = defaultPossesable;
        defaultPossesable.OnPossessed(this);
    }

    public Vector3 GetPossessedPosition()
    {
        return activePossesable.GetCenterPosition();
    }
}
