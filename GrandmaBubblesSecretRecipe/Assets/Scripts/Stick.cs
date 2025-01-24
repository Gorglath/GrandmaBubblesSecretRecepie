using System;
using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.UIElements;

public class Stick : MonoBehaviour, IPossesable
{
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private float maxMovementSpeed;

    [SerializeField]
    private float ariseSpeed;

    [SerializeField]
    private float ariseRetractSpeed;

    [SerializeField]
    private float maxAriseScale;

    [SerializeField]
    private GameObject stickViewPrefab;

    [SerializeField]
    private Rigidbody2D stickRigidbody;

    private GameObject view;
    private bool isActive;
    private float initialYScale;
    public void OnActionDown()
    {
        isActive = true;
    }

    public void OnActionUp()
    {
        isActive = false;
    }

    public void OnAction()
    {
    }

    public void OnMove(Vector2 moveDirection)
    {
        TorqueInDirection(moveDirection);
    }

    private void TorqueInDirection(Vector2 moveDirection)
    {
        var currentSpeed = stickRigidbody.linearVelocity.magnitude;
        var moveStep = moveDirection * movementSpeed * Time.deltaTime;
        moveStep.y = 0.0f;
        var actualForce = moveStep.x * (1 - currentSpeed / maxMovementSpeed);
        stickRigidbody.AddTorque(actualForce * -1);
    }

    public void OnPossessed(PlayerController playerController)
    {
        view = Instantiate(stickViewPrefab, transform);
        initialYScale = view.transform.localScale.y;
    }

    public void OnDeath()
    {
        Destroy(view);
        Destroy(gameObject);
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

    public void Update()
    {
        Arise();
    }

    public void Arise()
    {
        var scale = view.transform.localScale;
        if ((isActive && scale.y >= maxAriseScale) || (!isActive && scale.y <= initialYScale))
        {
            return;
        }

        var riseDirection = isActive ? ariseSpeed : ariseRetractSpeed;
        scale.y += riseDirection * Time.deltaTime;
        view.transform.localScale = scale;
    }
}
