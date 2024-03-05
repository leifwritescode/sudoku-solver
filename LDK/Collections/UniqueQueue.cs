namespace LDK.Collections;

/// <summary>
/// A queue that only allows unique items.
/// </summary>
/// <typeparam name="T">The type of queued item.</typeparam>
public class UniqueQueue<T> : IQueue<T>
{
    /// <summary>
    /// The set of items in the queue.
    /// </summary>
    private readonly HashSet<T> _set = new();

    public int Length => _set.Count();

    /// <inheritdoc/>
    public bool HasAny => _set.Count > 0;

    /// <inheritdoc/>
    public void Enqueue(T item)
    {
        _set.Add(item);
    }

    /// <inheritdoc/>
    public void EnqueueRange(IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            _set.Add(item);
        }
    }

    /// <inheritdoc/>
    public T Dequeue()
    {
        var item = _set.First();
        _set.Remove(item);
        return item;
    }


    /// <inheritdoc/>
    public T Peek()
    {
        var item = _set.First();
        return item;
    }
}
