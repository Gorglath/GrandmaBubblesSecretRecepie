using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputSystem_Actions;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private BubblePossesable defaultPossesable;

    IPossesable activePossesable;
    private void OnEnable()
    {
        defaultPossesable.RegisterController(this);
        Possess(defaultPossesable);
    }

    public void OnMove(InputValue inputValue)
    {
        var moveVector = inputValue.Get<Vector2>();
        activePossesable.OnMove(moveVector);
    }

    public void OnInputAction(InputValue inputValue)
    {
        activePossesable.OnAction();
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
}
