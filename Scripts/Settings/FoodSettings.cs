using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FoodSettings : MonoBehaviour
{
    public List<FoodConfig> foodConfigs = new List<FoodConfig>();

    public FoodConfig GetConfig(FoodCode foodCode)
    {
        return foodConfigs.Find(x => x.foodCode == foodCode);
    }

    //public List<FoodConfig> GetFoodConfigsByType(FoodType foodType)
    //{ 

    //}

}
