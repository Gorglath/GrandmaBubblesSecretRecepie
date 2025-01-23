using UnityEngine;

public interface IPossesable
{
    public void OnPossessed();
    public void OnMove(Vector2 moveDirection);
    public void OnAction();
    public void OnDeath();
}
