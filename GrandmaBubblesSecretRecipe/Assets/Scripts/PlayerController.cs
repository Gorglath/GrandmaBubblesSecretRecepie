using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputSystem_Actions;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private BubblePossesable defaultPossesable;

    IPossesable activePossesable;
    private Vector2 moveVector;
    private CameraManager cameraManager;

    private void OnEnable()
    {
        defaultPossesable.RegisterController(this);
        Possess(defaultPossesable);
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

    private void Update()
    {
        if (activePossesable != null && !IsLeavingCameraRect()) {
            activePossesable.OnMove(moveVector);
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
        activePossesable.OnPossessed();
    }
    public void Die()
    {
        activePossesable.OnDeath();
        activePossesable = defaultPossesable;
        defaultPossesable.OnPossessed();
    }

    public Vector3 GetPossessedPosition()
    {
        return activePossesable.GetCenterPosition();
    }
}
