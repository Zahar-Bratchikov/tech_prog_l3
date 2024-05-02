using System;

namespace lab3
{
    public class ArrList<T> : BaseList<T> where T : IComparable<T>
    {
        private T[] buffer; // массив для хранения элементов списка
        private int capacity; // начальная емкость массива

        public ArrList()
        {
            capacity = 2;
            buffer = new T[capacity];
            count = 0;
        }

        void Expand()
        {
            capacity *= 2;
            T[] newBuffer = new T[capacity];
            Array.Copy(buffer, newBuffer, count);
            buffer = newBuffer;
        }

        public override void Add(T item)
        {
            if (count == capacity)
            {
                Expand();
            }

            buffer[count] = item;
            count++;
            OnChange();
        }

        public override T this[int index]
        {
            get
            {
                if (index < 0 || index >= count)
                {
                    throw new BadIndexException();
                }
                return buffer[index];
            }
            set
            {
                if (index < 0 || index >= count)
                {
                    throw new BadIndexException();
                }
                buffer[index] = value;
            }
        }

        public override void Insert(int position, T item)
        {
            if (position < 0 || position > count)
            {
                throw new BadIndexException();
            }

            if (count == capacity)
            {
                Expand();
            }

            for (int i = count; i > position; i--)
            {
                buffer[i] = buffer[i - 1];
            }

            buffer[position] = item;
            count++;
            OnChange();
        }

        public override void Delete(int position)
        {
            if (position < 0 || position >= count)
            {
                throw new BadIndexException();
            }

            for (int i = position; i < count - 1; i++)
            {
                buffer[i] = buffer[i + 1];
            }

            count--;
            OnChange();
        }

        public override void Clear()
        {
            buffer = new T[capacity];
            count = 0;
            OnChange();
        }

        protected override BaseList<T> EmptyClone()
        {
            return new ArrList<T>();
        }
    }
}
