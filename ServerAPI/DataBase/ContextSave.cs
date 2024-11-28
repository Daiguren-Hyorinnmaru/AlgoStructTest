using System.Data.SqlClient;

namespace ServerAPI.DataBase
{
    public static class ContextSave
    {
        public static void SaveChangesAsync(DataContext dataContext)
        {
            try
            {
                dataContext.SaveChangesAsync().Wait();
            }
            catch (AggregateException aggEx)
            {
                var sqlEx = aggEx.InnerException as SqlException;
                if (sqlEx != null && sqlEx.Number == 19)
                {
                    Console.WriteLine("Error 19 (IsUnique): " + sqlEx.Message);
                }
                else
                {
                    Console.WriteLine("Error: " + aggEx.InnerException?.Message);
                }
            }
        }
    }
}
