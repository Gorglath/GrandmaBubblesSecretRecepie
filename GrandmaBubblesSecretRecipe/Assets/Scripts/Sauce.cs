using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sauce : MonoBehaviour
{
    private void OnEnable()
    {
        var spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (var sprite in spriteRenderers)
        {
            var material = sprite.material;
            if (!material.HasInt("_isSauced"))
            {
                continue;
            }
            material.SetInt("_isSauced", 1);
        }
    }

    private void OnDestroy()
    {
        var spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (var sprite in spriteRenderers)
        {
            var material = sprite.material;
            if (!material.HasInt("_isSauced"))
            {
                continue;
            }
            material.SetInt("_isSauced", 0);
        }
    }
}
