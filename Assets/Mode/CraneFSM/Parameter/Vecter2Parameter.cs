using CraneFSM.Core;
using UnityEngine;

namespace CraneFSM.Core
{
    public class Vector2Parameter : Parameter
    {
        public Vector2 value;

        protected override void Update()
        {
            base.Update();
            name = $"{parameterName}(Vector2): {Value}";
        }
        
        protected override object ReadValue()
        {
            return value;
        }
    }
}
