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
    private float jumpForce;

    [SerializeField]
    private GameObject eggViewPrefab;

    [SerializeField]
    private Rigidbody2D eggRigidbody;

    private GameObject view;
    private PlayerController playerController;
    private bool grounded;

    public IngredientType IngerdientType => IngredientType.Egg;
    public void OnActionDown()
    {
    }

    public void OnActionUp()
    {
    }

    public void OnAction()
    {
        if (!grounded)
        {
            return;
        }

        grounded = false;
        eggRigidbody.AddForceY(jumpForce, ForceMode2D.Impulse);
    }

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
        if (collision.CompareTag("Platform"))
        {
            grounded = true;
        }

        var possessable = collision.GetComponentInParent<IPossesable>();
        if (possessable != null && possessable is Jelly jelly && jelly.Active)
        {
            eggRigidbody.linearVelocity = Vector2.zero;
            eggRigidbody.angularVelocity = 0.0f;
            return;
        }

        if(collision.TryGetComponent<PossessableGenerator>(out _))
        {
            return;
        }

        if(eggRigidbody.linearVelocity.magnitude > breakMovementSpeed)
        {
            playerController.Die();
        }
    }
}
