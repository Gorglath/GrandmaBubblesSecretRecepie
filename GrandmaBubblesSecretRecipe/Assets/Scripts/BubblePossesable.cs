using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BubblePossesable : MonoBehaviour, IPossesable
{
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private GameObject bubbleViewPrefab;

    private GameObject view;
    IPossesable availablePossesable;
    private PlayerController playerController;

    public void OnAction()
    {
        if(availablePossesable != null)
        {
            playerController.Possess(availablePossesable);
        }
    }

    public void OnMove(Vector2 moveDirection)
    {
        var moveStep = moveDirection * movementSpeed * Time.deltaTime;
        transform.position += new Vector3(moveStep.x, moveStep.y, 0);
    }

    public void OnPossessed()
    {
        view = Instantiate(bubbleViewPrefab, transform);
    }

    public void OnDeath()
    {
        Destroy(view);
    }

    public void RegisterPossesable(IPossesable possesable)
    {
        availablePossesable = possesable;
    }

    public void DeregisterPossesable(IPossesable possesable)
    {
        if (availablePossesable != possesable)
        {
            return;
        }

        availablePossesable = null;
    }

    internal void RegisterController(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public Vector3 GetCenterPosition()
    {
        return view.transform.position;
    }

    public Vector3 GetPredictedPosition(Vector2 moveDirection)
    {
        var moveStep = moveDirection * movementSpeed * Time.deltaTime;
        return transform.position + new Vector3(moveStep.x, moveStep.y, 0);
    }
}
