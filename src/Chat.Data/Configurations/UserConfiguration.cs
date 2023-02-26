using Chat.Data.Constants;
using Chat.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Data.Configurations
{
    /// <summary>
    /// EF Configuration for User entity.
    /// </summary>
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(Table.Users, Schema.User)
                .HasKey(user => user.Id);
        }
    }
}
