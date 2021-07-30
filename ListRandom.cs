using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Static Value
namespace RandomList
{
    public class ListRandom<T>: IEnumerable<T>, IList<T>, IList, IReadOnlyList<T>, IReadOnlyCollection<T>
    {
        public class ListRandomIEnumerator: IEnumerator<T>
        {
            ListRandom<T> random;
            T current;
            public ListRandomIEnumerator(ListRandom<T> that)
            {
                random = that;
            }
            public T Current => current;

            object IEnumerator.Current => current;

            public void Dispose()
            {
                random = null;
                current = default(T);
            }

            public bool MoveNext()
            {
                if (random != null && random.NoTouched != 0)
                {
                    current = random.GetRandomValue(true);
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                if (random != null)
                {
                    random.Reset();
                }
            }
        }
        T[] _array;
        BitArray valuestouched;
        int noTouched;
        Random random;
        public ListRandom(T[] values)
        {
            Inicialize(values);
        }
        public ListRandom(IEnumerable<T> values)
        {
            Inicialize(values.ToArray());
        }
        void Inicialize(T[] value)
        {
            _array = value;
            noTouched = (_array != null) ? _array.Length : 0;
            valuestouched = new BitArray(noTouched);
            random = new Random(DateTime.Now.Millisecond);
        }
        void RecreateArray(T[] value)
        {
            _array = value;
            noTouched = (_array != null) ? _array.Length : 0;
            valuestouched = new BitArray(noTouched);
        }
        public IEnumerator<T> GetEnumerator()
        {
            return new ListRandomIEnumerator(this);
        }

        public T GetRandomValue(bool ignoretouched)
        {
            if (Count != 0 && this != null && _array != null)
            {
                var capacity = Count;
                int max = (ignoretouched)? noTouched: capacity;
                var index = random.Next(0, max);
                if (ignoretouched)
                {
                    if (noTouched == 0)
                    {
                        return default(T);
                    }
                    noTouched--;
                }
                var lengtharr = capacity;
                var i2 = 0;
                for (int i = 0; i < lengtharr; i++)
                {
                    if (ignoretouched && valuestouched[i])
                    {
                        continue;
                    }
                    if (i2 == index)
                    {
                        if (ignoretouched)
                        {
                            valuestouched[i] = true;
                        }
                        return _array[i];
                    }
                    i2++;
                }
            }
            return default(T);
        }
        public T[] CreateRandomNewArray()
        {
            if (noTouched != 0 && this != null && _array != null)
            {
                T[] ret = new T[_array.Length];
                int i = 0;
                foreach (var val in this)
                {
                    ret[i] = val;
                    i++;
                }
                return ret;
            }
            List<byte> e;
            return null;
        }
        public T[] CreateRandomNewArray(int size)
        {
            if (Count != 0 && this != null && _array != null)
            {
                T[] ret = new T[size];
                for (int i = 0; i < size; i++)
                {
                    ret[i] = GetRandomValue(false);
                }
                return ret;
            }

            return null;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ListRandomIEnumerator(this);
        }
        public void Reset()
        {
            noTouched = (_array != null ) ? _array.Length : 0;
            valuestouched.SetAll(false);

        }

        public int IndexOf(T item)
        {
            return Array.IndexOf(_array, item);
        }

        public void Insert(int index, T item)
        {
            var count = Count;
            var len = count + 1;
            var arr = new T[len];
            arr[index] = item;
            for (int i = 0; i < count; i++)
            {
                var i2 = i;
                if (count >= index)
                {
                    i2 += 1;
                }
                arr[i2] = _array[i];
            }
        }

        public void RemoveAt(int index)
        {
            var item = _array[index];
            Remove(item);
        }

        public void Add(T item)
        {
            var arr = _array.Concat(new T[] { item }).ToArray();
            RecreateArray(arr);
        }
        public void AddRange(T[] item)
        {
            var arr = _array.Concat(item).ToArray();
            RecreateArray(arr);
        }
        public void Clear()
        {
            RecreateArray(new T[0]);
        }

        public bool Contains(T item)
        {
            return _array.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(_array, 0, array, arrayIndex, _array.Length - arrayIndex);
        }

        public bool Remove(T item)
        {
            var arr = _array.Where((val) => !val.Equals(item)).ToArray();
            var flag = arr.Length != _array.Length;
            RecreateArray(arr);
            return flag;
        }

        public int Add(object value)
        {
            Add((T)value);
            return Count-1;
        }

        public bool Contains(object value)
        {
            return Contains((T)value);
        }

        public int IndexOf(object value)
        {
            return IndexOf((T)value);
        }

        public void Insert(int index, object value)
        {
            Insert(index, (T)value);
        }

        public void Remove(object value)
        {
            Remove((T)value);
        }

        public void CopyTo(Array array, int index)
        {
            CopyTo(array,index);
        }

        public int NoTouched
        {
            get
            {
                return noTouched;
            }
        }
        public int Count
        {
            get
            {
                return _array != null ? _array.Length : 0;
            }
        }


        public bool IsReadOnly => false;

        public bool IsFixedSize => false;

        public object SyncRoot => false;

        public bool IsSynchronized => false;

        object IList.this[int index] { get => this[index]; set => this[index] = (T)value; }
        public T this[int index] { get => _array[index]; set => _array[index] = value; }
    }
}
