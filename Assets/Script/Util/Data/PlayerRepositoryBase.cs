using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NL
{
    public class PlayerRepositoryBase<T>
    {
        protected List<T> entrys;

        protected PlayerRepositoryBase(IList<T> entrys)
        {
            this.entrys = entrys.ToList();
        }
    }
}

