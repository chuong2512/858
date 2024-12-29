using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(FloatReference))]
public class FloatReferenceDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        bool _useConstant = property.FindPropertyRelative(nameof(FloatReference.UseConstant)).boolValue;

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var _rect = new Rect(position.position, Vector2.one * 20f);

        if (EditorGUI.DropdownButton(_rect,
            new GUIContent(GetTexture()),
            FocusType.Keyboard, new GUIStyle()
            {
                fixedWidth = 50f,
                border = new RectOffset(1, 1, 1, 1)
            }))
        {
            GenericMenu _menu = new GenericMenu();

            _menu.AddItem(new GUIContent("Constant"),
            _useConstant,
            () => SetProperty(property, true));

            _menu.AddItem(new GUIContent("Variable"),
            !_useConstant,
            () => SetProperty(property, false));

            _menu.ShowAsContext();
        }

        position.position += Vector2.right * 20f;
        position.size -= Vector2.right * 20f;
        float _value = property.FindPropertyRelative(nameof(FloatReference.ConstantValue)).floatValue;

        if (_useConstant)
        {
            string _newValue = EditorGUI.TextField(position, _value.ToString());
            float.TryParse(_newValue, out _value);
            property.FindPropertyRelative(nameof(FloatReference.ConstantValue)).floatValue = _value;
        }
        else
        {
            EditorGUI.ObjectField(position, property.FindPropertyRelative(nameof(FloatReference.Variable)), GUIContent.none);
        }

        EditorGUI.EndProperty();
    }

    private void SetProperty(SerializedProperty property, bool value)
    {
        var _propertyValue = property.FindPropertyRelative(nameof(FloatReference.UseConstant));
        _propertyValue.boolValue = value;
        property.serializedObject.ApplyModifiedProperties();
    }

    private Texture GetTexture()
    {
       
        var _textures = Resources.FindObjectsOfTypeAll(typeof(Texture))
         .Where(t => t.name.ToLower().Contains("dropdown_icon")).
         Cast<Texture>().ToList();

        return _textures[0];
    }
}
