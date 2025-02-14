using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : Singleton<MainMenuUIManager>
{
    public DebugTextController debugTextController;

    public AuthorizationUIController authorizationUIController;
    public RegistrationUIController registrationUIController;
    public FoodMenuUIController foodMenuUIController;
    public ErrorPopupController errorPopupController;
    public BasketMenuUIController basketMenuUIController;

    public void ResetApp()
    {
        StartCoroutine(AppReset());
    }

    private IEnumerator AppReset()
    {
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("SampleScene");
    }
}
