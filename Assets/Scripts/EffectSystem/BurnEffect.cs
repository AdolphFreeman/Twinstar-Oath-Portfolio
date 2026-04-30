using UnityEngine;

namespace EffectSystem
{
    // 繼承自 StatusEffect
    public class BurnEffect : StatusEffect
    {
        // 這是這個狀態專屬的變數，例如每次燃燒扣多少血
        private float _burnDamagePerTick = 10f; 

        // 建構子：必須有這段來接收基礎資料 (StatusEffectData)
        public BurnEffect(StatusEffectData data) : base(data)
        {
        }

        // 當狀態「剛套用」到角色身上時執行
        protected override void OnApply()
        {
            Debug.Log($"{Target.gameObject.name} 著火了！");
            // 你也可以在這裡取得角色的 ParticleSystem 並播放火焰特效
        }

        // 當狀態每次「跳動 (Tick)」時執行。
        // 多久跳一次取決於你在 Unity 編輯器設定的 TickInterval。
        protected override void OnTick()
        {
            // 呼叫 IEffectable 介面裡的扣血方法，並根據疊加層數 (CurrentStacks) 增加傷害
            float totalDamage = _burnDamagePerTick * CurrentStacks;
            Target.TakeDamageFromEffect(totalDamage, "Burn");
            
            Debug.Log($"燃燒造成了 {totalDamage} 點傷害給 {Target.gameObject.name}。");
        }

        // 當狀態「結束」或被移除時執行
        protected override void OnRemove()
        {
            Debug.Log($"{Target.gameObject.name} 身上的火熄滅了。");
        }
    }
}