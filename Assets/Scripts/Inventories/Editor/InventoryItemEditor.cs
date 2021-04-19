using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace GameDevTV.Inventories.Editor
{
    public class InventoryItemEditor : EditorWindow
    {
        InventoryItem selected = null;
        GUIStyle previewStyle;
        GUIStyle descriptionStyle;
        GUIStyle headerStyle;
        bool stylesInitialized = false;
        Vector2 scrollPosition;

        private void OnEnable()
        {
            previewStyle = new GUIStyle();
            var tex2D = EditorGUIUtility.Load
            ("Assets/AssetPacks/GUI/GUI Pro Kit Fantasy RPG/Sprites/99_Popup/common_popup.png") as Texture2D;
            Color myColor = new Color(255, 168, 0);
            previewStyle.normal.background = tex2D;
            previewStyle.padding = new RectOffset(40, 40, 40, 40);
            previewStyle.border = new RectOffset(0, 0, 0, 0);
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; i++)
            {
                pix[i] = col;
            }
            Texture2D result = EditorGUIUtility.Load
            ("Assets/AssetPacks/GUI/GUI Pro Kit Fantasy RPG/Sprites/99_Popup/common_popup.png") as Texture2D;
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        [MenuItem("Window/InventoryItem Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(InventoryItemEditor), false, "InventoryItem");
        }

        public static void ShowEditorWindow(InventoryItem candidate)
        {
            InventoryItemEditor window = GetWindow(typeof(InventoryItemEditor),
            false, "InventoryItem") as InventoryItemEditor;

            if (candidate)
            {
                window.OnSelectionChange();

            }
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            InventoryItem candidate = EditorUtility.InstanceIDToObject(instanceID) as InventoryItem;
            if (candidate != null)
            {
                ShowEditorWindow(candidate);
                return true;
            }
            return false;
        }

        private void OnSelectionChange()
        {
            var candidate = EditorUtility.InstanceIDToObject(Selection.activeInstanceID) as InventoryItem;
            if (candidate == null) return;
            selected = candidate;
            Repaint();
        }

        private void OnGUI()
        {
            if (!selected)
            {
                EditorGUILayout.HelpBox("No InventoryItem Selected", MessageType.Error);
                return;
            }
            if (!stylesInitialized)
            {
                descriptionStyle = new GUIStyle(GUI.skin.label)
                {
                    richText = true,
                    wordWrap = true,
                    stretchHeight = true,
                    fontSize = 14,
                    alignment = TextAnchor.MiddleCenter
                };
                headerStyle = new GUIStyle(descriptionStyle) {fontSize = 24};
                stylesInitialized = true;
            }
            Rect rect = new Rect(0,0, position.width * .65f, position.height);
            DrawInspector(rect);
            rect.x = rect.width;
            rect.width /= 2.0f;
            DrawPreviewTooltip(rect);    
        }

        private void DrawPreviewTooltip(Rect rect)
        {
            GUILayout.BeginArea(rect, previewStyle);
            if (selected.GetIcon() != null)
            {
                float iconSize = Mathf.Min(rect.width * .33f, rect.height * .33f);
                Rect texRect = GUILayoutUtility.GetRect(iconSize, iconSize);
                GUI.DrawTexture(texRect, selected.GetIcon().texture, ScaleMode.ScaleToFit);
            }
            EditorGUILayout.LabelField(selected.GetDisplayName(), headerStyle);
            EditorGUILayout.LabelField(selected.GetDescription(), descriptionStyle);
            GUILayout.EndArea();
        }

        private void DrawInspector(Rect rect)
        {
           GUILayout.BeginArea(rect);
           scrollPosition = GUILayout.BeginScrollView(scrollPosition);
           selected.DrawCustomInspector();
           GUILayout.EndScrollView();
           GUILayout.EndArea();
        }
    }
}
