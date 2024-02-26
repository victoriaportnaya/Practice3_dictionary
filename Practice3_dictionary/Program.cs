// linked list
public class KeyValuePair
{
    public string Key { get; }

    public string Value { get; }

    public KeyValuePair(string key, string value)
    {
        Key = key;
        Value = value;
    }
    
}

// linked list node
public class LinkedListNode
{
    public KeyValuePair Pair { get; }

    public LinkedListNode Next { get; set; }
    
    public LinkedListNode Previous { get; set; }

    public LinkedListNode(KeyValuePair pair, LinkedListNode next = null)
    {
        Pair = pair;
        Next = next;
        Previous = null;
    }
}

// linked list itself
public class LinkedList
{
    private LinkedListNode _first;
    public int Count { get; set; }
    
    public void Add(KeyValuePair pair)
    {
        var newNode = new LinkedListNode(pair);
        if (_first != null)
        {
            newNode.Next = _first;
            _first.Previous = newNode;
        }

        Count++;
        _first = newNode;
    }

    public void RemoveByKey(string key)
    {
        if (_first == null)
        {
            throw new Exception("The list is empty!");
        }

        if (_first.Pair.Key.Equals(key))
        {
            _first = _first.Next;
            if (_first != null)
            {
                _first.Previous = null;
            }

            Count--;
            return;
        }

        LinkedListNode current = _first;
        LinkedListNode  previous = null;

        while (current != null && !current.Pair.Key.Equals(key))
        {
            previous = current;
            current = current.Next;
        }

        if (current == null)
        {
            throw new Exception("Element with the key is not found!");
        }

        if (current.Next != null)
        {
            current.Next.Previous = previous;
        }

        Count--;
    }

    public KeyValuePair GetItemWithKey(string key)
    {
        LinkedListNode current = _first;

        while (current != null)
        {
            if (current.Pair.Key.Equals(key))
            {
                return current.Pair;
            }

            current = current.Next;
        }

        throw new InvalidOperationException("Element with the key is not found!");
    }
}

public class StringDictionary
{
    private const int InitialSize = 25;
    
    private LinkedList[] _buckets = new LinkedList[InitialSize];
    public int count;
    
    public void Add(string key, string value)
    {
        int hash = CalculateHash(key);
        if (_buckets[hash] == null)
        {
            _buckets[hash] = new LinkedList();
        }

        foreach (var pair in _buckets[hash])
        {
            if (pair.Key == key)
            {
                pair.Value = value;
                return;
            }
        }

        _buckets[hash].Add(new KeyValuePair(key, value));
        count++;
    }

    public void Remove(string key)
    {
        int hash = CalculateHash(key);

        if (_buckets[hash] != null)
        {
            var nodeToRemove = _buckets[hash].First;
            while (nodeToRemove != null)
            {
                if (nodeToRemove.Value.Key == key)
                {
                    _buckets[hash].Remove(nodeToRemove);
                    count--;
                    return;
                }
            }

            nodeToRemove = nodeToRemove.Next;

        }
    }

    public string Get(string key)
    {
        int hash = CalculateHash(key);

        if (_buckets[hash] != null)
        {
            foreach (var pair in _buckets[hash])
            {
                if (pair.Key == key)
                {
                    return pair.Value;
                }
            }
        }

        throw new Exception($"{key} does not exist in the dictionary!");
    }

    public int Count
    {
        get { return count; }
    }

    private int CalculateHash(string key)
    {
        var hash = key.Length % 25;
        return hash;
    }
}
