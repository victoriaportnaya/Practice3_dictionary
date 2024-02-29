// linked list
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        var lines = File.ReadLines(@"C:\Users\victo\RiderProjects\Practice3_dictionary\Practice3_dictionary\dict.txt");
        var dict = new StringDictionary();
        foreach (var line in lines)
        {
            var splitted = line.Split('|');

            if (splitted.Length > 2)
            {
                var word = splitted[0];
                var def = splitted[2];
                dict.Add(word, def);
            }
            else
            {
                Console.WriteLine(".");
            }
        }

        float load_factor = (float)dict.Count / dict._buckets.Length; 
        Console.WriteLine($"The load factor is {load_factor}"); 
        Console.WriteLine("Type your word >>");
        string userWord = Console.ReadLine().ToLower();
        try
        {
            string def = dict.Get(userWord);
            Console.WriteLine($"DEFINITION OF {userWord.ToUpper()}:{def}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }   
}
public class KeyValuePair
{
    public string Key { get; set; }

    public string Value { get; set; }

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
    }
}

// linked list itself
public class LinkedList : IEnumerable<KeyValuePair>
{
    public LinkedListNode? _first;
    public int Count { get; private set; }

    public LinkedListNode? First
    {
        get { return _first; }
        set { _first = value; }
    }
    
    public void Add(KeyValuePair pair)
    {
        var newNode = new LinkedListNode(pair);
        if (_first != null)
            _first.Previous = newNode;

        newNode.Next = _first;
        _first = newNode;

        Count++;


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

    public IEnumerator<KeyValuePair> GetEnumerator()
    {
        LinkedListNode current = _first;

        while (current != null)
        {
            yield return new KeyValuePair(current.Pair.Key, current.Pair.Value);
            current = current.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
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
    public const int InitialSize = 25;

    public LinkedList[] _buckets = new LinkedList[InitialSize];
    public int count;
    
    public void Add(string key, string value)
    {
        if ((Count + 1) / _buckets.Length > 0.7f)
        {
            ExtendBuckets();
        }
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
            var previous = (LinkedListNode)null;
            var current = _buckets[hash].First;
            
            while (current != null)
            {
                if (current.Pair.Key == key)
                {
                    if (previous == null)
                    {
                        _buckets[hash].First = current.Next;
                    }
                    else
                    {
                        previous.Next = current.Next;
                    }

                    count--;
                    return;
                }

                previous = current;
                current = current.Next;


            }

            
        }

        throw new Exception("Key was not found!");
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

    public int CalculateHash(string key)
    {
        return key.Length % InitialSize;
    }

    public void ExtendBuckets()
    {
        int newSize = _buckets.Length * 2;
        var newBuckets = new LinkedList[newSize];

        foreach (var list in _buckets)
        {
            if (list != null)
            {
                foreach (var pair in list)
                {
                    int newHash = CalculateHash(pair.Key) % newSize;

                    if (newBuckets[newHash] == null)
                        newBuckets[newHash] = new LinkedList();
                    
                    newBuckets[newHash].Add(pair);
                }
            }
        }

        _buckets = newBuckets;
    }
}

