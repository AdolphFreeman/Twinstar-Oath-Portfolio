using UnityEngine;

namespace CraneFSM.Core
{
    public class BoolParameter : Parameter
    {
        public bool value;

        protected override void Update()
        {
            base.Update();
            name = $"{parameterName}(Bool): {Value}";
        }
        
        protected override object ReadValue()
        {
            return value;
        }
    }
}
