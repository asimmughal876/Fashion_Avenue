using System;
using System.Collections.Generic;
using Fashion_Avenue.Models;
using Microsoft.EntityFrameworkCore;

namespace Fashion_Avenue.Data;

public partial class FashionAvenueContext : DbContext
{
    public FashionAvenueContext()
    {
    }

    public FashionAvenueContext(DbContextOptions<FashionAvenueContext> options)
        : base(options)
    {
    }

    public virtual DbSet<About> Abouts { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<BlogComment> BlogComments { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Coupon> Coupons { get; set; }

    public virtual DbSet<LikeCount> LikeCounts { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductColor> ProductColors { get; set; }

    public virtual DbSet<ProductLike> ProductLikes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<UsedCoupon> UsedCoupons { get; set; }

    public virtual DbSet<UserRecord> UserRecords { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-1LCAB9T\\SQLEXPRESS;Initial Catalog=FASHION_AVENUE;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<About>(entity =>
        {
            entity.HasKey(e => e.AId).HasName("PK__ABOUT__71AC6D412601CB3C");

            entity.ToTable("ABOUT");

            entity.Property(e => e.AId).HasColumnName("A_ID");
            entity.Property(e => e.ADesc)
                .HasMaxLength(7000)
                .IsUnicode(false)
                .HasColumnName("A_DESC");
            entity.Property(e => e.AImage)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("A_IMAGE");
            entity.Property(e => e.ATitle)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("A_TITLE");
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.BId).HasName("PK__BLOG__4B26EFE6C132AB04");

            entity.ToTable("BLOG");

            entity.Property(e => e.BId).HasColumnName("B_ID");
            entity.Property(e => e.BDate)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasColumnType("date")
                .HasColumnName("B_DATE");
            entity.Property(e => e.BDesc)
                .HasMaxLength(5000)
                .IsUnicode(false)
                .HasColumnName("B_DESC");
            entity.Property(e => e.BImage)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("B_IMAGE");
            entity.Property(e => e.BTitle)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("B_TITLE");
        });

        modelBuilder.Entity<BlogComment>(entity =>
        {
            entity.HasKey(e => e.BlogCId).HasName("PK__BLOG_COM__334D871CC3097171");

            entity.ToTable("BLOG_COMMENT");

            entity.Property(e => e.BlogCId).HasColumnName("BLOG_C_ID");
            entity.Property(e => e.BlogCName)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("BLOG_C_NAME");
            entity.Property(e => e.BlogId).HasColumnName("BLOG_ID");
            entity.Property(e => e.BlogUId).HasColumnName("BLOG_U_ID");

            entity.HasOne(d => d.Blog).WithMany(p => p.BlogComments)
                .HasForeignKey(d => d.BlogId)
                .HasConstraintName("FK__BLOG_COMM__BLOG___66603565");

            entity.HasOne(d => d.Blo).WithMany(p => p.BlogComments)
                .HasForeignKey(d => d.BlogUId)
                .HasConstraintName("FK__BLOG_COMM__BLOG___6754599E");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CId).HasName("PK__CATEGORY__A9FDEC1212010D2F");

            entity.ToTable("CATEGORY");

            entity.Property(e => e.CId).HasColumnName("C_ID");
            entity.Property(e => e.CName)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("C_NAME");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.CId).HasName("PK__CONTACT__A9FDEC12CF587D3F");

            entity.ToTable("CONTACT");

            entity.Property(e => e.CId).HasColumnName("C_ID");
            entity.Property(e => e.CEmail)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("C_EMAIL");
            entity.Property(e => e.CMess)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("C_MESS");
            entity.Property(e => e.CName)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("C_NAME");
        });

        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(e => e.CId).HasName("PK__COUPON__A9FDEC1237513B62");

            entity.ToTable("COUPON");

            entity.Property(e => e.CId).HasColumnName("C_ID");
            entity.Property(e => e.CDiscount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("C_DISCOUNT");
            entity.Property(e => e.CName)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("C_NAME");
        });

