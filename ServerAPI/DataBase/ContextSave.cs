using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace ServerAPI.DataBase
{
    public static class ContextSave
    {
        /// <summary>
        /// Saves changes to the database context asynchronously while ignoring unique constraint violations.
        /// </summary>
        public static async Task SaveChangesAsync(DataContext dataContext)
        {
            try
            {
                await dataContext.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx) when (dbEx.InnerException is SqlException sqlEx && sqlEx.Number == 19)
            {
                // Log or handle SQL error 19 (unique constraint violation)
                Console.WriteLine("Error 19 (IsUnique): " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                // Log or rethrow other exceptions as necessary
                Console.WriteLine("Unhandled Error: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Saves changes to the database context synchronously while ignoring unique constraint violations.
        /// </summary>
        /// <param name="dataContext">The database context.</param>
        public static void SaveChanges(DataContext dataContext)
        {
            try
            {
                dataContext.SaveChanges();
            }
            catch (DbUpdateException dbEx) when (dbEx.InnerException is SqlException sqlEx && sqlEx.Number == 19)
            {
                // Log or handle SQL error 19 (unique constraint violation)
                Console.WriteLine("Error 19 (IsUnique): " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                // Log or rethrow other exceptions as necessary
                Console.WriteLine("Unhandled Error: " + ex.Message);
                throw;
            }
        }
    }
}
