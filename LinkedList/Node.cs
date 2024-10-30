﻿using System.Collections;

namespace CircularLinkedList;

public class Node<T> : ICollection<T>
{
    public T Value { get; set; }
    public Node<T> Next { get; private set; }

    public int Count => throw new NotImplementedException();

    public bool IsReadOnly => throw new NotImplementedException();

    public Node(T value)
    {
        Value = value;
        Next = this;
    }

    public override string ToString()
    {
        // TODO: Ask if this is a appropriate for handling null values
        return Value?.ToString() ?? $"{nameof(Value)} does not exist.";
    }

    public void Append(T value)
    {
        if (Exists(value))
        {
            throw new InvalidOperationException("Value already exists in this list.");
        }
        Node<T> node = new(value);
        Node<T> current = this;
        while (current.Next != this)
        {
            current = current.Next;
        }
        current.Next = node;
        node.Next = this;
    }

    public void Clear()
    {
        /* We have decided to iterate through the list, clearing each node.
         * This does not necessarily have to be done, as the garbage collector
         * should be able to handle this. However, this is a good practice to
         * ensure that the garbage collector can do its job efficiently.
        */
        Node<T> current = this.Next;
        while (current != this)
        {
            Node<T> temp = current;
            current = current.Next;
            temp.Next = null!;
        }
        this.Next = this;
    }

    public bool Exists(T value)
    {
        Node<T> current = this;
        if (current.Value != null && current.Value.Equals(value))
        {
            return true;
        }
        while (current.Next != this)
        {
            current = current.Next;
            if (current.Value != null && current.Value.Equals(value))
            {
                return true;
            }
        }
        return false;
    }

    public void Add(T item)
    {
        Append(item);
    }

    public bool Contains(T item)
    {
        return Exists(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public bool Remove(T item)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}