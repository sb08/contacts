using System;
using System.Reflection;
using Contacts.Domain.Entities;
using Contacts.Domain.Interfaces;
using Contacts.Infrastructure.Repository;
using Contacts.Infrastructure.SeedData;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

namespace Contacts
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string mongoConnectionString = Environment.GetEnvironmentVariable("MongoDBStore__ConnectionString");
            services.AddScoped<IMongoClient>(provider => new MongoClient(mongoConnectionString));
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.Configure<MongoDbConfig>(Configuration.GetSection("MongoDBStore"));
            services.AddControllers();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Contacts API", Version = "v1" });
                c.EnableAnnotations();
            });
            services.AddScoped<IMongoClient>(provider => new MongoClient(mongoConnectionString));
            services.AddScoped(CreateMongoCollection<AddressBook>);
            services.AddScoped<IAddressBookRepository<AddressBook>, AddressBookRepository>();
            services.AddScoped<IDatabaseCreationService, DatabaseCreationService>();
            services.AddScoped<IMongoDbContext, MongoDbContext>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            services.CreateAndSeedDbOnFirstRun();

            app.UseRouting();

            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Contacts API"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static IMongoCollection<TCollectionType> CreateMongoCollection<TCollectionType>(IServiceProvider provider)
            where TCollectionType : IAggregate
        {
            string databaseName = Environment.GetEnvironmentVariable("MongoDBStore__Database");

            var client = provider.GetService<IMongoClient>();
            var database = client.GetDatabase(databaseName);
            return database.GetCollection<TCollectionType>($"{typeof(TCollectionType).Name}".ToLower());
        }
    }
}
