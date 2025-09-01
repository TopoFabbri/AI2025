#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Pathfinder;
using Pathfinder.Coordinate;

[CustomPropertyDrawer(typeof(Coordinate))]
public class CoordinateDrawer : PropertyDrawer
{
    // Centralized layout configuration
    public static class Layout
    {
        // Indentation
        public const float IndentWidthPerLevel = 0f;
        public const bool RespectIndentForLabel = true;

        // Label area
        public const bool UseEditorLabelWidth = true; // if false, uses customLabelWidth
        public const float CustomLabelWidth = 140f;

        // Field split and spacing
        public const float FieldsPadding = 0f; // gap between X and Y
        public const float XWidthRatio = 0.5f; // portion of field area width for X (0..1)
        public const float MinFieldWidth = 0f; // clamp to avoid negative/narrow widths

        // Vertical layout
        public const int MiniLabelsThresholdLines = 2; // show mini labels when height >= threshold * singleLineHeight
        public const float MiniLabelGap = 0f; // gap between mini-label and field
        public const bool UseSingleLineHeight = true; // if false, use provided rect height

        public static float EffectiveLabelWidth => UseEditorLabelWidth ? EditorGUIUtility.labelWidth : CustomLabelWidth;

        public static float LineHeight => EditorGUIUtility.singleLineHeight;

        public static float IndentOffset => RespectIndentForLabel ? EditorGUI.indentLevel * IndentWidthPerLevel : 0f;

        public static bool ShouldShowMiniLabels(float totalHeight)
        {
            return totalHeight >= (LineHeight * MiniLabelsThresholdLines);
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Draw label and reserve space for it using layout vars
        float indentOffset = Layout.IndentOffset;
        float labelWidth = Layout.EffectiveLabelWidth;
        float lineH = Layout.UseSingleLineHeight ? Layout.LineHeight : position.height;

        Rect labelRect = new Rect(position.x + indentOffset, position.y, Mathf.Max(0f, labelWidth - indentOffset), lineH);
        EditorGUI.LabelField(labelRect, label);

        // Field area after label
        float fieldsX = position.x + labelWidth;
        float fieldsW = Mathf.Max(0f, position.width - labelWidth);
        Rect fieldRect = new Rect(fieldsX, position.y, fieldsW, lineH);

        // Don't indent child fields
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Unity serializes auto-properties with [field: SerializeField] using backing field names: <X>k__BackingField
        SerializedProperty xProp = property.FindPropertyRelative("<X>k__BackingField");
        SerializedProperty yProp = property.FindPropertyRelative("<Y>k__BackingField");

        // Fallback if someone later changes to public fields named X/Y
        if (xProp == null) xProp = property.FindPropertyRelative("X");
        if (yProp == null) yProp = property.FindPropertyRelative("Y");

        // Width split using ratio and padding
        float pad = Mathf.Max(0f, Layout.FieldsPadding);
        float xWidth = Mathf.Max(Layout.MinFieldWidth, (fieldRect.width - pad) * Mathf.Clamp01(Layout.XWidthRatio));
        float yWidth = Mathf.Max(Layout.MinFieldWidth, (fieldRect.width - pad) - xWidth);

        Rect xRect = new Rect(fieldRect.x, fieldRect.y, xWidth, fieldRect.height);
        Rect yRect = new Rect(fieldRect.x + xWidth + pad, fieldRect.y, yWidth, fieldRect.height);

        bool showMiniLabels = Layout.ShouldShowMiniLabels(position.height);

        if (showMiniLabels)
        {
            GUIStyle miniLabelStyle = EditorStyles.miniLabel;
            Rect xLabel = new Rect(xRect.x, xRect.y, xRect.width, miniLabelStyle.lineHeight);
            Rect yLabel = new Rect(yRect.x, yRect.y, yRect.width, miniLabelStyle.lineHeight);
            EditorGUI.LabelField(xLabel, "X", miniLabelStyle);
            EditorGUI.LabelField(yLabel, "Y", miniLabelStyle);

            float fieldY = xLabel.y + miniLabelStyle.lineHeight + Layout.MiniLabelGap;
            float fieldH = Mathf.Max(0, xRect.height - miniLabelStyle.lineHeight - Layout.MiniLabelGap);
            xRect = new Rect(xRect.x, fieldY, xRect.width, fieldH);
            yRect = new Rect(yRect.x, fieldY, yRect.width, fieldH);

            EditorGUI.PropertyField(xRect, xProp, GUIContent.none);
            EditorGUI.PropertyField(yRect, yProp, GUIContent.none);
        }
        else
        {
            EditorGUI.PropertyField(xRect, xProp, new GUIContent("X"));
            EditorGUI.PropertyField(yRect, yProp, new GUIContent("Y"));
        }

        // Reset indent
        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Keep single line by default; can be toggled via Layout.useSingleLineHeight
        return Layout.UseSingleLineHeight ? Layout.LineHeight : EditorGUIUtility.singleLineHeight;
    }
}
#endif
