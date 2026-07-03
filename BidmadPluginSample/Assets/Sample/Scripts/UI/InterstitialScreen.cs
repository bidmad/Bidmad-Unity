using UnityEngine.SceneManagement;

namespace SampleUI
{
    /// <summary>Interstitial sample screen.</summary>
    public class InterstitialScreen : SampleScreenBase
    {
        protected override string UxmlName => "InterstitialScreen";

        private InterstitialAdSample _ad;

        protected override void Build()
        {
            _ad = gameObject.AddComponent<InterstitialAdSample>();
            _ad.OnStatus += ReportAdStatus;

            Bind("btn-load", () =>
            {
                SetStatus("Requesting interstitial…");
                _ad.LoadInterstitialAd();
            });

            Bind("btn-show", () => _ad.ShowInterstitialAd());

            Bind("btn-back", () => SceneManager.LoadScene("Main"));
        }
    }
}
