using Microsoft.EntityFrameworkCore;
using ServerAPI.DataBase.Models;
using ServerAPI.DataBase.Repository;

namespace ServerAPI.DataBase.UnitOfWork
{
    public class SortConfigManager
    {
        private readonly DataContext _context;
        private readonly Repository<SortsAlgorithm> _algorithmRepository;
        private readonly Repository<SortCollectionType> _collectionRepository;
        private readonly Repository<DataType> _dataTypeRepository;
        private readonly Repository<SortConfig> _sortConfigRepository;

        public SortConfigManager(DataContext context)
        {
            _context = context;
            _algorithmRepository = new Repository<SortsAlgorithm>(context);
            _collectionRepository = new Repository<SortCollectionType>(context);
            _dataTypeRepository = new Repository<DataType>(context);
            _sortConfigRepository = new Repository<SortConfig>(context);
        }

        public async Task AddSortConfigAsync(string algorithmName, string collectionName, string dataTypeName)
        {
            // Починаємо транзакцію для забезпечення атомарності операцій
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Шукаємо або створюємо алгоритм
                    var algorithm = (await _algorithmRepository.FindAsync(a => a.NameAlgorithm == algorithmName))
                                    .FirstOrDefault();
                    if (algorithm == null)
                    {
                        algorithm = new SortsAlgorithm { NameAlgorithm = algorithmName };
                        await _algorithmRepository.AddAsync(algorithm);
                    }

                    // Шукаємо або створюємо тип колекції
                    var collection = (await _collectionRepository.FindAsync(c => c.NameCollection == collectionName))
                                     .FirstOrDefault();
                    if (collection == null)
                    {
                        collection = new SortCollectionType { NameCollection = collectionName };
                        await _collectionRepository.AddAsync(collection);
                    }

                    // Шукаємо або створюємо тип даних
                    var dataType = (await _dataTypeRepository.FindAsync(dt => dt.NameDataType == dataTypeName))
                                   .FirstOrDefault();
                    if (dataType == null)
                    {
                        dataType = new DataType { NameDataType = dataTypeName };
                        await _dataTypeRepository.AddAsync(dataType);
                    }

                    SortConfig config = new SortConfig();
                    // Прив'язуємо знайдені або створені сутності до конфігурації
                    config.SortsAlgorithm = algorithm;
                    config.SortsCollectionType = collection;
                    config.DataType = dataType;

                    // Перевірка на існування конфігурації перед додаванням
                    //if (!await _sortConfigRepository.ExistsAsync(config))
                    {
                        // Додаємо нову конфігурацію
                        await _sortConfigRepository.AddAsync(config);
                        await _context.SaveChangesAsync(); // Зберігаємо зміни
                    }

                    // Підтверджуємо транзакцію
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    // Якщо сталася помилка, відкатуємо транзакцію
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }


    }

}
