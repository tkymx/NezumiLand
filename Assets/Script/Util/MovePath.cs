using UnityEngine;

namespace NL
{
    [System.Serializable]
    public class MovePathEntry
    {
        public Position3Entry AppearPosition;
        public Position3Entry DisappearPosition;
    }

    public class MovePath
    {
        public Vector3 AppearPosition { get; private set; }
        public Vector3 DisapearPosition { get; private set; }
        public MovePath(Vector3 appearPosition, Vector3 disaapearPosition)
        {
            this.AppearPosition = appearPosition;
            this.DisapearPosition = disaapearPosition;            
        }
    }
}