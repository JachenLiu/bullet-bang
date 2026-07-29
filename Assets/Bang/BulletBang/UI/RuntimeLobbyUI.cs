using System.Linq;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BulletBang
{
    /// <summary>
    /// Wires the prototype scene's existing menu at runtime. This prevents a
    /// missing inspector reference from making the only entry path unusable.
    /// </summary>
    public sealed class RuntimeLobbyUI : MonoBehaviour
    {
        private Button _host;
        private Button _join;
        private TMP_InputField _name;
        private GameObject _menu;
        private TextMeshProUGUI _status;
        private bool _busy;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Install()
        {
            if (SceneManager.GetActiveScene().name != "MainLobby") return;
            var canvas = FindFirstObjectByType<Canvas>();
            if (canvas == null)
            {
                var canvasObject = new GameObject("Main Menu Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
                canvas = canvasObject.GetComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = 100;
                var scaler = canvasObject.GetComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920, 1080);
                scaler.matchWidthOrHeight = 0.5f;
            }
            if (FindFirstObjectByType<EventSystem>() == null)
                new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            if (canvas.GetComponent<RuntimeLobbyUI>() == null)
                canvas.gameObject.AddComponent<RuntimeLobbyUI>();
        }

        private void Awake()
        {
            _host = FindButton("HostGameButton");
            _join = FindButton("JoinGameButton");
            _name = FindFirstObjectByType<TMP_InputField>();
            _menu = GameObject.Find("MainMenuPanel");
            if (_host == null || _join == null || _name == null || _menu == null)
                BuildMenu();
            _status = CreateStatus();

            if (_host != null) _host.onClick.AddListener(Host);
            if (_join != null) _join.onClick.AddListener(Join);
            SetStatus("Enter a name, then host or join the shared saloon.");
        }

        private void OnDestroy()
        {
            if (_host != null) _host.onClick.RemoveListener(Host);
            if (_join != null) _join.onClick.RemoveListener(Join);
        }

        private async void Host()
        {
            if (!Prepare("Creating saloon...")) return;
            var success = await MainLobbyManager.Instance.StartLobbyHost();
            Complete(success, "Saloon hosted.", "Could not host the saloon.");
        }

        private async void Join()
        {
            if (!Prepare("Joining saloon...")) return;
            var success = await MainLobbyManager.Instance.JoinLobby();
            Complete(success, "Joined the saloon.", "Could not join the saloon.");
        }

        private bool Prepare(string message)
        {
            if (_busy) return false;
            if (MainLobbyManager.Instance == null)
            {
                SetStatus("MainLobbyManager is missing.");
                return false;
            }
            var value = _name != null ? _name.text.Trim() : string.Empty;
            if (string.IsNullOrWhiteSpace(value))
            {
                SetStatus("Enter a player name first.");
                return false;
            }
            LocalPlayerData.NickName = value;
            _busy = true;
            SetButtons(false);
            SetStatus(message);
            return true;
        }

        private void Complete(bool success, string successMessage, string failureMessage)
        {
            _busy = false;
            SetButtons(!success);
            SetStatus(success ? successMessage : failureMessage);
            if (success && _menu != null) _menu.SetActive(false);
        }

        private void SetButtons(bool interactable)
        {
            if (_host != null) _host.interactable = interactable;
            if (_join != null) _join.interactable = interactable;
        }

        private void SetStatus(string message)
        {
            if (_status != null) _status.text = message;
        }

        private static Button FindButton(string name) =>
            FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .FirstOrDefault(button => button.name == name);

        private TextMeshProUGUI CreateStatus()
        {
            var go = new GameObject("Lobby Status", typeof(RectTransform), typeof(TextMeshProUGUI));
            go.transform.SetParent(transform, false);
            var rect = go.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0);
            rect.anchorMax = new Vector2(0.5f, 0);
            rect.pivot = new Vector2(0.5f, 0);
            rect.anchoredPosition = new Vector2(0, 24);
            rect.sizeDelta = new Vector2(720, 44);
            var text = go.GetComponent<TextMeshProUGUI>();
            text.alignment = TextAlignmentOptions.Center;
            text.fontSize = 22;
            text.color = new Color(1f, 0.82f, 0.52f);
            return text;
        }

        private void BuildMenu()
        {
            var veil = Panel("Menu Backdrop", transform, new Color(0.025f, 0.012f, 0.008f, 0.72f));
            Stretch(veil.rectTransform);

            var panel = Panel("MainMenuPanel", veil.transform, new Color(0.12f, 0.045f, 0.018f, 0.97f));
            var panelRect = panel.rectTransform;
            panelRect.anchorMin = panelRect.anchorMax = new Vector2(0.5f, 0.5f);
            panelRect.sizeDelta = new Vector2(650, 680);
            panelRect.anchoredPosition = Vector2.zero;
            var outline = panel.gameObject.AddComponent<Outline>();
            outline.effectColor = new Color(0.78f, 0.43f, 0.13f, 0.9f);
            outline.effectDistance = new Vector2(4, -4);

            Label("Brand", panel.transform, "BULLET BANG", 58, new Vector2(0, 238),
                new Vector2(570, 80), new Color(1f, 0.73f, 0.32f), FontStyles.Bold);
            Label("Tagline", panel.transform, "THE FRONTIER CARD SALOON", 19, new Vector2(0, 190),
                new Vector2(570, 36), new Color(0.84f, 0.62f, 0.39f), FontStyles.SmallCaps);
            var divider = Panel("Brass Divider", panel.transform, new Color(0.72f, 0.34f, 0.08f, 1));
            SetRect(divider.rectTransform, new Vector2(0, 154), new Vector2(500, 3));

            Label("Name Label", panel.transform, "YOUR NAME", 18, new Vector2(0, 103),
                new Vector2(500, 30), new Color(0.95f, 0.8f, 0.58f), FontStyles.Bold);
            _name = InputField(panel.transform, new Vector2(0, 48), new Vector2(500, 62));
            _host = MenuButton("HostGameButton", panel.transform, "HOST SALOON",
                new Vector2(0, -57), new Color(0.62f, 0.22f, 0.055f));
            _join = MenuButton("JoinGameButton", panel.transform, "JOIN SALOON",
                new Vector2(0, -137), new Color(0.18f, 0.36f, 0.25f));
            Label("Footer", panel.transform,
                "One shared lobby  •  Multiple tables  •  Public spectating",
                16, new Vector2(0, -242), new Vector2(550, 34),
                new Color(0.72f, 0.56f, 0.4f), FontStyles.Normal);
            _menu = veil.gameObject;
        }

        private static Image Panel(string name, Transform parent, Color color)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            var image = go.GetComponent<Image>();
            image.color = color;
            return image;
        }

        private static TextMeshProUGUI Label(string name, Transform parent, string value,
            float size, Vector2 position, Vector2 dimensions, Color color, FontStyles style)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(TextMeshProUGUI));
            go.transform.SetParent(parent, false);
            SetRect(go.GetComponent<RectTransform>(), position, dimensions);
            var text = go.GetComponent<TextMeshProUGUI>();
            text.text = value;
            text.fontSize = size;
            text.fontStyle = style;
            text.color = color;
            text.alignment = TextAlignmentOptions.Center;
            return text;
        }

        private static TMP_InputField InputField(Transform parent, Vector2 position, Vector2 dimensions)
        {
            var background = Panel("PlayerNameInput", parent, new Color(0.035f, 0.018f, 0.012f, 1));
            SetRect(background.rectTransform, position, dimensions);
            var field = background.gameObject.AddComponent<TMP_InputField>();
            var text = Label("Text", background.transform, string.Empty, 26, Vector2.zero,
                dimensions - new Vector2(36, 12), Color.white, FontStyles.Normal);
            text.alignment = TextAlignmentOptions.MidlineLeft;
            var placeholder = Label("Placeholder", background.transform, "Enter player name...", 24,
                Vector2.zero, dimensions - new Vector2(36, 12), new Color(0.58f, 0.45f, 0.36f),
                FontStyles.Italic);
            placeholder.alignment = TextAlignmentOptions.MidlineLeft;
            field.textComponent = text;
            field.placeholder = placeholder;
            field.characterLimit = 24;
            var outline = background.gameObject.AddComponent<Outline>();
            outline.effectColor = new Color(0.55f, 0.28f, 0.08f);
            return field;
        }

        private static Button MenuButton(string name, Transform parent, string caption,
            Vector2 position, Color color)
        {
            var image = Panel(name, parent, color);
            SetRect(image.rectTransform, position, new Vector2(500, 64));
            var button = image.gameObject.AddComponent<Button>();
            var colors = button.colors;
            colors.highlightedColor = color * 1.25f;
            colors.pressedColor = color * 0.75f;
            button.colors = colors;
            Label("Label", image.transform, caption, 23, Vector2.zero, new Vector2(480, 56),
                new Color(1f, 0.88f, 0.68f), FontStyles.Bold);
            return button;
        }

        private static void SetRect(RectTransform rect, Vector2 position, Vector2 dimensions)
        {
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = position;
            rect.sizeDelta = dimensions;
        }

        private static void Stretch(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = rect.offsetMax = Vector2.zero;
        }
    }

    /// <summary>
    /// Temporary but functional private setup UI. It is intentionally client-only:
    /// the server still validates that the submitted character was one of the two
    /// options sent to this player.
    /// </summary>
    public sealed class RuntimeMatchSetupUI : MonoBehaviour
    {
        private bool _visible;
        private bool _submitted;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InstallSetupUI()
        {
            if (SceneManager.GetActiveScene().name != "MainLobby") return;
            var go = new GameObject("Runtime Match Setup UI");
            go.AddComponent<RuntimeMatchSetupUI>();
        }

        private void OnEnable()
        {
            LocalMatchPrivateState.SetupReceived += Show;
            LocalMatchPrivateState.MatchReady += Hide;
            LocalMatchPrivateState.Cleared += Hide;
            _visible = LocalMatchPrivateState.HasSetup;
        }

        private void OnDisable()
        {
            LocalMatchPrivateState.SetupReceived -= Show;
            LocalMatchPrivateState.MatchReady -= Hide;
            LocalMatchPrivateState.Cleared -= Hide;
        }

        private void Show()
        {
            _visible = true;
            _submitted = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void Hide()
        {
            _visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnGUI()
        {
            if (!_visible) return;
            var width = 520f;
            var area = new Rect((Screen.width - width) * 0.5f, (Screen.height - 260f) * 0.5f, width, 260);
            GUI.Box(area, string.Empty);
            GUILayout.BeginArea(new Rect(area.x + 24, area.y + 20, area.width - 48, area.height - 40));
            GUILayout.Label($"Your role: {LocalMatchPrivateState.Role}", Centered(26));
            GUILayout.Space(18);
            GUILayout.Label(_submitted ? "Waiting for other players..." : "Choose your character", Centered(22));
            GUILayout.Space(14);
            GUI.enabled = !_submitted;
            if (GUILayout.Button(LocalMatchPrivateState.FirstCharacter.ToString(), GUILayout.Height(52)))
                Submit(LocalMatchPrivateState.FirstCharacter);
            if (GUILayout.Button(LocalMatchPrivateState.SecondCharacter.ToString(), GUILayout.Height(52)))
                Submit(LocalMatchPrivateState.SecondCharacter);
            GUI.enabled = true;
            GUILayout.EndArea();
        }

        private void Submit(CharacterType character)
        {
            var session = FindObjectsByType<GameSession>(FindObjectsSortMode.None)
                .FirstOrDefault(candidate => candidate.Object != null &&
                                             candidate.Object.Id == LocalMatchPrivateState.SessionId);
            if (session == null) return;
            session.RPC_ChooseCharacter(character);
            _submitted = true;
        }

        private static GUIStyle Centered(int size) => new(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = size,
            fontStyle = FontStyle.Bold
        };
    }

    /// <summary>
    /// First playable card HUD. It renders only LocalMatchPrivateState.Hand;
    /// spectators therefore cannot inspect a followed player's private cards.
    /// </summary>
    public sealed class RuntimeCardHUD : MonoBehaviour
    {
        private int _targetSeat = -1;
        private int _firstResponseCard = -1;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InstallCardHud()
        {
            if (SceneManager.GetActiveScene().name != "MainLobby") return;
            new GameObject("Runtime Card HUD").AddComponent<RuntimeCardHUD>();
        }

        private void Update()
        {
            var session = Session();
            if (session == null || !LocalMatchPrivateState.HasSetup) return;
            var localSeat = LocalSeat(session);
            var needsInput = session.CurrentPlayerSeat == localSeat ||
                             session.PendingResponderSeat == localSeat;
            if (!needsInput) return;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void OnGUI()
        {
            var session = Session();
            if (session == null || !LocalMatchPrivateState.HasSetup ||
                session.Phase == Rules.MatchPhase.Setup) return;
            var localSeat = LocalSeat(session);
            if (localSeat < 0) return;

            GUI.Box(new Rect(18, 18, 360, 126),
                $"Role: {LocalMatchPrivateState.Role}\n" +
                $"Character: {session.Characters[localSeat]}\n" +
                $"Health: {session.Health[localSeat]}/{session.MaxHealth[localSeat]}\n" +
                $"Deck: {session.DrawPileCount}   Discard: {DiscardName(session)}");

            DrawPublicPlayers(session, localSeat);
            DrawHand(session, localSeat);
        }

        private void DrawPublicPlayers(GameSession session, int localSeat)
        {
            GUILayout.BeginArea(new Rect(Screen.width - 310, 18, 292, 70 + session.PlayerCount * 38));
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label($"Turn: {session.PlayerNames[session.CurrentPlayerSeat]}  •  {session.Phase}");
            for (var seat = 0; seat < session.PlayerCount; seat++)
            {
                var role = session.VisibleRoles[seat] >= 0
                    ? $" • {(RoleType)session.VisibleRoles[seat]}" : string.Empty;
                var selected = _targetSeat == seat ? "► " : string.Empty;
                GUI.enabled = seat != localSeat;
                if (GUILayout.Button($"{selected}{session.PlayerNames[seat]}  " +
                                     $"{session.Health[seat]}/{session.MaxHealth[seat]}{role}"))
                    _targetSeat = seat;
                GUI.enabled = true;
            }
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        private void DrawHand(GameSession session, int localSeat)
        {
            var isTurn = session.CurrentPlayerSeat == localSeat;
            var responding = session.PendingResponderSeat == localSeat;
            var area = new Rect(18, Screen.height - 245, Screen.width - 36, 225);
            GUILayout.BeginArea(area);
            GUILayout.BeginVertical(GUI.skin.box);
            if (!string.IsNullOrEmpty(LocalMatchPrivateState.LastError))
                GUILayout.Label($"Cannot do that: {LocalMatchPrivateState.LastError}");

            if (responding)
            {
                GUILayout.Label("Response required: select the required BANG! or Missed!, or decline.");
                GUILayout.BeginHorizontal();
                foreach (var card in LocalMatchPrivateState.Hand)
                {
                    if (GUILayout.Button(CardLabel(card), GUILayout.Width(145), GUILayout.Height(72)))
                    {
                        if (_firstResponseCard < 0) _firstResponseCard = card.Id;
                        else
                        {
                            session.RPC_Respond(_firstResponseCard, card.Id);
                            _firstResponseCard = -1;
                        }
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                if (_firstResponseCard > 0 &&
                    GUILayout.Button("Play selected defense", GUILayout.Height(34)))
                {
                    session.RPC_Respond(_firstResponseCard, -1);
                    _firstResponseCard = -1;
                }
                if (GUILayout.Button("Take hit / decline", GUILayout.Height(34)))
                {
                    session.RPC_Respond(-1, -1);
                    _firstResponseCard = -1;
                }
                GUILayout.EndHorizontal();
            }
            else if (isTurn)
            {
                GUILayout.BeginHorizontal();
                if (session.Phase == Rules.MatchPhase.Draw &&
                    GUILayout.Button("Draw two cards", GUILayout.Height(38)))
                    session.RPC_DrawPhase();
                if (session.Phase == Rules.MatchPhase.Play &&
                    GUILayout.Button("End play phase", GUILayout.Height(38)))
                    session.RPC_EndPlayPhase();
                if (session.Phase == Rules.MatchPhase.Discard &&
                    GUILayout.Button("End turn", GUILayout.Height(38)))
                    session.RPC_EndTurn();
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                foreach (var card in LocalMatchPrivateState.Hand)
                {
                    if (!GUILayout.Button(CardLabel(card), GUILayout.Width(145), GUILayout.Height(92))) continue;
                    if (session.Phase == Rules.MatchPhase.Play)
                        session.RPC_PlayCard(card.Id, _targetSeat);
                    else if (session.Phase == Rules.MatchPhase.Discard)
                        session.RPC_Discard(card.Id);
                }
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.Label($"Waiting for {session.PlayerNames[session.CurrentPlayerSeat]}...");
                GUILayout.BeginHorizontal();
                foreach (var card in LocalMatchPrivateState.Hand)
                    GUILayout.Box(CardLabel(card), GUILayout.Width(145), GUILayout.Height(72));
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        private static string CardLabel(PrivateCardView card) =>
            $"{card.Name}\n{card.Rank} of {card.Suit}" +
            (card.Range > 0 ? $"\nRange {card.Range}" : string.Empty);

        private static string DiscardName(GameSession session) =>
            session.TopDiscardName >= 0
                ? ((PlayingCardName)session.TopDiscardName).ToString()
                : "Empty";

        private static int LocalSeat(GameSession session)
        {
            for (var seat = 0; seat < session.PlayerCount; seat++)
                if (session.SeatPlayers[seat] == session.Runner.LocalPlayer) return seat;
            return -1;
        }

        private static GameSession Session() =>
            FindObjectsByType<GameSession>(FindObjectsSortMode.None)
                .FirstOrDefault(candidate => candidate.Object != null &&
                                             candidate.Object.Id == LocalMatchPrivateState.SessionId);
    }
}
