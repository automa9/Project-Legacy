using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;
using Facebook.Unity;

public class AuthManager : MonoBehaviour
{
    public Text logTxt;
    public Text descriptionTxt;

    private string accessToken;

    async void Start()
    {
        await UnityServices.InitializeAsync();
    }

    void Awake ()
{
    if (!FB.IsInitialized) {
        // Initialize the Facebook SDK
        FB.Init(InitCallback, OnHideUnity);
    } else {
        // Already initialized, signal an app activation App Event
        FB.ActivateApp();
    }
}


    private void InitCallback ()
    {
        if (FB.IsInitialized) {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        } else {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity (bool isGameShown)
    {
        if (!isGameShown) {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        } else {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    //SIGN CALL ANONYMOUSLY
    public async void SignIn()
    {
        await signAnonymous();
    }

    public async void SignInFB()
    {
        await LinkWithFacebookAsync(accessToken);
    }

    //SIGN IN ANONYMOUSLY 
    async Task signAnonymous()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            print("Sign in success");
            print("Player Id:"+AuthenticationService.Instance.PlayerId);
            logTxt.text = "Player id: " + AuthenticationService.Instance.PlayerId;
        }
        catch(AuthenticationException ex)
        {
            print("Sign failed");
            Debug.LogException(ex);
        }
    }



//FACEBOOK SIGNIN
    async Task SignInWithFacebook(string accessToken)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithFacebookAsync(accessToken);
            Debug.Log("SignIn is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    public async Task LinkWithFacebookAsync(string accessToken)
    {
        try
        {
            await AuthenticationService.Instance.LinkWithFacebookAsync(accessToken);
            Debug.Log("Link is successful.");
        }
        catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
        {
            // Prompt the player with an error message.
            Debug.LogError("This user is already linked with another account. Log in instead.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }
}
