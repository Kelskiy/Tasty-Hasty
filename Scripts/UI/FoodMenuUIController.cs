using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FoodMenuUIController : MonoBehaviour
{
    private MainMenuUIManager mainMenuUIManager;

    public Button backButton;
    public Button allFoodFilterButton;
    public Button drinksFilterButton;
    public Button sweetFilterButton;
    public Button foodFilterButton;
    public Button bascketButton;

    public TextMeshProUGUI orderCountIndicateText;

    public GameObject foodPositionPrefab;

    public Transform holder;

    public GameObject view;

    private List<FoodItemUIController> foodItems = new List<FoodItemUIController>();

    private Dictionary<FoodCode, int> basket = new Dictionary<FoodCode, int>();

    private int basketItemCount;

    private int maxItemAmount = 10;

    private static string basketIsEmptyError = "Кошик пустий";

    void Start()
    {
        mainMenuUIManager = MainMenuUIManager.Instance;

        backButton.onClick.AddListener(BackToMainMenu);

        allFoodFilterButton.onClick.AddListener(ShowAllFood);
        drinksFilterButton.onClick.AddListener(ShowDrinks);
        sweetFilterButton.onClick.AddListener(ShowDeserts);
        foodFilterButton.onClick.AddListener(ShowMainFood);

        bascketButton.onClick.AddListener(HandleBasket);

        InitializeFoodMenu();
    }

    private void OnDestroy()
    {
        for (int i = 0; i < foodItems.Count; i++)
        {
            foodItems[i].OnItemAdded -= AddToBasket;
            foodItems[i].OnItemRemoved -= RemoveFromBasket;
        }
    }


    public void InitializeFoodMenu()
    {
        for (int i = 0; i < SettingsManager.Instance.foodSettings.foodConfigs.Count; i++)
        {
            FoodItemUIController foodItem = Instantiate(foodPositionPrefab, holder).GetComponent<FoodItemUIController>();

            foodItem.InitializeFoodItem(SettingsManager.Instance.foodSettings.foodConfigs[i]);

            foodItems.Add(foodItem);

            foodItem.OnItemAdded += AddToBasket;
            foodItem.OnItemRemoved += RemoveFromBasket;
        }
    }


    public void HandleView(bool isActive)
    {
        view.SetActive(isActive);
    }

    private void BackToMainMenu()
    {
        MainMenuUIManager.Instance.authorizationUIController.HandleView(true);
        HandleView(false);
    }

    private void ShowAllFood()
    {
        FilterFoodByType(FoodType.All);
    }

    private void ShowDrinks()
    {
        FilterFoodByType(FoodType.Drinks);
    }

    private void ShowDeserts()
    {
        FilterFoodByType(FoodType.Desserts);
    }

    private void ShowMainFood()
    {
        FilterFoodByType(FoodType.Food);
    }

    private void FilterFoodByType(FoodType foodType)
    {
        for (int i = 0; i < foodItems.Count; i++)
        {
            if (foodType != FoodType.All)
            {
                if (foodItems[i].foodConfig.foodType == foodType)
                {
                    foodItems[i].gameObject.SetActive(true);
                }
                else
                {
                    foodItems[i].gameObject.SetActive(false);
                }
            }
            else
            {
                foodItems[i].gameObject.SetActive(true);
            }
        }
    }

    private void AddToBasket(FoodCode foodCode, int amount)
    {
        basketItemCount++;

        if (basket.ContainsKey(foodCode))
        {
            basket.Remove(foodCode);
        }

        basket.Add(foodCode, amount);

        UpdateBasketCounterUI();
    }

    private void RemoveFromBasket(FoodCode foodCode, int amount)
    {
        if (basket.ContainsKey(foodCode))
        {
            basketItemCount--;

            basket.Remove(foodCode);

            if (amount > 0)
            {
                basket.Add(foodCode, amount);
            }
        }

        UpdateBasketCounterUI();
    }

    private void UpdateBasketCounterUI()
    {
        orderCountIndicateText.text = basketItemCount.ToString();
    }

    private void HandleBasket()
    {
        if (basketItemCount == 0)
        {
            mainMenuUIManager.errorPopupController.ShowNotificationText(basketIsEmptyError);
            return;
        }

        foreach (var item in basket)
        {
            //TODO: Handle with basket controller
            mainMenuUIManager.basketMenuUIController.HandleBasketItemSpawn(item.Key, item.Value);
            Debug.Log($"{item.Key}, {item.Value}");
        }

        mainMenuUIManager.basketMenuUIController.CheckTotalPrice();

        mainMenuUIManager.basketMenuUIController.HandleView(true);

        HandleView(false);
    }
    private void pressedBackButton()
    {
        mainMenuUIManager.basketMenuUIController.ClearBasket();
        mainMenuUIManager.authorizationUIController.HandleView(true);
        HandleView(false);

    }

}
