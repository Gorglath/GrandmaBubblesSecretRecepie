using System;
using UnityEngine;

[CreateAssetMenu(menuName = "SfxData")]
[Serializable]
public class SfxData : ScriptableObject
{
    public AudioDefinition MainMusic;
    public IngredientDef Ingredients;
    public AudioDefinition Posses;
    public KictchenUtilsDef KictchenUtils;

    [Serializable]
    public struct KictchenUtilsDef
    {
        public AudioDefinition[] Pot;
        public HotPotDef HotPot;
        public SlicerDef Slicer;
        public GraterDef Grater;
        public SaucerDef Saucer;
        public RecipeBookDef RecipeBook;

        [Serializable]
        public struct HotPotDef
        {
            public AudioDefinition Cooking;
            public AudioDefinition Burned;
        }
        [Serializable]
        public struct GraterDef
        {
            public AudioDefinition[] Grating;
        }
        [Serializable]
        public struct SlicerDef
        {
            public AudioDefinition[] Slice;
        }
        [Serializable]
        public struct SaucerDef
        {
            public AudioDefinition[] Sauce;
        }
        [Serializable]
        public struct RecipeBookDef
        {
            public AudioDefinition RecipeComplete;
            public AudioDefinition[] ItemCompleted;
        }
    }
    [Serializable]
    public struct IngredientDef
    {
        public ChickenDef Chicken;
        public EggDef Egg;
        public SludgeDef Sludge;
        public TentacleDef Tentacle;
        public CabbageDef Cabbage;
        public CheeseDef Cheese;
        public JellyDef Jelly;

        [Serializable]
        public struct ChickenDef
        {
            public AudioDefinition Jump;
        }
        [Serializable]
        public struct EggDef
        {
            public AudioDefinition Run;
            public AudioDefinition Jump;
            public AudioDefinition[] Break;
        }
        [Serializable]
        public struct SludgeDef
        {
            public AudioDefinition Move;
            public AudioDefinition Jump;
        }
        [Serializable]
        public struct TentacleDef
        {
            public AudioDefinition Prepare;
            public AudioDefinition Jump;
        }
        [Serializable]
        public struct CabbageDef
        {
            public AudioDefinition Move;
            public AudioDefinition Jump;
            public AudioDefinition Hit;

        }
        [Serializable]
        public struct CheeseDef
        {
            public AudioDefinition Move;
        }
        [Serializable]
        public struct JellyDef
        {
            public AudioDefinition Move;
            public AudioDefinition Hit;
        }
    }
}

[Serializable]
public struct AudioDefinition
{
    [Range(0f, 1f)] public float volume;
    public AudioClip audioClip;
}
