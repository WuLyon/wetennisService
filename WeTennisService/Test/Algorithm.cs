using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeTennisService
{
    public class Algorithm
    {
        public static Algorithm instance = new Algorithm();

        public string  MaoPao()
        {
            int[] shuJu = new int[] { 12, 43, 2, 34, 87, 54, 32, 16, 67, 49 };
            for (int i = 1; i <= shuJu.Length - 1; i++)
            {
                for (int j = shuJu.Length-1; j >= i; j--)
                {
                    if (shuJu[j] < shuJu[j - 1])
                    {
                        int temp = shuJu[j];
                        shuJu[j] = shuJu[j - 1];
                        shuJu[j - 1] = temp;
                    }
                }
            }
            string _Res = string.Join(",", shuJu);
            return _Res;
        }

        public string XuanZe()
        {
            int[] shuJu = new int[] { 12, 43, 2, 34, 87, 54, 32, 16, 67, 49 };
            int min, temp;//分别用来标记最小的数组中最小数的下表，和存储临时数据（数字交换时使用）
            for (int i = 0; i < shuJu.Length - 1; i++)
            {
               
                for (int j = i + 1; j < shuJu.Length; j++)//依次和数组中的其他数据进行比较，如果其他数据有比当前标记的最小值小的，则数组最小值下表赋值为两者之间较小值的下标
                {
                    min = i;//标记下表为（当前的排序次数-1）的数组数字为当前的最小值
                    if (shuJu[j] < shuJu[min])
                    {
                        min = j;
                        //如果未寻找到最小值，结果应该不变。只有找到比当前比较值小的，才调换。
                        temp = shuJu[i];
                        shuJu[i] = shuJu[min];
                        shuJu[min] = temp;
                    }
                   
                }
            }
            return string.Join(",", shuJu);
        }

        public string ChaRu()
        {
            int[] shuJu = new int[] { 12, 43, 2, 34, 87, 54, 32, 16, 67, 49 };
            int temp;
            for (int i = 1; i < shuJu.Length; i++)
            {
                int t = shuJu[i];//标记为排序的数据
                int j = i;
                while (j > 0 && shuJu[j - 1] > t) //如果当前未排数据小于上一个数据，且当前j大于0，则将上一个数据移致当前的数据所在的下标位置，同时j下标要减一，
                {
                    shuJu[j] = shuJu[j - 1];
                    j--;
                }
                shuJu[j] = t;
            }
            return string.Join(",", shuJu);
        }

        //快速排序算法
        public  string KaiSu(int[] shuJu, int left, int right)
        {
            shuJu = new int[] { 12, 43, 2, 34, 87, 54, 32, 16, 67, 49 };
            int a = left - 1;//左区域
            int b = right + 1;//右区域
            int mid = shuJu[(left + right) / 2];//基准数字
            if (left < right)
            {
                while (true)
                {
                    while (shuJu[++a] < mid)
                        ;
                    while (shuJu[--b] > mid)
                        ;
                    if (a >= b)
                    {
                        break;
                    }
                    int temp = shuJu[a];
                    shuJu[a] = shuJu[b];
                    shuJu[b] = temp;
                }
                KaiSu(shuJu, left, a);//再递归调用对左区域进行排序
                KaiSu(shuJu, b, right);//递归调用对右区域进行排序
            }
            return string.Join(",", shuJu);
        }


    }
}