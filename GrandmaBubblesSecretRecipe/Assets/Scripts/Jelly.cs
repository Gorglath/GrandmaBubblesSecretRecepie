using System.Linq;
using UnityEngine;

public class Jelly : MonoBehaviour, IPossesable
{
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private float maxMovementSpeed;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private GameObject activeColliders;

    [SerializeField]
    private GameObject inActiveColliders;

    [SerializeField]
    private GameObject jellyViewPrefab;

    [SerializeField]
    private Rigidbody2D jellyRigidbody;

    private AudioSource moveSource;
    private Animator viewAnimator;
    private GameObject view;
    private bool isActive;

    public bool Active => isActive;

    public IngredientType IngerdientType => IngredientType.Jelly;

    public void OnActionDown()
    {
        if (jellyRigidbody.linearVelocityY > 0.05f || jellyRigidbody.linearVelocityY < -0.05f)
        {
            return;
        }

        activeColliders.SetActive(!isActive);
        inActiveColliders.SetActive(isActive);

        isActive = true;

        var raycast = Physics2D.RaycastAll(jellyRigidbody.position, Vector2.down, 1.2f);
        if (raycast.Any(r => r.transform.CompareTag("Platform")))
        {
            var hit = raycast.First(r => r.transform.CompareTag("Platform"));
            activeColliders.transform.rotation = Quaternion.FromToRotation(Vector2.up, hit.normal);
            activeColliders.transform.position = hit.point;
        }
        else
        {
            activeColliders.transform.rotation = Quaternion.FromToRotation(Vector2.up, inActiveColliders.transform.up);
        }
        viewAnimator.SetTrigger("Mouse down");
    }

    public void OnActionUp()
    {
        if (!isActive)
        {
            return;
        }

        activeColliders.SetActive(!isActive);
        inActiveColliders.SetActive(isActive);
        isActive = false;

        jellyRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        viewAnimator.SetTrigger("Mouse up");
    }

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
            if (!moveSource.isPlaying)
            {
                moveSource.Play();
            }
        }

        var currentSpeed = jellyRigidbody.linearVelocity.magnitude;
        var moveStep = moveDirection * movementSpeed * Time.deltaTime;
        moveStep.y = 0.0f;
        var actualForce = moveStep * (1 - currentSpeed / maxMovementSpeed);
        jellyRigidbody.AddForce(actualForce);
    }

    public void OnPossessed(PlayerController playerController)
    {
        view = Instantiate(jellyViewPrefab, transform);
        viewAnimator = view.GetComponent<Animator>();
        var sfx = SfxService.Instance.SfxData.Ingredients.Jelly.Move;
        moveSource = SfxService.Instance.PrepareSound(sfx);
        moveSource.loop = true;

        foreach (Transform child in view.transform)
        {
            if (child.CompareTag("Indicator"))
            {
                child.GetComponent<SpriteRenderer>().color = playerController.GetColorByPlayerIndex();
            }
        }
    }

    public void OnDeath()
    {
        Destroy(view);
        Destroy(gameObject);
        Destroy(moveSource.gameObject);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.attachedRigidbody == null)
        {
            return;
        }

        if (collision.attachedRigidbody.CompareTag("Ingredient"))
        {
            var sfx = SfxService.Instance.SfxData.Ingredients.Jelly.Hit;
            SfxService.Instance.PlayOneShoot(sfx);
        }
    }
}