        modelBuilder.Entity<LikeCount>(entity =>
        {
            entity.HasKey(e => e.LcId).HasName("PK__LIKE_COU__A9D704DE65895B54");

            entity.ToTable("LIKE_COUNT");

            entity.Property(e => e.LcId).HasColumnName("LC_ID");
            entity.Property(e => e.LcCount).HasColumnName("LC_COUNT");
            entity.Property(e => e.LcProdId).HasColumnName("LC_PROD_ID");

            entity.HasOne(d => d.LcProd).WithMany(p => p.LikeCounts)
                .HasForeignKey(d => d.LcProdId)
                .HasConstraintName("FK__LIKE_COUN__LC_PR__68487DD7");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__orders__C3905BCF1B43F2EB");

            entity.ToTable("orders");

            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasColumnType("date");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.OrderUser).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderUserId)
                .HasConstraintName("FK__orders__OrderUse__6B24EA82");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemsId).HasName("PK__orderIte__D5BB255511096FDA");

            entity.ToTable("orderItems");

            entity.Property(e => e.OrderItemsOrderId).HasColumnName("OrderItems_OrderId");
            entity.Property(e => e.OrderItemsProdId).HasColumnName("OrderItems_ProdId");

            entity.HasOne(d => d.OrderItemsOrder).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderItemsOrderId)
                .HasConstraintName("FK__orderItem__Order__693CA210");

            entity.HasOne(d => d.OrderItemsProd).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderItemsProdId)
                .HasConstraintName("FK__orderItem__Order__6A30C649");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.PId).HasName("PK__PRODUCT__A3420A776054ED83");

            entity.ToTable("PRODUCT");

            entity.Property(e => e.PId).HasColumnName("P_ID");
            entity.Property(e => e.PCatgId).HasColumnName("P_CATG_ID");
            entity.Property(e => e.PColor).HasColumnName("P_COLOR");
            entity.Property(e => e.PImage)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("P_IMAGE");
            entity.Property(e => e.PName)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("P_NAME");
            entity.Property(e => e.PPrice).HasColumnName("P_PRICE");

            entity.HasOne(d => d.PCatg).WithMany(p => p.Products)
                .HasForeignKey(d => d.PCatgId)
                .HasConstraintName("FK__PRODUCT__P_CATG___6D0D32F4");

            entity.HasOne(d => d.PColorNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.PColor)
                .HasConstraintName("FK__PRODUCT__P_COLOR__6C190EBB");
        });

        modelBuilder.Entity<ProductColor>(entity =>
        {
            entity.HasKey(e => e.PcId).HasName("PK__PRODUCT___B9EB73ADF34FD27D");

            entity.ToTable("PRODUCT_COLOR");

            entity.Property(e => e.PcId).HasColumnName("PC_ID");
            entity.Property(e => e.PcColor)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("PC_COLOR");
        });

        modelBuilder.Entity<ProductLike>(entity =>
        {
            entity.HasKey(e => e.PlId).HasName("PK__PRODUCT___7015FCD5B81F31B6");

            entity.ToTable("PRODUCT_LIKE");

            entity.Property(e => e.PlId).HasColumnName("PL_ID");
            entity.Property(e => e.PlCount).HasColumnName("PL_COUNT");
            entity.Property(e => e.PlProdId).HasColumnName("PL_PROD_ID");
            entity.Property(e => e.PlUser).HasColumnName("PL_USER");

            entity.HasOne(d => d.PlProd).WithMany(p => p.ProductLikes)
                .HasForeignKey(d => d.PlProdId)
                .HasConstraintName("FK__PRODUCT_L__PL_PR__6EF57B66");

            entity.HasOne(d => d.PlUserNavigation).WithMany(p => p.ProductLikes)
                .HasForeignKey(d => d.PlUser)
                .HasConstraintName("FK__PRODUCT_L__PL_US__6E01572D");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RId).HasName("PK__ROLE__DE152E89A9D34B36");

            entity.ToTable("ROLE");

            entity.Property(e => e.RId).HasColumnName("R_ID");
            entity.Property(e => e.RName)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("R_NAME");
        });

        modelBuilder.Entity<UsedCoupon>(entity =>
        {
            entity.HasKey(e => e.UcId).HasName("PK__USED_COU__4D28389B40F61E7E");

            entity.ToTable("USED_COUPON");

            entity.Property(e => e.UcId).HasColumnName("UC_ID");
            entity.Property(e => e.UcCouponId).HasColumnName("UC_COUPON_ID");
            entity.Property(e => e.UcUserId).HasColumnName("UC_USER_ID");

            entity.HasOne(d => d.UcCoupon).WithMany(p => p.UsedCoupons)
                .HasForeignKey(d => d.UcCouponId)
                .HasConstraintName("FK__USED_COUP__UC_CO__6FE99F9F");

            entity.HasOne(d => d.UcUser).WithMany(p => p.UsedCoupons)
                .HasForeignKey(d => d.UcUserId)
                .HasConstraintName("FK__USED_COUP__UC_US__70DDC3D8");
        });

        modelBuilder.Entity<UserRecord>(entity =>
        {
            entity.HasKey(e => e.UId).HasName("PK__USER_REC__5A2040DB0EEE751D");

            entity.ToTable("USER_RECORD");

            entity.Property(e => e.UId).HasColumnName("U_ID");
            entity.Property(e => e.UEmail)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("U_EMAIL");
            entity.Property(e => e.UName)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("U_NAME");
            entity.Property(e => e.UPass)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("U_PASS");
            entity.Property(e => e.URoleId).HasColumnName("U_ROLE_ID");

            entity.HasOne(d => d.URole).WithMany(p => p.UserRecords)
                .HasForeignKey(d => d.URoleId)
                .HasConstraintName("FK__USER_RECO__U_ROL__71D1E811");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
