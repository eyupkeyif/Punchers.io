using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Firebase.Analytics;
using System;

public class GAEvent
{
    public static void LogEvent(string eventType = "Unidentified Event Type", List<Parameter> additionalParamList = null)
    {

        List<Parameter> paramList = new List<Parameter>();

        //string advertisingTracking;
        //string m_session_id;
        string internet_connection = "Unidentified Connection";
        string platform = "Unidentified Platform";

        //if (UnityEngine.iOS.Device.advertisingTrackingEnabled == true)
        //{
        //    advertisingTracking = "true";
        //}
        //else
        //{
        //    advertisingTracking = "false";
        //};
        //advertisingTracking = "true";
        if (UnityEngine.Application.internetReachability == NetworkReachability.NotReachable)
        {
            internet_connection = "offline";
        }
        else if (UnityEngine.Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            internet_connection = "wwan";
        }
        else if (UnityEngine.Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            internet_connection = "wifi";
        };

        if (UnityEngine.Application.platform == RuntimePlatform.Android)
        {
            platform = "android";
        }
        else if (UnityEngine.Application.platform == RuntimePlatform.IPhonePlayer)
        {
            platform = "ios";
        }
        else if (UnityEngine.Application.platform == RuntimePlatform.OSXEditor)
        {
            platform = "macOSUnity";
        }
        paramList.AddRange(new List<Parameter>{
                    new Parameter("platform",platform), // ios or android  - ok
                    new Parameter("device_model",UnityEngine.SystemInfo.deviceModel.ToString()), // device_model  - ok
                    new Parameter("os_version",UnityEngine.SystemInfo.operatingSystem.ToString()), // device_model  - ok
                    new Parameter("bundle_id",UnityEngine.Application.identifier.ToString()), // bundle id - ok
                    new Parameter("engine_version",UnityEngine.Application.unityVersion.ToString()), // ok
                    new Parameter("connection_type",internet_connection), // if else yazılacak wireless,data network, offline
                    new Parameter("build_no",UnityEngine.Application.version), // ok
                    new Parameter("local_time",System.DateTime.Now.ToString()), // ok
                    new Parameter("utc_time",System.DateTime.UtcNow.ToString()), // ok
                    new Parameter("device_id", SystemInfo.deviceUniqueIdentifier)
        });
        if (additionalParamList != null)
        {
            paramList.AddRange(additionalParamList);
        }
        FirebaseAnalytics.LogEvent(eventType, ParamList2Array(paramList));

    }

    private static Parameter[] ParamList2Array(List<Parameter> paramList)
    {
        return paramList.ToArray();
    }
}



