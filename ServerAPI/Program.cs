using Microsoft.EntityFrameworkCore;
using ServerAPI.DataBase;
using ServerAPI.DataBase.Models;
using ServerAPI.DataBase.Repository;
using ServerAPI.DataBase.UnitOfWork;
using System.Security.Cryptography;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options => options.UseSqlite("Data Source=TestingSystem.db"));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<SortConfigManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//to delete
MyStartupAction();

app.Run();

//to delete
async void MyStartupAction()
{
    Console.WriteLine("Start API!");

    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();

        var dataTypeRepository = new Repository<DataType>(context);
        var sortCollectionRepository = new Repository<SortCollectionType>(context);
        var sortAlgorithmRepository = new Repository<SortsAlgorithm>(context);

        var algorithmNames = new[] { "QuickSort", "MergeSort", "BubbleSort" };
        foreach (var algorithmName in algorithmNames)
        {
            var newAlgorithm = new SortsAlgorithm { NameAlgorithm = algorithmName };
            sortAlgorithmRepository.AddAsync(newAlgorithm).Wait();
        }

        var dataTypes = new[] { "Integer", "String", "Float" };
        foreach (var dataType in dataTypes)
        {
            var newDataType = new DataType { NameDataType = dataType };
            dataTypeRepository.AddAsync(newDataType).Wait();
        }

        var collectionTypes = new[] { "Array", "List", "Queue" };
        foreach (var collectionType in collectionTypes)
        {
            var newCollectionType = new SortCollectionType { NameCollection = collectionType };
            sortCollectionRepository.AddAsync(newCollectionType).Wait();
        }

        ContextSave.SaveChangesAsync(context);


        SortConfigManager sortConfigManager = new SortConfigManager(context);
        var SortConfigManagerRepository = new Repository<SortConfig>(context);
        foreach (var algorithmName in algorithmNames)
        {
            foreach (var collectionType in collectionTypes)
            {
                foreach (var dataType in dataTypes)
                {
                    SortConfig config = new SortConfig()
                    {
                        SortsAlgorithmId = sortAlgorithmRepository.FindAsync(a => a.NameAlgorithm == algorithmName).Result.FirstOrDefault().Id,
                        SortsCollectionId = sortCollectionRepository.FindAsync(a => a.NameCollection == collectionType).Result.FirstOrDefault().Id,
                        DataTypeId = dataTypeRepository.FindAsync(a => a.NameDataType == dataType).Result.FirstOrDefault().Id,
                    };
                    SortConfigManagerRepository.AddAsync(config).Wait();
                }
            }
        }

        ContextSave.SaveChangesAsync(context);

        Console.WriteLine($"SortsAlgorithm");
        var algorithms = sortAlgorithmRepository.GetAllAsync().Result;
        foreach (var algorithm in algorithms)
        {
            Console.WriteLine($"{algorithm.Id}");
            Console.WriteLine($"    {algorithm.NameAlgorithm}");
        }

        Console.WriteLine($"SortCollectionType");
        var collections = sortCollectionRepository.GetAllAsync().Result;
        foreach (var collection in collections)
        {
            Console.WriteLine($"{collection.Id}");
            Console.WriteLine($"    {collection.NameCollection}");
        }

        Console.WriteLine($"DataType");
        var dataTypesRepository = dataTypeRepository.GetAllAsync().Result;
        foreach (var dataType in dataTypesRepository)
        {
            Console.WriteLine($"{dataType.Id}");
            Console.WriteLine($"    {dataType.NameDataType}");
        }

        Console.WriteLine($"SortConfig");
        var repositoryConfig = new Repository<SortConfig>(context);
        var configs = repositoryConfig.GetAllAsync().Result;
        foreach (var config in configs)
        {
            Console.WriteLine($"{config.Id}");
            Console.WriteLine($"    SortsAlgorithm Id: {config.SortsAlgorithm?.Id}");
            Console.WriteLine($"    SortsAlgorithm Name: {config.SortsAlgorithm?.NameAlgorithm}");
            Console.WriteLine($"    SortCollectionType Name: {config.SortsCollectionType?.NameCollection}");
            Console.WriteLine($"    DataType Name: {config.DataType?.NameDataType}");
        }

        Console.WriteLine("SortConfig added successfully!");
    }
}