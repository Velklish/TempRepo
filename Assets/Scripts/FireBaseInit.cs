using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Firebase;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.Events;

public class FireBaseInit : MonoBehaviour
{
    public UnityEvent OnInitialized = new UnityEvent();
    public InitializationFailedEvent OnInitializationFailed = new InitializationFailedEvent();

    public FirebaseApp app;
    
    void Start()
    {
        /*Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;
                OnInitialized.Invoke();
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            } else {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });*/
        
        
        
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                OnInitializationFailed.Invoke(task.Exception);
            }
            else
            {
                OnInitialized.Invoke();
                Firebase.Analytics.FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                Debug.Log("Firebase Initialised");
            }
        });
    }

    [Serializable]
    public class InitializationFailedEvent : UnityEvent<Exception>
    {
    }
}
