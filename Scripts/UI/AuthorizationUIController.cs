using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Data;

public class AuthorizationUIController : MonoBehaviour
{
    public InputField phoneNumberInput;
    public InputField passwordInputField;

    public Button registrationButton;
    public Button continueButton;
    public Button continueWithoutAuthButton;

    public GameObject view;

    public static string emptyPhoneNumber = "Введіть дані в поля!";
    public static string userDoesntExist = "Користувача не існує";
    public static string passDoesntMatch = "Пароль не вірний";

    public int currentUserID { get; private set; }


    void Start()
    {
        registrationButton.onClick.AddListener(OpenRegistrationMenu);
        continueWithoutAuthButton.onClick.AddListener(OpenFoodMenu);
        continueButton.onClick.AddListener(GetUser);
    }

    public void HandleView(bool isActiive)
    {
        view.SetActive(isActiive);
    }

    private void OpenRegistrationMenu()
    {
        MainMenuUIManager.Instance.registrationUIController.HandleView(true);
        HandleView(false);
    }

    private void OpenFoodMenu()
    {
        MainMenuUIManager.Instance.foodMenuUIController.HandleView(true);
        HandleView(false);
    }

    private void GetUser()
    {
        if (string.IsNullOrEmpty(phoneNumberInput.text) || string.IsNullOrEmpty(passwordInputField.text))
        {
            MainMenuUIManager.Instance.errorPopupController.ShowNotificationText(emptyPhoneNumber);
            return;
        }

        //DataTable table = MyDataBase.GetTable($"SELECT * FROM Customers WHERE phoneNumber = {phoneNumberInput.text};");
        string phonenumber = MyDataBase.ExecuteQueryWithAnswer($"SELECT phoneNumber FROM Customers WHERE phoneNumber = {phoneNumberInput.text}");

        if (string.IsNullOrEmpty(phonenumber))
        {
            MainMenuUIManager.Instance.errorPopupController.ShowNotificationText(userDoesntExist);
            return;
        }

        string password = MyDataBase.ExecuteQueryWithAnswer($"SELECT password FROM Customers WHERE phoneNumber = {phonenumber}");

        if (password != passwordInputField.text)
        {
            MainMenuUIManager.Instance.errorPopupController.ShowNotificationText(passDoesntMatch);
            return;
        }

        currentUserID = int.Parse(MyDataBase.ExecuteQueryWithAnswer($"SELECT id FROM Customers WHERE phoneNumber = {phonenumber}"));

        MainMenuUIManager.Instance.basketMenuUIController.InitializeBasketSystem(currentUserID);

        OpenFoodMenu();
    }
}
