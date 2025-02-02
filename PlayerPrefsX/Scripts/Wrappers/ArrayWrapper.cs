using UnityEngine;

namespace UnityPlayerPrefsX.Wrappers
{
    public class ArrayWrapper<T>
    {
        [SerializeField] T[] items; // read by reflection

        public ArrayWrapper(T[] array)
        {
            items = array;
        }
    }
}