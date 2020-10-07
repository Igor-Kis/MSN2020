using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Additionals
{
    #region Обертка для List

    public class ColectionDebugView<T>
    {
        private ICollection<T> _collection;


        public ColectionDebugView(ICollection<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            _collection = collection;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                T[] items = new T[_collection.Count];
                _collection.CopyTo(items, 0);
                return items;
            }
        }
    }

    /// <summary>
    /// Функционирует как List но в DataTip передает свой ToString()
    /// </summary>
    [DebuggerDisplay("{ToString()}")]
    [DebuggerTypeProxy(typeof(ColectionDebugView<>))]
    public class DebugViewList<T> : List<T> { }

    #endregion
}