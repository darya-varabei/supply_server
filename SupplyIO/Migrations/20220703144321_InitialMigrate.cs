using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SupplyIO.Migrations
{
    public partial class InitialMigrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChemicalComposition",
                columns: table => new
                {
                    ChemicalCompositionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    C = table.Column<double>(type: "double precision", nullable: true),
                    Mn = table.Column<double>(type: "double precision", nullable: true),
                    Si = table.Column<double>(type: "double precision", nullable: true),
                    S = table.Column<double>(type: "double precision", nullable: true),
                    P = table.Column<double>(type: "double precision", nullable: true),
                    Cr = table.Column<double>(type: "double precision", nullable: true),
                    Ni = table.Column<double>(type: "double precision", nullable: true),
                    Cu = table.Column<double>(type: "double precision", nullable: true),
                    As = table.Column<double>(type: "double precision", nullable: true),
                    N2 = table.Column<double>(type: "double precision", nullable: true),
                    Al = table.Column<double>(type: "double precision", nullable: true),
                    Ti = table.Column<double>(type: "double precision", nullable: true),
                    Mo = table.Column<double>(type: "double precision", nullable: true),
                    W = table.Column<double>(type: "double precision", nullable: true),
                    V = table.Column<double>(type: "double precision", nullable: true),
                    AlWithN2 = table.Column<double>(type: "double precision", nullable: true),
                    Cev = table.Column<double>(type: "double precision", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChemicalComposition", x => x.ChemicalCompositionId);
                });

            migrationBuilder.CreateTable(
                name: "ImpactStrengths",
                columns: table => new
                {
                    ImpactStrengthId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KCU = table.Column<double>(type: "double precision", nullable: true),
                    KCU1 = table.Column<double>(type: "double precision", nullable: true),
                    KCV = table.Column<double>(type: "double precision", nullable: true),
                    KCV1 = table.Column<double>(type: "double precision", nullable: true),
                    AfterMechAgeing = table.Column<double>(type: "double precision", nullable: true),
                    AfterMechAgeing1 = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImpactStrengths", x => x.ImpactStrengthId);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Labeling = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "Size",
                columns: table => new
                {
                    SizeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Thickness = table.Column<double>(type: "double precision", nullable: true),
                    Width = table.Column<double>(type: "double precision", nullable: true),
                    Length = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Size", x => x.SizeId);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StatusName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Login = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Weight",
                columns: table => new
                {
                    WeightId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Gross = table.Column<double>(type: "double precision", nullable: true),
                    Gross2 = table.Column<double>(type: "double precision", nullable: true),
                    Net = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weight", x => x.WeightId);
                });

            migrationBuilder.CreateTable(
                name: "Certificate",
                columns: table => new
                {
                    CertificateId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Link = table.Column<List<string>>(type: "text[]", nullable: false),
                    Number = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Author = table.Column<string>(type: "text", nullable: true),
                    AuthorAddress = table.Column<string>(type: "text", nullable: true),
                    Fax = table.Column<string>(type: "text", nullable: true),
                    Recipient = table.Column<string>(type: "text", nullable: true),
                    RecipientCountry = table.Column<string>(type: "text", nullable: true),
                    Contract = table.Column<string>(type: "text", nullable: true),
                    SpecificationNumber = table.Column<string>(type: "text", nullable: true),
                    ProductId = table.Column<int>(type: "integer", nullable: true),
                    ShipmentShop = table.Column<string>(type: "text", nullable: true),
                    WagonNumber = table.Column<string>(type: "text", nullable: true),
                    OrderNumber = table.Column<string>(type: "text", nullable: true),
                    TypeOfRollingStock = table.Column<string>(type: "text", nullable: true),
                    TypeOfPackaging = table.Column<string>(type: "text", nullable: true),
                    PlaceNumber = table.Column<string>(type: "text", nullable: true),
                    Gosts = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificate", x => x.CertificateId);
                    table.ForeignKey(
                        name: "FK_Certificate_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    NumberOfPhone = table.Column<string>(type: "text", nullable: true),
                    Position = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInfo_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Package",
                columns: table => new
                {
                    PackageId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CertificateId = table.Column<int>(type: "integer", nullable: true),
                    Link = table.Column<string>(type: "text", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateChange = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StatusId = table.Column<int>(type: "integer", nullable: true),
                    NamberConsignmentPackage = table.Column<string>(type: "text", nullable: true),
                    Heat = table.Column<string>(type: "text", nullable: true),
                    Batch = table.Column<string>(type: "text", nullable: true),
                    OrderPosition = table.Column<int>(type: "integer", nullable: true),
                    NumberOfClientMaterial = table.Column<string>(type: "text", nullable: true),
                    SerialNumber = table.Column<string>(type: "text", nullable: true),
                    Grade = table.Column<string>(type: "text", nullable: true),
                    Category = table.Column<string>(type: "text", nullable: true),
                    StrengthGroup = table.Column<string>(type: "text", nullable: true),
                    Profile = table.Column<string>(type: "text", nullable: true),
                    Barcode = table.Column<string>(type: "text", nullable: true),
                    SizeId = table.Column<int>(type: "integer", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: true),
                    Variety = table.Column<string>(type: "text", nullable: true),
                    Gost = table.Column<string>(type: "text", nullable: true),
                    WeightId = table.Column<int>(type: "integer", nullable: true),
                    CustomerItemNumber = table.Column<int>(type: "integer", nullable: true),
                    Treatment = table.Column<string>(type: "text", nullable: true),
                    GroupCode = table.Column<int>(type: "integer", nullable: true),
                    PattemCutting = table.Column<string>(type: "text", nullable: true),
                    SurfaceQuality = table.Column<string>(type: "text", nullable: true),
                    RollingAccuracy = table.Column<string>(type: "text", nullable: true),
                    CategoryOfDrawing = table.Column<string>(type: "text", nullable: true),
                    StateOfMatirial = table.Column<string>(type: "text", nullable: true),
                    Roughness = table.Column<string>(type: "text", nullable: true),
                    Flatness = table.Column<string>(type: "text", nullable: true),
                    TrimOfEdge = table.Column<string>(type: "text", nullable: true),
                    Weldability = table.Column<string>(type: "text", nullable: true),
                    OrderFeatures = table.Column<string>(type: "text", nullable: true),
                    ChemicalCompositionId = table.Column<int>(type: "integer", nullable: true),
                    SampleLocation = table.Column<string>(type: "text", nullable: true),
                    DirectOfTestPicses = table.Column<string>(type: "text", nullable: true),
                    TemporalResistance = table.Column<double>(type: "double precision", nullable: true),
                    YieldPoint = table.Column<string>(type: "text", nullable: true),
                    TensilePoint = table.Column<string>(type: "text", nullable: true),
                    Elongation = table.Column<double>(type: "double precision", nullable: true),
                    Bend = table.Column<string>(type: "text", nullable: true),
                    Hardness = table.Column<string>(type: "text", nullable: true),
                    Rockwell = table.Column<string>(type: "text", nullable: true),
                    Brinel = table.Column<string>(type: "text", nullable: true),
                    Eriksen = table.Column<string>(type: "text", nullable: true),
                    ImpactStrengthId = table.Column<int>(type: "integer", nullable: true),
                    GrainSize = table.Column<string>(type: "text", nullable: true),
                    Decarburiization = table.Column<string>(type: "text", nullable: true),
                    Cementite = table.Column<string>(type: "text", nullable: true),
                    Banding = table.Column<string>(type: "text", nullable: true),
                    Corrosion = table.Column<string>(type: "text", nullable: true),
                    TestingMethod = table.Column<string>(type: "text", nullable: true),
                    UnitTemporaryResistance = table.Column<string>(type: "text", nullable: true),
                    UnitYieldStrength = table.Column<string>(type: "text", nullable: true),
                    SphericalHoleDepth = table.Column<double>(type: "double precision", nullable: true),
                    MicroBallCem = table.Column<double>(type: "double precision", nullable: true),
                    R90 = table.Column<double>(type: "double precision", nullable: true),
                    N90 = table.Column<double>(type: "double precision", nullable: true),
                    KoafNavodorag = table.Column<double>(type: "double precision", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Photo = table.Column<List<byte[]>>(type: "bytea[]", nullable: true),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Package", x => x.PackageId);
                    table.ForeignKey(
                        name: "FK_Package_Certificate_CertificateId",
                        column: x => x.CertificateId,
                        principalTable: "Certificate",
                        principalColumn: "CertificateId");
                    table.ForeignKey(
                        name: "FK_Package_ChemicalComposition_ChemicalCompositionId",
                        column: x => x.ChemicalCompositionId,
                        principalTable: "ChemicalComposition",
                        principalColumn: "ChemicalCompositionId");
                    table.ForeignKey(
                        name: "FK_Package_ImpactStrengths_ImpactStrengthId",
                        column: x => x.ImpactStrengthId,
                        principalTable: "ImpactStrengths",
                        principalColumn: "ImpactStrengthId");
                    table.ForeignKey(
                        name: "FK_Package_Size_SizeId",
                        column: x => x.SizeId,
                        principalTable: "Size",
                        principalColumn: "SizeId");
                    table.ForeignKey(
                        name: "FK_Package_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "StatusId");
                    table.ForeignKey(
                        name: "FK_Package_Weight_WeightId",
                        column: x => x.WeightId,
                        principalTable: "Weight",
                        principalColumn: "WeightId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Certificate_ProductId",
                table: "Certificate",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Package_CertificateId",
                table: "Package",
                column: "CertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_Package_ChemicalCompositionId",
                table: "Package",
                column: "ChemicalCompositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Package_ImpactStrengthId",
                table: "Package",
                column: "ImpactStrengthId");

            migrationBuilder.CreateIndex(
                name: "IX_Package_SizeId",
                table: "Package",
                column: "SizeId");

            migrationBuilder.CreateIndex(
                name: "IX_Package_StatusId",
                table: "Package",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Package_WeightId",
                table: "Package",
                column: "WeightId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInfo_UserId",
                table: "UserInfo",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Package");

            migrationBuilder.DropTable(
                name: "UserInfo");

            migrationBuilder.DropTable(
                name: "Certificate");

            migrationBuilder.DropTable(
                name: "ChemicalComposition");

            migrationBuilder.DropTable(
                name: "ImpactStrengths");

            migrationBuilder.DropTable(
                name: "Size");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "Weight");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
