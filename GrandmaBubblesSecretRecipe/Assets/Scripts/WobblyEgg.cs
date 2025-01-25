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

    [SerializeField]
    private GameObject eggBreakVFX;

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
        if (collision.CompareTag("Kitchen"))
        {
            return;
        }

        if (collision.CompareTag("Platform"))
        {
            grounded = true;
        }

        var possessable = collision.GetComponentInParent<IPossesable>();
        if (possessable != null)
        {
            if (possessable is Jelly jelly && jelly.Active) {
                eggRigidbody.linearVelocity = Vector2.zero;
                eggRigidbody.angularVelocity = 0.0f;
                return;
            } else if(possessable is LeafyBall leafyBall && leafyBall.Active)
            {
                return;
            }
        }

        if(collision.TryGetComponent<PossessableGenerator>(out _))
        {
            return;
        }

        if(eggRigidbody.linearVelocity.magnitude > breakMovementSpeed)
        {
            var eggVFX = Instantiate(eggBreakVFX, eggRigidbody.position, eggRigidbody.transform.rotation);
            Destroy(eggVFX, 5.0f);
            playerController.Die();
        }
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
