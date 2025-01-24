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

    private GameObject view;

    public void OnAction()
    {
        //TODO : implement.
    }

    public void OnMove(Vector2 moveDirection)
    {
        var currentSpeed = leafRigidbody.linearVelocity.magnitude;
        var moveStep = moveDirection * movementSpeed * Time.deltaTime;
        moveStep.y = 0.0f;
        var actualForce = moveStep * (1 - currentSpeed / maxMovementSpeed);
        leafRigidbody.AddForce(actualForce);
        Debug.Log(actualForce);
    }

    public void OnPossessed(PlayerController playerController)
    {
        view = Instantiate(leafViewPrefab, transform);
    }

    public void OnDeath()
    {
        Destroy(view);
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
