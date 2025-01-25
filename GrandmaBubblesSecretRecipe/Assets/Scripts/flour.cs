using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class flour : MonoBehaviour, IPossesable
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
    private GameObject flourView;

    [SerializeField]
    private Rigidbody2D flourRigidbody;

    [SerializeField]
    private float flourSpreadDistance;

    private GameObject mainView;
    private bool grounded;

    public IngredientType IngerdientType => IngredientType.Flour;
    public void OnAction()
    {
        if (!grounded)
        {
            return;
        }

        grounded = false;
        flourRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void OnMove(Vector2 moveDirection)
    {
        var currentSpeed = flourRigidbody.linearVelocity.magnitude;
        var moveStep = moveDirection * movementSpeed * Time.deltaTime;
        moveStep.y = 0.0f;
        var actualForce = moveStep * (1 - currentSpeed / maxMovementSpeed);
        flourRigidbody.AddForce(actualForce);
    }

    public void OnPossessed(PlayerController playerController)
    {
        mainView = Instantiate(flourView, transform);

        foreach (Transform child in mainView.transform)
        {
            if (child.CompareTag("Indicator"))
            {
                child.GetComponent<SpriteRenderer>().color = playerController.GetColorByPlayerIndex();
            }
        }
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

    private void Update()
    {
        if (grounded)
        {
            return;
        }

        var hits = Physics2D.RaycastAll(flourRigidbody.position, Vector2.down, flourSpreadDistance);
        if(hits.Any(h => h.transform.CompareTag("Ingredient")))
        {
            var ingredients = hits.Where(h => h.transform.CompareTag("Ingredient"));
            foreach (var ingredient in ingredients)
            {
                if(!ingredient.rigidbody.TryGetComponent<FlourPowder>(out _)){
                    ingredient.rigidbody.AddComponent<FlourPowder>();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        Arise();
    }

    public void Arise()
    {
        var maxAngle = 1f;
        var lookDirection = flourRigidbody.transform.up;
        var cross = Vector3.Cross(Vector2.up, lookDirection);
        var sign = Mathf.Sign(cross.z);

        var angle = Vector2.Angle(Vector2.up, lookDirection);

        angle *= sign;

        var absAngle = Mathf.Abs(angle);

        if (absAngle > maxAngle)
        {
            flourRigidbody.AddTorque(-sign * Time.deltaTime * ariseForce);
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
