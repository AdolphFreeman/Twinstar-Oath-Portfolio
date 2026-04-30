using UnityEngine;

namespace EffectSystem
{
    /// <summary>
    /// Defines the properties of a status effect. This allows designers to create 
    /// new effects in the Unity Editor without writing code.
    /// </summary>
    [CreateAssetMenu(fileName = "New Status Effect", menuName = "Effect System/Status Effect Data")]
    public class StatusEffectData : ScriptableObject
    {
        [Tooltip("The name of the effect.")]
        public string EffectName;

        [Tooltip("Duration in seconds. Set to 0 for infinite duration or instant effects.")]
        public float Duration = 5f;

        [Tooltip("Maximum number of times this effect can be stacked. 1 means it cannot stack.")]
        public int MaxStacks = 1;

        [Tooltip("How often the effect ticks (in seconds). Relevant for damage over time (DoT) or healing over time (HoT). 0 means it does not tick.")]
        public float TickInterval = 0f;

        [Tooltip("Prefab to instantiate when the effect is applied (e.g., a visual effect).")]
        public GameObject VisualEffectPrefab;

        // You can add more generic data fields here, like:
        // public Sprite Icon;
        // public string Description;
    }
}
