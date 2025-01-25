using System.Linq;
using UnityEngine;

public class Sludge : MonoBehaviour, IPossesable
{
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private float maxMovementSpeed;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private GameObject sludgeMainViewPrefab;

    [SerializeField]
    private Rigidbody2D sludgeRigidbody;

    private AudioSource moveSource;
    private GameObject mainView;
    private bool attachedToWall;
    private bool grounded;

    private Animator viewAnimator;
    public IngredientType IngerdientType => IngredientType.Sludge;
    public void OnAction()
    {
        if (!grounded)
        {
            return;
        }

        grounded = false;
        // find closest location.
        var sludgePosition = sludgeRigidbody.position;
        var leftRay = Physics2D.RaycastAll(sludgePosition, sludgePosition + Vector2.left, 1f);
        var rightRay = Physics2D.RaycastAll(sludgePosition, sludgePosition + Vector2.right, 1f);
        var jumpDirection = Vector2.up;
        if (leftRay.Any(r => r.transform.CompareTag("ClimbableWall")))
        {
            jumpDirection += Vector2.left;
        } else if (rightRay.Any(r => r.transform.CompareTag("ClimbableWall")))
        {
            jumpDirection += Vector2.right;
        }

        sludgeRigidbody.AddForce(jumpDirection.normalized * jumpForce, ForceMode2D.Impulse);
        viewAnimator.SetTrigger("Mouse down");
    }

    public void OnMove(Vector2 moveDirection)
    {
        var gravity = -Physics.gravity.y * 0.5f;
        if (moveDirection == Vector2.zero)
        {
            if (moveSource.isPlaying)
            {
                moveSource.Stop();
            }

            if (attachedToWall)
            {
                sludgeRigidbody.AddForce(new Vector2(0.0f, gravity));
            }
            return;
        } else
        {
            if (!moveSource.isPlaying)
            {
                moveSource.Play();
            }
        }

        var currentSpeed = sludgeRigidbody.linearVelocity.magnitude;
        var moveStep = moveDirection * movementSpeed * Time.deltaTime;
        moveStep.y = attachedToWall? moveStep.y + gravity : 0.0f;
        var actualForce = moveStep * (1 - currentSpeed / maxMovementSpeed);
        sludgeRigidbody.AddForce(actualForce);
    }

    public void OnPossessed(PlayerController playerController)
    {
        mainView = Instantiate(sludgeMainViewPrefab, transform);
        viewAnimator = mainView.GetComponent<Animator>();
        var sfx = SfxService.Instance.SfxData.Ingredients.Sludge.Move;
        moveSource = SfxService.Instance.PrepareSound(sfx);
        moveSource.loop = true; 
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ClimbableWall"))
        {
            attachedToWall = true;
            grounded = true;
        } else if (collision.CompareTag("Platform"))
        {
            grounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ClimbableWall"))
        {
            attachedToWall = false;
        }
    }

    public void OnActionDown()
    {
    }

    public void OnActionUp()
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(sludgeRigidbody.position, sludgeRigidbody.position + Vector2.right * 0.8f);
        Gizmos.DrawLine(sludgeRigidbody.position, sludgeRigidbody.position + Vector2.left * 0.8f);
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
