using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardInterstitialAdPopupSample : MonoBehaviour
{
    [SerializeField] 
    public Text txtMsg; 
    [SerializeField]
    public Text txtButtonYes; 
    [SerializeField] 
    public Text txtButtonNo;

    public delegate void CallBack(); 
    private event CallBack positiveCallBack; 
    private event CallBack nagativeCallBack;

    public string mTimer = @"00"; 
    public float mTotalSeconds = 5;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("RewardInterstitialAdPopupSample Start");
        mTimer = CountdownTimer(false);
    }

    void Update(){
        mTimer = CountdownTimer(); 

        if (mTotalSeconds <= 0) 
        { 
            SetZero(); 
        } 

        txtMsg.text = "seconds remaining : " + mTimer;
    }

    private string CountdownTimer(bool IsUpdate = true) 
    { 
        if(IsUpdate) 
            mTotalSeconds -= Time.deltaTime; 

        TimeSpan timespan = TimeSpan.FromSeconds(mTotalSeconds); 
        string timer = string.Format("{0:00}",timespan.Seconds); 

        return timer; 
    } 

    private void SetZero() 
    { 
        mTimer = @"00"; 
        mTotalSeconds = 0; 
        OnPositiveEvent();
    } 

    public void SetPositiveCallBack(CallBack listener) 
    { 
        Debug.Log("SetPositiveCallBack");
        positiveCallBack += listener; 
    }
    public void SetNagativeCallBack(CallBack listener) 
    { 
        Debug.Log("SetNagativeCallBack");
        nagativeCallBack += listener; 
    } 
    public void OnPositiveEvent() 
    { 
        Debug.Log("OnPositiveEvent");
        positiveCallBack?.Invoke(); 
    } 
    public void OnNagativeEvent() 
    { 
        Debug.Log("OnNagativeEvent");
        nagativeCallBack?.Invoke(); 
    }
}
