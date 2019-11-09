using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class SatisfactionCalculater
    {
        public static Satisfaction CalcFieldSatisfaction()
        {
            Satisfaction satisfaction = new Satisfaction(0);
            satisfaction += GameManager.Instance.MonoManager.GetAllSatisfaction();
            return satisfaction;
        }
    }
}

