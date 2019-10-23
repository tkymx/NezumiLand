using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NL
{
    public class ArrangementTarget : IArrangementTarget
    {
        Vector3 centerPosition;
        float range;

        public ArrangementTarget(List<GameObject> gameObjectList, ArrangementInfo arrangementInfo)
        {
            // 中心座標
            centerPosition = new Vector3();
            foreach (var gameObject in gameObjectList)
            {
                centerPosition += gameObject.transform.position;
            }
            centerPosition = centerPosition / gameObjectList.Count();

            // 半径
            range = (arrangementInfo.mono.Height * ArrangementAnnotater.ArrangementHeight + arrangementInfo.mono.Width * ArrangementAnnotater.ArrangementWidth) / 2 / 2;
        }

        public float GetRange()
        {
            return range;
        }

        public Vector3 GetCenterPosition()
        {
            return centerPosition;
        }
    }
}