using UnityEngine;

namespace EffectSystem
{
    /// <summary>
    /// Example implementation of a Stat Modifying effect (e.g., speed buff or attack debuff).
    /// </summary>
    public class StateModifierEffect : StatusEffect
    {
        // Example: How much to modify speed. Usually this would be defined in a subclass of StatusEffectData.
        private float _speedModifier = -0.5f; // 50% slow

        public StateModifierEffect(StatusEffectData data) : base(data)
        {
            
        }

        protected override void OnApply()
        {
            Debug.Log($"{Data.EffectName} applied! Stat modified on {Target.gameObject.name}.");
            // Example: Target.ModifySpeed(_speedModifier * CurrentStacks);
        }

        protected override void OnRemove()
        {
            Debug.Log($"{Data.EffectName} removed! Stat restored on {Target.gameObject.name}.");
            // Example: Target.ModifySpeed( 1f / (_speedModifier * CurrentStacks) ); // Restore original speed
        }
    }
}
