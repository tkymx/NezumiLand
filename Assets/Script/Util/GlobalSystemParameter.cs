using System;

namespace NL
{
    public class GlobalSystemParameter
    {
        private float pinchMoveFactor = 0.1f;
        public float PinchMoveFactor => pinchMoveFactor;

#if DEVELOPMENT_BUILD
        public float OverridePinchMoveFactor(float factor)
        {
            this.pinchMoveFactor = factor;
        }    
#endif
    }
}