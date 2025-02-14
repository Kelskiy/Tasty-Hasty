using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Data;

public class RegistrationUIController : MonoBehaviour
{
    public TMP_InputField phoneNumberInputField;
    public TMP_InputField nameInputField;
    public TMP_InputField passwordInputField;

    public Button applyButton;
    public Button backButton;

    public GameObject view;

    public static string incorrectData = "Please fill in correct data";
    public static string userAlreadyExist = "Користувач з таким номером уже існує";
    public static string registationSuccess = "Реєстрація успішна";
    public static string incorrectPhoneNumberFormat = "Введіть номер телефону у форматі 380*********";

    private int minPhoneNumberCharLenght = 12;

    void Start()
    {
        applyButton.onClick.AddListener(ApplyRegistration);
        backButton.onClick.AddListener(BackToMainMenu);
    }

    public void HandleView(bool isActive)
    {
        view.SetActive(isActive);
    }

    private void ApplyRegistration()
    {
        if (string.IsNullOrEmpty(phoneNumberInputField.text) || string.IsNullOrEmpty(nameInputField.text) || string.IsNullOrEmpty(passwordInputField.text))
        {
            MainMenuUIManager.Instance.errorPopupController.ShowNotificationText(incorrectData);
            return;
        }

        if (phoneNumberInputField.text.Length < minPhoneNumberCharLenght)
        {
            MainMenuUIManager.Instance.errorPopupController.ShowNotificationText(incorrectPhoneNumberFormat);
            return;
        }

        string phonenumber = MyDataBase.ExecuteQueryWithAnswer($"SELECT phoneNumber FROM Customers WHERE phoneNumber = {phoneNumberInputField.text}");

        if (string.IsNullOrEmpty(phonenumber) == false)
        {
            MainMenuUIManager.Instance.errorPopupController.ShowNotificationText(userAlreadyExist);
            return;
        }

        MyDataBase.ExecuteQueryWithoutAnswer($"INSERT INTO Customers (name, phoneNumber, bonus, password) VALUES ('{nameInputField.text}', '{phoneNumberInputField.text}', {10}, '{passwordInputField.text}')");

        MainMenuUIManager.Instance.errorPopupController.ShowNotificationText(registationSuccess);

        BackToMainMenu();
    }

    private void BackToMainMenu()
    {
        MainMenuUIManager.Instance.authorizationUIController.HandleView(true);
        HandleView(false);
    }
}
