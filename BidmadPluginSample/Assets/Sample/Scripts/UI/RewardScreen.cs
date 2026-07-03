using UnityEngine.SceneManagement;

namespace SampleUI
{
    /// <summary>Rewarded-video sample screen.</summary>
    public class RewardScreen : SampleScreenBase
    {
        protected override string UxmlName => "RewardScreen";

        private RewardAdSample _ad;

        protected override void Build()
        {
            _ad = gameObject.AddComponent<RewardAdSample>();
            _ad.OnStatus += ReportAdStatus;

            Bind("btn-load", () =>
            {
                SetStatus("Requesting rewarded ad…");
                _ad.LoadRewardAd();
            });

            Bind("btn-show", () => _ad.ShowRewardAd());

            Bind("btn-back", () => SceneManager.LoadScene("Main"));
        }
    }
}
