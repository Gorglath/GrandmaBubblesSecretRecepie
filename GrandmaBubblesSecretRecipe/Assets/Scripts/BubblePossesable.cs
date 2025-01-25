using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BubblePossesable : MonoBehaviour, IPossesable
{
    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private GameObject bubbleViewPrefab;

    [SerializeField]
    private Rigidbody2D bubbleRigidbody;

    [SerializeField]
    private Sprite[] bubbleSprites;

    private GameObject view;
    private PossessableGenerator availableGenerator;
    private PlayerController playerController;
    public IngredientType IngerdientType => IngredientType.None;
    public void OnActionDown()
    {
    }

    public void OnActionUp()
    {
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void OnAction()
    {
        if(availableGenerator != null)
        {
            var possessable = availableGenerator.GetPossesableInstance(playerController.transform);
            playerController.Possess(possessable);

            var sfxService = SfxService.Instance;
            sfxService.PlayOneShoot(sfxService.SfxData.Posses);
        }
    }

    public void OnMove(Vector2 moveDirection)
    {
        var moveStep = moveDirection * movementSpeed * Time.deltaTime;
        bubbleRigidbody.linearVelocity = new Vector3(moveStep.x, moveStep.y, 0);
    }

    public void OnPossessed(PlayerController playerController)
    {
        view = Instantiate(bubbleViewPrefab, transform);
        var spriteRenderer = view.GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = bubbleSprites[playerController.GetPlayerIndex()];
    }

    public void OnDeath()
    {
        Destroy(view);
    }

    public void RegisterPossesable(PossessableGenerator generator)
    {
        availableGenerator = generator;
    }

    public void DeregisterPossesable(PossessableGenerator generator)
    {
        if (availableGenerator != generator)
        {
            return;
        }

        availableGenerator = null;
    }

    internal void RegisterController(PlayerController playerController)
    {
        this.playerController = playerController;
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
}
