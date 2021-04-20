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
            previewStyle.normal.background = EditorGUIUtility.Load
            ("Assets/AssetPacks/GUI/GUI Pro Kit Fantasy RPG/Sprites/99_Popup/common_popup.png") as Texture2D;
            previewStyle.padding = new RectOffset(30, 20, 10, 40);
            previewStyle.border = new RectOffset(80, 80, 120, 120);
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
                    fontSize = 20,
                    alignment = TextAnchor.MiddleCenter,
                    font = EditorGUIUtility.Load("Assets/AssetPacks/GUI/GUI Pro Kit Fantasy RPG/Font/Alata-Regular.ttf") as Font
                };
                headerStyle = new GUIStyle(descriptionStyle) 
                {
                    fontSize = 35,
                    font = EditorGUIUtility.Load("Assets/AssetPacks/GUI/GUI Pro Kit Fantasy RPG/Font/Alata-Regular.ttf") as Font
                };
                stylesInitialized = true;
            }
            Rect rect = new Rect(0,0, position.width * .65f, position.height );
            DrawInspector(rect);
            rect.x = rect.width;
            rect.width /= 2.0f;
            DrawPreviewTooltip(rect);    
        }

        private void DrawPreviewTooltip(Rect rect)
        {
            Color oldColor = GUI.backgroundColor;
            GUI.backgroundColor = new Color(1,0.5f,0);
            GUILayout.BeginArea(rect, previewStyle);
            EditorGUILayout.BeginVertical();
            headerStyle.fixedWidth = rect.width - 60;
            EditorGUILayout.LabelField(selected.GetDisplayName(), headerStyle);
            if (selected.GetIcon() != null)
            {
                float iconSize = Mathf.Min(rect.width * .33f, rect.height * .33f);
                Rect texRect = GUILayoutUtility.GetRect(iconSize, iconSize);
                GUI.DrawTexture(texRect, selected.GetIcon().texture, ScaleMode.ScaleToFit);
            }
            descriptionStyle.fixedWidth = rect.width - 60;
            EditorGUILayout.LabelField(selected.GetDescription(), descriptionStyle);
            EditorGUILayout.EndVertical();
            GUILayout.EndArea();
            GUI.backgroundColor = oldColor;
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
