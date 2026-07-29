using UnityEngine;
using UnityEngine.SceneManagement;

namespace BulletBang
{
    /// <summary>
    /// Makes the current prototype immediately explorable in Play Mode. The
    /// authored/generated scene replaces this fallback automatically because both
    /// use the same root name.
    /// </summary>
    public sealed class LobbyPlayModePreview : MonoBehaviour
    {
        private const string RootName = "Generated Saloon Environment";
        private Camera _camera;
        private float _yaw;
        private float _pitch = 12f;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Install()
        {
            // Fusion reloads the network scene when hosting or joining. Runtime
            // initialization only covers the first load, so also listen for every
            // subsequent MainLobby load.
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
            EnsureEnvironment();
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            EnsureEnvironment();
        }

        public static void EnsureEnvironment()
        {
            // In Play Mode Unity may expose an unsaved scene as
            // Temp/__Backupscenes/0.backup. The manager is a more reliable marker
            // than the scene name.
            if (FindFirstObjectByType<MainLobbyManager>() == null) return;
            if (GameObject.Find(RootName) == null) BuildFallbackSaloon();

            if (FindFirstObjectByType<LobbyPlayModePreview>() == null)
            {
                var preview = new GameObject("Lobby Play Mode Preview");
                preview.AddComponent<LobbyPlayModePreview>();
            }
        }

        private void Start()
        {
            _camera = Camera.main;
            if (_camera == null)
            {
                var cameraObject = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
                cameraObject.tag = "MainCamera";
                _camera = cameraObject.GetComponent<Camera>();
            }

            _camera.transform.SetPositionAndRotation(new Vector3(0, 6.5f, -13),
                Quaternion.Euler(12, 0, 0));
            _camera.fieldOfView = 62;
            _yaw = _camera.transform.eulerAngles.y;
        }

        private void Update()
        {
            if (_camera == null) return;

            // Hold right mouse to look. WASD/arrow keys and mouse wheel allow a
            // quick environment walkthrough without competing with lobby UI.
            if (Input.GetMouseButton(1))
            {
                _yaw += Input.GetAxis("Mouse X") * 3f;
                _pitch = Mathf.Clamp(_pitch - Input.GetAxis("Mouse Y") * 2.5f, -25f, 65f);
                _camera.transform.rotation = Quaternion.Euler(_pitch, _yaw, 0);
            }

            var move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            move += Vector3.up * Input.mouseScrollDelta.y * 2f;
            var planarForward = Vector3.ProjectOnPlane(_camera.transform.forward, Vector3.up).normalized;
            var worldMove = _camera.transform.right * move.x + planarForward * move.z + Vector3.up * move.y;
            _camera.transform.position += worldMove * (8f * Time.deltaTime);
        }

        private static void BuildFallbackSaloon()
        {
            var rootObject = new GameObject(RootName);
            // Keep the local presentation environment alive while Fusion reloads
            // the network scene. Network objects remain owned by Fusion.
            DontDestroyOnLoad(rootObject);
            var root = rootObject.transform;
            var wood = RuntimeMaterial(new Color(0.23f, 0.085f, 0.025f), 0.08f);
            var floor = RuntimeMaterial(new Color(0.38f, 0.18f, 0.065f), 0.02f);
            var wall = RuntimeMaterial(new Color(0.48f, 0.29f, 0.15f), 0f);
            var felt = RuntimeMaterial(new Color(0.035f, 0.23f, 0.14f), 0.05f);
            var available = RuntimeMaterial(new Color(0.1f, 0.65f, 0.32f), 0.15f);
            var occupied = RuntimeMaterial(new Color(0.85f, 0.28f, 0.06f), 0.15f);

            Box("Floor", root, new Vector3(0, -0.2f, 3), new Vector3(42, 0.4f, 34), floor);
            Box("Back Wall", root, new Vector3(0, 4f, 20), new Vector3(42, 8, 0.35f), wall);
            Box("Left Wall", root, new Vector3(-21, 4f, 3), new Vector3(0.35f, 8, 34), wall);
            Box("Right Wall", root, new Vector3(21, 4f, 3), new Vector3(0.35f, 8, 34), wall);
            Box("Simple Bar", root, new Vector3(0, 0.85f, 17.5f), new Vector3(12, 1.7f, 1.25f), wood);

            BuildTable(root, new Vector3(0f, 0f, 3f), wood, felt, available, 1);

            ConfigureLighting(root);
        }

