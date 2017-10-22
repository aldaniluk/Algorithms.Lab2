using Logic;
using Logic.Hashtable;
using Logic.Search;
using System;
using System.IO;
using System.Text;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            #region BST
            //BinarySearchTree<int> tree = new BinarySearchTree<int>(new int[] { 20, 10, 25, 2, 12, 21, 40, 1, 9, 18, 36 });

            //tree.PrintByLevels();
            //Console.WriteLine();
            //Console.WriteLine();

            ////tree.RotationLeft(20);
            //tree.RotationLeft(25);
            ////tree.RotationRight(20);
            ////tree.RotationRight(10);

            //tree.PrintByLevels();

            ////foreach(var i in tree.PreorderTraversal())
            ////{
            ////    Console.Write(i + " ");
            ////}

            #endregion

            #region Searches
            //Random rand = new Random();
            //string path = Directory.GetCurrentDirectory() + @"\..\..\..\Logic\Search\arrays.txt";

            //int[] a1 = new int[100_000];
            //int myBirth = 15_552;// 4 * 6 * 9 * 9 * 8;

            ////using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
            ////{
            ////    for (int i = 0; i < a1.Length * 50; i++)
            ////    {
            ////        int randInt = rand.Next(0, 800_000);
            ////        writer.Write(randInt);
            ////    }
            ////}

            //using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open), Encoding.ASCII))
            //{
            //    int numberArr = 1;
            //    Console.Write(String.Format($"{"Array",6} {"BS",8} {"IS",8} \n"));
            //    while (reader.PeekChar() > -1)
            //    {
            //        for (int i = 0; i < a1.Length; i++)
            //        {
            //            a1[i] = reader.ReadInt32();
            //        }
            //        Array.Sort(a1);
            //        Console.Write(String.Format($"{numberArr,6} {a1.BinarySearch(myBirth),8} {a1.InterpolationSearch(myBirth),8} \n"));

            //        numberArr++;

            //    }
            //}
            #endregion

            #region Hashtable
            Hashtable<int> table = new Hashtable<int>(97);

            //Random rand = new Random();
            string path = Directory.GetCurrentDirectory() + @"\..\..\..\Logic\Hashtable\ints.txt";

            //using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
            //{
            //    for (int i = 0; i < 1000; i++)
            //    {
            //        int randInt = rand.Next(0, 1_000_000_000);
            //        writer.Write(randInt);
            //    }
            //}



            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open), Encoding.ASCII))
            {
                while (reader.PeekChar() > -1)
                {
                    table.Add(reader.ReadInt32());
                }
            }

            Console.WriteLine(table); 

            Console.WriteLine(table.Contains(976632844)); //true
            Console.WriteLine(table.Contains(976632845)); //false
            table.Remove(976632844);
            Console.WriteLine(table.Contains(976632844)); //false

            #endregion;
        }
    }
}
