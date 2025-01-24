using UnityEngine;

public class LeafyBall : MonoBehaviour, IPossesable
{
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private float maxMovementSpeed;

    [SerializeField]
    private float ariseSpeed;

    [SerializeField]
    private GameObject leafViewPrefab;

    [SerializeField]
    private Rigidbody2D leafRigidbody;

    [SerializeField]
    private GameObject bounceObject;

    private GameObject view;
    private bool isActive;
    private bool tryingToActivate;

    public void OnActionDown()
    {
    }

    public void OnActionUp()
    {
    }

    public void OnAction()
    {
        if(leafRigidbody.linearVelocityY > 0.05f || leafRigidbody.linearVelocityY < -0.05f)
        {
            return;
        }

        if (isActive)
        {
            tryingToActivate = false;
            isActive = false;
            leafRigidbody.constraints = RigidbodyConstraints2D.None;
            bounceObject.SetActive(isActive);
            view.SetActive(!isActive);
            return;
        }

        tryingToActivate = true;
    }

    public void OnMove(Vector2 moveDirection)
    {
        if (isActive)
        {
            return;
        }

        var currentSpeed = leafRigidbody.linearVelocity.magnitude;
        var moveStep = moveDirection * movementSpeed * Time.deltaTime;
        moveStep.y = 0.0f;
        var actualForce = moveStep * (1 - currentSpeed / maxMovementSpeed);
        leafRigidbody.AddForce(actualForce);
    }
    public void Update()
    {
        if (!tryingToActivate)
        {
            return;
        }

        Arise();
    }

    public void OnPossessed(PlayerController playerController)
    {
        view = Instantiate(leafViewPrefab, transform);
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

    public void Arise()
    {
        var maxAngle = 1f;
        var lookDirection = leafRigidbody.transform.up;
        var cross = Vector3.Cross(Vector2.up, lookDirection);
        var sign = Mathf.Sign(cross.z);

        var angle = Vector2.Angle(Vector2.up, lookDirection);

        angle *= sign;

        var absAngle = Mathf.Abs(angle);

        if (absAngle > maxAngle)
        {
            leafRigidbody.AddTorque(-sign * Time.deltaTime * ariseSpeed);
        } else
        {
            isActive = true;
            tryingToActivate = false;
            leafRigidbody.linearVelocity = Vector2.zero;
            leafRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            bounceObject.SetActive(isActive);
            view.SetActive(!isActive);
        }
    }
}
