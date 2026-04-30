using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace EffectSystem
{
    /// <summary>
    /// Component attached to any GameObject that can have status effects (Player, Enemy, Boss).
    /// Requires the entity to also implement IEffectable.
    /// </summary>
    public class EffectManager : MonoBehaviour
    {
        private IEffectable _entity;
        private List<StatusEffect> _activeEffects = new List<StatusEffect>();

        private void Awake()
        {
            _entity = GetComponent<IEffectable>();
            if (_entity == null)
            {
                Debug.LogError($"EffectManager on {gameObject.name} requires a component implementing IEffectable!");
            }
        }

        private void Update()
        {
            // Iterate backwards so we can safely remove finished effects
            for (int i = _activeEffects.Count - 1; i >= 0; i--)
            {
                StatusEffect effect = _activeEffects[i];
                if (effect.IsFinished)
                {
                    _activeEffects.RemoveAt(i);
                    continue;
                }

                effect.UpdateEffect();
            }
        }

        /// <summary>
        /// Applies a new effect to this entity.
        /// </summary>
        /// <param name="effectDefinition">The data of the effect to construct.</param>
        /// <param name="effectSource">Optional instantiator of the effect to pass along.</param>
        public void ApplyEffect(StatusEffectData data, System.Type effectClassType)
        {
            if (data == null || _entity == null) return;

            // Check if we already have this effect
            StatusEffect existingEffect = _activeEffects.FirstOrDefault(e => e.Data.EffectName == data.EffectName);

            if (existingEffect != null)
            {
                // Refresh or stack the existing effect
                existingEffect.AddStack();
                Debug.Log($"Refreshed/Stacked Effect: {data.EffectName} on {_entity.gameObject.name}. Stacks: {existingEffect.CurrentStacks}");
            }
            else
            {
                // Create a new instance dynamically if passed a type
                if (!typeof(StatusEffect).IsAssignableFrom(effectClassType))
                {
                    Debug.LogError($"ApplyEffect failed: {effectClassType.Name} does not inherit from StatusEffect.");
                    return;
                }

                StatusEffect newEffect = (StatusEffect)System.Activator.CreateInstance(effectClassType, new object[] { data });
                newEffect.Initialize(_entity);
                _activeEffects.Add(newEffect);
                Debug.Log($"Applied New Effect: {data.EffectName} to {_entity.gameObject.name}");
                
                // Optional: Spawn visual effect
                if (data.VisualEffectPrefab != null)
                {
                    Instantiate(data.VisualEffectPrefab, transform.position, Quaternion.identity, transform);
                }
            }
        }

        /// <summary>
        /// Removes an effect early by name.
        /// </summary>
        public void RemoveEffect(string effectName)
        {
            StatusEffect existingEffect = _activeEffects.FirstOrDefault(e => e.Data.EffectName == effectName);
            if (existingEffect != null)
            {
                existingEffect.EndEffect(); // Sets IsFinished = true, which will remove it on next Update
                Debug.Log($"Removed Effect: {effectName} from {_entity.gameObject.name}");
            }
        }

        /// <summary>
        /// Clears all active status effects.
        /// </summary>
        public void ClearAllEffects()
        {
            foreach (var effect in _activeEffects)
            {
                effect.EndEffect();
            }
            _activeEffects.Clear();
            Debug.Log($"Cleared all effects from {_entity.gameObject.name}");
        }
    }
}
