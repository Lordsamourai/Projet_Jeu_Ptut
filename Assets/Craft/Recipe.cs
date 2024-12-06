using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recipe : MonoBehaviour
{
    private RecipeData currentRecipe;

    [SerializeField]
    private Image craftableItemImage;

    [SerializeField]
    private GameObject elementRequiredPrefab;

    [SerializeField]
    private Transform elementsRequiredPrefab;

    [SerializeField]
    private Button craftButton;

    [SerializeField]
    private Sprite canBuildIcon;

    [SerializeField]
    private Sprite cantBuildIcon;

    private RecipeData recipe;

    private int RecipeAmount;

    public void Configure(RecipeData recipe)
    {
        currentRecipe = recipe;
        //Item = currentRecipe.craftableItem.prefab;

        craftableItemImage.sprite = recipe.craftableItem.visual;

        RecipeAmount = recipe.amount;

        for (int i = 0; i < recipe.requiredItems.Length; i++)
        {
            GameObject requiredItem = Instantiate(elementRequiredPrefab, elementsRequiredPrefab);
            requiredItem.transform.GetChild(0).GetComponent<Image>().sprite = recipe.requiredItems[i].visual;
        }

        ResizeElementsRequiredParent();

    }

    private void ResizeElementsRequiredParent()
    {
        Canvas.ForceUpdateCanvases();
        elementsRequiredPrefab.GetComponent<ContentSizeFitter>().enabled = false;
        elementsRequiredPrefab.GetComponent<ContentSizeFitter>().enabled = true;
    }

    public void CraftItem()
    {
        if (ListeItems.instance == null)
        {
            Debug.LogError("ListeItems.instance est null.");
            return;
        }

        GameObject prefab = currentRecipe.craftableItem.prefab;
        if (prefab == null)
        {
            Debug.LogError("Prefab du craftableItem est null.");
            return;
        }

        // Instanciation de l'objet à partir du prefab
        GameObject instance = Instantiate(prefab);
        instance.GetComponent<Item>().amount = RecipeAmount;
        ListeItems.instance.AddtoInventory(instance); 
    }
}