        private static void BuildTable(Transform root, Vector3 position, Material wood,
            Material felt, Material status, int number)
        {
            var table = new GameObject($"Table {number} - {(number <= 4 ? "JOINABLE" : "IN GAME")}").transform;
            table.SetParent(root);
            table.position = position;
            Cylinder("Wood Table", table, new Vector3(0, 0.85f, 0), new Vector3(2.3f, 0.12f, 2.3f), wood);
            Cylinder("Felt", table, new Vector3(0, 1f, 0), new Vector3(2.02f, 0.025f, 2.02f), felt);
            Cylinder("Pedestal", table, new Vector3(0, 0.4f, 0), new Vector3(0.48f, 0.4f, 0.48f), wood);

            for (var seat = 0; seat < 8; seat++)
            {
                var angle = seat * Mathf.PI / 4f;
                var seatPosition = new Vector3(Mathf.Sin(angle) * 3.1f, 0.45f, Mathf.Cos(angle) * 3.1f);
                Cylinder($"Seat {seat + 1}", table, seatPosition, new Vector3(0.42f, 0.12f, 0.42f), wood);
            }

            var beacon = Cylinder("Table Status", table, new Vector3(0, 2.1f, 0),
                new Vector3(0.16f, 0.04f, 0.16f), status);
            var light = beacon.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = status.color;
            light.range = 4.5f;
            light.intensity = 1.6f;
        }

        private static void ConfigureLighting(Transform root)
        {
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogColor = new Color(0.16f, 0.085f, 0.045f);
            RenderSettings.fogStartDistance = 20;
            RenderSettings.fogEndDistance = 45;
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = new Color(0.35f, 0.2f, 0.11f);
            RenderSettings.ambientEquatorColor = new Color(0.16f, 0.075f, 0.035f);
            RenderSettings.ambientGroundColor = new Color(0.045f, 0.02f, 0.01f);

            var sun = Object.FindFirstObjectByType<Light>();
            if (sun == null)
            {
                var sunObject = new GameObject("Warm Directional Light", typeof(Light));
                sunObject.transform.SetParent(root);
                sun = sunObject.GetComponent<Light>();
            }
            sun.type = LightType.Directional;
            sun.color = new Color(1f, 0.7f, 0.42f);
            sun.intensity = 1.1f;
            sun.transform.rotation = Quaternion.Euler(48, -32, 0);
            RenderSettings.sun = sun;

            foreach (var x in new[] { -8f, 0f, 8f })
            {
                var lamp = new GameObject("Saloon Lamp", typeof(Light));
                lamp.transform.SetParent(root);
                lamp.transform.position = new Vector3(x, 4.5f, 2);
                var light = lamp.GetComponent<Light>();
                light.type = LightType.Point;
                light.color = new Color(1f, 0.42f, 0.12f);
                light.range = 10;
                light.intensity = 2.2f;
            }
        }

        private static GameObject Box(string name, Transform parent, Vector3 position,
            Vector3 scale, Material material) =>
            Primitive(PrimitiveType.Cube, name, parent, position, scale, material);

        private static GameObject Cylinder(string name, Transform parent, Vector3 position,
            Vector3 scale, Material material) =>
            Primitive(PrimitiveType.Cylinder, name, parent, position, scale, material);

        private static GameObject Primitive(PrimitiveType type, string name, Transform parent,
            Vector3 position, Vector3 scale, Material material)
        {
            var go = GameObject.CreatePrimitive(type);
            go.name = name;
            go.transform.SetParent(parent);
            go.transform.localPosition = position;
            go.transform.localScale = scale;
            go.GetComponent<Renderer>().sharedMaterial = material;
            return go;
        }

        private static Material RuntimeMaterial(Color color, float metallic)
        {
            var material = new Material(Shader.Find("Standard")) { color = color };
            material.SetFloat("_Metallic", metallic);
            material.SetFloat("_Glossiness", 0.28f);
            return material;
        }
    }
}
