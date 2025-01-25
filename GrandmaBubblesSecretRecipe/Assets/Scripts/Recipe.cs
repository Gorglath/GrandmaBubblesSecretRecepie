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

    public List<IngredientListing> IngredientListings => ingredientListings;

    public (bool complete, bool correct, IngredientListing ingredientListing) UpdateListing(IPossesable ingredient)
    {
        IngredientListing listing = new IngredientListing();
        bool correct = false;
        foreach (var ingredientListing in ingredientListings)
        {
            if (ingredientListing.ingredientType != ingredient.IngerdientType)
            {
                continue;
            }

            if((ingredientListing.shouldBeCooked && !ingredient.isCooked())
                || (!ingredientListing.shouldBeCooked && ingredient.isCooked()))
            {
                continue;
            }

            if((ingredientListing.shouldBeGrated && !ingredient.isGrated())
                || (!ingredientListing.shouldBeGrated && ingredient.isGrated()))
            {
                continue;
            }

            if((ingredientListing.shouldBeSauced && !ingredient.isSauced())
                || (!ingredientListing.shouldBeSauced && ingredient.isSauced()))
            {
                continue;
            }

            if((ingredientListing.shouldBePowdered && !ingredient.isPowdered())
                || (!ingredientListing.shouldBePowdered && ingredient.isPowdered()))
            {
                continue;
            }

            if((ingredientListing.shouldBeSliced && !ingredient.isSliced()) 
                || (!ingredientListing.shouldBeSliced && ingredient.isSliced()))
            {
                continue;
            }

            listing = ingredientListing;
            correct = true;
        }

        if (ingredientListings.Contains(listing))
        {
            ingredientListings.Remove(listing);
            if(ingredientListings.Count == 0)
            {
                return (true, correct, listing);
            }
        }

        return (false, correct, listing);
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
    public static bool operator ==(IngredientListing c1, IngredientListing c2)
    {
        return c1.Equals(c2);
    }
    public static bool operator !=(IngredientListing c1, IngredientListing c2)
    {
        return !c1.Equals(c2);
    }
}
