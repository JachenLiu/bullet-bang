using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BulletBang
{
    /// <summary>Self-contained entry UI for the generic shared social lobby.</summary>
    public sealed class RuntimeLobbyUI : MonoBehaviour
    {
        private TMP_InputField _name;
        private Button _enter;
        private TextMeshProUGUI _status;
        private GameObject _menu;
        private bool _busy;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Install()
        {
            if (FindFirstObjectByType<MainLobbyManager>() == null ||
                FindFirstObjectByType<RuntimeLobbyUI>() != null) return;
            var root = new GameObject("Generic Lobby UI", typeof(Canvas), typeof(CanvasScaler),
                typeof(GraphicRaycaster), typeof(RuntimeLobbyUI));
            var canvas = root.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
            var scaler = root.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            if (FindFirstObjectByType<EventSystem>() == null)
                new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }

        private void Awake()
        {
            Build();
            _enter.onClick.AddListener(EnterMainLobby);
        }

        private async void EnterMainLobby()
        {
            if (!Prepare("Entering the main lobby...")) return;
            Complete(await MainLobbyManager.Instance.ConnectMainLobby(), "Main lobby connected.");
        }

        private bool Prepare(string message)
        {
            if (_busy) return false;
            var value = _name.text.Trim();
            if (string.IsNullOrWhiteSpace(value))
            {
                _status.text = "Enter a player name.";
                return false;
            }
            LocalPlayerData.NickName = value;
            _busy = true;
            _enter.interactable = false;
            _status.text = message;
            return true;
        }

        private void Complete(bool success, string message)
        {
            _busy = false;
            _status.text = success ? message : "Could not reach the main lobby. Please try again.";
            _enter.interactable = !success;
            if (success) _menu.SetActive(false);
        }

        private void Build()
        {
            _menu = Panel("Lobby Menu", transform, new Color(0.04f, 0.025f, 0.02f, 0.96f)).gameObject;
            Stretch(_menu.GetComponent<RectTransform>());
            var card = Panel("Menu Card", _menu.transform, new Color(0.14f, 0.075f, 0.04f, 0.98f));
            SetRect(card.rectTransform, Vector2.zero, new Vector2(620, 560));
            Label(card.transform, "MAIN LOBBY", 52, new Vector2(0, 190), new Vector2(540, 70));
            Label(card.transform, "A shared space for social tables and future games", 18,
                new Vector2(0, 135), new Vector2(540, 40));
            _name = Input(card.transform, new Vector2(0, 55));
            _enter = Button(card.transform, "ENTER MAIN LOBBY", new Vector2(0, -70),
                new Color(0.25f, 0.42f, 0.24f));
            _status = Label(card.transform, "Enter a name to begin.", 17,
                new Vector2(0, -165), new Vector2(540, 42));
        }

        private static Image Panel(string name, Transform parent, Color color)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            var image = go.GetComponent<Image>();
            image.color = color;
            return image;
        }

        private static TextMeshProUGUI Label(Transform parent, string text, float size,
            Vector2 position, Vector2 dimensions)
        {
            var go = new GameObject("Label", typeof(RectTransform), typeof(TextMeshProUGUI));
            go.transform.SetParent(parent, false);
            SetRect(go.GetComponent<RectTransform>(), position, dimensions);
            var label = go.GetComponent<TextMeshProUGUI>();
            label.text = text;
            label.fontSize = size;
            label.alignment = TextAlignmentOptions.Center;
            label.color = new Color(1f, 0.82f, 0.55f);
            return label;
        }

        private static TMP_InputField Input(Transform parent, Vector2 position)
        {
            var image = Panel("Player Name", parent, new Color(0.025f, 0.018f, 0.015f, 1));
            SetRect(image.rectTransform, position, new Vector2(480, 60));
            var input = image.gameObject.AddComponent<TMP_InputField>();
            input.textComponent = Label(image.transform, "", 25, Vector2.zero, new Vector2(440, 52));
            input.textComponent.alignment = TextAlignmentOptions.MidlineLeft;
            input.placeholder = Label(image.transform, "Player name...", 23, Vector2.zero, new Vector2(440, 52));
            return input;
        }

        private static Button Button(Transform parent, string text, Vector2 position, Color color)
        {
            var image = Panel(text, parent, color);
            SetRect(image.rectTransform, position, new Vector2(480, 62));
            var button = image.gameObject.AddComponent<Button>();
            Label(image.transform, text, 23, Vector2.zero, new Vector2(450, 55));
            return button;
        }

        private static void SetRect(RectTransform rect, Vector2 position, Vector2 size)
        {
            rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
        }

        private static void Stretch(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = rect.offsetMax = Vector2.zero;
        }
    }
}
