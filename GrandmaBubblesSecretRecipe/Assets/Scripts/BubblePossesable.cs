using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BubblePossesable : MonoBehaviour, IPossesable
{
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private GameObject bubbleViewPrefab;

    [SerializeField]
    private Rigidbody2D bubbleRigidbody;

    private GameObject view;
    private PossessableGenerator availableGenerator;
    private PlayerController playerController;
    public void OnActionDown()
    {
    }

    public void OnActionUp()
    {
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void OnAction()
    {
        if(availableGenerator != null)
        {
            var possessable = availableGenerator.GetPossesableInstance(playerController.transform);
            playerController.Possess(possessable);
        }
    }

    public void OnMove(Vector2 moveDirection)
    {
        var moveStep = moveDirection * movementSpeed * Time.deltaTime;
        bubbleRigidbody.linearVelocity = new Vector3(moveStep.x, moveStep.y, 0);
    }

    public void OnPossessed(PlayerController playerController)
    {
        view = Instantiate(bubbleViewPrefab, transform);
    }

    public void OnDeath()
    {
        Destroy(view);
    }

    public void RegisterPossesable(PossessableGenerator generator)
    {
        availableGenerator = generator;
    }

    public void DeregisterPossesable(PossessableGenerator generator)
    {
        if (availableGenerator != generator)
        {
            return;
        }

        availableGenerator = null;
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
        var velocity = moveDirection * movementSpeed * Time.deltaTime;
        return transform.position + new Vector3(velocity.x, velocity.y, 0) * Time.deltaTime * 5;
    }
}
