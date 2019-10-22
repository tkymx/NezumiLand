using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class ArrangementInfoGenerator
    {
        static public ArrangementInfo Generate(int x, int z)
        {
            var info = new ArrangementInfo()
            {
                x = x,
                z = z,
                mono = new Mono()
                {
                    Width = 2,
                    Height = 2,
                    monoPrefab = ResourceLoader.LoadPrefab("Model/branko"),
                }
            };

            return info;
        }
    }
}