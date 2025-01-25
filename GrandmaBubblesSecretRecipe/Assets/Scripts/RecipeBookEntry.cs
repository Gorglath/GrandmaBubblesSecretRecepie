using System;
using System.Collections.Generic;
using UnityEngine;

public class RecipeBookEntry : MonoBehaviour
{
    private const string keyVarientSeperator = "_";

    [SerializeField]
    private List<Sprite> recipeEntrySprite = new List<Sprite>();

    [SerializeField]
    private List<string> recipeEntryListing = new List<string>();

    [SerializeField]
    private SpriteRenderer recipeEntryRenderer;

    [SerializeField]
    private GameObject completeObject;

    private IngredientListing ingredientListing;
    public IngredientListing IngredientListing => ingredientListing;
    public void Populate(IngredientListing ingredientListing)
    {
        this.ingredientListing = ingredientListing;
        var spriteKey = GetSpriteKey(ingredientListing);
        var spriteIndex = recipeEntryListing.IndexOf(spriteKey);
        recipeEntryRenderer.sprite = recipeEntrySprite[spriteIndex];
    }

    public void Complete()
    {
        completeObject.SetActive(true);
    }

    private string GetSpriteKey(IngredientListing ingredientListing)
    {
        var spriteKey = ingredientListing.ingredientType.ToString();
        if (ingredientListing.shouldBeSauced)
        {
            spriteKey += keyVarientSeperator;
            spriteKey += "Sauced";
        }

        if (ingredientListing.shouldBeGrated)
        {
            spriteKey += keyVarientSeperator;
            spriteKey += "Grated";
        }

        if (ingredientListing.shouldBeCooked)
        {
            spriteKey += keyVarientSeperator;
            spriteKey += "Cooked";
        }

        if (ingredientListing.shouldBeSliced)
        {
            spriteKey += keyVarientSeperator;
            spriteKey += "Sliced";
        }

        if (ingredientListing.shouldBePowdered)
        {
            spriteKey += keyVarientSeperator;
            spriteKey += "Powdered";
        }

        return spriteKey;
    }
}
