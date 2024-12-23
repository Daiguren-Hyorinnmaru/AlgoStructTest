using System;
using System.Collections.Generic;

namespace Tests.Algorithms
{
    public enum SortsAlgorithms
    {
        QuickSort,
        MergeSort,
        TimSort,
        HeapSort,
    }

    internal class SortingAlgorithms
    {
        public static Action GetActionAlgorithm<T>(SortsAlgorithms sortingAlgorithms, IList<T> list) where T : IComparable<T>
        {
            Dictionary<SortsAlgorithms, Action> sortingActions = new Dictionary<SortsAlgorithms, Action>
            {
                { SortsAlgorithms.MergeSort, () => MergeSortIterative(list) },
                { SortsAlgorithms.QuickSort, () => QuickSortIterative(list) },
                { SortsAlgorithms.TimSort, () => TimSort(list) },
                { SortsAlgorithms.HeapSort, () => HeapSort(list) },
            };

            return sortingActions.TryGetValue(sortingAlgorithms, out var action) ? action : null;
        }

        private static void QuickSortIterative<T>(IList<T> collection) where T : IComparable<T>
        {
            void Swap(IList<T> coll, int i, int j)
            {
                T temp = coll[i];
                coll[i] = coll[j];
                coll[j] = temp;
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

            if (collection == null || collection.Count <= 1)
                return;

            Stack<(int Left, int Right)> stack = new Stack<(int Left, int Right)>();
            stack.Push((0, collection.Count - 1));

            while (stack.Count > 0)
            {
                var (left, right) = stack.Pop();

                if (left < right)
                {
                    int pivotIndex = Partition(collection, left, right);

                    // Додаємо індекси підмасивів до стека
                    stack.Push((left, pivotIndex - 1));
                    stack.Push((pivotIndex + 1, right));
                }
            }
        }

        private static void MergeSortIterative<T>(IList<T> collection) where T : IComparable<T>
        {
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

            if (collection == null || collection.Count <= 1)
                return;

            int n = collection.Count;

            // Розмір підмасиву, який злиття відбуватиметься
            for (int currentSize = 1; currentSize < n; currentSize *= 2)
            {
                // Проходимо по всіх підмасивах для поточного розміру
                for (int left = 0; left < n - 1; left += 2 * currentSize)
                {
                    int middle = Math.Min(left + currentSize - 1, n - 1);
                    int right = Math.Min(left + 2 * currentSize - 1, n - 1);

                    // Зливаємо два сусідніх підмасиви
                    Merge(collection, left, middle, right);
                }
            }
        }

        private static void TimSort<T>(IList<T> collection) where T : IComparable<T>
        {
            const int Run = 32;

            void InsertionSort(IList<T> coll, int left, int right)
            {
                for (int i = left + 1; i <= right; i++)
                {
                    T temp = coll[i];
                    int j = i - 1;

                    while (j >= left && coll[j].CompareTo(temp) > 0)
                    {
                        coll[j + 1] = coll[j];
                        j--;
                    }

                    coll[j + 1] = temp;
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

                int iLeft = 0, iRight = 0, k = left;

                while (iLeft < leftSize && iRight < rightSize)
                {
                    if (leftArray[iLeft].CompareTo(rightArray[iRight]) <= 0)
                    {
                        coll[k] = leftArray[iLeft];
                        iLeft++;
                    }
                    else
                    {
                        coll[k] = rightArray[iRight];
                        iRight++;
                    }
                    k++;
                }

                while (iLeft < leftSize)
                {
                    coll[k] = leftArray[iLeft];
                    iLeft++;
                    k++;
                }

                while (iRight < rightSize)
                {
                    coll[k] = rightArray[iRight];
                    iRight++;
                    k++;
                }
            }

            if (collection == null || collection.Count <= 1)
                return;

            int n = collection.Count;

            // 1. Відсортуємо кожен підмасив (run) розміром Run за допомогою сортування вставками
            for (int i = 0; i < n; i += Run)
            {
                int right = Math.Min(i + Run - 1, n - 1);
                InsertionSort(collection, i, right);
            }

            // 2. Зливаємо відсортовані runs, поступово збільшуючи їх розмір
            for (int size = Run; size < n; size *= 2)
            {
                for (int left = 0; left < n; left += 2 * size)
                {
                    int middle = Math.Min(left + size - 1, n - 1);
                    int right = Math.Min(left + 2 * size - 1, n - 1);

                    if (middle < right)
                        Merge(collection, left, middle, right);
                }
            }
        }

        private static void HeapSort<T>(IList<T> collection) where T : IComparable<T>
        {
            void Heapify(IList<T> coll, int n, int i)
            {
                int largest = i; // Індекс найбільшого елемента
                int left = 2 * i + 1; // Лівий дочірній вузол
                int right = 2 * i + 2; // Правий дочірній вузол

                // Перевіряємо, чи лівий дочірній вузол більший за корінь
                if (left < n && coll[left].CompareTo(coll[largest]) > 0)
                {
                    largest = left;
                }

                // Перевіряємо, чи правий дочірній вузол більший за найбільший елемент на даний момент
                if (right < n && coll[right].CompareTo(coll[largest]) > 0)
                {
                    largest = right;
                }

                // Якщо найбільший елемент не корінь
                if (largest != i)
                {
                    Swap(coll, i, largest);

                    // Рекурсивно викликаємо Heapify для дочірнього вузла
                    Heapify(coll, n, largest);
                }
            }

            void Swap(IList<T> coll, int i, int j)
            {
                T temp = coll[i];
                coll[i] = coll[j];
                coll[j] = temp;
            }

            if (collection == null || collection.Count <= 1)
                return;

            int n = collection.Count;

            // 1. Побудова max-heap
            for (int i = n / 2 - 1; i >= 0; i--)
            {
                Heapify(collection, n, i);
            }

            // 2. Видалення елементів з дерева та сортування
            for (int i = n - 1; i > 0; i--)
            {
                // Переміщуємо корінь (максимальний елемент) у кінець
                Swap(collection, 0, i);

                // Зменшуємо розмір heap і викликаємо Heapify для кореня
                Heapify(collection, i, 0);
            }
        }
    }
}
