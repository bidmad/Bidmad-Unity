using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEvent : MonoBehaviour
{
    public void MoveBannerScene()
    {
        SceneManager.LoadScene("Banner");
    }

    public void MoveInterstitialScene()
    {
        SceneManager.LoadScene("Interstitial");
    }

    public void MoveRewardScene()
    {
        SceneManager.LoadScene("Reward");
    }

    public void MoveRewardInterstitialScene()
    {
        SceneManager.LoadScene("RewardInterstitial");
    }

    public void MoveMainScene()
    {
        SceneManager.LoadScene("Main");
    }
}
