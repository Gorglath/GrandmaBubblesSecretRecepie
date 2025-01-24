using UnityEngine;

public class WobblyEgg : MonoBehaviour, IPossesable
{
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private float maxMovementSpeed;

    [SerializeField]
    private float breakMovementSpeed;

    [SerializeField]
    private GameObject eggViewPrefab;

    [SerializeField]
    private Rigidbody2D eggRigidbody;

    private GameObject view;
    private PlayerController playerController;

    public void OnAction()
    {}

    public void OnMove(Vector2 moveDirection)
    {
        var currentSpeed = eggRigidbody.linearVelocity.magnitude;
        var moveStep = moveDirection * movementSpeed * Time.deltaTime;
        moveStep.y = 0.0f;
        var actualForce = moveStep.x * (1 - currentSpeed / maxMovementSpeed);
        eggRigidbody.AddTorque(actualForce *-1);
    }

    public void OnPossessed(PlayerController playerController)
    {
        view = Instantiate(eggViewPrefab, transform);
        this.playerController = playerController;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(eggRigidbody.linearVelocity.magnitude > breakMovementSpeed)
        {
            playerController.Die();
        }
    }
}
