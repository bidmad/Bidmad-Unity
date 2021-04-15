using UnityEngine;
using System.Collections.Generic;
using System;

//Map을 사용하여 ZoneId를 기준으로 유니크하게 관리
//Callback 수신 시 zoneId(msg)로 get, 있으면 콜백
public class BidmadManager : MonoBehaviour
{
/*** Banner Dictionnary ***/
    public static Dictionary<string, Action> dicBannerLoad = new Dictionary<string, Action>();
    public static Dictionary<string, Action> dicBannerFail = new Dictionary<string, Action>();
    public static Dictionary<string, Action> dicBannerClick = new Dictionary<string, Action>();
/*** Interstitial Dictionnary ***/
    public static Dictionary<string, Action> dicInterstitialLoad = new Dictionary<string, Action>();
    public static Dictionary<string, Action> dicInterstitialShow = new Dictionary<string, Action>();
    public static Dictionary<string, Action> dicInterstitialFail = new Dictionary<string, Action>();
    public static Dictionary<string, Action> dicInterstitialClose = new Dictionary<string, Action>();
/*** Reward Dictionnary ***/
    public static Dictionary<string, Action> dicRewardLoad = new Dictionary<string, Action>();
    public static Dictionary<string, Action> dicRewardShow = new Dictionary<string, Action>();
    public static Dictionary<string, Action> dicRewardFail = new Dictionary<string, Action>();
    public static Dictionary<string, Action> dicRewardComplete = new Dictionary<string, Action>();
    public static Dictionary<string, Action> dicRewardSkip = new Dictionary<string, Action>();
    public static Dictionary<string, Action> dicRewardClose = new Dictionary<string, Action>();
/*** Common Callback ***/
    public static Action<BidmadTrackingAuthorizationStatus> adTrackingAuthResponse;
/*** googleGdpr Callback ***/
    public static Action consentInfoUpdateSuccess;
    public static Action<string> consentInfoUpdateFailure;
    public static Action consentFormLoadSuccess;
    public static Action<string> consentFormLoadFailure;
    public static Action<string> consentFormDismissed;

/*** Banner Callback ***/
    void OnBannerLoad(string zoneId)
    {
        Debug.Log("OnBannerLoaded");
        if(dicBannerLoad.ContainsKey(zoneId)){
            Action onBannerLoad = dicBannerLoad[zoneId];
            onBannerLoad();
        }
    }

    void OnBannerFail(string zoneId)
    {
        Debug.Log("OnBannerFailed");
        if(dicBannerFail.ContainsKey(zoneId)){
            Action onBannerFail = dicBannerFail[zoneId];
            onBannerFail();
        }

    }

    void OnBannerClick(string zoneId)
    {
        Debug.Log("OnBannerClicked");
        if(dicBannerClick.ContainsKey(zoneId)){
            Action onBannerClick = dicBannerClick[zoneId];
            onBannerClick();
        }
    }
/*** Banner Callback ***/
/*** Interstitial Callback ***/
    void OnInterstitialLoad(string zoneId)
    {
        Debug.Log("OnInterstitialLoad");
        if(dicInterstitialLoad.ContainsKey(zoneId)){
            Action onInterstitialLoad = dicInterstitialLoad[zoneId];
            onInterstitialLoad();
        }
    }

    void OnInterstitialShow(string zoneId)
    {
        Debug.Log("OnInterstitialShow");
        if(dicInterstitialShow.ContainsKey(zoneId)){
            Action onInterstitialShow = dicInterstitialShow[zoneId];
            onInterstitialShow();

            BidmadInterstitial interstitial = new BidmadInterstitial(zoneId);
            interstitial.load();
        }
    }
    void OnInterstitialFail(string zoneId)
    {
        Debug.Log("OnInterstitialLoadFail");
        if(dicInterstitialFail.ContainsKey(zoneId)){
            Action onInterstitialFail = dicInterstitialFail[zoneId];
            onInterstitialFail();
        }
       
    }
    void OnInterstitialClose(string zoneId)
    {
        Debug.Log("OnInterstitialClose");
        if(dicInterstitialClose.ContainsKey(zoneId)){
            Action onInterstitialClose = dicInterstitialClose[zoneId];
            onInterstitialClose();
        }
    }
/*** Interstitial Callback ***/
/*** Reward Callback ***/
    void OnRewardLoad(string zoneId) 
    {
        Debug.Log("OnRewardLoad");
        if(dicRewardLoad.ContainsKey(zoneId)){
            Action onRewardLoad = dicRewardLoad[zoneId];
            onRewardLoad();
        }
    }

