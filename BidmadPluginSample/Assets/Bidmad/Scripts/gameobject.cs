using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameobject : MonoBehaviour
{
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
}
