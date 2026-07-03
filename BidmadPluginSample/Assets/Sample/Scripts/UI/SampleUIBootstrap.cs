using UnityEngine;
using UnityEngine.SceneManagement;

namespace SampleUI
{
    /// <summary>
    /// Replaces the legacy uGUI canvas in each sample scene with a UI Toolkit
    /// panel. Runs automatically at startup and on every scene load, so the
    /// scenes themselves never need to be modified.
    /// </summary>
    public static class SampleUIBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Init()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            // The first scene is already loaded when this runs, so handle it directly.
            Apply(SceneManager.GetActiveScene());
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode) => Apply(scene);

        private static void Apply(Scene scene)
        {
            if (!scene.IsValid())
                return;

            // Skip if a sample screen is already present (avoids duplicates).
            if (Object.FindFirstObjectByType<SampleScreenBase>() != null)
                return;

            System.Type controller = scene.name switch
            {
                "Main"         => typeof(MainMenuScreen),
                "Banner"       => typeof(BannerScreen),
                "Interstitial" => typeof(InterstitialScreen),
                "Reward"       => typeof(RewardScreen),
                _              => null,
            };

            if (controller == null)
                return;

            RemoveLegacyCanvases();

            var go = new GameObject("SampleUI [" + scene.name + "]");
            go.AddComponent(controller);
        }

        /// <summary>Tear down the old uGUI UI so it does not draw underneath UI Toolkit.</summary>
        private static void RemoveLegacyCanvases()
        {
            var canvases = Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None);
            foreach (var canvas in canvases)
            {
                // Destroy root canvases; nested canvases are removed with their parent.
                // Destroy() is deferred to end-of-frame, so the array stays valid here.
                if (canvas != null && canvas.isRootCanvas)
                    Object.Destroy(canvas.gameObject);
            }
        }
    }
}
