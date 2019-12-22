using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL {

    public interface IConsumableAmount
    {
        IConsumableAmount Add_Implementation(IConsumableAmount right);
        IConsumableAmount Subtraction_Implementation(IConsumableAmount right);
    }
}