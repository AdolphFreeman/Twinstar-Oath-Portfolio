using UnityEngine;

namespace EffectSystem
{
    /// <summary>
    /// Interface that any entity (Player, Enemy, Boss) must implement
    /// to be able to receive and handle status effects.
    /// </summary>
    public interface IEffectable
    {
        /// <summary>
        /// Required component reference to get the entity's transform or GameObject.
        /// </summary>
        GameObject gameObject { get; }
        Transform transform { get; }

        /// <summary>
        /// Handles receiving damage from effects (e.g., Poison).
        /// Override this logically in your existing Health system.
        /// </summary>
        void TakeDamageFromEffect(float amount, string effectSource = "");

        /// <summary>
        /// Add generic methods for stat modifications here if needed.
        /// e.g., void ModifySpeed(float multiplier);
        /// e.g., void ModifyAttackPower(float amount);
        /// </summary>
    }
}
