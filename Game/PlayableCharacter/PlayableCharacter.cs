using System;
using ArcaneSurvivorsClient.Locale;
using UnityEditor;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    [Serializable]
    [CreateAssetMenu(fileName = "PlayableCharacter", menuName = "ScriptableObject/PlayableCharacter", order = 1)]
    public class PlayableCharacter : ScriptableObject, ILibraryData {
        public object Key => (int)type;

        public Color StatTitleColor {
            get {
                HSV hsv = ColorUtility.ToHSV(themeColor);
                hsv.saturation = 0.48f;
                hsv.value = 0.65f;
                return ColorUtility.ToColor(hsv);
            }
        }

        public PlayableCharacterType type;

        [LocaleKey]
        public string name;

        [LocaleKey]
        public string desc;

        public PawnAbility ability;
        public WeaponType startWeaponType;

        public Color themeColor;
        public Sprite cardIllust;
        public Sprite slotIllust;
    }
}