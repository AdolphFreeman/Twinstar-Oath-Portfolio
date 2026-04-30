using CraneFSM.Core;

namespace CraneFSM.Core
{
    public class IntegerParameter : Parameter
    {
        public int value;

        protected override void Update()
        {
            base.Update();
            name = $"{parameterName}(Integer): {Value}";
        }
        
        protected override object ReadValue()
        {
            return value;
        }
    }
}
