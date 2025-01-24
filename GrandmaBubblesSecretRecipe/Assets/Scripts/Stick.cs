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
    private GameObject stickViewPrefab;

    [SerializeField]
    private Rigidbody2D stickRigidbody;

    private GameObject view;
    private bool isActive;

    public void OnAction()
    {
        if (stickRigidbody.linearVelocityY > 0.05f || stickRigidbody.linearVelocityY < -0.05f)
        {
            return;
        }

        isActive = !isActive;
    }

    public void OnMove(Vector2 moveDirection)
    {
        if (moveDirection != Vector2.zero)
        {
            isActive = false;
        }
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

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        Arise();
    }

    public void Arise()
    {
        var maxAngle = 1f;
        var lookDirection = stickRigidbody.transform.up;
        var cross = Vector3.Cross(Vector2.up, lookDirection);
        var sign = Mathf.Sign(cross.z);

        var angle = Vector2.Angle(Vector2.up, lookDirection);

        angle *= sign;

        var absAngle = Mathf.Abs(angle);

        if (absAngle > maxAngle) 
        {
            stickRigidbody.AddTorque(-sign * Time.deltaTime * ariseSpeed);
        }
    }
}
