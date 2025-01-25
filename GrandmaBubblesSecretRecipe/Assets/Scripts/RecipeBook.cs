using System.Collections.Generic;
using UnityEngine;

public class RecipeBook : MonoBehaviour
{
    [SerializeField]
    private RecipeBookEntry bookEntryPrefab;

    [SerializeField]
    private Transform recipeEntryContainer;

    private List<RecipeBookEntry> bookEntries = new List<RecipeBookEntry>();
   public void PopulateByRecipe(Recipe newRecipe)
    {
        foreach (Transform child in recipeEntryContainer)
        {
            Destroy(child.gameObject);
        }
        bookEntries.Clear();

        foreach (var listing in newRecipe.IngredientListings)
        {
            var bookEntryInstance = Instantiate(bookEntryPrefab, recipeEntryContainer);
            bookEntryInstance.Populate(listing);
            bookEntries.Add(bookEntryInstance);
        }
    }

    public void CrossBookEntry(IngredientListing listing)
    {
        foreach (var entry in bookEntries)
        {
            if(entry.IngredientListing == listing)
            {
                entry.Complete();
                var sfxCollection = SfxService.Instance.SfxData.KictchenUtils.RecipeBook.ItemCompleted;
                var sfx = sfxCollection[Random.Range(0, sfxCollection.Length)];
                SfxService.Instance.PlayOneShoot(sfx);
                break;
            }
        }
    }
}
