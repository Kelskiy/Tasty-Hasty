using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FoodItemUIController : MonoBehaviour
{
    public event Action<FoodCode, int> OnItemAdded;
    public event Action<FoodCode, int> OnItemRemoved;

    public TextMeshProUGUI foodText;
    public TextMeshProUGUI orderCountText;
    public TextMeshProUGUI foodPriceText;

    public Button plusButton;
    public Button minusButton;

    public Image foodImage;

    public FoodConfig foodConfig { get; private set; }

    public int orderCount;

    private int maxItemAmount = 10;

    void Start()
    {
        plusButton.onClick.AddListener(AddOrderCount);
        minusButton.onClick.AddListener(SubstractOrderCount);
    }

    public void InitializeFoodItem(FoodConfig config, int amount = 0)
    {
        foodConfig = config;

        foodText.text = foodConfig.name;
        foodPriceText.text = foodConfig.price.ToString();

        orderCount = amount;

        if (foodConfig.icon != null)
        {
            foodImage.sprite = foodConfig.icon;
        }

        UpdateCounterUI();
    }

    private void AddOrderCount()
    {
        if (orderCount == maxItemAmount)
            return;

        orderCount++;
        orderCount = Mathf.Clamp(orderCount, 0, 10);

        OnItemAdded?.Invoke(foodConfig.foodCode, orderCount);

        UpdateCounterUI();
    }

    private void SubstractOrderCount()
    {
        if (orderCount == 0)
            return;

        orderCount--;
        orderCount = Mathf.Clamp(orderCount, 0, 10);

        OnItemRemoved?.Invoke(foodConfig.foodCode, orderCount);

        UpdateCounterUI();
    }

    private void UpdateCounterUI()
    {
        orderCountText.text = orderCount.ToString();
    }
}
