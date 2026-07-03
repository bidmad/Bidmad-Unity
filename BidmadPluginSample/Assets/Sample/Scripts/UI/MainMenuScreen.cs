using UnityEngine.SceneManagement;

namespace SampleUI
{
    /// <summary>Main menu: navigates to each ad-format sample scene.</summary>
    public class MainMenuScreen : SampleScreenBase
    {
        protected override string UxmlName => "MainScreen";

        protected override void Build()
        {
            Bind("btn-banner", () => SceneManager.LoadScene("Banner"));
            Bind("btn-interstitial", () => SceneManager.LoadScene("Interstitial"));
            Bind("btn-reward", () => SceneManager.LoadScene("Reward"));
        }
    }
}
