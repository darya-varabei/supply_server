﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SupplyIO.SupplyIO.Services.Models.Context;

#nullable disable

namespace SupplyIO.Migrations
{
    [DbContext(typeof(MetalContext))]
    partial class MetalContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SupplyIO.SupplyIO.Services.Models.CertificateModel.Certificate", b =>
                {
                    b.Property<int>("CertificateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CertificateId"));

                    b.Property<string>("Author")
                        .HasColumnType("text");

                    b.Property<string>("AuthorAddress")
                        .HasColumnType("text");

                    b.Property<string>("Contract")
                        .HasColumnType("text");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Fax")
                        .HasColumnType("text");

                    b.Property<string>("Gosts")
                        .HasColumnType("text");

                    b.Property<List<string>>("Link")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("Notes")
                        .HasColumnType("text");

                    b.Property<string>("Number")
                        .HasColumnType("text");

                    b.Property<string>("OrderNumber")
                        .HasColumnType("text");

                    b.Property<string>("PlaceNumber")
                        .HasColumnType("text");

                    b.Property<int?>("ProductId")
                        .HasColumnType("integer");

                    b.Property<string>("Recipient")
                        .HasColumnType("text");

                    b.Property<string>("RecipientCountry")
                        .HasColumnType("text");

                    b.Property<string>("ShipmentShop")
                        .HasColumnType("text");

                    b.Property<string>("SpecificationNumber")
                        .HasColumnType("text");

                    b.Property<string>("TypeOfPackaging")
                        .HasColumnType("text");

                    b.Property<string>("TypeOfRollingStock")
                        .HasColumnType("text");

                    b.Property<string>("WagonNumber")
                        .HasColumnType("text");

                    b.HasKey("CertificateId");

                    b.HasIndex("ProductId");

