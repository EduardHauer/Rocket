using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Assets.ColorPaletteTester.Dev.Scripts
{
    public class ColorTesterWindow : EditorWindow
    {
        private ColorPaletteCollection _colorPaletteCollection;
        private int _currentColorPalette;
        private float _gridThickness = 1;
        private float _bloomValue = 1;
        private float _lensDistortionValue = 1;
        private static Bloom _bloom;
        private static LensDistortion _lensDistortion;
        private static ColorTesterHidden _colorTesterHidden;

        [MenuItem("Window/Color Palette Editor")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            ColorTesterWindow window = GetWindow<ColorTesterWindow>("Color Palette Editor");
            window.Show(); 
            GameObject obj = GameObject.FindGameObjectWithTag("ColorTesterHidden");
            if (obj != null && obj.GetComponent<ColorTesterHidden>() != null)
            {
                _colorTesterHidden = obj.GetComponent<ColorTesterHidden>();
                _colorTesterHidden.DefaultSVP.TryGet(out _bloom);
                _colorTesterHidden.DefaultSVP.TryGet(out _lensDistortion);
            }
        }

        private void OnGUI()
        {
            if (_colorTesterHidden == null)
                return;
            GUIStyle header = new GUIStyle();
            header.fontStyle = FontStyle.Bold;
            header.alignment = TextAnchor.MiddleCenter;
            EditorGUILayout.LabelField("Color Palette Collection", header);
            _colorTesterHidden.ColorPaletteCollection = (ColorPaletteCollection)EditorGUILayout.ObjectField("Color Palette Collection", _colorTesterHidden.ColorPaletteCollection, typeof(ColorPaletteCollection), true);
            _colorPaletteCollection = _colorTesterHidden.ColorPaletteCollection;
            if (_colorTesterHidden.ColorPaletteCollection != null)
                _colorTesterHidden.ColorPaletteCollection.Name = EditorGUILayout.TextField("Name", _colorTesterHidden.ColorPaletteCollection.Name);
            if (GUILayout.Button("Create Color Palette"))
            {
                ColorPaletteCollection asset = CreateInstance<ColorPaletteCollection>();

                _colorTesterHidden.ColorPaletteCollection = asset;
                _colorPaletteCollection = asset;
                AssetDatabase.CreateAsset(asset, "Assets/ColorPaletteTester/NewColorPaletteCollection.asset");
                AssetDatabase.SaveAssets();
            }
            if (_colorTesterHidden.ColorPaletteCollection == null)
                return;

            EditorGUILayout.Space();
            string[] options = new string[_colorPaletteCollection.Count + 1];
            options[0] = "None";
            string o = "\u2800";
            for (int i = 0; i < options.Length - 1; i++)
            {
                options[i + 1] = _colorPaletteCollection[i].Name + o;
                o += '\u2800';
            }
            EditorGUILayout.Space();
            if (options.Length > 1)
                _currentColorPalette = EditorGUILayout.Popup("Color Palette", _currentColorPalette + 1, options) - 1;
            else
                _currentColorPalette = EditorGUILayout.Popup("Color Palette", 0, options) - 1;
            if (_currentColorPalette >= 0) 
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Color Palette", header);
                _colorPaletteCollection[_currentColorPalette].Name = EditorGUILayout.TextField("Name", _colorPaletteCollection[_currentColorPalette].Name);
                _colorPaletteCollection[_currentColorPalette].MainColor = EditorGUILayout.ColorField(new GUIContent("Main Color"), _colorPaletteCollection[_currentColorPalette].MainColor, true, true, true);
                _colorPaletteCollection[_currentColorPalette].SecondColor = EditorGUILayout.ColorField(new GUIContent("Second Color"), _colorPaletteCollection[_currentColorPalette].SecondColor, true, true, true);
                _colorTesterHidden.ColorTestMaterial.SetColor("_MainColor", _colorPaletteCollection[_currentColorPalette].MainColor);
                _colorTesterHidden.ColorTestMaterial.SetColor("_SecondColor", _colorPaletteCollection[_currentColorPalette].SecondColor);
                if (GUILayout.Button("Remove Color Palette"))
                {
                    _colorPaletteCollection.RemoveAt(_currentColorPalette);
                    _currentColorPalette--;
                }
            }
            EditorGUILayout.Space();
            if (GUILayout.Button("Add Color Palette"))
            {
                _colorPaletteCollection.Add(new ColorPalette());
                _currentColorPalette = _colorPaletteCollection.Count - 1;
            }
            EditorGUILayout.Space();
            if (GUILayout.Button("Export Color Palette Collection..."))
            {
                SaveFile();
            }
            EditorGUILayout.Space();
            if (GUILayout.Button("Import Color Palette Collection..."))
            {
                LoadFile();
            }
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Scene Settings", header);
            _gridThickness = EditorGUILayout.Slider("Grid Thickness", _gridThickness, 0, 1);
            _bloomValue = EditorGUILayout.Slider("Bloom", _bloomValue, 0, 1);
            _lensDistortionValue = EditorGUILayout.Slider("Lens Distortion", _lensDistortionValue, 0, 1);
            _colorTesterHidden.Grid.SetFloat("_Thickness", _gridThickness);
            _bloom.intensity.value = _bloomValue * 0.4f;
            _lensDistortion.intensity.value = _lensDistortionValue * 0.4f;
        }

        public void SaveFile()
        {
            var path = EditorUtility.SaveFilePanel("Export color palette collection ...", "", "", "json");
            if (path.Length > 0)
            {
                string json = JsonUtility.ToJson(new JsonCollection().FromCollection(_colorPaletteCollection));
                File.WriteAllText(path, json);
            }
        }

        public void LoadFile()
        {
            var path = EditorUtility.OpenFilePanel("Import color palette collection ...", "", "json");
            if (path.Length > 0)
            {
                string json = File.ReadAllText(path);
                ColorPaletteCollection asset = JsonUtility.FromJson<JsonCollection>(json).ToCollection();

                _colorTesterHidden.ColorPaletteCollection = asset;
                _colorPaletteCollection = asset;
                AssetDatabase.CreateAsset(asset, "Assets/ColorPaletteTester/" + asset.Name + ".asset");
                AssetDatabase.SaveAssets();
            }
        }
    }
}
