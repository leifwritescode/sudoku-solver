namespace LDK.Collections;

/// <summary>
/// Defines a queue data structure.
/// </summary>
/// <typeparam name="T">The type of queued item.</typeparam>
public interface IQueue<T>
{
    /// <summary>
    /// True if the queue has items.
    /// </summary>
    bool HasAny { get; }

    /// <summary>
    /// Adds an item to the back of the queue.
    /// </summary>
    /// <param name="item">The item to add to the back of the queue.</param>
    void Enqueue(T item);

    /// <summary>
    /// Adds a range of items to the back of the queue.
    /// </summary>
    void EnqueueRange(IEnumerable<T> items);

    /// <summary>
    /// Removes and returns the item at the front of the queue.
    /// </summary>
    /// <returns>The item at the front of the queue.</returns>
    T Dequeue();

    /// <summary>
    /// Returns the item at the front of the queue without removing it.
    /// </summary>
    /// <returns>The item at the front of the queue.</returns>
    T Peek();
}
