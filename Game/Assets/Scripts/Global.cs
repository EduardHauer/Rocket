using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class Global : MonoBehaviour
    {
        public static Global Instance;

        public InputActionAsset InputActionAsset;
        public Options DefaultOptions;
        public Options CurrentOptions;
        public VolumeProfile DefaultSVP;
        public Vector2 BloomRange;
        public Vector2 LensDistortionRange;
        public Material GridMaterial;
        public Vector2 CameraSizeRange;
        public ColorPaletteCollection ColorPaletteCollection;
        public ColorPaletteCollection DefaultColorPaletteCollection;
        public Material[] Materials;
        public Texture2D FontTexture;
        public Shader SpriteShader;
        public Shader TextShader;
        public ColorPalette CurrentColorPalette { get { return ColorPaletteCollection[CurrentOptions.colorPalette]; } }

        private Bloom _bloom;
        private LensDistortion _lensDistortion;
        private Camera _camera;

        [Serializable]
        public class Options
        {
            public FullScreenMode fullscreenMode;
            public float gridThickness;
            public float bloom;
            public float lensDistortion;
            public int colorPalette;

            public Options(FullScreenMode fullscreenMode, float gridThickness, float bloom, float lensDistortion, int colorPalette)
            {
                this.fullscreenMode = fullscreenMode;
                this.gridThickness = gridThickness;
                this.bloom = bloom;
                this.lensDistortion = lensDistortion;
                this.colorPalette = colorPalette;
            }

            public Options(Options options)
            {
                fullscreenMode = options.fullscreenMode;
                gridThickness = options.gridThickness;
                bloom = options.bloom;
                lensDistortion = options.lensDistortion;
                colorPalette = options.colorPalette;
            }
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            Cursor.visible = false;
            DontDestroyOnLoad(gameObject);
            DefaultSVP.TryGet(out _bloom);
            DefaultSVP.TryGet(out _lensDistortion);
            _camera = Camera.main;
            SceneManager.sceneLoaded += OnSceneLoaded;
            CurrentOptions = new Options(DefaultOptions);
            SetColorPalette(CurrentOptions.colorPalette);
            ColorPaletteCollection = DefaultColorPaletteCollection;
        }

        public void SetColorPalette(int id)
        {
            CurrentOptions.colorPalette = id % ColorPaletteCollection.Count;
            foreach (Material mat in Materials)
            {
                mat.SetColor("_MainColor", ColorPaletteCollection[CurrentOptions.colorPalette % ColorPaletteCollection.Count].MainColor);
                mat.SetColor("_SecondColor", ColorPaletteCollection[CurrentOptions.colorPalette % ColorPaletteCollection.Count].SecondColor);
            }
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            InputActionAsset.Disable();
            _camera = Camera.main;
            SetLensDistortion(CurrentOptions.lensDistortion);
            InputActionAsset.Enable();
        }

        private void Update()
        {
            if (_camera == null)
                _camera = Camera.main;
        }

        private void OnEnable()
        {
            InputActionAsset.Enable();
        }

        private void OnDisable()
        {
            InputActionAsset.Disable();
        }

        public void SetBloom(float value)
        {
            CurrentOptions.bloom = value;
            _bloom.intensity.value = value * (BloomRange.y - BloomRange.x) + BloomRange.x;
        }

        public void SetLensDistortion(float value)
        {
            CurrentOptions.lensDistortion = value;
            _lensDistortion.intensity.value = value * (LensDistortionRange.y - LensDistortionRange.x) + LensDistortionRange.x;
            _camera.orthographicSize = value * (CameraSizeRange.y - CameraSizeRange.x) + CameraSizeRange.x;
        }

        public void SetGridThickness(float thickness)
        {
            CurrentOptions.gridThickness = thickness;
            GridMaterial.SetFloat("_Thickness", thickness);
        }

        public void LoadCollection()
        {
            Cursor.visible = true;
            var path = EditorUtility.OpenFilePanel("Load color palette collection ...", "", "json");
            Cursor.visible = false;
            if (path.Length > 0)
            {
                string json = File.ReadAllText(path);
                ColorPaletteCollection asset = JsonUtility.FromJson<JsonCollection>(json).ToCollection();

                ColorPaletteCollection = asset;
                AssetDatabase.CreateAsset(asset, "Assets/ColorPalettes/" + asset.Name + ".asset");
                AssetDatabase.SaveAssets();
            }
            SetColorPalette(0);
        }

        public void ResetCollection()
        {
            ColorPaletteCollection = DefaultColorPaletteCollection;
            SetColorPalette(0);
        }
    }
}