                    b.ToTable("Certificate");
                });

            modelBuilder.Entity("SupplyIO.SupplyIO.Services.Models.CertificateModel.ChemicalComposition", b =>
                {
                    b.Property<int>("ChemicalCompositionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ChemicalCompositionId"));

                    b.Property<double?>("Al")
                        .HasColumnType("double precision");

                    b.Property<double?>("AlWithN2")
                        .HasColumnType("double precision");

                    b.Property<double?>("As")
                        .HasColumnType("double precision");

                    b.Property<double?>("C")
                        .HasColumnType("double precision");

                    b.Property<double?>("Cev")
                        .HasColumnType("double precision");

                    b.Property<double?>("Cr")
                        .HasColumnType("double precision");

                    b.Property<double?>("Cu")
                        .HasColumnType("double precision");

                    b.Property<double?>("Mn")
                        .HasColumnType("double precision");

                    b.Property<double?>("Mo")
                        .HasColumnType("double precision");

                    b.Property<double?>("N2")
                        .HasColumnType("double precision");

                    b.Property<double?>("Ni")
                        .HasColumnType("double precision");

                    b.Property<string>("Notes")
                        .HasColumnType("text");

                    b.Property<double?>("P")
                        .HasColumnType("double precision");

                    b.Property<double?>("S")
                        .HasColumnType("double precision");

                    b.Property<double?>("Si")
                        .HasColumnType("double precision");

                    b.Property<double?>("Ti")
                        .HasColumnType("double precision");

                    b.Property<double?>("V")
                        .HasColumnType("double precision");

                    b.Property<double?>("W")
                        .HasColumnType("double precision");

                    b.HasKey("ChemicalCompositionId");

                    b.ToTable("ChemicalComposition");
                });

            modelBuilder.Entity("SupplyIO.SupplyIO.Services.Models.CertificateModel.ImpactStrength", b =>
                {
                    b.Property<int>("ImpactStrengthId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ImpactStrengthId"));

                    b.Property<double?>("AfterMechAgeing")
                        .HasColumnType("double precision");

                    b.Property<double?>("AfterMechAgeing1")
                        .HasColumnType("double precision");

                    b.Property<double?>("KCU")
                        .HasColumnType("double precision");

                    b.Property<double?>("KCU1")
                        .HasColumnType("double precision");

                    b.Property<double?>("KCV")
                        .HasColumnType("double precision");

                    b.Property<double?>("KCV1")
                        .HasColumnType("double precision");

                    b.HasKey("ImpactStrengthId");

                    b.ToTable("ImpactStrengths");
                });

            modelBuilder.Entity("SupplyIO.SupplyIO.Services.Models.CertificateModel.Package", b =>
                {
                    b.Property<int>("PackageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PackageId"));

                    b.Property<string>("Banding")
                        .HasColumnType("text");

                    b.Property<string>("Barcode")
                        .HasColumnType("text");

                    b.Property<string>("Batch")
                        .HasColumnType("text");

                    b.Property<string>("Bend")
                        .HasColumnType("text");

                    b.Property<string>("Brinel")
                        .HasColumnType("text");

                    b.Property<string>("Category")
                        .HasColumnType("text");

                    b.Property<string>("CategoryOfDrawing")
                        .HasColumnType("text");

                    b.Property<string>("Cementite")
                        .HasColumnType("text");

                    b.Property<int?>("CertificateId")
                        .HasColumnType("integer");

                    b.Property<int?>("ChemicalCompositionId")
                        .HasColumnType("integer");

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<string>("Corrosion")
                        .HasColumnType("text");

                    b.Property<int?>("CustomerItemNumber")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("DateAdded")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DateChange")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Decarburiization")
                        .HasColumnType("text");

                    b.Property<string>("DirectOfTestPicses")
                        .HasColumnType("text");

                    b.Property<double?>("Elongation")
                        .HasColumnType("double precision");

                    b.Property<string>("Eriksen")
                        .HasColumnType("text");

                    b.Property<string>("Flatness")
                        .HasColumnType("text");

                    b.Property<string>("Gost")
                        .HasColumnType("text");

                    b.Property<string>("Grade")
                        .HasColumnType("text");

                    b.Property<string>("GrainSize")
                        .HasColumnType("text");

                    b.Property<int?>("GroupCode")
                        .HasColumnType("integer");

                    b.Property<string>("Hardness")
                        .HasColumnType("text");

                    b.Property<string>("Heat")
                        .HasColumnType("text");

                    b.Property<int?>("ImpactStrengthId")
                        .HasColumnType("integer");

                    b.Property<double?>("KoafNavodorag")
                        .HasColumnType("double precision");

                    b.Property<string>("Link")
                        .HasColumnType("text");

                    b.Property<double?>("MicroBallCem")
                        .HasColumnType("double precision");

                    b.Property<double?>("N90")
                        .HasColumnType("double precision");

                    b.Property<string>("NamberConsignmentPackage")
                        .HasColumnType("text");

                    b.Property<string>("Notes")
                        .HasColumnType("text");

                    b.Property<string>("NumberOfClientMaterial")
                        .HasColumnType("text");

                    b.Property<string>("OrderFeatures")
                        .HasColumnType("text");

                    b.Property<int?>("OrderPosition")
                        .HasColumnType("integer");

                    b.Property<string>("PattemCutting")
                        .HasColumnType("text");

                    b.Property<List<byte[]>>("Photo")
                        .HasColumnType("bytea[]");

                    b.Property<double?>("Price")
                        .HasColumnType("double precision");

                    b.Property<string>("Profile")
                        .HasColumnType("text");

                    b.Property<int?>("Quantity")
                        .HasColumnType("integer");

                    b.Property<double?>("R90")
                        .HasColumnType("double precision");

                    b.Property<string>("Rockwell")
                        .HasColumnType("text");

                    b.Property<string>("RollingAccuracy")
                        .HasColumnType("text");

                    b.Property<string>("Roughness")
                        .HasColumnType("text");

                    b.Property<string>("SampleLocation")
                        .HasColumnType("text");

                    b.Property<string>("SerialNumber")
                        .HasColumnType("text");

                    b.Property<int?>("SizeId")
                        .HasColumnType("integer");

                    b.Property<double?>("SphericalHoleDepth")
                        .HasColumnType("double precision");

                    b.Property<string>("StateOfMatirial")
                        .HasColumnType("text");

                    b.Property<int?>("StatusId")
                        .HasColumnType("integer");

                    b.Property<string>("StrengthGroup")
                        .HasColumnType("text");

                    b.Property<string>("SurfaceQuality")
                        .HasColumnType("text");

                    b.Property<double?>("TemporalResistance")
                        .HasColumnType("double precision");

                    b.Property<string>("TensilePoint")
                        .HasColumnType("text");

                    b.Property<string>("TestingMethod")
                        .HasColumnType("text");

                    b.Property<string>("Treatment")
                        .HasColumnType("text");

                    b.Property<string>("TrimOfEdge")
                        .HasColumnType("text");

                    b.Property<string>("UnitTemporaryResistance")
                        .HasColumnType("text");

                    b.Property<string>("UnitYieldStrength")
                        .HasColumnType("text");

                    b.Property<string>("Variety")
                        .HasColumnType("text");

                    b.Property<int?>("WeightId")
                        .HasColumnType("integer");

                    b.Property<string>("Weldability")
                        .HasColumnType("text");

                    b.Property<string>("YieldPoint")
                        .HasColumnType("text");

                    b.HasKey("PackageId");

                    b.HasIndex("CertificateId");

                    b.HasIndex("ChemicalCompositionId");

                    b.HasIndex("ImpactStrengthId");

                    b.HasIndex("SizeId");

                    b.HasIndex("StatusId");

                    b.HasIndex("WeightId");

                    b.ToTable("Package");
                });

            modelBuilder.Entity("SupplyIO.SupplyIO.Services.Models.CertificateModel.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ProductId"));

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("Labeling")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("ProductId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("SupplyIO.SupplyIO.Services.Models.CertificateModel.Size", b =>
                {
                    b.Property<int>("SizeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("SizeId"));

                    b.Property<string>("Length")
                        .HasColumnType("text");

                    b.Property<double?>("Thickness")
                        .HasColumnType("double precision");

                    b.Property<double?>("Width")
                        .HasColumnType("double precision");

                    b.HasKey("SizeId");

                    b.ToTable("Size");
                });

            modelBuilder.Entity("SupplyIO.SupplyIO.Services.Models.CertificateModel.Status", b =>
                {
                    b.Property<int>("StatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("StatusId"));

                    b.Property<string>("StatusName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("StatusId");

                    b.ToTable("Status");
                });

            modelBuilder.Entity("SupplyIO.SupplyIO.Services.Models.CertificateModel.Weight", b =>
                {
                    b.Property<int>("WeightId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("WeightId"));

                    b.Property<double?>("Gross")
                        .HasColumnType("double precision");

                    b.Property<double?>("Gross2")
                        .HasColumnType("double precision");

                    b.Property<double?>("Net")
                        .HasColumnType("double precision");

                    b.HasKey("WeightId");

                    b.ToTable("Weight");
                });

            modelBuilder.Entity("SupplyIO.SupplyIO.Services.Models.Login.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Login")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text");

                    b.Property<DateTime?>("RefreshTokenExpiryTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("SupplyIO.SupplyIO.Services.Models.Login.UserInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("NumberOfPhone")
                        .HasColumnType("text");

                    b.Property<string>("Position")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserInfo");
                });

            modelBuilder.Entity("SupplyIO.SupplyIO.Services.Models.CertificateModel.Certificate", b =>
                {
                    b.HasOne("SupplyIO.SupplyIO.Services.Models.CertificateModel.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("SupplyIO.SupplyIO.Services.Models.CertificateModel.Package", b =>
                {
                    b.HasOne("SupplyIO.SupplyIO.Services.Models.CertificateModel.Certificate", "Certificate")
                        .WithMany("Packages")
                        .HasForeignKey("CertificateId");

                    b.HasOne("SupplyIO.SupplyIO.Services.Models.CertificateModel.ChemicalComposition", "ChemicalComposition")
                        .WithMany()
                        .HasForeignKey("ChemicalCompositionId");

                    b.HasOne("SupplyIO.SupplyIO.Services.Models.CertificateModel.ImpactStrength", "ImpactStrength")
                        .WithMany()
                        .HasForeignKey("ImpactStrengthId");

                    b.HasOne("SupplyIO.SupplyIO.Services.Models.CertificateModel.Size", "Size")
                        .WithMany()
                        .HasForeignKey("SizeId");

                    b.HasOne("SupplyIO.SupplyIO.Services.Models.CertificateModel.Status", "Status")
                        .WithMany("Packages")
                        .HasForeignKey("StatusId");

                    b.HasOne("SupplyIO.SupplyIO.Services.Models.CertificateModel.Weight", "Weight")
                        .WithMany()
                        .HasForeignKey("WeightId");

                    b.Navigation("Certificate");

                    b.Navigation("ChemicalComposition");

                    b.Navigation("ImpactStrength");

                    b.Navigation("Size");

                    b.Navigation("Status");

                    b.Navigation("Weight");
                });

            modelBuilder.Entity("SupplyIO.SupplyIO.Services.Models.Login.UserInfo", b =>
                {
                    b.HasOne("SupplyIO.SupplyIO.Services.Models.Login.User", "User")
                        .WithOne("UserInfo")
                        .HasForeignKey("SupplyIO.SupplyIO.Services.Models.Login.UserInfo", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SupplyIO.SupplyIO.Services.Models.CertificateModel.Certificate", b =>
                {
                    b.Navigation("Packages");
                });

            modelBuilder.Entity("SupplyIO.SupplyIO.Services.Models.CertificateModel.Status", b =>
                {
                    b.Navigation("Packages");
                });

            modelBuilder.Entity("SupplyIO.SupplyIO.Services.Models.Login.User", b =>
                {
                    b.Navigation("UserInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
