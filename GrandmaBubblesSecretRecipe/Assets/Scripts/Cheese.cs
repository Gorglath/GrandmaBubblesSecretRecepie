using System.Linq;
using UnityEngine;

public class Cheese : MonoBehaviour, IPossesable
{
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private float maxMovementSpeed;

    [SerializeField]
    private float ariseForce;

    [SerializeField]
    private float activeAriseForce;

    [SerializeField]
    private GameObject cheeseView;

    [SerializeField]
    private Rigidbody2D CheeseRigidbody;

    private GameObject mainView;
    private bool isFacingRight;
    private bool isActive;

    public IngredientType IngerdientType => IngredientType.Cheese;
    public void OnAction()
    {

    }

    public void OnMove(Vector2 moveDirection)
    {
        if(moveDirection != Vector2.zero)
        {
            isFacingRight = moveDirection.x > 0;
        }
        var currentSpeed = CheeseRigidbody.linearVelocity.magnitude;
        var moveStep = moveDirection * movementSpeed * Time.deltaTime;
        moveStep.y = 0.0f;
        var actualForce = moveStep * (1 - currentSpeed / maxMovementSpeed);
        CheeseRigidbody.AddForce(actualForce);
    }

    public void OnPossessed(PlayerController playerController)
    {
        mainView = Instantiate(cheeseView, transform);
    }

    public void OnDeath()
    {
        Destroy(mainView);
        Destroy(gameObject);
    }

    public Vector3 GetCenterPosition()
    {
        return mainView.transform.position;
    }

    public Vector3 GetPredictedPosition(Vector2 moveDirection)
    {
        var velocity = moveDirection * movementSpeed * Time.deltaTime;
        return transform.position + new Vector3(velocity.x, velocity.y, 0) * Time.deltaTime * 5;
    }


    private void FixedUpdate()
    {
        Arise();
    }

    public void Arise()
    {
        var maxAngle = 1f;
        var lookDirection = CheeseRigidbody.transform.up;
        var targetDirection = isActive ? isFacingRight ? Vector2.right : Vector2.left : Vector2.up;
        var cross = Vector3.Cross(targetDirection, lookDirection);
        var sign = Mathf.Sign(cross.z);

        var angle = Vector2.Angle(targetDirection, lookDirection);

        angle *= sign;

        var absAngle = Mathf.Abs(angle);

        if (absAngle > maxAngle)
        {
            var targetForce = isActive ? activeAriseForce : ariseForce;
            CheeseRigidbody.AddTorque(-sign * Time.deltaTime * targetForce);
        }
    }
    public void OnActionDown()
    {
        isActive = true;
    }

    public void OnActionUp()
    {
        isActive = false;
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
