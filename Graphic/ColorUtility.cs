using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public static class ColorUtility {
        public static string ToHex(this Color32 color) {
            return color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2") + color.a.ToString("X2");
        }

        public static Color32 ToColor(this string hex) {
            hex = hex.Replace("0x", "");
            hex = hex.Replace("#", "");

            byte a = 255;
            byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);

            if (hex.Length == 8) { a = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber); }

            return new Color32(r, g, b, a);
        }

        public static Color32 ToColor(this HSV hsv) {
            hsv.hue = Mathf.Clamp(hsv.hue, 0f, 360f);
            hsv.saturation = Mathf.Clamp(hsv.saturation, 0f, 1f);
            hsv.value = Mathf.Clamp(hsv.value, 0f, 1f);

            float hD60 = hsv.hue / 60;
            double f = hD60 - Mathf.Floor(hD60);

            hsv.value = hsv.value * 255;
            byte v = (byte)hsv.value;
            byte p = (byte)(hsv.value * (1 - hsv.saturation));
            byte q = (byte)(hsv.value * (1 - f * hsv.saturation));
            byte t = (byte)(hsv.value * (1 - (1 - f) * hsv.saturation));

            int hi = (int)Mathf.Floor(hD60) % 6;

            byte alpha = (byte)Mathf.Clamp(hsv.alpha * 255f, 0, 255);
            if (hi == 0) { return new Color32(v, t, p, alpha); } else if
                (hi == 1) { return new Color32(q, v, p, alpha); } else if
                (hi == 2) { return new Color32(p, v, t, alpha); } else if (hi == 3) {
                return new Color32(p, q, v, alpha);
            } else if (hi == 4) { return new Color32(t, p, v, alpha); } else { return new Color32(v, p, q, alpha); }
        }

        public static HSV ToHSV(this Color32 color) {
            float hue, saturation, value, alpha;

            int max = Mathf.Max(color.r, Mathf.Max(color.g, color.b));
            int min = Mathf.Min(color.r, Mathf.Min(color.g, color.b));
            float delta = max - min;
            if (delta == 0) {
                hue = 0;
                saturation = 0;
            } else {
                saturation = 1f - 1f * min / max;
                if (Mathf.Abs(delta) < float.Epsilon) { delta = float.Epsilon; }

                if (color.r == max) { hue = (color.g - color.b) / delta; } else if (color.g == max) {
                    hue = 2 + (color.b - color.r) / delta;
                } else { hue = 4 + (color.r - color.g) / delta; }

                hue *= 60;
                if (hue < 0) { hue += 360; }
            }

            alpha = color.a / 255f;

            value = max / 255f;

            return new HSV(hue, saturation, value, alpha);
        }

        public static float GetHashFloat(this string text) {
            const float _765To1 = 0.001307f;
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
            if (hash.Length < 3) {
                if (hash.Length == 0) { hash = new byte[] { 0, 0, 0 }; } else if (hash.Length == 1) {
                    hash = new byte[] { hash[0], 0, 0 };
                } else if (hash.Length == 2) { hash = new byte[] { hash[0], hash[1], 0 }; }
            }

            return ((float)hash[0] + hash[1] + hash[2]) * _765To1;
        }

        public static Color32 GetHashColor(this string Strtextng, float saturation = 0.5f, float value = 0.9f) {
            const float _765To360 = 0.470588f;
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(Strtextng));
            if (hash.Length < 3) {
                if (hash.Length == 0) { hash = new byte[] { 0, 0, 0 }; } else if (hash.Length == 1) {
                    hash = new byte[] { hash[0], 0, 0 };
                } else if (hash.Length == 2) { hash = new byte[] { hash[0], hash[1], 0 }; }
            }

            float hue = ((float)hash[0] + hash[1] + hash[2]) * _765To360;
            return ToColor(new HSV(hue, saturation, value));
        }
    }
}