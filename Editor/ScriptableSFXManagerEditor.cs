using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Text;
using System.IO;
using UnityEngine.Events;
using System.Reflection;

namespace Varollo.SFXManager.Editor
{
    [CustomEditor(typeof(ScriptableSFXManager))]
    public class ScriptableSFXManagerEditor : UnityEditor.Editor
    {
        private bool _subscribedToEvent;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10);

            bool generateCsClass = serializedObject.FindProperty("_generateCsClass").boolValue;
            bool autoSave = serializedObject.FindProperty("_autoSave").boolValue;

            if (generateCsClass)
            {
                GUILayout.BeginHorizontal();

                EditorGUI.BeginDisabledGroup(autoSave);

                if (GUILayout.Button("Save"))
                {
                    GenerateCsClass();
                }

                EditorGUI.EndDisabledGroup();

                serializedObject.FindProperty("_autoSave").boolValue = GUILayout.Toggle(autoSave, "Auto-Save");

                if (autoSave)
                {
                    EnableAutoSave();
                }
                else
                {
                    DisableAutoSave();
                }

                GUILayout.EndHorizontal();

                serializedObject.ApplyModifiedProperties();
            }
        }

        private void EnableAutoSave()
        {
            if (_subscribedToEvent) return;

            _subscribedToEvent = true;

            GetInspectorChangeEvent().AddListener(GenerateCsClass);
            GenerateCsClass();
        }

        private void DisableAutoSave()
        {
            if (!_subscribedToEvent) return;

            _subscribedToEvent = false;

            GetInspectorChangeEvent().RemoveListener(GenerateCsClass);
        }

        private UnityEvent GetInspectorChangeEvent() => target.GetType().GetField("_onInspectorChange", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(target) as UnityEvent;

        private string GetFullDefaultPath() => GetDefaultPath() + GetFileName();

        private string GetDefaultPath()
        {
            string path = AssetDatabase.GetAssetPath(target);

            path = path.Substring(0, path.LastIndexOf("/") + 1);

            return path;
        }

        private string GetFileName()
        {
            string fileName = target.name + "Tracks";
            return CamelCase(fileName);
        }

        private string CamelCase(string str)
        {
            var splitStr = str.Split(' ');

            str = "";
            foreach (var word in splitStr)
            {
                var firstLetterUpper = word[0].ToString().ToUpper() + word.Substring(1);
                str += firstLetterUpper;
            }

            return str;
        }

        private void GenerateCsClass()
        {
            var builder = new StringBuilder($"// This is a script generated from the {typeof(ScriptableSFXManager).Name} scriptable object. Do not edit manually\n\n");
            builder.AppendLine($"public static class {GetFileName()}");
            builder.AppendLine("{");

            var tracks = ((ScriptableSFXManager)target).GetTracks();

            foreach (var track in tracks)
            {
                builder.AppendLine($"public struct {track.Name}");
                builder.AppendLine("{");
                foreach (var sound in track.Sounds)
                {
                    builder.AppendLine($"public const string {CamelCase(sound.Name)} = \"{track.Name}_{sound.Name}\";");
                }
                builder.AppendLine("}");
                builder.AppendLine();
            }

            builder.Append("}");

            File.WriteAllText(GetFullDefaultPath() + ".cs", builder.ToString());
            AssetDatabase.Refresh();
        }
    }
}
