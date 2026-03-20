using Growtify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Growtify.Infrastructure.Configuration
{
    public class MemberLikeConfiguration : IEntityTypeConfiguration<MemberLike>
    {
        public void Configure(EntityTypeBuilder<MemberLike> entity)
        {
            entity.HasKey(x => new { x.SourceMemberId, x.TargetMemberId });

            entity.HasOne(s => s.SourceMember)
                .WithMany(t => t.LikedMembers)
                .HasForeignKey(s => s.SourceMemberId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(t => t.TargetMember)
                .WithMany(s => s.LikedByMembers)
                .HasForeignKey(t => t.TargetMemberId);
        }
    }
}
