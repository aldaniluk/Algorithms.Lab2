using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Hashtable
{
    public class Hashtable<T> 
    {
        private int size;
        private List<T>[] table;

        public Hashtable(int size)
        {
            table = new List<T>[size];
            this.size = size;
        }

        public void Add(T element)
        {
            int hashCode = GetHashCodeForElement(element); 

            if(table[hashCode] == null) 
            {
                table[hashCode] = new List<T>();
            }

            table[hashCode].Add(element);  
        }

        public void Remove(T element)
        {
            int hashCode = GetHashCodeForElement(element);
            List<T> values = table[hashCode];

            if (values == null)
            {
                return;
            }
            if (!values.Contains(element))
            {
                return;
            }

            values.Remove(element);
        }

        public bool Contains(T element)
        {
            int hashCode = GetHashCodeForElement(element);
            List<T> values = table[hashCode];

            if (values == null)
            {
                return false;
            }

            return values.Contains(element);
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < table.Length; i++)
            {
                if (table[i] == null)
                {
                    continue;
                }

                result.Append(i + " : ");

                foreach (var value in table[i])
                {
                    result.Append(value.ToString() + ' ');
                }

                result.Append('\n');
            }

            return result.ToString();
        }

        private int GetHashCodeForElement(T element)
        {
            return element.GetHashCode() % size;
        }

    }
}
