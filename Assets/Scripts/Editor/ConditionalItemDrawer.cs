using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using CustomAttributes;
using UnityEditor.Rendering;
using Enums;
using System;
using System.Linq;

[CustomPropertyDrawer(typeof(ConditionalItemAttribute))]
public class ConditionalItemDrawer : PropertyDrawer
{
    ConditionalItemAttribute conditionalItem;
    SerializedProperty comparedField;

    private float propertyHeight;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return propertyHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Set the global variables.
        conditionalItem = attribute as ConditionalItemAttribute;
        comparedField = property.serializedObject.FindProperty(conditionalItem.propertyName);
        var comparedFieldValue = comparedField.GetEnumName<Enum>();

        // Get the value of the compared field.
        propertyHeight = base.GetPropertyHeight(property, label);

        if (conditionalItem.propertyValue.ToList().Exists(f => f.ToString() == comparedFieldValue))
        {
            EditorGUI.PropertyField(position, property, label);
        }
        else
        {
            propertyHeight = 0f;
        }
    }
}
