using System.Collections.Generic;
using UnityEngine;

namespace UnityPlayerPrefsX.Wrappers
{
    public class ListWrapper<T>
    {
        [SerializeField] List<T> items; // read by reflection

        public ListWrapper(List<T> list)
        {
            items = list;
        }
    }
}