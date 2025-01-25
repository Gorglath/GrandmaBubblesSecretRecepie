using UnityEngine;
using UnityEngine.InputSystem;

public class MenuPlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;

    [SerializeField]
    private Sprite[] bubbleSprites;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        spriteRenderer.sprite = bubbleSprites[playerInput.playerIndex];
    }
}