    void OnRewardShow(string zoneId)
    {
        Debug.Log("OnRewardShow");
        if(dicRewardShow.ContainsKey(zoneId)){
            Action onRewardShow = dicRewardShow[zoneId];
            onRewardShow();

            BidmadReward reward = new BidmadReward(zoneId);
            reward.load();
        }
    }

    void OnRewardFail(string zoneId)
    {
        Debug.Log("OnRewardFail");
        if(dicRewardFail.ContainsKey(zoneId)){
            Action onRewardFail = dicRewardFail[zoneId];
            onRewardFail();
        }
    }

    void OnRewardComplete(string zoneId)
    {
        Debug.Log("OnRewardComplete");
        if(dicRewardComplete.ContainsKey(zoneId)){
            Action onRewardComplete = dicRewardComplete[zoneId];
            onRewardComplete();
        }
    }

    void OnRewardSkip(string zoneId)
    {
        Debug.Log("OnRewardSkip");
        if(dicRewardSkip.ContainsKey(zoneId)){
            Action onRewardSkip = dicRewardSkip[zoneId];
            onRewardSkip();
        }
    }

    void OnRewardClose(string zoneId)
    {
        Debug.Log("OnRewardClose");
        if(dicRewardClose.ContainsKey(zoneId)){
            Action onRewardClose = dicRewardClose[zoneId];
            onRewardClose();
        }
    }
 /*** Reward Callback ***/
 /*** Common Callback ***/
    void OnAdTrackingAuthorizationResponse(string responseCode)
    {
        Debug.Log("OnAdTrackingAuthorizationResponse");
        if (adTrackingAuthResponse != null)
        {
            switch (responseCode)
            {
                case "0":
                    adTrackingAuthResponse(BidmadTrackingAuthorizationStatus.BidmadAuthorizationStatusNotDetermined);
                    break;
                case "1":
                    adTrackingAuthResponse(BidmadTrackingAuthorizationStatus.BidmadAuthorizationStatusRestricted);
                    break;
                case "2":
                    adTrackingAuthResponse(BidmadTrackingAuthorizationStatus.BidmadAuthorizationStatusDenied);
                    break;
                case "3":
                    adTrackingAuthResponse(BidmadTrackingAuthorizationStatus.BidmadAuthorizationStatusAuthorized);
                    break;
                case "4":
                    adTrackingAuthResponse(BidmadTrackingAuthorizationStatus.BidmadAuthorizationStatusLessThaniOS14);
                    break;
                default:
                    adTrackingAuthResponse(BidmadTrackingAuthorizationStatus.BidmadAuthorizationStatusDenied);
                    break;
            }
        }
    }
/*** Common Callback ***/

/*** googleGdpr Callback ***/
    void onConsentInfoUpdateSuccess()
    {
        Debug.Log("onConsentInfoUpdateSuccess");
        if (consentInfoUpdateSuccess != null)
        {
           consentInfoUpdateSuccess();
        }
    }

    void onConsentInfoUpdateFailure(string msg)
    {
        Debug.Log("onConsentInfoUpdateFailure");
        if (consentInfoUpdateFailure != null)
        {
            consentInfoUpdateFailure(msg);  
        }
    }

    void onConsentFormLoadSuccess()
    {
        Debug.Log("onConsentFormLoadSuccess");
        if (consentFormLoadSuccess != null)
        {
            consentFormLoadSuccess();  
        }
    }

    void onConsentFormLoadFailure(string msg)
    {
        Debug.Log("onConsentFormLoadFailure");
        if (consentFormLoadFailure != null)
        {
           consentFormLoadFailure(msg);
        }
    }

    void onConsentFormDismissed(string msg)
    {
        Debug.Log("onConsentFormDismissed");
        if (consentFormDismissed != null)
        {
           consentFormDismissed(msg);
        }
    }
/*** googleGdpr Callback ***/
}
