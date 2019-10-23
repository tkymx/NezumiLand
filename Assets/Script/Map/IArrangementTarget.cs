using System;
using UnityEngine;

namespace NL
{
    public interface IArrangementTarget
    {
        Vector3 GetCenterPosition();
        float GetRange();
    }
}