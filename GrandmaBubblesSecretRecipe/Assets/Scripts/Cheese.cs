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
    private float yeetForce;

    [SerializeField]
    private GameObject cheeseView;

    [SerializeField]
    private Rigidbody2D cheeseRigidbody;

    private AudioSource moveSource;
    private GameObject mainView;
    private bool isFacingRight;
    private bool isActive;

    public IngredientType IngerdientType => IngredientType.Cheese;
    public void OnAction()
    {

    }

    public void OnMove(Vector2 moveDirection)
    {

        if (moveDirection == Vector2.zero)
        {
            if (moveSource.isPlaying)
            {
                moveSource.Stop();
            }
        }
        else
        {
            isFacingRight = moveDirection.x > 0;
            if (!moveSource.isPlaying)
            {
                moveSource.Play();
            }
        }

        var currentSpeed = cheeseRigidbody.linearVelocity.magnitude;
        var moveStep = moveDirection * movementSpeed * Time.deltaTime;
        moveStep.y = 0.0f;
        var actualForce = moveStep * (1 - currentSpeed / maxMovementSpeed);
        cheeseRigidbody.AddForce(actualForce);
    }

    public void OnPossessed(PlayerController playerController)
    {
        mainView = Instantiate(cheeseView, transform);
        var sfx = SfxService.Instance.SfxData.Ingredients.Jelly.Move;
        moveSource = SfxService.Instance.PrepareSound(sfx);
        moveSource.loop = true;

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
        Destroy(moveSource.gameObject);
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
        var lookDirection = cheeseRigidbody.transform.up;
        var targetDirection = isActive ? isFacingRight ? Vector2.right : Vector2.left : Vector2.up;
        var cross = Vector3.Cross(targetDirection, lookDirection);
        var sign = Mathf.Sign(cross.z);

        var angle = Vector2.Angle(targetDirection, lookDirection);

        angle *= sign;

        var absAngle = Mathf.Abs(angle);

        if (absAngle > maxAngle)
        {
            var targetForce = isActive ? activeAriseForce : ariseForce;
            cheeseRigidbody.AddTorque(-sign * Time.deltaTime * targetForce);
        }
    }
    public void OnActionDown()
    {
        isActive = true;
    }

    public void OnActionUp()
    {
        isActive = false;
        var forceDirection = isFacingRight ? Vector2.left : Vector2.right;
        var finalForce = forceDirection / 2.0f + Vector2.up;
        cheeseRigidbody.AddForce(finalForce * yeetForce, ForceMode2D.Impulse);
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
