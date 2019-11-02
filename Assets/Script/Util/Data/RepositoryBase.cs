using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NL
{
    public class RepositoryBase<T>
    {
        protected IList<T> entrys;

        protected RepositoryBase(IList<T> entrys)
        {
            this.entrys = entrys;
        }
    }
}

