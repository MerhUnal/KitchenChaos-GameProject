using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class DeliveryManager : MonoBehaviour
{

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeFailed;
    public event EventHandler OnRecipeSuccess;
    public static DeliveryManager Instance {  get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;
    [SerializeField] private KitchenObjectSO meat;


    private List <RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;
    private int successfulRecipeAmount;
    private int currentLevelIndex;
    [SerializeField] private List<int> recipeList = new List<int>();
    private void Awake()
    {   
        Instance = this;

        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Start()
    {
        currentLevelIndex=LevelSystem.MainLevelSystem.GetCurrentLevelIndex();
        recipeList.AddRange(LevelSystem.MainLevelSystem.levelDatas.levelDatas[currentLevelIndex].recipeSOs);
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer < 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            if (KitchenGameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count < waitingRecipesMax)
            {
                int rnd = Random.Range(0, recipeList.Count);
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[recipeList[rnd]];
                Debug.Log(waitingRecipeSO.recipeName);
                waitingRecipeSOList.Add(waitingRecipeSO);
            }
            OnRecipeSpawned?.Invoke(this, EventArgs.Empty);

        }
    }
    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
          
                RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                //Has the same number of ingredients
                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {   
                    //Cyclying through all ingredients in the recipe
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                    //Cycling through all ingredients in the Plate
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            //ingredient matches!
                            ingredientFound = true;
                            break;
                         }
                    }
                    if (!ingredientFound)
                    {
                        //This Recipe ingredient was not found on the Plate
                        plateContentsMatchesRecipe = false;
                    }
                }
                
                  if (plateContentsMatchesRecipe)
                {
                    //Player delivered the correct recipe!
                    successfulRecipeAmount++;
                    waitingRecipeSOList.RemoveAt(1);

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);

                    //GameTimeAdd
                    float timeToAdd = 0f;
                    foreach (KitchenObjectSO ingredient in waitingRecipeSO.kitchenObjectSOList)
                    {
                        if (ingredient == meat)
                        {
                            timeToAdd += 5f; 
                        }
                        else
                        {
                            timeToAdd += 3f; 
                        }
                    }
                    KitchenGameManager.Instance.AddTime(timeToAdd);
                    return;

                }  
                
            }
        }

        //No Matches Found!
        //Player did not deliver a correct recipe
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }
    public int GetsuccessfulRecipesAmount()
    {
        return successfulRecipeAmount;
    }
    public void IncrementSuccessfulRecipes()
    {
        successfulRecipeAmount++;
    }

    public void ResetSuccessfulRecipes()
    {
        successfulRecipeAmount = 0;
    }
}


