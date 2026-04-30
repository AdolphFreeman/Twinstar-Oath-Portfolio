using UnityEngine;

namespace EffectSystem
{
    /// <summary>
    /// The runtime instance of a status effect attached to an entity.
    /// Handles duration tracking, stacking, and tick intervals.
    /// </summary>
    public abstract class StatusEffect
    {
        public StatusEffectData Data { get; private set; }
        public IEffectable Target { get; private set; }

        public float CurrentDuration { get; private set; }
        public int CurrentStacks { get; private set; }
        private float _nextTickTime;
        public bool IsFinished { get; private set; }

        public StatusEffect(StatusEffectData data)
        {
            Data = data;
        }

        /// <summary>
        /// Initializes the effect when first applied to a target.
        /// </summary>
        public virtual void Initialize(IEffectable target)
        {
            Target = target;
            CurrentDuration = Data.Duration;
            CurrentStacks = 1;
            IsFinished = false;

            if (Data.TickInterval > 0)
            {
                _nextTickTime = Time.time + Data.TickInterval;
            }

            OnApply();
        }

        /// <summary>
        /// Logic executed when the effect is first applied.
        /// </summary>
        protected virtual void OnApply() { }

        /// <summary>
        /// Logic executed when an existing effect is applied again, increasing its stacks.
        /// </summary>
        public virtual void AddStack()
        {
            if (CurrentStacks < Data.MaxStacks)
            {
                CurrentStacks++;
            }
            // Usually, applying a stack also refreshes the duration
            CurrentDuration = Data.Duration;
        }

        /// <summary>
        /// Updates the effect's duration and triggers ticks if applicable.
        /// Returns false if the effect has expired.
        /// </summary>
        public virtual void UpdateEffect()
        {
            if (IsFinished) return;

            // Handle ticking (for DoTs/HoTs)
            if (Data.TickInterval > 0 && Time.time >= _nextTickTime)
            {
                OnTick();
                _nextTickTime = Time.time + Data.TickInterval;
            }

            // Handle duration
            if (Data.Duration > 0)
            {
                CurrentDuration -= Time.deltaTime;
                if (CurrentDuration <= 0)
                {
                    EndEffect();
                }
            }
        }

        /// <summary>
        /// Logic executed on every tick interval (if TickInterval > 0).
        /// </summary>
        protected virtual void OnTick() { }

        /// <summary>
        /// Forcibly ends the effect.
        /// </summary>
        public void EndEffect()
        {
            if (IsFinished) return;
            IsFinished = true;
            OnRemove();
        }

        /// <summary>
        /// Logic executed when the effect is removed or its duration expires.
        /// </summary>
        protected virtual void OnRemove() { }
    }
}
