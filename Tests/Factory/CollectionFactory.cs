using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Factory
{
    public enum CollectionType
    {
        Array,
        List
    }

    public class CollectionFactory
    {
        DataFactory dataFactory;

        public CollectionFactory(DataFactory dataFactory)
        {
            this.dataFactory = dataFactory;
        }

        public IList<T> GetListCollection<T>(DataType dataType, CollectionType collectionType, int length) where T : IComparable<T>
        {
            IList<T> CreateList(IEnumerable<T> items) => new List<T>(items);

            IList<T> CreateArray(IEnumerable<T> items) => items.ToArray();

            switch (collectionType)
            {
                case (CollectionType.Array):
                    return CreateArray(GenerateValues<T>(dataType, length));
                case (CollectionType.List):
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
