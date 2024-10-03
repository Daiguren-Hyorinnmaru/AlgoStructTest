using System;
using System.Collections.Generic;

namespace Tests.Algorithms
{
    public enum SortsAlgorithms
    {
        QuickSort,
        MergeSort
    }

    internal class SortingAlgorithms
    {
        public static Action GetActionAlgorithm<T>(SortsAlgorithms sortingAlgorithms, IList<T> list) where T : IComparable<T>
        {
            Dictionary<SortsAlgorithms, Action> sortingActions = new Dictionary<SortsAlgorithms, Action>
            {
                { SortsAlgorithms.MergeSort, () => MergeSort(list) },
                { SortsAlgorithms.QuickSort, () => QuickSort(list) }
            };

            return sortingActions.TryGetValue(sortingAlgorithms, out var action) ? action : null;
        }

        private static void QuickSort<T>(IList<T> collection) where T : IComparable<T>
        {
            void QuickSortInner(IList<T> coll, int left, int right)
            {
                if (left < right)
                {
                    int pivotIndex = Partition(coll, left, right);
                    QuickSortInner(coll, left, pivotIndex - 1);
                    QuickSortInner(coll, pivotIndex + 1, right);
                }
            }

            int Partition(IList<T> coll, int left, int right)
            {
                T pivot = coll[right];
                int i = left - 1;

                for (int j = left; j < right; j++)
                {
                    if (coll[j].CompareTo(pivot) <= 0)
                    {
                        i++;
                        Swap(coll, i, j);
                    }
                }
                Swap(coll, i + 1, right);
                return i + 1;
            }

            void Swap(IList<T> coll, int i, int j)
            {
                T temp = coll[i];
                coll[i] = coll[j];
                coll[j] = temp;
            }

            QuickSortInner(collection, 0, collection.Count - 1);
        }

        private static void MergeSort<T>(IList<T> collection) where T : IComparable<T>
        {
            void MergeSortInner(IList<T> coll, int left, int right)
            {
                if (left < right)
                {
                    int middle = (left + right) / 2;
                    MergeSortInner(coll, left, middle);
                    MergeSortInner(coll, middle + 1, right);
                    Merge(coll, left, middle, right);
                }
            }

            void Merge(IList<T> coll, int left, int middle, int right)
            {
                int leftSize = middle - left + 1;
                int rightSize = right - middle;

                T[] leftArray = new T[leftSize];
                T[] rightArray = new T[rightSize];

                for (int i = 0; i < leftSize; i++)
                    leftArray[i] = coll[left + i];
                for (int i = 0; i < rightSize; i++)
                    rightArray[i] = coll[middle + 1 + i];

                int leftIndex = 0, rightIndex = 0, k = left;

                while (leftIndex < leftSize && rightIndex < rightSize)
                {
                    if (leftArray[leftIndex].CompareTo(rightArray[rightIndex]) <= 0)
                    {
                        coll[k] = leftArray[leftIndex];
                        leftIndex++;
                    }
                    else
                    {
                        coll[k] = rightArray[rightIndex];
                        rightIndex++;
                    }
                    k++;
                }

                while (leftIndex < leftSize)
                {
                    coll[k] = leftArray[leftIndex];
                    leftIndex++;
                    k++;
                }

                while (rightIndex < rightSize)
                {
                    coll[k] = rightArray[rightIndex];
                    rightIndex++;
                    k++;
                }
            }

            MergeSortInner(collection, 0, collection.Count - 1);
        }
    }
}
