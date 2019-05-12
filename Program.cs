using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semestrovka__2
{
    public class Program
    {
        private static StreamReader reader = new StreamReader(@"C:\Users\мвидео\source\repos\Semestrovka_№2\Semestrovka_№2\Cin_Kits.txt");     
        public static int[] array;

        public static void Main()
        {
            //Preparation kits
            #region
            /*
             var random = new Random();
             var write = new StreamWriter(@"C: \Users\мвидео\source\repos\Semestrovka_№2\Semestrovka_№2\Cin_Kits.txt");
             for (int i = 0; i < 50; i++)
             {
                var count = 100 * i;
                 for (int j = 0; j < count; j++)
                 {
                     if(j != count-1) write.Write(random.Next(-1000, 1000) + " ");
                     else write.Write(random.Next(-1000, 1000));
                 }
                 write.WriteLine();
             } */
             
            #endregion
            
            int left_index = 0;
            int right_index = 3;
            for(int i = 0; i < 50; i++)
            {
                array = reader.ReadLine().Split(' ').Select(element => Convert.ToInt32(element)).ToArray();
                Tree tree = new Tree(array);
                int sum = tree.Get_Sum(1, 0, array.Length - 1, left_index, right_index);
                tree.Cout_Result_Sum(sum, left_index, right_index);
            }    
        }
    }

    public struct Tree
    {
        //Timers
        #region
        public static int count = 0;
        public static Stopwatch watch = new Stopwatch();
        public static long search_time = 0;
        public static long clean_time = 0;
        public static long build_time = 0;
        #endregion

        private static int[] tree;
        private static int[] pretree;
        private static StreamWriter writer = new StreamWriter(@"C:\Users\мвидео\source\repos\Semestrovka_№2\Semestrovka_№2\Result.txt");

        public Tree(int[] array)
        {
            tree = new int[array.Length * 4];
            pretree = array;
            watch.Reset();
            watch.Start();
            Creat_Tree( array, 1, 0, array.Length - 1);
            watch.Stop();
            build_time = watch.ElapsedTicks;
            watch.Reset();
        }

        public static void Creat_Tree(int[] array, int edge, int l_treeborder, int r_treeborder)
        {         
            if (l_treeborder == r_treeborder) tree[edge] = array[l_treeborder];
            else
            {
                int average = (l_treeborder + r_treeborder) / 2;
                Creat_Tree(array, edge * 2, l_treeborder, average);
                Creat_Tree(array, edge * 2 + 1, average + 1, r_treeborder);
                tree[edge] = tree[edge * 2] + tree[edge * 2 + 1];
            }          
        }

        public int Get_Sum(int edge, int left_border, int right_border, int left_index, int right_index)
        {
            if (left_index > right_index) return 0;
            if (left_index == left_border && right_index == right_border) return tree[edge];
            int average = (left_border + right_border) / 2;
            return Get_Sum(edge * 2, left_border, average, left_index, Math.Min(right_index, average))
                + Get_Sum(edge * 2 + 1, average + 1, right_border, Math.Max(left_index, average + 1), right_index);
        }

        public void Cout_Result_Sum(int sum, int left_index, int right_index)
        {
            Cout_Tree();
            writer.Write("Сумма элементов отрезка [" + left_index + ";" + right_index + "] " + " = " + sum);
            writer.WriteLine();
            writer.Write("----------------------------------------------------------------------------------");
            writer.WriteLine();
        }

        public void Cout_Tree()
        {
            writer.Write("Изначальный массив:");
            writer.WriteLine();
            foreach (var e in pretree)
                writer.Write(e + " ");
            writer.WriteLine();
            writer.Write("Количество элементов массива: " + pretree.Length);
            writer.WriteLine();
            writer.Write("Количество элементов дерева: " + tree.Length);
            writer.WriteLine();
            writer.Write("Построенное дерево отрезков:");
            writer.WriteLine();
            foreach (var e in tree)
                writer.Write(e + " ");
            writer.WriteLine();
            writer.Write("Затраченное время на построение дерева(tics): " + build_time);
            writer.WriteLine();
        }

        public int Search(int index) => tree[index];

        public void Clean_Tree()
        {
            watch.Start();
            tree = tree.Select(element => 0).ToArray();
            watch.Stop();
            clean_time = watch.ElapsedTicks;
            watch.Reset();
        }    
    }
}
