using System;
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

    [SerializeField]
    private ParticleSystem bubbleSelectedParticleSystem;

    [SerializeField]
    private int emitAmount = 3;
    private bool isActive;
    private void OnEnable()
    {
        spriteRenderer.sprite = bubbleSprites[playerInput.playerIndex];
        var main = bubbleSelectedParticleSystem.main;
        main.startColor = GetColorByPlayerIndex();
        bubbleSelectedParticleSystem.Play();

        var action = playerInput.actions["Bubble"];
        action.started += PressedBubble;
        action.canceled += ReleasedBubble;
    }

    private void ReleasedBubble(InputAction.CallbackContext context)
    {
        isActive = false;
    }

    private void PressedBubble(InputAction.CallbackContext context)
    {
        isActive = true;
    }

    private void Update()
    {
        if (isActive)
        {
            bubbleSelectedParticleSystem.Emit(emitAmount);
        }
    }

    private Color GetColorByPlayerIndex()
    {
        switch (playerInput.playerIndex)
        {
            case 0:
                ColorUtility.TryParseHtmlString("#c23551", out var redColor);
                return redColor;
            case 1:
                ColorUtility.TryParseHtmlString("#c2a537", out var yellowColor);
                return yellowColor;
            case 2:
                ColorUtility.TryParseHtmlString("#3590c4 ", out var blueColor);
                return blueColor;
            case 3:
            default:
                ColorUtility.TryParseHtmlString("#c23551", out var greenColor);
                return greenColor;
        }
    }
}
