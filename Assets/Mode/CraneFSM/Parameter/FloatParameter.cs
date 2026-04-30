using CraneFSM.Core;

namespace CraneFSM.Core
{
    public class FloatParameter : Parameter
    {
        public float value;
        
        protected override void Update()
        {
            base.Update();
            name = $"{parameterName}(Float): {Value}";
        }

        protected override object ReadValue()
        {
            return value;
        }
    }
}
