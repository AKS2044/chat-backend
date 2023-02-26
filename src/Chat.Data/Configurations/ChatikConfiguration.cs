using Chat.Data.Constants;
using Chat.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics;
using System.Reflection.Emit;

namespace Chat.Data.Configurations
{
    /// <summary>
    /// EF Configuration for Chatik entity.
    /// </summary>
    internal class ChatikConfiguration : IEntityTypeConfiguration<Chatik>
    {
        public void Configure(EntityTypeBuilder<Chatik> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(Table.Chatik, Schema.Chat)
                .HasKey(chat => chat.Id);

            builder.Property(chat => chat.Id)
                .UseIdentityColumn();
        }
    }
}
