using UnityEngine;

namespace EffectSystem
{
    /// <summary>
    /// Example implementation of a Damage Over Time (DoT) status effect like Poison or Burn.
    /// Needs a specific property or extra data setup for damage amount.
    /// </summary>
    public class DamageOverTimeEffect : StatusEffect
    {
        // For a generic generic implementation, you might want to extend StatusEffectData 
        // to hold damage values, or pass it via constructor. We'll use a hardcoded value 
        // to keep this simple, or you can expand the Data class.
        private float _damagePerTick = 5f; 

        public DamageOverTimeEffect(StatusEffectData data) : base(data)
        {
        }

        public DamageOverTimeEffect(StatusEffectData data, float damagePerTick) : base(data)
        {
            _damagePerTick = damagePerTick;
        }

        protected override void OnApply()
        {
            Debug.Log($"{Target.gameObject.name} started suffering from {Data.EffectName}.");
        }

        protected override void OnTick()
        {
            // Calculate total damage based on stacks
            float totalDamage = _damagePerTick * CurrentStacks;
            
            Target.TakeDamageFromEffect(totalDamage, Data.EffectName);
            Debug.Log($"{Data.EffectName} Ticked: Dealt {totalDamage} damage to {Target.gameObject.name}.");
        }

        protected override void OnRemove()
        {
            Debug.Log($"{Data.EffectName} has worn off from {Target.gameObject.name}.");
        }
    }
}
