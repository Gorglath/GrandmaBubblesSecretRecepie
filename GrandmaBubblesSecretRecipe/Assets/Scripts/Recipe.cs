using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName ="Recipe")]
[Serializable]
public class Recipe : ScriptableObject
{
    [SerializeField]
    private List<IngredientListing> ingredientListings = new List<IngredientListing>();

    public bool UpdateListing(IPossesable ingredient)
    {
        IngredientListing listing = new IngredientListing();
        foreach (var ingredientListing in ingredientListings)
        {
            if (ingredientListing.ingredientType != ingredient.IngerdientType)
            {
                continue;
            }

            if(ingredientListing.shouldBeCooked && !ingredient.isCooked())
            {
                continue;
            }

            if(ingredientListing.shouldBeGrated && !ingredient.isGrated())
            {
                continue;
            }

            if(ingredientListing.shouldBeSauced && !ingredient.isSauced())
            {
                continue;
            }

            if(ingredientListing.shouldBePowdered && !ingredient.isPowdered())
            {
                continue;
            }

            if(ingredientListing.shouldBeSliced && !ingredient.isSliced())
            {
                continue;
            }

            listing = ingredientListing;
        }

        if (ingredientListings.Contains(listing))
        {
            ingredientListings.Remove(listing);
            if(ingredientListings.Count == 0)
            {
                return true;
            }
        }

        return false;
    }
}

[Serializable]
public struct IngredientListing
{
    public IngredientType ingredientType;
    public bool shouldBeCooked;
    public bool shouldBeGrated;
    public bool shouldBeSliced;
    public bool shouldBeSauced;
    public bool shouldBePowdered;
}
