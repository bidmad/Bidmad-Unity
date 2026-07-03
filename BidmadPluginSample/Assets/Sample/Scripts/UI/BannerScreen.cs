using UnityEngine.SceneManagement;

namespace SampleUI
{
    /// <summary>Banner sample screen.</summary>
    public class BannerScreen : SampleScreenBase
    {
        protected override string UxmlName => "BannerScreen";

        private BannerAdSample _ad;

        protected override void Build()
        {
            _ad = gameObject.AddComponent<BannerAdSample>();
            _ad.OnStatus += ReportAdStatus;

            Bind("btn-load", () =>
            {
                SetStatus("Requesting banner…");
                _ad.LoadBannerAd();
            });

            // Preserve the original behaviour: leaving the screen removes the banner.
            Bind("btn-back", () =>
            {
                _ad.removeBanner();
                SceneManager.LoadScene("Main");
            });
        }
    }
}
