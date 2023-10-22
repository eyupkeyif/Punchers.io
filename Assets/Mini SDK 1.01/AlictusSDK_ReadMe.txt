This version of AlictusSDK was designed to be as non-intrusive as possible. It assumes familiarity with the setting up of Firebase, GameAnalytics, ApplovinMax, and Adjust. If you are not familiar with the setup of these assets, links to setup instructions are provided down below.

Please install the following packages:


Firebase Setup:
	Download URL: https://firebase.google.com/download/unity
	After extracting the downloaded file, please select and install Analytics and Crashlytics package from Net4 folder
	Firebase Setup Instructions: https://firebase.google.com/docs/unity/setup


GameAnalytics Setup:
	Download URL: https://download.gameanalytics.com/unity/GA_SDK_UNITY.unitypackage
	Setup Instructions: https://gameanalytics.com/docs/s/article/Integration-Unity-SDK
						Please read the "Initializing SDK" section

Adjust Setup:
	Download URL: https://github.com/adjust/unity_sdk/releases/download/v4.30.0/Adjust_v4.30.0.unitypackage
	Our SDK handles the initialization of Adjust; we only need you to install the package. No other steps are necessary. You don't need to send any Adjust events. Internally SDK waits for the Applovin Max to initialize before initializing Adjust


Applovin Max:
	Download URL: https://artifacts.applovin.com/unity/com/applovin/applovin-sdk/AppLovin-MAX-Unity-Plugin-5.3.1-Android-11.3.3-iOS-11.3.3.unitypackage
	Setup Instructions: https://dash.applovin.com/documentation/mediation/unity/getting-started/integration


All the relevant keys will be send to you. NOTE: If you see the following keys, UnityGameID, AD_GOAL_1, AD_GOAL_2, AD_GOAL_3, please ignore them, we use them internally.


AlictusSDK sets itself up by binding to RuntimeInitializeOnLoadMethod. Please DO NOT add it as a component to a GameObject.


After installation & setup please set the following keys in AlictusKey.cs file

     	public const string AdjustAppToken = "FILL_WITH_ADJUST_APP_TOKEN";
        public const string AdjustInterstitialOpportunityEventId = "FILL_WITH_APP_TRACKING_INTERSTITIAL_OPPORTUNITY_EVENT_ID";
        public const string AdjustRewardedOpportunityEventId = "FILL_WITH_APP_TRACKING_REWARDED_OPPORTUNITY_EVENT_ID";
        public const string AdjustInterstitialAdId = "FILL_WITH_ADJUST_INTERSTITIAL_AD_ID";
        public const string AdjustRewardedAdId = "FILL_WITH_ADJUST_REWARDED_AD_ID";


Please select and install the following ad networks from Applovin Integration Manager in Unity Editor: Admob, Facebook, Unity


AlictusSDK.cs requires the following functions to be integrated into your code to function properly:

Please update the following variables at the START of each level, Tag is the name of the level while Index refers to the order of the level in PlayerSettings:

	AlictusSDK.LevelTag = "LEVELTAG";
	AlictusSDK.LevelIndex = -1;



IMMEDIATELY after obtaining consent from user please notify the SDK about the status of the consent with the following function:
	
	AlictusSDK.SetConsentStatus(bool consentStatus)



After the completion of the level and before loading the next level please call the following function:
	
	AlictusSDK.LevelComplete(int levelDuration, bool isSuccess)



BEFORE showing an interstitial ad, please call:
	
	AlictusSDK.ShowingInterstitialAd()



BEFORE showing a rewarded ad, please call:
	
	AlictusSDK.ShowingRewardedAd()


That's all :) Please let us know if you encounter any bugs or problems with the SDK.