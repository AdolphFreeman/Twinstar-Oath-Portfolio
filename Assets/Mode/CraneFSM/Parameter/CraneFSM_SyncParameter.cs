using System;
using System.Reflection;
using CraneFSM.Core;
using UnityEngine;

public class CraneFSM_SyncParameter : MonoBehaviour
{
    public Component sourceComponent;
    public string sourceMemberName;
    public Parameter targetParameter;

    private FieldInfo _sourceField;
    private PropertyInfo _sourceProperty;

    private void Awake()
    {
        CacheSourceMember();
    }

    private void Update()
    {
        if (sourceComponent == null || targetParameter == null || string.IsNullOrWhiteSpace(sourceMemberName))
            return;

        if (_sourceField == null && _sourceProperty == null)
            CacheSourceMember();

        object sourceValue = GetSourceValue();
        if (sourceValue == null)
            return;

        ApplyValue(targetParameter, sourceValue);
        targetParameter.RefreshValue();
    }

    private void CacheSourceMember()
    {
        if (sourceComponent == null || string.IsNullOrWhiteSpace(sourceMemberName))
            return;

        BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
        Type type = sourceComponent.GetType();
        _sourceField = type.GetField(sourceMemberName, flags);
        _sourceProperty = type.GetProperty(sourceMemberName, flags);

        if (_sourceField == null && _sourceProperty == null)
        {
            Debug.LogError($"[{name}] Cannot find source member '{sourceMemberName}' on {type.Name}");
        }
    }

    private object GetSourceValue()
    {
        if (_sourceField != null)
            return _sourceField.GetValue(sourceComponent);

        if (_sourceProperty != null && _sourceProperty.CanRead)
            return _sourceProperty.GetValue(sourceComponent);

        return null;
    }

    private static void ApplyValue(Parameter parameter, object sourceValue)
    {
        switch (parameter)
        {
            case BoolParameter boolParameter when sourceValue is bool boolValue:
                boolParameter.value = boolValue;
                break;
            case IntegerParameter integerParameter when sourceValue is int intValue:
                integerParameter.value = intValue;
                break;
            case FloatParameter floatParameter when sourceValue is float floatValue:
                floatParameter.value = floatValue;
                break;
            case Vector2Parameter vector2Parameter when sourceValue is Vector2 vector2Value:
                vector2Parameter.value = vector2Value;
                break;
            case Vector3Parameter vector3Parameter when sourceValue is Vector3 vector3Value:
                vector3Parameter.value = vector3Value;
                break;
        }
    }
}
