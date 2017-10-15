namespace Logic.Search
{
    public static class Search
    {
        public static int BinarySearch(this int[] array, int element)
        {
            if (element < array[0] || element > array[array.Length - 1]) return -1;

            int start = 0;
            int finish = array.Length - 1;
            int middle;

            while (start < finish)
            {
                middle = (start + finish) / 2;

                if (element == array[middle])
                {
                    return middle;
                }
                else if (element < array[middle])
                {
                    finish = middle;
                }
                else
                {
                    start = middle + 1;
                }
            }
            return (array[finish] == element) ? finish : -1;
        }

        public static int InterpolationSearch(this int[] array, int element)
        {
            int left = 0;
            int right = array.Length - 1;

            while (array[left] < element && element < array[right])
            {
                int middle = left + (element - array[left]) * (right - left) / (array[right] - array[left]);
                if (array[middle] < element)
                {
                    left = middle + 1;
                }
                else if (array[middle] > element)
                {
                    right = middle - 1;
                }
                else
                {
                    return middle;
                }
            }

            if (array[left] == element)
            {
                return left;
            }
            else if (array[right] == element)
            {
                return right;
            }
            else
            {
                return -1;
            }
        }

    }
}
