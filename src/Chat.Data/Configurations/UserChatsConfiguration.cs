using Chat.Data.Constants;
using Chat.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Data.Configurations
{
    /// <summary>
    /// EF Configuration for UserChats entity.
    /// </summary>
    internal class UserChatsConfiguration : IEntityTypeConfiguration<UserChats>
    {
        public void Configure(EntityTypeBuilder<UserChats> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(Table.UserChats, Schema.User)
                .HasKey(userChat => userChat.Id);

            builder.Property(userChat => userChat.Id)
                .UseIdentityColumn();

            builder.HasOne(userChat => userChat.User)
                .WithMany(user => user.UserChats)
                .HasForeignKey(userChat => userChat.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(userChat => userChat.Chatik)
                .WithMany(chat => chat.UserChats)
                .HasForeignKey(userChat => userChat.ChatId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
