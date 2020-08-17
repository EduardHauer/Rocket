using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class Local : MonoBehaviour
    {
        public static Local Instance;

        [Header("Screen")]
        public UIList FullscreenMode;
        [Header("Graphics")]
        public UISlider GridThickness;
        public UISlider Bloom;
        public UISlider LensDistortion;
        public UIList ColorPalette;

        private FullScreenMode _mode;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            FullscreenMode.ChangeOptionTo((int)Global.Instance.CurrentOptions.fullscreenMode);
            GridThickness.ChangeValueTo(Global.Instance.CurrentOptions.gridThickness);
            Bloom.ChangeValueTo(Global.Instance.CurrentOptions.bloom);
            LensDistortion.ChangeValueTo(Global.Instance.CurrentOptions.lensDistortion);
            ColorPalette.Options = new string[Global.Instance.ColorPaletteCollection.Count];
            for (int i = 0; i < Global.Instance.ColorPaletteCollection.Count; i++)
            {
                ColorPalette.Options[i] = Global.Instance.ColorPaletteCollection[i].Name;
            }
            ColorPalette.ChangeOptionTo(Global.Instance.CurrentOptions.colorPalette);
        }

        //public void CurrantOptions()
        //{
        //    FullscreenMode.ChangeOptionTo((int)Global.Instance.DefaultOptions.fullscreenMode);
        //    GridThickness.ChangeValueTo(Global.Instance.DefaultOptions.gridThickness);
        //    Bloom.ChangeValueTo(Global.Instance.DefaultOptions.bloom);
        //    LensDistortion.ChangeValueTo(Global.Instance.DefaultOptions.lensDistortion);
        //    ColorPalette.ChangeOptionTo(Global.Instance.CurrentOptions.colorPalette);
        //}

        public void Exit()
        {
            Application.Quit();
        }

        public void SetBloom(float value)
        {
            Global.Instance.SetBloom(value);
        }

        public void SetLensDistortion(float value)
        {
            Global.Instance.SetLensDistortion(value);
        }

        public void SetGridThickness(float thickness)
        {
            Global.Instance.SetGridThickness(thickness);
        }

        public void SetFullscreenMode(int modeID)
        {
            _mode = (FullScreenMode) modeID;
        }

        public void SetColorPalette(int id)
        {
            Global.Instance.SetColorPalette(id);
        }

        public void Apply()
        {
            Screen.fullScreenMode = _mode;
        }

        public void Revert()
        {
            FullscreenMode.ChangeOptionTo((int)Screen.fullScreenMode);
        }

        public void LoadCollection()
        {
            Global.Instance.LoadCollection();
            ColorPalette.Options = new string[Global.Instance.ColorPaletteCollection.Count];
            for (int i = 0; i < Global.Instance.ColorPaletteCollection.Count; i++)
            {
                ColorPalette.Options[i] = Global.Instance.ColorPaletteCollection[i].Name;
            }
            ColorPalette.ChangeOptionTo(Global.Instance.CurrentOptions.colorPalette);
        }

        public void ResetCollection()
        {
            Global.Instance.ResetCollection();
            ColorPalette.Options = new string[Global.Instance.ColorPaletteCollection.Count];
            for (int i = 0; i < Global.Instance.ColorPaletteCollection.Count; i++)
            {
                ColorPalette.Options[i] = Global.Instance.ColorPaletteCollection[i].Name;
            }
            ColorPalette.ChangeOptionTo(Global.Instance.CurrentOptions.colorPalette);
        }
    }
}
