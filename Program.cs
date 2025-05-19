using Microsoft.EntityFrameworkCore;
using Pokemon.Data;
using Pokemon.Services;

namespace Pokemon
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configurar serviços da aplicação

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Banco de dados
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Serviços
            builder.Services.AddScoped<GameService>();
            builder.Services.AddScoped<UserService>();

            var app = builder.Build();

            // Configuração do pipeline HTTP

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            // Opcional: seed de dados
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate(); // aplica migrações

                var seeder = new PokemonSeeder(db);
                await seeder.SeedFirst151Async();
            }

            app.Run();
        }
    }
}
