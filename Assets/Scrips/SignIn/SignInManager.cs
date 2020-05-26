using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading;
using Firebase.Extensions;

public class SignInManager : MonoBehaviour
{
    public static FirebaseAuth firebaseAuth;
    public static FirebaseApp firebaseApp;
    public static FirebaseUser firebaseUser;

    public InputField emailField;
    public InputField passwordField;
    public Button signInButton;

    public bool isFirebaseReady { get; private set; }
    public bool isSignInOnProgress { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
        signInButton.interactable = false;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task=> { 
            if(task.Result != DependencyStatus.Available)
            {
                Debug.LogError(task.Result.ToString());
                isFirebaseReady = false;
            }

            else
            {
                isFirebaseReady = true;

                firebaseApp = FirebaseApp.DefaultInstance;
                firebaseAuth = FirebaseAuth.DefaultInstance;
                Debug.Log("FF");
            }

            signInButton.interactable = isFirebaseReady;
        });
    }

    public void SignIn()
    {
        if(!isFirebaseReady || isSignInOnProgress || firebaseUser != null)
        {
            return;
        }
        isSignInOnProgress = true;
        signInButton.interactable = false;
        firebaseAuth.SignInWithEmailAndPasswordAsync(emailField.text, passwordField.text).ContinueWithOnMainThread(task =>
        {
            Debug.Log($"Sign in status : {task.Status}");

            isSignInOnProgress = false;
            signInButton.interactable = true;

            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else if (task.IsCanceled)
            {
                Debug.LogError("Sign In canceled");
            }
            else
            {
                firebaseUser = task.Result;
                Debug.Log(firebaseUser.Email);
                SceneManager.LoadScene("Lobby");
            }
        });
    }
}
