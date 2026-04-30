using System;
using System.Collections.Generic;
using CraneFSM.Core;
using UnityEngine;

namespace CraneFSM.Core
{
    [System.Serializable]
    public class TransitionCondition
    {
        public Parameter parameter;
        public ConditionType type;
        public string value;

        object GetValue()
        {
            if (parameter == null) return null;

            try
            {
                switch (parameter.Value)
                {
                    case int inValue:
                        return int.Parse(value);
                    case float:
                        return float.Parse(value);
                    case bool:
                        return bool.Parse(value);
                    case Vector2:
                        string[] vec2Str = value.Split(", "); 
                        return  new Vector2(float.Parse(vec2Str[0]), float.Parse(vec2Str[1]));
                    case Vector3:
                        string[] vec3Str = value.Split(", "); 
                        return  new Vector3(float.Parse(vec3Str[0]), float.Parse(vec3Str[1]), float.Parse(vec3Str[2]));
                    case TriggerClass:
                        if (bool.Parse(value))
                            return true;
                        return false;
                    default:
                        return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public bool Evaluate()
        {
            if (parameter == null) return false;

            object targetValue = GetValue();
            if (targetValue == null) return false;

            switch (parameter.Value)
            {
                case int:
                    int intVal = (int)targetValue;
                    int paramIntValue = (int)parameter.Value;
                    
                    if (type == ConditionType.Equal) return paramIntValue == intVal;
                    if (type == ConditionType.NotEqual) return paramIntValue != intVal;
                    if (type == ConditionType.Large) return paramIntValue > intVal;
                    if (type == ConditionType.Less) return paramIntValue < intVal;
                    break;
                case float:
                    float floatVal = (float)targetValue;
                    float paramFloatValue = (float)parameter.Value;
                    
                    if (type == ConditionType.Equal) return Mathf.Approximately(paramFloatValue, floatVal);
                    if (type == ConditionType.NotEqual) return !Mathf.Approximately(paramFloatValue, floatVal);
                    if (type == ConditionType.Large) return paramFloatValue > floatVal;
                    if (type == ConditionType.Less) return paramFloatValue < floatVal;
                    break;
                case bool:
                    bool boolVal = (bool)targetValue;
                    bool paramBoolValue = (bool)parameter.Value;
                    
                    if (type == ConditionType.Equal) return paramBoolValue == boolVal;
                    if (type == ConditionType.NotEqual) return paramBoolValue != boolVal;
                    break;
                case Vector2:
                    Vector2 vec2 = (Vector2)targetValue;
                    Vector2 paramVec2 = (Vector2)parameter.Value;
                    
                    if (type == ConditionType.Equal) return vec2 == paramVec2;
                    if (type == ConditionType.NotEqual) return vec2 != paramVec2;
                    break;
                case Vector3:
                    Vector3 vec3 = (Vector3)targetValue;
                    Vector3 paramVec3 = (Vector3)parameter.Value;
                    
                    if (type == ConditionType.Equal) return vec3 == paramVec3;
                    if (type == ConditionType.NotEqual) return vec3 != paramVec3;
                    break;
                case TriggerClass:
                    bool paramTriggerVal = ((TriggerClass)parameter.Value).isTrigger;
                    bool triggerVal = (bool)targetValue;

                    if (type == ConditionType.Equal) return paramTriggerVal == triggerVal;
                    if (type == ConditionType.NotEqual) return paramTriggerVal != triggerVal;
                    break;
            }
            return false;
        }
    }
    
    public enum ConditionType
    {
        Equal, NotEqual, Large, Less
    }
}