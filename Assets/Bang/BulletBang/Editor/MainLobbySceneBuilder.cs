#if UNITY_EDITOR
using System.IO;
using BulletBang;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BulletBang.Editor
{
    /// <summary>
    /// Rebuilds only the generated saloon environment. Networking objects and UI
    /// remain untouched, making visual iteration safe and repeatable.
    /// </summary>
    public static class MainLobbySceneBuilder
    {
        private const string ScenePath = "Assets/Scenes/MainLobby.unity";
        private const string MaterialFolder = "Assets/Bang/BulletBang/Environment/Materials";
        private const string RootName = "Generated Saloon Environment";

        [MenuItem("Bullet Bang/Scenes/Rebuild Main Lobby Saloon")]
        public static void Rebuild()
        {
            var scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            var oldRoot = GameObject.Find(RootName);
            if (oldRoot != null) Object.DestroyImmediate(oldRoot);

            EnsureFolder("Assets/Bang/BulletBang/Environment");
            EnsureFolder(MaterialFolder);
            var wood = Material("Dark Wood", new Color(0.18f, 0.075f, 0.03f), 0.1f);
            var floor = Material("Warm Floor", new Color(0.34f, 0.16f, 0.065f), 0.05f);
            var plaster = Material("Adobe Plaster", new Color(0.48f, 0.31f, 0.18f), 0f);
            var felt = Material("Table Felt", new Color(0.09f, 0.22f, 0.16f), 0.15f);
            var brass = Material("Warm Brass", new Color(0.55f, 0.3f, 0.08f), 0.65f);
            var red = Material("Table Accent", new Color(0.36f, 0.045f, 0.03f), 0.1f);

            var root = new GameObject(RootName);
            BuildRoom(root.transform, floor, plaster, wood);
            BuildBar(root.transform, wood, brass);
            BuildTableBays(root.transform, wood, felt, red);
            BuildSpectatorRail(root.transform, wood, brass);
            BuildLighting(root.transform);
            BuildSpawnAndWayfinding(root.transform, red);
            ConfigureExistingScene(root.transform);

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            Selection.activeGameObject = root;
            Debug.Log("Main lobby saloon rebuilt and saved.");
        }

        private static void BuildRoom(Transform root, Material floor, Material wall, Material wood)
        {
            Box("Floor", root, new Vector3(0, -0.15f, 2), new Vector3(30, 0.3f, 24), floor);
            Box("Back Wall", root, new Vector3(0, 3, 13.85f), new Vector3(30, 6, 0.3f), wall);
            Box("Left Wall", root, new Vector3(-14.85f, 3, 2), new Vector3(0.3f, 6, 24), wall);
            Box("Right Wall", root, new Vector3(14.85f, 3, 2), new Vector3(0.3f, 6, 24), wall);
            Box("Front Wall Left", root, new Vector3(-9.5f, 3, -9.85f), new Vector3(11, 6, 0.3f), wall);
            Box("Front Wall Right", root, new Vector3(9.5f, 3, -9.85f), new Vector3(11, 6, 0.3f), wall);
            Box("Front Header", root, new Vector3(0, 5, -9.85f), new Vector3(8, 2, 0.3f), wood);
            for (var x = -13f; x <= 13f; x += 2f)
                Box($"Floor Board {x}", root, new Vector3(x, 0.015f, 2), new Vector3(0.035f, 0.02f, 23.5f), wood);
        }

        private static void BuildBar(Transform root, Material wood, Material brass)
        {
            var barRoot = new GameObject("Saloon Bar").transform;
            barRoot.SetParent(root);
            Box("Counter", barRoot, new Vector3(0, 1.1f, 11.4f), new Vector3(15, 0.25f, 1.2f), wood);
            Box("Front", barRoot, new Vector3(0, 0.5f, 11.8f), new Vector3(15, 1, 0.3f), wood);
            for (var x = -6.5f; x <= 6.5f; x += 2.6f)
            {
                Cylinder("Bar Stool", barRoot, new Vector3(x, 0.5f, 9.9f), new Vector3(0.42f, 0.08f, 0.42f), wood);
                Cylinder("Stool Post", barRoot, new Vector3(x, 0.25f, 9.9f), new Vector3(0.09f, 0.25f, 0.09f), brass);
            }
        }

        private static void BuildTableBays(Transform root, Material wood, Material felt, Material accent)
        {
            var positions = new[]
            {
                new Vector3(-8, 0, 4.5f), new Vector3(0, 0, 4.5f), new Vector3(8, 0, 4.5f),
                new Vector3(-8, 0, -3), new Vector3(0, 0, -3), new Vector3(8, 0, -3)
            };
            var spawnRoot = new GameObject("Table Spawn Points").transform;
            spawnRoot.SetParent(root);
            for (var i = 0; i < positions.Length; i++)
            {
                var bay = new GameObject($"Table Bay {i + 1}").transform;
                bay.SetParent(root);
                bay.position = positions[i];
                Cylinder("Table", bay, new Vector3(0, 0.82f, 0), new Vector3(2.35f, 0.12f, 2.35f), wood);
                Cylinder("Felt", bay, new Vector3(0, 0.96f, 0), new Vector3(2.05f, 0.025f, 2.05f), felt);
                Cylinder("Base", bay, new Vector3(0, 0.38f, 0), new Vector3(0.45f, 0.38f, 0.45f), wood);
                for (var seat = 0; seat < 8; seat++)
                {
                    var angle = seat * Mathf.PI * 0.25f;
                    var p = new Vector3(Mathf.Sin(angle) * 3.15f, 0.45f, Mathf.Cos(angle) * 3.15f);
                    Cylinder($"Seat {seat + 1}", bay, p, new Vector3(0.45f, 0.12f, 0.45f), accent);
                }
                var spawn = new GameObject($"Table Spawn {i + 1}").transform;
                spawn.SetParent(spawnRoot);
                spawn.position = positions[i];
            }

            var manager = Object.FindFirstObjectByType<MainLobbyManager>();
            if (manager != null)
            {
                var serialized = new SerializedObject(manager);
                var points = serialized.FindProperty("tableSpawnPoints");
                points.arraySize = spawnRoot.childCount;
                for (var i = 0; i < spawnRoot.childCount; i++)
                    points.GetArrayElementAtIndex(i).objectReferenceValue = spawnRoot.GetChild(i);
                serialized.ApplyModifiedPropertiesWithoutUndo();
            }
        }

        private static void BuildSpectatorRail(Transform root, Material wood, Material brass)
        {
            var rail = new GameObject("Spectator Rail").transform;
            rail.SetParent(root);
            Box("Platform", rail, new Vector3(12.5f, 0.35f, 2), new Vector3(4.2f, 0.7f, 15), wood);
            Box("Top Rail", rail, new Vector3(10.35f, 1.25f, 2), new Vector3(0.16f, 0.16f, 15), brass);
            for (var z = -5f; z <= 9f; z += 2f)
                Box($"Post {z}", rail, new Vector3(10.35f, 0.75f, z), new Vector3(0.12f, 1, 0.12f), wood);
        }

        private static void BuildLighting(Transform root)
        {
            var existing = Object.FindFirstObjectByType<Light>();
            if (existing != null)
            {
                existing.type = LightType.Directional;
                existing.color = new Color(1f, 0.72f, 0.48f);
                existing.intensity = 1.1f;
                existing.transform.rotation = Quaternion.Euler(42, -28, 0);
                RenderSettings.sun = existing;
            }
            RenderSettings.fog = true;
            RenderSettings.fogColor = new Color(0.17f, 0.105f, 0.07f);
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogStartDistance = 18;
            RenderSettings.fogEndDistance = 42;
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = new Color(0.32f, 0.2f, 0.13f);
            RenderSettings.ambientEquatorColor = new Color(0.18f, 0.09f, 0.045f);
            RenderSettings.ambientGroundColor = new Color(0.055f, 0.03f, 0.02f);

            foreach (var position in new[] { new Vector3(-8, 4.5f, 3), new Vector3(0, 4.5f, 3), new Vector3(8, 4.5f, 3) })
            {
                var lamp = new GameObject("Warm Chandelier", typeof(Light));
                lamp.transform.SetParent(root);
                lamp.transform.position = position;
                var light = lamp.GetComponent<Light>();
                light.type = LightType.Point;
                light.color = new Color(1f, 0.48f, 0.18f);
                light.range = 10;
                light.intensity = 2.3f;
                light.shadows = LightShadows.Soft;
            }
        }

        private static void BuildSpawnAndWayfinding(Transform root, Material accent)
        {
            Box("Entrance Runner", root, new Vector3(0, 0.025f, -7), new Vector3(4.5f, 0.04f, 5), accent);
            var spawn = new GameObject("Lobby Player Spawn");
            spawn.transform.SetParent(root);
            spawn.transform.SetPositionAndRotation(new Vector3(0, 0.1f, -7.5f), Quaternion.identity);
        }

        private static void ConfigureExistingScene(Transform environment)
        {
            var camera = Camera.main;
            if (camera != null)
            {
                camera.transform.SetPositionAndRotation(new Vector3(0, 8.5f, -15f), Quaternion.Euler(18, 0, 0));
                camera.fieldOfView = 55;
            }
            environment.gameObject.isStatic = true;
        }

        private static GameObject Box(string name, Transform parent, Vector3 position, Vector3 scale, Material material)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Setup(go, name, parent, position, scale, material);
            return go;
        }

        private static GameObject Cylinder(string name, Transform parent, Vector3 position, Vector3 scale, Material material)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            Setup(go, name, parent, position, scale, material);
            return go;
        }

        private static void Setup(GameObject go, string name, Transform parent, Vector3 position, Vector3 scale, Material material)
        {
            go.name = name;
            go.transform.SetParent(parent);
            go.transform.localPosition = position;
            go.transform.localScale = scale;
            go.GetComponent<Renderer>().sharedMaterial = material;
            go.isStatic = true;
        }

        private static Material Material(string name, Color color, float metallic)
        {
            var path = $"{MaterialFolder}/{name}.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                material = new Material(Shader.Find("Standard"));
                AssetDatabase.CreateAsset(material, path);
            }
            material.color = color;
            material.SetFloat("_Metallic", metallic);
            material.SetFloat("_Glossiness", 0.32f);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path)) return;
            var parent = Path.GetDirectoryName(path)?.Replace('\\', '/');
            var name = Path.GetFileName(path);
            AssetDatabase.CreateFolder(parent, name);
        }
    }
}
#endif
