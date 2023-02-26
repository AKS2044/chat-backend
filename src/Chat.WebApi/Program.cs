using Chat.Data.Contexts;
using Chat.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        string connection = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddIdentity<User, IdentityRole>(opts =>
        {
            opts.Password.RequiredLength = 6;
            opts.Password.RequireNonAlphanumeric = false;
            opts.Password.RequireLowercase = false;
            opts.Password.RequireUppercase = false;
            opts.Password.RequireDigit = false;
            opts.User.RequireUniqueEmail = true;
            opts.User.AllowedUserNameCharacters = ".@abcdefghijklmnopqrstuvwxyz1234567890";
        }).AddEntityFrameworkStores<ApplicationDbContext>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}