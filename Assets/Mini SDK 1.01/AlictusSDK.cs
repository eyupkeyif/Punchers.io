using System.Globalization;
using com.adjust.sdk;
using Firebase.Analytics;
using UnityEditor;
using UnityEngine;

namespace com.alictus.sdklite
{
    public class AlictusSDK : MonoBehaviour
    {
        public static string LevelTag = "LEVELTAG";
        public static int LevelIndex = -1;
        


#if !UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            if (FindObjectOfType<AlictusSDK>() != null)
            {
                Debug.LogError("AlictusSDK detected! Please remove all instances of the script from the scene");
                return;
            }
            var go = new GameObject("AlictusSDK", typeof(AlictusSDK));
            DontDestroyOnLoad(go);
        }
#endif
        
        
        void Awake()
        {
            MaxSdkCallbacks.OnSdkInitializedEvent += OnSDKInitialized;
            
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += TrackRevenue;
            
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialAdLoaded;
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += TrackRevenue;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;
            
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoaded;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += TrackRevenue;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        }

        public static void SetConsentStatus(bool userHasGivenConsent)
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(userHasGivenConsent);
            Adjust.trackMeasurementConsent(userHasGivenConsent);
        }

        public static void LevelComplete(int levelDuration, bool isSuccess)
        {
            GameAnalyticsSDK.GAProgressionStatus progressionStatus;
            if (isSuccess)
                progressionStatus = GameAnalyticsSDK.GAProgressionStatus.Complete;
            else
                progressionStatus = GameAnalyticsSDK.GAProgressionStatus.Fail;
            
            GameAnalyticsSDK.GameAnalytics.NewProgressionEvent(progressionStatus, LevelTag);
            
            Parameter[] firebaseParameters = new Parameter[4];
            firebaseParameters[0] = new Parameter("LevelIndex", LevelIndex);
            firebaseParameters[1] = new Parameter("LevelTag", LevelTag);
            firebaseParameters[2] = new Parameter("LevelDuration", levelDuration);
            firebaseParameters[3] = new Parameter("IsSuccess", isSuccess.ToString());
            FirebaseAnalytics.LogEvent("GD_LEVEL_COMPLETE", firebaseParameters);
        }
        
        //Please call this function BEFORE showing a rewarded ad
        public static void ShowingRewardedAd()
        {
            SendFirebaseAdEvent("AD_INT_START_Applovin", LevelIndex, LevelTag);
            SendAdjustEvent(AlictusKey.AdjustRewardedAdId, _loadedRewardedInfo.ADId, _loadedRewardedInfo.ADInfo, "showed");
        }
        
        //Please call this function BEFORE showing an interstitial ad
        public static void ShowingIntersititaldAd()
        {
            SendFirebaseAdEvent("AD_INT_START_Applovin", LevelIndex, LevelTag);
            SendAdjustEvent(AlictusKey.AdjustInterstitialAdId, _loadedInterstitialInfo.ADId, _loadedInterstitialInfo.ADInfo, "showed");
        }

        #region Internal

        private const string AdjustAppToken = AlictusKey.AdjustAppToken;

        private static FillData _loadedInterstitialInfo;
        private static FillData _loadedRewardedInfo;
        
        private class FillData
        {
            public MaxSdkBase.AdInfo ADInfo;
            public string ADId;
        }
        
        private void OnSDKInitialized(MaxSdkBase.SdkConfiguration sdkConfiguration)
        {
            if (sdkConfiguration.IsSuccessfullyInitialized)
            {
                InitializeAdjust();
            }
        }
        
        private static void OnRewardedAdLoaded(string adId, MaxSdkBase.AdInfo info)
        {
            _loadedRewardedInfo = new FillData(){ADId = adId, ADInfo = info};
            SendAdjustEvent(AlictusKey.AdjustRewardedOpportunityEventId, adId, info);
        }
        
        private static void OnInterstitialAdLoaded(string adId, MaxSdkBase.AdInfo info)
        {
            _loadedInterstitialInfo = new FillData(){ADId = adId, ADInfo = info};
            SendAdjustEvent(AlictusKey.AdjustInterstitialOpportunityEventId, adId, info);
        }
        
        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            SendFirebaseAdErrorEvent("AD_REW_SHOW_ERROR", errorInfo);
            SendAdjustErrorEvent(AlictusKey.AdjustRewardedAdId, errorInfo, adInfo);
        }
        
        private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            SendFirebaseAdErrorEvent("AD_INT_SHOW_ERROR", errorInfo);
            SendAdjustErrorEvent(AlictusKey.AdjustInterstitialAdId, errorInfo, adInfo);
        }

        private static void SendFirebaseAdEvent(string label, int levelIndex, string levelTag)
        {
            Parameter[] firebaseParameters = new Parameter[2];
            firebaseParameters[0] = new Parameter("levelIndex", levelIndex);
            firebaseParameters[1] = new Parameter("levelTag", levelTag);
            FirebaseAnalytics.LogEvent(label, firebaseParameters);
        }
        
        private static void SendFirebaseAdErrorEvent(string label, MaxSdkBase.ErrorInfo errorInfo)
        {
            Parameter[] firebaseParameters = new Parameter[1];
            firebaseParameters[0] = new Parameter("Code", errorInfo.Code.ToString());
            FirebaseAnalytics.LogEvent(label, firebaseParameters);
        }

        private static void InitializeAdjust()
        {
            AdjustConfig config = null;
            GameObject go = new GameObject("Adjust", typeof(Adjust));
            DontDestroyOnLoad(go);
            AdjustAttribution attribution = null;

            AdjustLogLevel logLevel = AdjustLogLevel.Suppress;
            AdjustEnvironment environment = AdjustEnvironment.Production;
            config = new AdjustConfig(AdjustAppToken, environment);

            //config.setAttributionChangedDelegate(OnAttributionChanged);
            config.setLogLevel(logLevel);

            Adjust.start(config);
        }

        

        private static void TrackRevenue(string adId, MaxSdkBase.AdInfo info)
        {
            var adRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAppLovinMAX);
            adRevenue.setRevenue(info.Revenue, "USD");
            adRevenue.setAdRevenueNetwork(info.NetworkName);
            adRevenue.setAdRevenueUnit(info.AdUnitIdentifier);
            adRevenue.setAdRevenuePlacement(info.Placement);
            adRevenue.addCallbackParameter("LevelIndex", LevelIndex.ToString());
            adRevenue.addCallbackParameter("LevelTag", LevelTag);
            Adjust.trackAdRevenue(adRevenue);
            
            SendFirebaseRoasEvent(adId, info);
        }

        private static void SendFirebaseRoasEvent(string adUnitId, MaxSdkBase.AdInfo info)
        {
            Parameter[] parameters =
            {
                new Parameter(FirebaseAnalytics.ParameterAdPlatform, "AppLovin"),
                new Parameter(FirebaseAnalytics.ParameterAdSource, info.NetworkName),
                new Parameter(FirebaseAnalytics.ParameterAdUnitName, adUnitId),
                new Parameter(FirebaseAnalytics.ParameterAdFormat, info.AdFormat),
                new Parameter(FirebaseAnalytics.ParameterValue, info.Revenue),
                new Parameter(FirebaseAnalytics.ParameterCurrency, "USD"),
            };
        
            FirebaseAnalytics.LogEvent("ad_impression", parameters);
            FirebaseAnalytics.LogEvent("custom_ad_impression", parameters);
        }

        private static void SendAdjustEvent(string eventName, string adId, MaxSdkBase.AdInfo adInfo, string status)
        {
            MaxSdkBase.NetworkResponseInfo networkResponseInfo =
                adInfo.WaterfallInfo.NetworkResponses.Find(x => x.MediatedNetwork.Name.Equals(adInfo.NetworkName));
            
            AdjustEvent adjustEvent = new AdjustEvent (eventName);
            if(string.IsNullOrEmpty(status)) adjustEvent.addCallbackParameter("status", status);
            adjustEvent.addCallbackParameter("LevelIndex", LevelIndex.ToString());
            adjustEvent.addCallbackParameter("LevelTag", LevelTag);
            adjustEvent.addCallbackParameter("med", "applovin");
            adjustEvent.addCallbackParameter("adUnitId", adId);
            adjustEvent.addCallbackParameter("item", "na");
            adjustEvent.addCallbackParameter("netPlace", adInfo.NetworkPlacement);
            adjustEvent.addCallbackParameter("WfName", adInfo.WaterfallInfo.Name);
            adjustEvent.addCallbackParameter("WfTestName", adInfo.WaterfallInfo.TestName);
            adjustEvent.addCallbackParameter("networkName", adInfo.NetworkName.ToLower(CultureInfo.InvariantCulture));
            adjustEvent.addCallbackParameter("respT", adInfo.WaterfallInfo.LatencyMillis.ToString("0.00", CultureInfo.InvariantCulture));
            adjustEvent.addCallbackParameter("netRespT", (networkResponseInfo == null ? -1 : (float)networkResponseInfo.LatencyMillis).ToString("0.00", CultureInfo.InvariantCulture));
            adjustEvent.addCallbackParameter("precType", adInfo.RevenuePrecision.ToString(CultureInfo.InvariantCulture));
            adjustEvent.addCallbackParameter("rev", adInfo.Revenue.ToString(CultureInfo.InvariantCulture));
            /*
            adjustEvent.addCallbackParameter("conValue", conversionValue.ToString(CultureInfo.InvariantCulture));
            adjustEvent.addCallbackParameter("EstValue", estValue.ToString(CultureInfo.InvariantCulture));
            adjustEvent.addCallbackParameter("Undisclosed", undisclosed.ToString(CultureInfo.InvariantCulture));
            adjustEvent.addCallbackParameter("FormatIssue", formatIssue.ToString(CultureInfo.InvariantCulture));
            adjustEvent.addCallbackParameter("ShowOrder", showCount.ToString(CultureInfo.InvariantCulture));
            */
        }

        private static void SendAdjustEvent(string eventName, string adId, MaxSdkBase.AdInfo adInfo)
        {
            SendAdjustEvent(eventName, adId, adInfo, null);
        }

        private void SendAdjustErrorEvent(string eventName, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            AdjustEvent adjustEvent = new AdjustEvent (eventName);
            adjustEvent.addCallbackParameter("code", errorInfo.Code.ToString());
            Adjust.trackEvent(adjustEvent);
        }
        #endregion
    }
}