using System;
using System.Collections.Generic;
using System.Linq;
using Tests.Params;

namespace Tests.Factory
{
    public enum CollectionType
    {
        Array,
        List
    }

    internal class CollectionFactory
    {
        private DataFactory dataFactory;

        public readonly Dictionary<DataType, Func<CollectionType, int, dynamic>> functions;

        public CollectionFactory(DataFactory dataFactory)
        {
            Dictionary<DataType, Func<CollectionType, int, dynamic>> InitializeFunctions()
            {
                return new Dictionary<DataType, Func<CollectionType, int, dynamic>>
                {
                    { DataType.String, (collectionType, length) =>
                        GetCollection<string>(DataType.String, collectionType, length) },
                    { DataType.Integer, (collectionType, length) =>
                        GetCollection<int>(DataType.Integer, collectionType, length) },
                    { DataType.Double, (collectionType, length) =>
                        GetCollection<double>(DataType.Double, collectionType, length) }
                };
            }

            this.dataFactory = dataFactory;
            functions = InitializeFunctions();
        }

        public void SetDataParams(DataParams dataParams) =>
            dataFactory.DataParams = dataParams;

        private IList<T> GetCollection<T>(DataType dataType, CollectionType collectionType, int length) where T : IComparable<T>
        {
            IList<T> CreateList(IEnumerable<T> items) => new List<T>(items);
            IList<T> CreateArray(IEnumerable<T> items) => items.ToArray();

            switch (collectionType)
            {
                case CollectionType.Array:
                    return CreateArray(GenerateValues<T>(dataType, length));
                case CollectionType.List:
                    return CreateList(GenerateValues<T>(dataType, length));
            }
            return null;
        }

        private IEnumerable<T> GenerateValues<T>(DataType dataType, int length) where T : IComparable<T>
        {
            for (int i = 0; i < length; i++)
            {
                yield return (T)dataFactory.RandomValueType(dataType);
            }
        }
    }
}
