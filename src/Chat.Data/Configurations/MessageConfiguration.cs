using Chat.Data.Constants;
using Chat.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Data.Configurations
{
    /// <summary>
    /// EF Configuration for message entity.
    /// </summary>
    internal class MessageConfiguration : IEntityTypeConfiguration<Messages>
    {
        public void Configure(EntityTypeBuilder<Messages> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(Table.Messages, Schema.Chat)
                .HasKey(message => message.Id);


            builder.HasOne(chat => chat.Chatik)
                .WithMany(message => message.Messages)
                .HasForeignKey(chat => chat.ChatId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
