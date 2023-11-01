using UnityEngine;

namespace ArcaneSurvivorsClient {
    public enum DisplayNumberType {
        Plane,
        WithComma,
        MetricPrefix
    }

    public static class NumberUtility {
        public static string ToDisplayString(this int number, DisplayNumberType type) {
            switch (type) {
                case DisplayNumberType.Plane:
                    return number.ToString();
                case DisplayNumberType.WithComma:
                    return number.ToString("N0");
                case DisplayNumberType.MetricPrefix:
                    return FormatWithMetricPrefix(number);
                default:
                    return number.ToString();
            }
        }

        public static string ToDisplayString(this float number, DisplayNumberType type) {
            switch (type) {
                case DisplayNumberType.Plane:
                    return number.ToString();
                case DisplayNumberType.WithComma:
                    return number.ToString("N0");
                case DisplayNumberType.MetricPrefix:
                    return FormatWithMetricPrefix(number);
                default:
                    return number.ToString("0.0");
            }
        }

        private static string FormatWithMetricPrefix(float number) {
            const int metricBase = 1000;
            if (number < metricBase)
                return number.ToString(); // 1000 미만의 수는 메트릭 접두사를 사용하지 않고 그대로 반환

            // SI 접두사 배열 (1K, 1M, 1G, 1T 등)
            string[] prefixes = { "", "K", "M", "G", "T", "P", "E", "Z", "Y" };

            int exp = (int)(Mathf.Log(number) / Mathf.Log(metricBase));
            double normalizedNumber = (double)number / Mathf.Pow(metricBase, exp);

            return $"{normalizedNumber:F1}{prefixes[exp]}"; // 소수점 첫째 자리까지 반올림하여 접두사를 붙여 반환 (예: 1.2K)
        }
    }
}