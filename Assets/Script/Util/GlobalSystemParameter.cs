using System;

namespace NL
{
    public class GlobalSystemParameter
    {
        // Pinch

        private float pinchMoveFactor = -0.1f;
        public float PinchMoveFactor => pinchMoveFactor;

        private float pinchNearLength = 60;
        public float PinchNearLength => pinchNearLength; 

        private float pinchFarLength = 200;
        public float PinchFarLength => pinchFarLength; 

        // Rotation

        private float rotationMoveFactor = 0.035f;
        public float RotationMoveFactor => rotationMoveFactor;

#if DEVELOPMENT_BUILD || UNITY_EDITOR

        public void OverridePinchMoveFactor(float factor)
        {
            this.pinchMoveFactor = factor;
        }    

        public void OverridePinchNearLength(float length)
        {
            this.pinchNearLength = length;
        }    

        public void OverridePinchFarLength(float length)
        {
            this.pinchFarLength = length;
        }    

        public void OverrideRotationMoveFactor(float factor)
        {
            this.rotationMoveFactor = factor;
        }    

#endif
    }
}