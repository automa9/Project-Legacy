using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class AuthManager : MonoBehaviour
{
    private PlayGamesClientConfiguration clientConfig;
    public Text logTxt;
    public Text descriptionTxt;

    private string token;

    async void Start(){
        await UnityServices.InitializeAsync();
        Configure();
    }

    internal void Configure(){
        clientConfig = new PlayGamesClientConfiguration.Builder().Build();
    }

    internal void SignIntoAuthManager(SignInInteractivity interactivity, PlayGamesClientConfiguration config){
        configuration = clientConfiguration;
        PlayGamesPlatform.InitalizeInstance(configuration);
        PlayGamesPlatform.Activate();

        PlayGamesPlatform.Instance.Authentication(interactivity,(code)=>
        {
            statusTxt.text = "Authenticating...";
            if(code == SignInStatus.Success)
            {
                statusTxt.text = " Successfully Authenticated ";
                descriptionTxt= "Hello" + Social.localUser.username + "You have an ID of " +Social.localUser.id;
            }else
            {
                status.text ="failed to Authenticate";
                descriptionTxt.text = "reason authentication failed: " + code;
            }
        });
    }
    

    //SIGN IN FUNCTION  
    public async void SignIn()
    {
        await signAnonymous();
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

    async Task signInWithGoogle(string token){
        try
        {
            await AuthenticationService.Instance.SignInWithGoogle(token);
            print("Sign in with google sucess");
        }
        catch (AuthenticationException ex)
        {
            print("Sign failed");
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex){
            print("Sign if failed");
            Debug.LogException(ex);
        }
    }
}
