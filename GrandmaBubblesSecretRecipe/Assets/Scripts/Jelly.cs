using System.Linq;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Jelly : MonoBehaviour, IPossesable
{
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private float maxMovementSpeed;

    [SerializeField]
    private GameObject activeColliders;

    [SerializeField]
    private GameObject inActiveColliders;

    [SerializeField]
    private GameObject jellyViewPrefab;

    [SerializeField]
    private Rigidbody2D jellyRigidbody;

    [SerializeField]
    private float activeScale;

    private GameObject view;
    private bool isActive;
    private float initialXScale;

    public bool Active => isActive;

    public void OnActionDown()
    {
    }

    public void OnActionUp()
    {
    }

    public void OnAction()
    {
        if (jellyRigidbody.linearVelocityY > 0.05f || jellyRigidbody.linearVelocityY < -0.05f)
        {
            return;
        }

        activeColliders.SetActive(!isActive);
        inActiveColliders.SetActive(isActive);
        var raycast = Physics2D.RaycastAll(jellyRigidbody.position, Vector2.down, 1.2f);
        if(raycast.Any(r => r.transform.CompareTag("Platform"))){
            var hit = raycast.First(r => r.transform.CompareTag("Platform"));
            activeColliders.transform.rotation = Quaternion.FromToRotation(Vector2.up, hit.normal);
            activeColliders.transform.position = hit.point;
        } else
        {
            activeColliders.transform.rotation = Quaternion.FromToRotation(Vector2.up, inActiveColliders.transform.up);
        }
        if (isActive)
        {
            isActive = false;
            jellyRigidbody.constraints = RigidbodyConstraints2D.None;
            var scale = jellyRigidbody.transform.localScale;
            scale.x = initialXScale;
            jellyRigidbody.transform.localScale = scale;
        }
        else
        {
            isActive = true;
            jellyRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            var scale = jellyRigidbody.transform.localScale;
            scale.x = activeScale;
            jellyRigidbody.transform.localScale = scale;
        }
    }

    public void OnMove(Vector2 moveDirection)
    {
        if (isActive)
        {
            return;
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
        initialXScale = jellyRigidbody.transform.localScale.x;
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
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(jellyRigidbody.position, jellyRigidbody.position + Vector2.down * 1.2f);
    }
}
