using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QuickCourtBackend.Models;

public partial class QuickCourtContext : DbContext
{
    public QuickCourtContext()
    {
    }

    public QuickCourtContext(DbContextOptions<QuickCourtContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Amenity> Amenities { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Court> Courts { get; set; }

    public virtual DbSet<Facility> Facilities { get; set; }

    public virtual DbSet<FacilityPhoto> FacilityPhotos { get; set; }

    public virtual DbSet<OtpVerification> OtpVerifications { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Sport> Sports { get; set; }

    public virtual DbSet<TimeSlot> TimeSlots { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Amenity>(entity =>
        {
            entity.HasKey(e => e.AmenityId).HasName("PK__Amenitie__5C8402B6D669E9C8");

            entity.HasIndex(e => e.Name, "UQ__Amenitie__72E12F1BE83896D3").IsUnique();

            entity.Property(e => e.AmenityId).HasColumnName("amenityId");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Bookings__C6D03BCD44A8142A");

            entity.Property(e => e.BookingId).HasColumnName("bookingId");
            entity.Property(e => e.CancellationReason).HasColumnName("cancellationReason");
            entity.Property(e => e.CourtId).HasColumnName("courtId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("endTime");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(20)
                .HasDefaultValue("pending")
                .HasColumnName("paymentStatus");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("startTime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("confirmed")
                .HasColumnName("status");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("totalPrice");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Court).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.CourtId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bookings__courtI__72C60C4A");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Bookings__userId__71D1E811");
        });

        modelBuilder.Entity<Court>(entity =>
        {
            entity.HasKey(e => e.CourtId).HasName("PK__Courts__4E6E36E830273100");

            entity.Property(e => e.CourtId).HasColumnName("courtId");
            entity.Property(e => e.Capacity)
                .HasDefaultValue(1)
                .HasColumnName("capacity");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.FacilityId).HasColumnName("facilityId");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.PricePerHour)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("pricePerHour");
            entity.Property(e => e.SportId).HasColumnName("sportId");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.Facility).WithMany(p => p.Courts)
                .HasForeignKey(d => d.FacilityId)
                .HasConstraintName("FK__Courts__facility__6383C8BA");

            entity.HasOne(d => d.Sport).WithMany(p => p.Courts)
                .HasForeignKey(d => d.SportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Courts__sportId__6477ECF3");
        });

        modelBuilder.Entity<Facility>(entity =>
        {
            entity.HasKey(e => e.FacilityId).HasName("PK__Faciliti__AA5481E4AB8CBC9D");

            entity.Property(e => e.FacilityId).HasColumnName("facilityId");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.ApprovalComments).HasColumnName("approvalComments");
            entity.Property(e => e.ApprovedBy).HasColumnName("approvedBy");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.OperatingHoursEnd).HasColumnName("operatingHoursEnd");
            entity.Property(e => e.OperatingHoursStart).HasColumnName("operatingHoursStart");
            entity.Property(e => e.OwnerId).HasColumnName("ownerId");
            entity.Property(e => e.RejectionReason).HasColumnName("rejectionReason");
            entity.Property(e => e.State)
                .HasMaxLength(100)
                .HasColumnName("state");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("pending")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.FacilityApprovedByNavigations)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("FK__Facilitie__appro__5DCAEF64");

            entity.HasOne(d => d.Owner).WithMany(p => p.FacilityOwners)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK__Facilitie__owner__5AEE82B9");

            entity.HasMany(d => d.Amenities).WithMany(p => p.Facilities)
                .UsingEntity<Dictionary<string, object>>(
                    "FacilityAmenity",
                    r => r.HasOne<Amenity>().WithMany()
                        .HasForeignKey("AmenityId")
                        .HasConstraintName("FK__FacilityA__ameni__02FC7413"),
                    l => l.HasOne<Facility>().WithMany()
                        .HasForeignKey("FacilityId")
                        .HasConstraintName("FK__FacilityA__facil__02084FDA"),
                    j =>
                    {
                        j.HasKey("FacilityId", "AmenityId").HasName("PK__Facility__DF9CC1CF24E71E9C");
                        j.ToTable("FacilityAmenities");
                        j.IndexerProperty<int>("FacilityId").HasColumnName("facilityId");
                        j.IndexerProperty<int>("AmenityId").HasColumnName("amenityId");
                    });
        });

        modelBuilder.Entity<FacilityPhoto>(entity =>
        {
            entity.HasKey(e => e.PhotoId).HasName("PK__Facility__547C322DEF4FBDA7");

            entity.Property(e => e.PhotoId).HasColumnName("photoId");
            entity.Property(e => e.Caption)
                .HasMaxLength(255)
                .HasColumnName("caption");
            entity.Property(e => e.FacilityId).HasColumnName("facilityId");
            entity.Property(e => e.PhotoUrl)
                .HasMaxLength(255)
                .HasColumnName("photoUrl");

            entity.HasOne(d => d.Facility).WithMany(p => p.FacilityPhotos)
                .HasForeignKey(d => d.FacilityId)
                .HasConstraintName("FK__FacilityP__facil__05D8E0BE");
        });

        modelBuilder.Entity<OtpVerification>(entity =>
        {
            entity.HasKey(e => e.OtpId)
                .HasName("PK__OtpVerif__122D946A4245A0BC");

            entity.Property(e => e.OtpId)
                .HasColumnName("otpId");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");

            entity.Property(e => e.ExpiresAt)
                .HasColumnType("datetime")
                .HasColumnName("expiresAt");

            entity.Property(e => e.IsUsed)
                .HasDefaultValue(false)
                .HasColumnName("isUsed");

            entity.Property(e => e.OtpCode)
                .HasMaxLength(10)
                .HasColumnName("otpCode");

            entity.Property(e => e.Purpose)
                .HasMaxLength(20)
                .HasColumnName("purpose");

            entity.Property(e => e.UserId)
                .HasColumnName("userId")
                .IsRequired(false);  // nullable foreign key

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email")
                .IsRequired(false);

            entity.HasOne(d => d.User)
                .WithMany(p => p.OtpVerifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__OtpVerifi__userI__52593CB8")
                .IsRequired(false);  // optional relationship
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Reviews__2ECD6E04EFEDCB0C");

            entity.Property(e => e.ReviewId).HasColumnName("reviewId");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.FacilityId).HasColumnName("facilityId");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Facility).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.FacilityId)
                .HasConstraintName("FK__Reviews__facilit__0D7A0286");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reviews__userId__0C85DE4D");
        });

        modelBuilder.Entity<Sport>(entity =>
        {
            entity.HasKey(e => e.SportId).HasName("PK__Sports__AC9F704888A56EC0");

            entity.HasIndex(e => e.Name, "UQ__Sports__72E12F1BB63BA954").IsUnique();

            entity.Property(e => e.SportId).HasColumnName("sportId");
            entity.Property(e => e.IconUrl)
                .HasMaxLength(255)
                .HasColumnName("iconUrl");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TimeSlot>(entity =>
        {
            entity.HasKey(e => e.TimeSlotId).HasName("PK__TimeSlot__BF447D902169AD49");

            entity.Property(e => e.TimeSlotId).HasColumnName("timeSlotId");
            entity.Property(e => e.CourtId).HasColumnName("courtId");
            entity.Property(e => e.EndTime).HasColumnName("endTime");
            entity.Property(e => e.SlotDate).HasColumnName("slotDate");
            entity.Property(e => e.StartTime).HasColumnName("startTime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("available")
                .HasColumnName("status");

            entity.HasOne(d => d.Court).WithMany(p => p.TimeSlots)
                .HasForeignKey(d => d.CourtId)
                .HasConstraintName("FK__TimeSlots__court__6D0D32F4");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__CB9A1CFF91B01B0F");

            entity.HasIndex(e => e.Email, "UQ__Users__AB6E6164CAF18062").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.AvatarUrl)
                .HasMaxLength(255)
                .HasColumnName("avatarUrl");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("fullName");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.IsVerified)
                .HasDefaultValue(false)
                .HasColumnName("isVerified");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("passwordHash");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValue("user")
                .HasColumnName("role");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
