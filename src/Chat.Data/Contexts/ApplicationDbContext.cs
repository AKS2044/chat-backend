using Chat.Data.Configurations;
using Chat.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chat.Data.Contexts
{
    /// <summary>
    /// Main application context
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        /// <summary>
        /// DbSet for Comment.
        /// </summary>
        public DbSet<Chatik> Chats { get; set; }

        /// <summary>
        /// DbSet for Messages.
        /// </summary>
        public DbSet<Messages> Messages { get; set; }

        /// <summary>
        /// DbSet for UserChats.
        /// </summary>
        public DbSet<UserChats> UserChats { get; set; }

        /// <summary>
        /// Contructor with params.
        /// </summary>
        /// <param name="options">Database context options.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new ChatikConfiguration());
            builder.ApplyConfiguration(new MessageConfiguration());
            builder.ApplyConfiguration(new UserChatsConfiguration());

            base.OnModelCreating(builder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ChatBD;Trusted_Connection=True;");
        //}
    }
}
