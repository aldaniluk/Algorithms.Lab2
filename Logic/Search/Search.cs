using System;

namespace Logic.Search
{
    public static class Search
    {
        public static int BinarySearch(this int[] array, int element)
        {
            int step = 0;
            int left = 0;
            int right = array.Length - 1;
            int middle = 0;

            while (left < right)
            {
                step++;
                middle = (left + right) / 2;

                if (element < array[middle])
                {
                    right = middle;
                }
                else if (element > array[middle])
                {
                    left = middle + 1;
                }
                else
                {
                    break;
                }
            }

            return (array[middle] == element) ? step : -1;
        }

        public static int InterpolationSearch(this int[] array, int element)
        {
            int step = 0;
            int left = 0;
            int right = array.Length - 1;
            int middle = 0;

            while(array[left] < element && element < array[right])
            {
                step++;
                middle = left + (element - array[left]) * (right - left) / (array[right] - array[left]);

                if (element < array[middle])
                {
                    right = middle;
                }
                else if (element > array[middle])
                {
                    left = middle + 1;
                }
                else
                {
                    break;
                }
            }

            if (array[left] == element)
            {
                middle = left;
            }
            else if (array[right] == element)
            {
                middle = right;
            }

            return (array[middle] == element) ? step : -1;
        }

    }
}
