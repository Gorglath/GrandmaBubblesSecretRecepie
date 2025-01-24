using System;
using UnityEngine;

public interface IPossesable
{
    public IngredientType IngerdientType { get; }
    public void OnPossessed(PlayerController playerController);
    public void OnMove(Vector2 moveDirection);
    public void OnAction();
    public void OnActionDown();
    public void OnActionUp();
    public void OnDeath();
    public Vector3 GetCenterPosition();
    public Vector3 GetPredictedPosition(Vector2 movement);
}
