using System;

public class PriorityQueue<T> where T : IComparable<T>
{
    private T[] container;
    private int count = 0;

    public int Count { get => count; set => count = value; }

    public PriorityQueue()
    {
        container = new T[2];
    }

    public void Enqueue(T element)
    {
        container[++Count] = element;
        if (Count == container.Length - 1)
            Resize(2 * container.Length);
        Swim(Count);
    }
    private void Swim(int k)
    {
        while (k > 1 && Less(k / 2, k))
        {
            Swap(k, k / 2);
            k = k / 2;
        }
    }
    private void Swap(int i, int j)
    {
        T t = container[i];
        container[i] = container[j];
        container[j] = t;
    }

    public T Dequeue()
    {
        T top = container[1];
        Swap(1, Count--);
        Sink(1);
        if (Count > 0 && Count == container.Length / 4)
            Resize(container.Length / 2 + 1);
        return top;
    }
    private void Sink(int i)
    {
        while (2 * i <= Count)
        {
            int j = 2 * i;
            if (j < Count && Less(j, j + 1))
                j++;
            if (!Less(i, j))
                break;
            Swap(i, j);
            i = j;
        }
    }
    private bool Less(int i, int j)
    {
        return container[i].CompareTo(container[j]) > 0;
    }
    public bool IsEmpty()
    {
        return Count == 0;
    }
    private void Resize(int capacity)
    {
        T[] copy = new T[capacity];
        for (int i = 0; i <= Count; i++)
        {
            copy[i] = container[i];
        }
        container = copy;
    }
    public void Clear()
    {
        container = new T[2];
        count = 0;
    }
    public bool Contains(T element)
    {
        for(int i = 1; i <= Count; i++)
        {
            if (container[i].Equals(element))
                return true;

        }
        return false;
    }
    
}
