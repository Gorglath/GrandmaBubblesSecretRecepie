using UnityEngine;

public class LeafyBall : MonoBehaviour, IPossesable
{
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private float maxMovementSpeed;

    [SerializeField]
    private GameObject leafViewPrefab;

    [SerializeField]
    private Rigidbody2D leafRigidbody;

    [SerializeField]
    private GameObject bounceObject;

    private GameObject view;
    private bool isActive;
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

        isActive = !isActive;
        leafRigidbody.linearVelocity = Vector2.zero;
        leafRigidbody.constraints = isActive ? RigidbodyConstraints2D.FreezeAll : RigidbodyConstraints2D.None;
        bounceObject.SetActive(isActive);
        view.SetActive(!isActive);
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
}
