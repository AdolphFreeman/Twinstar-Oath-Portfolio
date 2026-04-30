using UnityEngine;

namespace CraneFSM.Core
{
    public class TriggerParameter : Parameter
    {
        public TriggerClass value;

        protected override void Update()
        {
            base.Update();
            name = $"{parameterName}(Trigger): {value.isTrigger}";
        }

        protected override object ReadValue()
        {
            return value;
        }

        public void Set()
        {
            value.Set();
        }

        public void Reset()
        {
            value.Reset();
        }
    }

    [System.Serializable]
    public class TriggerClass
    {
        public bool isTrigger;

        public void Set() => isTrigger = true;
        public void Reset() => isTrigger = false;
    }
}
