using Microsoft.EntityFrameworkCore;
using DataBaseModels.Models;
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

        public async Task<int> AddOrGetSortConfigIdAsync(SortsAlgorithm algorithm, SortCollectionType collectionType, DataType dataType)
        {
            // Begin a transaction to ensure atomicity
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Get or add algorithm
                    var algorithmId = await GetOrAddAlgorithmAsync(algorithm);

                    // Get or add collection type
                    var collectionTypeId = await GetOrAddCollectionTypeAsync(collectionType);

                    // Get or add data type
                    var dataTypeId = await GetOrAddDataTypeAsync(dataType);

                    // Create a new SortConfig instance
                    SortConfig newConfig = new SortConfig
                    {
                        SortsAlgorithmId = algorithmId,
                        SortsCollectionId = collectionTypeId,
                        DataTypeId = dataTypeId
                    };

                    // Check if configuration already exists
                    var existingConfig = await _sortConfigRepository.FindAsync(sc =>
                        newConfig.DataTypeId == dataTypeId &&
                        newConfig.SortsAlgorithmId == algorithmId &&
                        newConfig.SortsCollectionId == collectionTypeId);

                    if (existingConfig.Any())
                    {
                        return existingConfig.First().Id; // Return existing configuration Id
                    }

                    // Add the new configuration if it does not exist
                    await _sortConfigRepository.AddAsync(newConfig);
                    await _context.SaveChangesAsync();

                    // Commit the transaction
                    await transaction.CommitAsync();

                    return newConfig.Id; // Return new configuration Id
                }
                catch (Exception)
                {
                    // Roll back the transaction in case of an error
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        // Get or add algorithm
        private async Task<int> GetOrAddAlgorithmAsync(SortsAlgorithm algorithm)
        {
            var existingAlgorithm = await _algorithmRepository.FindAsync(a => a.NameAlgorithm == algorithm.NameAlgorithm);

            if (existingAlgorithm.Any())
            {
                return existingAlgorithm.First().Id; // Return existing algorithm Id
            }

            await _algorithmRepository.AddAsync(algorithm);
            await _context.SaveChangesAsync();

            return algorithm.Id; // Return newly created algorithm Id
        }

        // Get or add collection type
        private async Task<int> GetOrAddCollectionTypeAsync(SortCollectionType collectionType)
        {
            var existingCollection = await _collectionRepository.FindAsync(c => c.NameCollection == collectionType.NameCollection);

            if (existingCollection.Any())
            {
                return existingCollection.First().Id; // Return existing collection type Id
            }

            await _collectionRepository.AddAsync(collectionType);
            await _context.SaveChangesAsync();

            return collectionType.Id; // Return newly created collection type Id
        }

        // Get or add data type
        private async Task<int> GetOrAddDataTypeAsync(DataType dataType)
        {
            var existingDataType = await _dataTypeRepository.FindAsync(dt => dt.NameDataType == dataType.NameDataType);

            if (existingDataType.Any())
            {
                return existingDataType.First().Id; // Return existing data type Id
            }

            await _dataTypeRepository.AddAsync(dataType);
            await _context.SaveChangesAsync();

            return dataType.Id; // Return newly created data type Id
        }



    }

}
