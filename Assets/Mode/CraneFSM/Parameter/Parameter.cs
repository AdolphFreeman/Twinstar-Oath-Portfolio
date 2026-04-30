using UnityEngine;

namespace CraneFSM.Core
{
    public class Parameter : MonoBehaviour
    {
        public string parameterName;
        public object Value { get; private set; }

        protected virtual void Awake()
        {
            RefreshValue();
        }

        protected virtual void Update()
        {
            //name = $"{parameterName}({Value.GetType()}): {Value}";
            RefreshValue();
        }

        public void RefreshValue()
        {
            Value = ReadValue();
        }

        protected virtual object ReadValue()
        {
            return Value;
        }
    }
}
