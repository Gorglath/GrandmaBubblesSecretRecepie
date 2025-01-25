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
        return playerInput.playerIndex switch
        {
            0 => Color.red,
            1 => Color.yellow,
            2 => Color.cyan,
            3 => Color.green
        };
    }
}
