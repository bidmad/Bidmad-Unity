using UnityEngine;
using UnityEngine.UIElements;

namespace SampleUI
{
    /// <summary>
    /// Base class for every sample screen. Builds a UI Toolkit panel
    /// (UIDocument + PanelSettings) entirely from code so no scene assets or
    /// serialized references are required. Subclasses only declare which UXML
    /// to load and how to wire its buttons.
    /// </summary>
    public abstract class SampleScreenBase : MonoBehaviour
    {
        // UXML / USS / theme all live under a Resources folder and are loaded by path.
        private const string ResourceRoot = "SampleUI/";

        protected VisualElement Root { get; private set; }

        private Label _statusLabel;
        private VisualElement _statusDot;

        /// <summary>UXML resource name (without folder or extension), e.g. "MainScreen".</summary>
        protected abstract string UxmlName { get; }

        protected virtual void Awake()
        {
            var tree = Resources.Load<VisualTreeAsset>(ResourceRoot + UxmlName);
            if (tree == null)
            {
                Debug.LogError($"[SampleUI] Missing UXML resource: {ResourceRoot + UxmlName}");
                return;
            }

            var panel = ScriptableObject.CreateInstance<PanelSettings>();
            panel.name = "SamplePanelSettings";
            panel.themeStyleSheet = Resources.Load<ThemeStyleSheet>(ResourceRoot + "SampleTheme");
            // Design against a dp-like reference so px values map to natural mobile
            // sizes; match width (0) keeps sizing consistent across portrait aspects.
            panel.scaleMode = PanelScaleMode.ScaleWithScreenSize;
            panel.screenMatchMode = PanelScreenMatchMode.MatchWidthOrHeight;
            panel.referenceResolution = new Vector2Int(390, 844);
            panel.match = 0f;

            var doc = gameObject.AddComponent<UIDocument>();
            doc.panelSettings = panel;
            doc.visualTreeAsset = tree;

            Root = doc.rootVisualElement;
            if (Root == null)
            {
                Debug.LogError("[SampleUI] UIDocument produced no root visual element.");
                return;
            }

            // Guard against timing differences: ensure the tree is actually cloned.
            if (Root.childCount == 0)
                tree.CloneTree(Root);

            Root.style.flexGrow = 1;

            var styles = Resources.Load<StyleSheet>(ResourceRoot + "SampleStyles");
            if (styles != null)
                Root.styleSheets.Add(styles);

            // Assign a font explicitly so text renders regardless of theme resolution.
            var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            if (font == null)
                font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            if (font != null)
                Root.style.unityFont = font;

            _statusLabel = Root.Q<Label>("status");
            _statusDot = Root.Q<VisualElement>("status-dot");

            Build();
        }

        /// <summary>Wire buttons and subscribe to ad callbacks. Root is ready here.</summary>
        protected abstract void Build();

        protected enum StatusKind { Info, Success, Error }

        protected void SetStatus(string message, StatusKind kind = StatusKind.Info)
        {
            if (_statusLabel != null)
            {
                _statusLabel.text = message;
                _statusLabel.RemoveFromClassList("status--info");
                _statusLabel.RemoveFromClassList("status--success");
                _statusLabel.RemoveFromClassList("status--error");
                _statusLabel.AddToClassList("status--" + Suffix(kind));
            }

            if (_statusDot != null)
            {
                _statusDot.RemoveFromClassList("dot--info");
                _statusDot.RemoveFromClassList("dot--success");
                _statusDot.RemoveFromClassList("dot--error");
                _statusDot.AddToClassList("dot--" + Suffix(kind));
            }
        }

        /// <summary>Route an ad SDK message to the status panel, colouring by content.</summary>
        protected void ReportAdStatus(string message)
        {
            var lower = message.ToLowerInvariant();
            StatusKind kind;
            if (lower.Contains("fail") || lower.Contains("error"))
                kind = StatusKind.Error;
            else if (lower.Contains("loaded") || lower.Contains("shown") ||
                     lower.Contains("earned") || lower.Contains("clicked"))
                kind = StatusKind.Success;
            else
                kind = StatusKind.Info;

            SetStatus(message, kind);
        }

        private static string Suffix(StatusKind kind)
        {
            switch (kind)
            {
                case StatusKind.Success: return "success";
                case StatusKind.Error: return "error";
                default: return "info";
            }
        }

        /// <summary>Query a Button by name and attach a click handler if it exists.</summary>
        protected Button Bind(string buttonName, System.Action onClick)
        {
            var button = Root?.Q<Button>(buttonName);
            if (button == null)
            {
                Debug.LogWarning($"[SampleUI] Button '{buttonName}' not found in {UxmlName}.");
                return null;
            }
            button.clicked += onClick;
            return button;
        }
    }
}
