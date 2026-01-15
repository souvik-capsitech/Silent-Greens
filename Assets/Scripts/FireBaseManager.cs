using UnityEngine;
using Firebase;
using Firebase.Analytics;
using Firebase.Crashlytics;
using System;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;

    public static string SessionId { get; private set; }

    void Awake()
    {
        // Singleton protection
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Create a session ID for this app launch
        SessionId = Guid.NewGuid().ToString();

        InitializeFirebase();
    }

    void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;

                // Enable Analytics
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

                // Enable Crashlytics
                Crashlytics.IsCrashlyticsCollectionEnabled = true;

                Debug.Log("[FirebaseManager] Firebase initialized successfully");
                Debug.Log("[FirebaseManager] Session ID: " + SessionId);
            }
            else
            {
                Debug.LogError("[FirebaseManager] Firebase dependency error: " + task.Result);
            }
        });
    }
}
