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
    private float maxJumpDuration;

    [SerializeField]
    private float maxJumpScale;

    [SerializeField]
    private GameObject stickViewPrefab;

    [SerializeField]
    private Rigidbody2D stickRigidbody;

    [SerializeField]
    private Vector2 jumpForceMinMax;

    private Animator viewAnimator;
    private GameObject view;
    private bool isActive;
    private float holdDuration;

    public IngredientType IngerdientType => IngredientType.Tentacle;
    public void OnActionDown()
    {
        isActive = true;
        holdDuration = 0;
        viewAnimator.ResetTrigger("Mouse up");
        viewAnimator.SetTrigger("Mouse down");
    }

    public void OnActionUp()
    {
        isActive = false;

        var forceDirection = stickRigidbody.transform.up;
        var forceAmount = Mathf.Lerp(jumpForceMinMax.x, jumpForceMinMax.y, holdDuration / maxJumpDuration);
        stickRigidbody.AddForce(forceDirection * forceAmount, ForceMode2D.Impulse);
        viewAnimator.SetTrigger("Mouse up");
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
        viewAnimator = view.GetComponent<Animator>();
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
        if (!isActive)
        {
            return;
        }
        var scale = view.transform.localScale;
        if (holdDuration/maxJumpDuration >= 1.0f)
        {
            return;
        }
        holdDuration += Time.deltaTime;
    }

    public bool isCooked()
    {
        return gameObject.TryGetComponent<Cooked>(out var cooked) && cooked.cookValue > 1.0f;
    }

    public bool isSauced()
    {
        return gameObject.TryGetComponent<Sauce>(out _);
    }

    public bool isGrated()
    {
        return gameObject.TryGetComponent<GrateState>(out var state) && state.grateValue >= 1.0f;
    }

    public bool isPowdered()
    {
        return gameObject.TryGetComponent<FlourPowder>(out _);
    }

    public bool isSliced()
    {
        return gameObject.TryGetComponent<Sliced>(out _);
    }
}
