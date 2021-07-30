using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomList
{
    public class ListRandom<T>: IEnumerable<T>
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
                if (random != null && random.Length != 0)
                {
                    current = random.GetRandomValue();
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                random.Reset();
            }
        }
        T[] _array;
        bool[] valuestouched;
        int length;
        Random random;
        public ListRandom(T[] values)
        {
            _array = values;
            length = values.Length;
            valuestouched = new bool[length];
            random = new Random(DateTime.Now.Millisecond);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ListRandomIEnumerator(this);
        }

        public T GetRandomValue()
        {
            var index = random.Next(0, length);
            length--;
            var lengtharr = _array.Length;
            var i2 = 0;
            for (int i = 0; i < lengtharr; i++)
            {
                if (valuestouched[i])
                {
                    continue;
                }
                if (i2 == index)
                {
                    valuestouched[i] = true;
                    return _array[i];
                }
                i2++;
            }
            return default(T);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ListRandomIEnumerator(this);
        }
        public void Reset()
        {
            length = _array.Length;
            for (int i = 0; i < length; i++)
            {
                valuestouched[i] = false;
            }
        }
        public int Length
        {
            get
            {
                return length;
            }
        }
    }
}
