using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace lab3
{
    public abstract class BaseList<T> : IEnumerable<T> where T : IComparable<T>
    {
        public delegate void ListChangedEventHandler(object sender, EventArgs e);

        protected int count = 0;
        public int Count { get { return count; } }

        public abstract void Add(T item);
        public abstract void Insert(int pos, T item);
        public abstract void Delete(int pos);
        public abstract void Clear();
        public abstract T this[int i] { get; set; }

        public void Print()
        {
            foreach (var i in this)
            {
                Console.WriteLine(i);
            }
        }

        protected abstract BaseList<T> EmptyClone();

        public BaseList<T> Clone()
        {
            BaseList<T> clone = EmptyClone();
            clone.Assign(this);
            return clone;
        }

        public void Assign(BaseList<T> source)
        {
            Clear();
            for (int i = 0; i < source.Count; i++)
            {
                Add(source[i]);
            }
        }

        public void AssignTo(BaseList<T> dest)
        {
            dest.Assign(this);
        }

        public virtual void Sort()
        {
            for (int i = 0; i < count; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    if (this[i].CompareTo(this[j]) > 0)
                    {
                        T temp = this[i];
                        this[i] = this[j];
                        this[j] = temp;
                    }
                }
            }

        }

        public bool IsEqual(BaseList<T> array)
        {
            if (this.count != array.count)
            {
                return false;
            }
            for (int i = 0; i < this.count; i++)
            {
                if (this[i].CompareTo(array[i]) != 0)
                {
                    return false;
                }
            }
            return true;
        }

        public void SaveToFile(string fileName)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    for (int i = 0; i < count; i++)
                    {
                        writer.WriteLine(this[i].ToString());
                    }
                }
            }
            catch (IOException)
            {
                throw new BadFileException();
            }
        }

        public void LoadFromFile(string fileName)
        {
            try
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        T item = (T)Convert.ChangeType(line, typeof(T));
                        Add(item);
                    }
                }
            }
            catch (IOException)
            {
                throw new BadFileException();
            }
            catch (FormatException)
            {
                throw new BadFileException();
            }
        }

        // События для каждого типа изменения списка
        public event ListChangedEventHandler ItemChange;

        // Методы для вызова событий
        protected virtual void OnChange()
        {
            ItemChange?.Invoke(this, EventArgs.Empty);
        }

        public void ChangeArrayListEventHandlers()
        {
            ItemChange += (sender, e) => { };
        }

        public void ChangeChainListEventHandlers()
        {
            ItemChange += (sender, e) => { };
        }

        public static bool operator ==(BaseList<T> first, BaseList<T> second) { return first.IsEqual(second); }

        public static bool operator !=(BaseList<T> first, BaseList<T> second) { return !first.IsEqual(second); }

        public static BaseList<T> operator +(BaseList<T> first, BaseList<T> second)
        {
            BaseList<T> list = first.Clone();
            for (int i = 0; i < second.Count; i++)
            {
                list.Add(second[i]);
            }
            return list;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new BaseListEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class BaseListEnumerator : IEnumerator<T>
        {
            private BaseList<T> list;
            private int currentIndex = -1;

            public BaseListEnumerator(BaseList<T> list)
            {
                this.list = list;
            }

            public T Current => list[currentIndex];

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                currentIndex++;
                return currentIndex < list.Count;
            }

            public void Reset()
            {
                currentIndex = -1;
            }

            public void Dispose()
            {
            }
        }
    }

    public class BadIndexException : Exception
    {
        public BadIndexException() : base("exception")
        {
        }
    }

    public class BadFileException : Exception
    {
        public BadFileException() : base("exception")
        {
        }
    }
}