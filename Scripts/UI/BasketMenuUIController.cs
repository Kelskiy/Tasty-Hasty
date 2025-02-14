using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Data;
using System;

public class BasketMenuUIController : MonoBehaviour
{
    public Transform holder;

    public GameObject view;
    public GameObject foodPositionPrefab;

    public Button useBonusesButton;
    public Button payButton;
    public Button backButton;

    public TextMeshProUGUI bonusAmountText;
    public TextMeshProUGUI totalOrderPriceText;

    private List<FoodItemUIController> basketItems = new List<FoodItemUIController>();

    private int userID;
    private int bonusCount;
    private int totalOrderPrice;

    private bool isUserAuthorized;

    private static string notAuthorizedMessage = "Авторизуйтесь щоб використати бонуси";
    private static string paymentMessage = "Замовлення успішне, візьміть Ваш чек";

    private void Start()
    {
        useBonusesButton.onClick.AddListener(HandlePayWithBonuses);
        payButton.onClick.AddListener(HandlePaymentProcess);
    }

    private void HandlePaymentProcess()
    {
        MainMenuUIManager.Instance.errorPopupController.ShowNotificationText(paymentMessage);
        HandleView(false);
        ClearBasket();

        MainMenuUIManager.Instance.ResetApp();
    }

    private void HandlePayWithBonuses()
    {
        if (isUserAuthorized == false)
        {
            MainMenuUIManager.Instance.errorPopupController.ShowNotificationText(notAuthorizedMessage);
            return;
        }

        int orderPriceWithBonusDiscount = totalOrderPrice - bonusCount;

        if (orderPriceWithBonusDiscount >= 0)
        {
            totalOrderPriceText.text = orderPriceWithBonusDiscount.ToString();
            bonusCount = 0;
            MyDataBase.ExecuteQueryWithoutAnswer($"UPDATE Customers SET bonus = {bonusCount} WHERE id = {userID}");
        }
        else
        {
            totalOrderPrice = 0;
            bonusCount = -orderPriceWithBonusDiscount;

            UpdateBasketUI();

            MyDataBase.ExecuteQueryWithoutAnswer($"UPDATE Customers SET bonus = {bonusCount} WHERE id = {userID}");
        }
    }

    public void CheckTotalPrice()
    {
        for (int i = 0; i < basketItems.Count; i++)
        {
            int itemPrice = basketItems[i].foodConfig.price * basketItems[i].orderCount;

            totalOrderPrice += itemPrice;
        }

        UpdateBasketUI();
    }

    public void InitializeBasketSystem(int ID)
    {
        userID = ID;

        bonusCount = int.Parse(MyDataBase.ExecuteQueryWithAnswer($"SELECT bonus FROM Customers WHERE id = {userID}"));

        isUserAuthorized = true;

        UpdateBasketUI();
    }

    public void HandleBasketItemSpawn(FoodCode foodCode, int amount)
    {
        FoodItemUIController foodItem = Instantiate(foodPositionPrefab, holder).GetComponent<FoodItemUIController>();

        FoodConfig config = SettingsManager.Instance.foodSettings.GetConfig(foodCode);

        foodItem.InitializeFoodItem(config, amount);

        basketItems.Add(foodItem);
    }

    public void HandleView(bool isActive)
    {
        view.SetActive(isActive);
    }

    private void BackToFoodMenu()
    {
        MainMenuUIManager.Instance.foodMenuUIController.HandleView(true);
        HandleView(false);
    }

    private void pressBackButton()
    {
        MainMenuUIManager.Instance.foodMenuUIController.HandleView(true);
        HandleView(false);
    }

    private void UpdateBasketUI()
    {
        bonusAmountText.text = $"Ваші бонуси: {bonusCount}";
        totalOrderPriceText.text = $"Сума замовлення: {totalOrderPrice}UAH";
    }

    public void ClearBasket()
    {
        for (int i = 0; i < basketItems.Count; i++)
        {
            Destroy(basketItems[i].gameObject);
        }

        basketItems.Clear();
    }
}
