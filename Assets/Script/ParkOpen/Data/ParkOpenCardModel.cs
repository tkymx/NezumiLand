using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    /// <summary>
    /// 公園開放時のカードのモデル
    /// </summary>
    public class ParkOpenCardModel : ModelBase
    {
        public string Name { get; private set; }
        public string ImageName { get; private set; }
        public string Description { get; private set; }
        public ParkOpenCardActionModel ParkOpenCardActionModel { get; private set; }

        public ParkOpenCardModel(
            uint id,
            string name,
            string imageName,
            string description,
            ParkOpenCardActionModel parkOpenCardActionModel
        )
        {
            this.Id = id;
            this.Name = name;
            this.ImageName = imageName;
            this.Description = description;
            this.ParkOpenCardActionModel = parkOpenCardActionModel;
        }
    }
}

