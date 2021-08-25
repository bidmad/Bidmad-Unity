using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class GDPRPopupSample {
    static BidmadGoogleGDPR gdprGoogle;

    public void GDPRforGooglePopupStart() {
        gdprGoogle = new BidmadGoogleGDPR();
        gdprGoogle.setDebug("8FA3C9C7-32C2-49E5-9C75-3A4FFE392936", true);
        gdprGoogle.reset();

        gdprGoogle.setConsentInfoUpdateSuccessCallback(ConsentInfoUpdateSuccessCallback);
        gdprGoogle.setConsentInfoUpdateFailureCallback(ConsentInfoUpdateFailureCallback);
        gdprGoogle.setConsentFormLoadSuccessCallback(ConsentFormLoadSuccessCallback);
        gdprGoogle.setConsentFormLoadFailureCallback(ConsentFormLoadFailureCallback);
        gdprGoogle.setConsentFormDismissedCallback(ConsentFormDismissedCallback);

        gdprGoogle.requestConsentInfoUpdate();

    }

    private void ConsentInfoUpdateSuccessCallback() {
        Debug.Log(MethodBase.GetCurrentMethod().Name);
        if (gdprGoogle.isConsentFormAvailable()) {
            gdprGoogle.loadForm();
        }
    }

    private void ConsentInfoUpdateFailureCallback(string errorLog) {
        Debug.Log(MethodBase.GetCurrentMethod().Name + ": " + errorLog);
    }

    private void ConsentFormLoadSuccessCallback() {
        Debug.Log(MethodBase.GetCurrentMethod().Name);
        if (gdprGoogle.getConsentStatus() == 1) {
            gdprGoogle.showForm();
        }
    }

    private void ConsentFormLoadFailureCallback(string errorLog) {
        Debug.Log(MethodBase.GetCurrentMethod().Name + ": " + errorLog);
    }

    private void ConsentFormDismissedCallback(string errorLog) {
        Debug.Log(MethodBase.GetCurrentMethod().Name + ": " + errorLog);
    }
}