using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameobject : MonoBehaviour
{
    BidmadGoogleGDPR gGdpr;
    // Start is called before the first frame update
    void Start()
    {
        GameObject bidmadManager = new GameObject("BidmadManager");
        bidmadManager.AddComponent<BidmadManager>();
        DontDestroyOnLoad(bidmadManager);
        var obj = FindObjectsOfType<BidmadManager>();

        if (obj.Length == 1)
        {
            DontDestroyOnLoad(bidmadManager);
        }

        else
        {
            Destroy(bidmadManager);
        }

        gGdpr = new BidmadGoogleGDPR();
        gGdpr.reset();
        gGdpr.setConsentInfoUpdateSuccessCallback(onConsentInfoUpdateSuccess);
        gGdpr.setConsentInfoUpdateFailureCallback(onConsentInfoUpdateFailure);
        gGdpr.setConsentFormLoadSuccessCallback(onConsentFormLoadSuccess);
        gGdpr.setConsentFormLoadFailureCallback(onConsentFormLoadFailure);
        gGdpr.setConsentFormDismissedCallback(onConsentFormDismissed);

        gGdpr.setDebug("24CA94DDEB5A9979BF934BB443157007", true);
        gGdpr.requestConsentInfoUpdate();

        BidmadCommon.setGdprConsent(false, true);
        Debug.Log("getGdprConsent : " + BidmadCommon.getGdprConsent(true));
        Debug.Log("getGdprConsent : " + BidmadCommon.getGdprConsent(false));

        // BidmadCommon.setGgTestDeviceid("24CA94DDEB5A9979BF934BB443157007");

        #if UNITY_IOS
        BidmadCommon.reqAdTrackingAuthorization(adTrackingAuthCallback);
        #endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #if UNITY_IOS
    void adTrackingAuthCallback(BidmadTrackingAuthorizationStatus status)
    {
        if (status == BidmadTrackingAuthorizationStatus.BidmadAuthorizationStatusAuthorized)
        {
            Debug.Log("BidmadAuthorizationStatusAuthorized");
        }
        else if (status == BidmadTrackingAuthorizationStatus.BidmadAuthorizationStatusDenied)
        {
            Debug.Log("BidmadAuthorizationStatusDenied");
        }
        else if (status == BidmadTrackingAuthorizationStatus.BidmadAuthorizationStatusLessThaniOS14)
        {
            Debug.Log("BidmadAuthorizationStatusLessThaniOS14");
        }
    }
    #endif

    void onConsentInfoUpdateSuccess()
    {
        Debug.Log("onConsentInfoUpdateSuccess callback : " + gGdpr.isConsentFormAvailable());
        if(gGdpr.isConsentFormAvailable()){
            gGdpr.loadForm();
        }
    }

    void onConsentInfoUpdateFailure(string msg)
    {
        Debug.Log("onConsentInfoUpdateFailure callback : " + msg);
    }

    void onConsentFormLoadSuccess()
    {
        Debug.Log("onConsentFormLoadSuccess callback : " + gGdpr.getConsentStatus());
        if(gGdpr.getConsentStatus() == 1)
            gGdpr.showForm();
    }

    void onConsentFormLoadFailure(string msg)
    {
        Debug.Log("onConsentFormLoadFailure callback : " + msg);
    }

    void onConsentFormDismissed(string msg)
    {
        Debug.Log("onConsentFormDismissed callback : " + msg);
    }
}
