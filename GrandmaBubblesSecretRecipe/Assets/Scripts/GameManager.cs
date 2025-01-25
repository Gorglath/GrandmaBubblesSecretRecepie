using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerInputManager playerInputManager;

    [SerializeField]
    private PlayerInput playerPrefab;

    [SerializeField]
    private CameraManager cameraManager;

    [SerializeField]
    private RecipeBook recipeBook;

    [SerializeField]
    private Recipe[] recipes;

    [SerializeField]
    private int numberOfRecipes;

    [SerializeField]
    private GameObject newRecipeVFX;
    
    [SerializeField]
    private GameEndEffect gameOverVFX;

    private DateTime startTime;
    private Recipe activeRecipe;
    private int activeRecipeIndex;
    private int completedRecipies;

    public void StartGame((int, InputDevice)[] playerIndices)
    {
        foreach (var playerIndex in playerIndices)
        {
            var player = playerInputManager.JoinPlayer(playerIndex.Item1, pairWithDevice: playerIndex.Item2);
            PlayerJoined(player);
        }

        ChooseRandomRecipe();
        startTime = DateTime.Now;
    }

    private void PlayerJoined(PlayerInput input)
    {
        var controller = input.GetComponent<PlayerController>();
        controller.transform.position = cameraManager.GetSpawnLocation();
        cameraManager.RegisterPlayer(controller);
        controller.RegisterGameManager(this);
    }

    public void DeliverIngredient(IPossesable ingredient)
    {
        var recipeTopple = activeRecipe.UpdateListing(ingredient);
        recipeBook.CrossBookEntry(recipeTopple.ingredientListing);
        if (recipeTopple.complete)
        {
            var sfx = SfxService.Instance.SfxData.KictchenUtils.RecipeBook.RecipeComplete;
            SfxService.Instance.PlayOneShoot(sfx);
            completedRecipies++;
            if(completedRecipies >= numberOfRecipes)
            {
                // Finish game.
                var gameOverEffect = Instantiate(gameOverVFX);
                gameOverEffect.Set(DateTime.Now - startTime);
                return;
            }

            ChooseRandomRecipe();
        }
    }

    public void ChooseRandomRecipe()
    {
        var recipeIndex = UnityEngine.Random.Range(0, recipes.Length);
        if(recipeIndex == activeRecipeIndex)
        {
            recipeIndex = activeRecipeIndex == 0 ? UnityEngine.Random.Range(1, recipes.Length) : activeRecipeIndex - 1;
        }
        var recipe = recipes[recipeIndex];
        if(activeRecipe != null)
        {
            Destroy(activeRecipe);
        }

        activeRecipe = Instantiate(recipe);

        recipeBook.PopulateByRecipe(activeRecipe);

        var newRecipeEffect = Instantiate(newRecipeVFX);
        Destroy(newRecipeEffect, 5.0f);
    }
}
