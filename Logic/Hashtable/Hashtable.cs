using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Hashtable
{
    public class Hashtable<T> 
    {
        private Dictionary<int, List<T>> table;

        public Hashtable()
        {
            table = new Dictionary<int, List<T>>();
        }

        public void Add(T element)
        {
            int hashCode = GetHashCodeForElement(element); 
            if(table.ContainsKey(hashCode)) 
            {
                if(table[hashCode].Contains(element)) 
                {
                    return;
                }

                table[hashCode].Add(element);  
            }
            else
            {
                table.Add(hashCode, new List<T> { element }); 
            }
        }

        public void Remove(T element)
        {
            List<T> values;
            int hashCode = GetHashCodeForElement(element);
            if (!table.TryGetValue(hashCode, out values))
            {
                return;
            }

            values.Remove(element);
            if(values.Count == 0)
            {
                table.Remove(hashCode);
            }
        }

        public bool Contains(T element) 
        {
            List<T> values;
            if (!table.TryGetValue(GetHashCodeForElement(element), out values))
            {
                return false;
            }

            return values.Contains(element);
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            for(int i = 0; i < table.Keys.Count; i++)
            {
                List<T> values;
                if(!table.TryGetValue(i, out values))
                {
                    continue;
                }
                result.Append(i + " : ");
                foreach(var value in values)
                {
                    result.Append(value.ToString() + ' ');
                }

                result.Append('\n');
            }

            return result.ToString();
        }

        private int GetHashCodeForElement(T element)
        {
            return element.GetHashCode() % 97;
        }

    }
}
