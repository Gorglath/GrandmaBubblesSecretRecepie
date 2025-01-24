using UnityEngine;

public class Sludge : MonoBehaviour, IPossesable
{
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private float maxMovementSpeed;

    [SerializeField]
    private GameObject sludgeMainViewPrefab;

    [SerializeField]
    private GameObject sludgeActiveViewPrefab;

    [SerializeField]
    private Rigidbody2D sludgeRigidbody;

    private GameObject activeView;
    private GameObject mainView;
    private bool isActive;
    private bool attachedToWall;

    public void OnAction()
    {
        isActive = !isActive;
        activeView.SetActive(isActive);
        mainView.SetActive(!isActive);
    }

    public void OnMove(Vector2 moveDirection)
    {
        var currentSpeed = sludgeRigidbody.linearVelocity.magnitude;
        var moveStep = moveDirection * movementSpeed * Time.deltaTime;
        moveStep.y = attachedToWall? moveStep.y + -Physics.gravity.y : 0.0f;
        var actualForce = moveStep * (1 - currentSpeed / maxMovementSpeed);
        sludgeRigidbody.AddForce(actualForce);
    }

    public void OnPossessed(PlayerController playerController)
    {
        activeView = Instantiate(sludgeActiveViewPrefab, transform);
        mainView = Instantiate(sludgeMainViewPrefab, transform);
    }

    public void OnDeath()
    {
        Destroy(mainView);
        Destroy(activeView);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ClimbableWall"))
        {
            attachedToWall = true;
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
}
