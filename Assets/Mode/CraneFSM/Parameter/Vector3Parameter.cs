using CraneFSM.Core;
using UnityEngine;

namespace CraneFSM.Core
{
    public class Vector3Parameter : Parameter
    {
        public Vector3 value;

        protected override void Update()
        {
            base.Update();
            name = $"{parameterName}(Vector 3): {Value}";
        }
        
        protected override object ReadValue()
        {
            return value;
        }
    }
}
