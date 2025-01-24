using System.Linq;
using UnityEngine;

public class Chicken : MonoBehaviour, IPossesable
{
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private float maxMovementSpeed;

    [SerializeField]
    private float ariseForce;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private GameObject chickenView;

    [SerializeField]
    private Rigidbody2D chickenRigidbody;

    [SerializeField]
    private float descendGravity;

    private GameObject mainView;
    private bool waitingToDecsend;
    private bool grounded;

    public IngredientType IngerdientType => IngredientType.Chicken;
    public void OnAction()
    {
        if (!grounded)
        {
            return;
        }

        grounded = false;
        waitingToDecsend = true;
        chickenRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void OnMove(Vector2 moveDirection)
    {
        var currentSpeed = chickenRigidbody.linearVelocity.magnitude;
        var moveStep = moveDirection * movementSpeed * Time.deltaTime;
        moveStep.y = 0.0f;
        var actualForce = moveStep * (1 - currentSpeed / maxMovementSpeed);
        chickenRigidbody.AddForce(actualForce);
    }

    public void OnPossessed(PlayerController playerController)
    {
        mainView = Instantiate(chickenView, transform);
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
        if (waitingToDecsend)
        {
            if(chickenRigidbody.linearVelocityY < 0.0f)
            {
                waitingToDecsend = false;
                chickenRigidbody.gravityScale = descendGravity;
            }
        }
    }

    public void Arise()
    {
        var maxAngle = 1f;
        var lookDirection = chickenRigidbody.transform.up;
        var cross = Vector3.Cross(Vector2.up, lookDirection);
        var sign = Mathf.Sign(cross.z);

        var angle = Vector2.Angle(Vector2.up, lookDirection);

        angle *= sign;

        var absAngle = Mathf.Abs(angle);

        if (absAngle > maxAngle)
        {
            chickenRigidbody.AddTorque(-sign * Time.deltaTime * ariseForce);
        }
    }
    public void OnActionDown()
    {

    }

    public void OnActionUp()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Platform"))
        {
            grounded = true;
            chickenRigidbody.gravityScale = 1.0f;
        }
    }
}
