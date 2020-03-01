using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public interface IParkOpenCellElement
    {
        GameObject MainPrefab { get; }
    }   

    public class ParkOpenStartRewardListContentViewElement : IParkOpenCellElement
    {
        public GameObject MainPrefab { get; private set; }
        public string Name { get; private set; }
        public Sprite Icon { get; private set; }
        public string Count  { get; private set; }

        public ParkOpenStartRewardListContentViewElement(GameObject mainPrefab, string name, Sprite icon, string count)
        {
            this.MainPrefab = mainPrefab;
            this.Name = name;
            this.Icon = icon;
            this.Count = count;
        }
    }

    public class ParkOpenStartRewardListHeaderViewElement : IParkOpenCellElement
    {
        public GameObject MainPrefab { get; private set; }
        public string HeaderTitle { get; private set; }

        public ParkOpenStartRewardListHeaderViewElement(GameObject mainPrefab, string headerTitle)
        {
            this.MainPrefab = mainPrefab;
            this.HeaderTitle = headerTitle;
        }
    }    
}
