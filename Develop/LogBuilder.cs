using System;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public static class LogBuilder {
        public static void Log(LogType type, string path, string message, params LogElement[] parameters) {
            string logText = Build(type, path, message, parameters);
            switch (type) {
                default:
                case LogType.Info:
                    Debug.Log(logText);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(logText);
                    break;
                case LogType.Error:
                    Debug.LogError(logText);
                    break;
            }
        }

        public static string Build(LogType type, string path, string message, params LogElement[] parameters) {
            string log = "";
            switch (type) {
                case LogType.Info:
                    log += "[INFO] ";
                    break;
                case LogType.Warning:
                    log += "[WARNING] ";
                    break;
                case LogType.Error:
                    log += "[ERROR] ";
                    break;
            }

            log += $"[{path}] {message}";
            foreach (LogElement param in parameters) {
                log += $"\n{param.name} : {param.value}";
            }

            return log;
        }

        public static Exception BuildException(LogType type, string path, string message,
            params LogElement[] elements) {
            string log = "";
            switch (type) {
                case LogType.Info:
                    log += "[INFO] ";
                    break;
                case LogType.Warning:
                    log += "[WARNING] ";
                    break;
                case LogType.Error:
                    log += "[ERROR] ";
                    break;
            }

            log += $"[{path}] {message}";
            foreach (LogElement element in elements) {
                log += $"\n{element.name} : {element.value}";
            }

            return new Exception(log);
        }
    }

    public struct LogElement {
        public string name;
        public string value;

        public LogElement(string name, string value) {
            this.name = name;
            this.value = value;
        }
    }

    public enum LogType {
        Info,
        Warning,
        Error
    }
}