using System;
using UnityEngine;

public interface IPossesable
{
    public void OnPossessed(PlayerController playerController);
    public void OnMove(Vector2 moveDirection);
    public void OnAction();
    public void OnDeath();
    public Vector3 GetCenterPosition();
    public Vector3 GetPredictedPosition(Vector2 movement);
}
