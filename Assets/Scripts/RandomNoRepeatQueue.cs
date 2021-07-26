using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNoRepeatQueue<T>
{
    private List<T> pool;
    private Queue<T> previous;

    public int repeatQueueLength = 1;

    public RandomNoRepeatQueue(List<T> elements)
    {
        pool = elements;
        previous = new Queue<T>();
    }

    public T GetNext()
    {
        while (previous.Count > repeatQueueLength)
            pool.Add(previous.Dequeue());

        int i = Random.Range(0, pool.Count);
        T element = pool[i];

        pool.RemoveAt(i);
        previous.Enqueue(element);

        return element;
    }
}
