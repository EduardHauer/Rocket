using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "New Color Palette Collection", menuName = "Color Palette/Color Palette Collection")]
    public class ColorPaletteCollection : ScriptableObject
    {
        public string Name;
        public List<ColorPalette> Palettes = new List<ColorPalette>();
        public int Count { get { return Palettes.Count; } }

        public ColorPalette this[int id]
        {
            get { return Palettes[id]; }
        }

        public void Add(ColorPalette item)
        {
            Palettes.Add(item);
        }

        public void RemoveAt(int index)
        {
            Palettes.RemoveAt(index);
        }
    }

    [Serializable]
    public class JsonCollection
    {
        public string Name;
        public ColorPalette[] Palettes;

        public ColorPaletteCollection ToCollection()
        {
            ColorPaletteCollection result = ScriptableObject.CreateInstance<ColorPaletteCollection>();
            result.Name = Name;
            result.Palettes = new List<ColorPalette>(Palettes);
            return result;
        }

        public JsonCollection FromCollection(ColorPaletteCollection collection)
        {
            Name = collection.Name;
            Palettes = collection.Palettes.ToArray();
            return this;
        }
    }

    [Serializable]
    public class ColorPalette
    {
        public string Name;
        [ColorUsage(true, true)] public Color MainColor = new Color(1, 1, 1, 1);
        [ColorUsage(true, true)] public Color SecondColor = new Color(0, 0, 0, 1);

        public ColorPalette()
        {
            Name = "NEW COLOR PALETTE";
            MainColor = new Color(1, 1, 1, 1);
            SecondColor = new Color(0, 0, 0, 1);
        }
    }
}
