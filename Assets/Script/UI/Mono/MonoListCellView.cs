﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NL
{
    public class MonoListCellView : ListCellViewBase
    {
        [SerializeField]
        private Text monoName = null;

        [SerializeField]
        private Text makingFree = null;

        [SerializeField]
        private Text appearCount = null;

        [SerializeField]
        private Text appearMaxCount = null;

        public void UpdateCell(string name, Currency free, long appearCount, long appearMaxCount)
        {
            this.monoName.text = name;
            this.makingFree.text = free.ToString();
            this.appearCount.text = appearCount.ToString();
            this.appearMaxCount.text = appearMaxCount.ToString();
        }
    }
}