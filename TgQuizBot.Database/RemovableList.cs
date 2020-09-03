using System.Collections;
using System.Collections.Generic;

namespace TgQuizBot.Database
{
    public class RemovableList<T> : IList<T>
    {
        private readonly List<T> _wrappedList = new List<T>();
        private readonly ISet<T> _deletedList = new HashSet<T>();

        public RemovableList() { }

        public RemovableList(int capacity)
        {
            _wrappedList = new List<T>(capacity);
        }

        public RemovableList(IEnumerable<T> items)
        {
            _wrappedList = new List<T>(items);
        }


        public IEnumerator<T> GetEnumerator()
        {
            return _wrappedList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            _wrappedList.Add(item);

            if (_deletedList.Contains(item))
                _deletedList.Remove(item);
        }

        public void Clear()
        {
            foreach (var item in _wrappedList)
                _deletedList.Add(item);

            _wrappedList.Clear();
        }

        public bool Contains(T item)
        {
            return _wrappedList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _wrappedList.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            var result = _wrappedList.Remove(item);
            if (result)
                _deletedList.Add(item);
            return result;
        }

        public int Count
        {
            get { return _wrappedList.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public int IndexOf(T item)
        {
            return _wrappedList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _wrappedList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            var item = _wrappedList[index];
            _wrappedList.RemoveAt(index);
            _deletedList.Add(item);
        }

        public T this[int index]
        {
            get { return _wrappedList[index]; }
            set { _wrappedList[index] = value; }
        }

        public IEnumerable<T> DeletedItems
        {
            get { return _deletedList; }
        }
    }
}
