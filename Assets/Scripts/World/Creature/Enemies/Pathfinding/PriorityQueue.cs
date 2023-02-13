using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T> where T : IComparable<T>
{
    private T[] data;
    private int count = 0;

    public int Count { get => count; set => count = value; }

    public PriorityQueue()
    {
        data = new T[2];
    }

    public void Enqueue(T element)
    {
        data[++Count] = element;
        if (Count == data.Length - 1)
            Resize(2 * data.Length);
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
        T t = data[i];
        data[i] = data[j];
        data[j] = t;
    }

    public T Dequeue()
    {
        T top = data[1];
        Swap(1, Count--);
        Sink(1);
        if (Count > 0 && Count == data.Length / 4)
            Resize(data.Length / 2 + 1);
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
        return data[i].CompareTo(data[j]) > 0;
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
            copy[i] = data[i];
        }
        data = copy;
    }
    public void Clear()
    {
        data = new T[2];
        count = 0;
    }
    public bool Contains(T element)
    {
        for(int i = 1; i <= Count; i++)
        {
            if (data[i].Equals(element))
                return true;

        }
        return false;
    }
    
}
