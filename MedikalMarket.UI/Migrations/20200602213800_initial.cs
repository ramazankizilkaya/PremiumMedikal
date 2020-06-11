using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MedikalMarket.UI.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    NameSurname = table.Column<string>(maxLength: 100, nullable: false),
                    EmailAddress = table.Column<string>(maxLength: 100, nullable: false),
                    Password = table.Column<string>(maxLength: 20, nullable: false),
                    CellPhoneNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    Culture = table.Column<string>(nullable: true),
                    PhotoFileName = table.Column<string>(nullable: true),
                    PhotoAltTag = table.Column<string>(nullable: true),
                    AdproHref = table.Column<string>(nullable: true),
                    AdproTargetType = table.Column<int>(nullable: false),
                    TargetTopCategoryId = table.Column<int>(nullable: true),
                    TargetMiddleCategoryId = table.Column<int>(nullable: true),
                    TargetSubCategoryId = table.Column<int>(nullable: true),
                    TargetProductId = table.Column<int>(nullable: true),
                    TargetBrandId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    BrandName = table.Column<string>(nullable: true),
                    PhotoFileName = table.Column<string>(nullable: true),
                    PhotoAltTagTR = table.Column<string>(nullable: true),
                    PhotoAltTagEN = table.Column<string>(nullable: true),
                    BandNameUrl = table.Column<string>(nullable: true),
                    PhotoAltTagRU = table.Column<string>(nullable: true),
                    MasterPageMetaTitleTR = table.Column<string>(nullable: true),
                    MasterPageMetaTitleEN = table.Column<string>(nullable: true),
                    MasterPageMetaTitleRU = table.Column<string>(nullable: true),
                    MasterPageMetaDescriptionTR = table.Column<string>(nullable: true),
                    MasterPageMetaDescriptionEN = table.Column<string>(nullable: true),
                    MasterPageMetaDescriptionRU = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactUsMessages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    NameSurname = table.Column<string>(maxLength: 100, nullable: false),
                    EmailAddress = table.Column<string>(maxLength: 100, nullable: false),
                    Subject = table.Column<string>(maxLength: 100, nullable: false),
                    Message = table.Column<string>(maxLength: 4000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactUsMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    NameSurname = table.Column<string>(maxLength: 100, nullable: false),
                    EmailAddress = table.Column<string>(maxLength: 100, nullable: false),
                    Password = table.Column<string>(maxLength: 20, nullable: false),
                    CellPhoneNumber = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    IsSubscribedToSMS = table.Column<bool>(nullable: false),
                    IsSubscribedToEmail = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailNewsletters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    EmailAddress = table.Column<string>(maxLength: 100, nullable: false),
                    UserIp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailNewsletters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ErrorLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    ErrorDetail = table.Column<string>(nullable: true),
                    ErrorLocation = table.Column<string>(nullable: true),
                    Culture = table.Column<string>(nullable: true),
                    ErrorUrl = table.Column<string>(nullable: true),
                    StatusCode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MiniSliders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    Culture = table.Column<string>(nullable: true),
                    PhotoFileName = table.Column<string>(nullable: true),
                    PhotoAltTag = table.Column<string>(nullable: true),
                    SliderHref = table.Column<string>(nullable: true),
                    TargetProductId = table.Column<int>(nullable: false),
                    TargetProductName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MiniSliders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sliders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    Culture = table.Column<string>(nullable: true),
                    PhotoFileName = table.Column<string>(nullable: true),
                    ThumbFileName = table.Column<string>(nullable: true),
                    PhotoAltTag = table.Column<string>(nullable: true),
                    SliderHref = table.Column<string>(nullable: true),
                    SliderTargetType = table.Column<int>(nullable: false),
                    TargetTopCategoryId = table.Column<int>(nullable: true),
                    TargetMiddleCategoryId = table.Column<int>(nullable: true),
                    TargetSubCategoryId = table.Column<int>(nullable: true),
                    TargetProductId = table.Column<int>(nullable: true),
                    TargetBrandId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sliders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TopCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    NameTR = table.Column<string>(maxLength: 200, nullable: false),
                    NameEN = table.Column<string>(maxLength: 200, nullable: false),
                    NameRU = table.Column<string>(maxLength: 200, nullable: false),
                    HasMiddleCategories = table.Column<bool>(nullable: false),
                    TopCategoryNameUrlTR = table.Column<string>(nullable: true),
                    TopCategoryNameUrlRU = table.Column<string>(nullable: true),
                    TopCategoryNameUrlEN = table.Column<string>(nullable: true),
                    HeadTitleTR = table.Column<string>(nullable: true),
                    HeadTitleEN = table.Column<string>(nullable: true),
                    HeadTitleRU = table.Column<string>(nullable: true),
                    HeadDescriptionTR = table.Column<string>(nullable: true),
                    HeadDescriptionEN = table.Column<string>(nullable: true),
                    HeadDescriptionRU = table.Column<string>(nullable: true),
                    CategoryType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MiddleCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    NameTR = table.Column<string>(maxLength: 100, nullable: false),
                    NameEN = table.Column<string>(maxLength: 100, nullable: false),
                    NameRU = table.Column<string>(maxLength: 100, nullable: false),
                    MiddleCategoryNameUrlTR = table.Column<string>(nullable: true),
                    MiddleCategoryNameUrlEN = table.Column<string>(nullable: true),
                    MiddleCategoryNameUrlRU = table.Column<string>(nullable: true),
                    HasSubCategories = table.Column<bool>(nullable: false),
                    TopCategoryId = table.Column<int>(nullable: false),
                    HeadTitleTR = table.Column<string>(nullable: true),
                    HeadTitleEN = table.Column<string>(nullable: true),
                    HeadTitleRU = table.Column<string>(nullable: true),
                    HeadDescriptionTR = table.Column<string>(nullable: true),
                    HeadDescriptionEN = table.Column<string>(nullable: true),
                    HeadDescriptionRU = table.Column<string>(nullable: true),
                    CategoryType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MiddleCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MiddleCategories_TopCategories_TopCategoryId",
                        column: x => x.TopCategoryId,
                        principalTable: "TopCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    NameTR = table.Column<string>(maxLength: 100, nullable: false),
                    NameEN = table.Column<string>(maxLength: 100, nullable: false),
                    NameRU = table.Column<string>(maxLength: 100, nullable: false),
                    SubCategoryNameUrlTR = table.Column<string>(nullable: true),
                    SubCategoryNameUrlEN = table.Column<string>(nullable: true),
                    SubCategoryNameUrlRU = table.Column<string>(nullable: true),
                    HeadTitleTR = table.Column<string>(nullable: true),
                    HeadTitleEN = table.Column<string>(nullable: true),
                    HeadTitleRU = table.Column<string>(nullable: true),
                    HeadDescriptionTR = table.Column<string>(nullable: true),
                    HeadDescriptionEN = table.Column<string>(nullable: true),
                    HeadDescriptionRU = table.Column<string>(nullable: true),
                    CategoryType = table.Column<int>(nullable: false),
                    MiddleCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCategories_MiddleCategories_MiddleCategoryId",
                        column: x => x.MiddleCategoryId,
                        principalTable: "MiddleCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    NameTR = table.Column<string>(nullable: false),
                    NameEN = table.Column<string>(nullable: false),
                    NameRU = table.Column<string>(nullable: false),
                    ProductDescriptionTR = table.Column<string>(nullable: false),
                    ProductDescriptionEN = table.Column<string>(nullable: false),
                    ProductDescriptionRU = table.Column<string>(nullable: false),
                    ProductNameUrlTR = table.Column<string>(nullable: true),
                    ProductNameUrlEN = table.Column<string>(nullable: true),
                    ProductNameUrlRU = table.Column<string>(nullable: true),
                    MainPhotoFileName = table.Column<string>(nullable: true),
                    ProductCode = table.Column<string>(nullable: true),
                    DiscountRate = table.Column<int>(nullable: true),
                    CountOfVisit = table.Column<int>(nullable: true),
                    CountOfFavorite = table.Column<int>(nullable: true),
                    CountOfPriceAlarm = table.Column<int>(nullable: true),
                    StockNumber = table.Column<int>(nullable: true),
                    NumberInStock = table.Column<int>(nullable: true),
                    ProductOfferType = table.Column<int>(nullable: false),
                    HasNewBadge = table.Column<bool>(nullable: false),
                    IsFreeShipping = table.Column<bool>(nullable: false),
                    HeadTitleTR = table.Column<string>(nullable: false),
                    HeadTitleEN = table.Column<string>(nullable: false),
                    HeadTitleRU = table.Column<string>(nullable: false),
                    HeadDescriptionTR = table.Column<string>(nullable: false),
                    HeadDescriptionEN = table.Column<string>(nullable: false),
                    HeadDescriptionRU = table.Column<string>(nullable: false),
                    PhotoAltTagTR = table.Column<string>(nullable: false),
                    PhotoAltTagEN = table.Column<string>(nullable: false),
                    PhotoAltTagRU = table.Column<string>(nullable: false),
                    BrandId = table.Column<int>(nullable: false),
                    TopCategoryId = table.Column<int>(nullable: false),
                    MiddleCategoryId = table.Column<int>(nullable: true),
                    SubCategoryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_MiddleCategories_MiddleCategoryId",
                        column: x => x.MiddleCategoryId,
                        principalTable: "MiddleCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_TopCategories_TopCategoryId",
                        column: x => x.TopCategoryId,
                        principalTable: "TopCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    CustomerId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteProducts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductPhotos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    PhotoFileName = table.Column<string>(nullable: true),
                    IsMainPhoto = table.Column<bool>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPhotos_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProducts_CustomerId",
                table: "FavoriteProducts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProducts_ProductId",
                table: "FavoriteProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_MiddleCategories_TopCategoryId",
                table: "MiddleCategories",
                column: "TopCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPhotos_ProductId",
                table: "ProductPhotos",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_MiddleCategoryId",
                table: "Products",
                column: "MiddleCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SubCategoryId",
                table: "Products",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_TopCategoryId",
                table: "Products",
                column: "TopCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_MiddleCategoryId",
                table: "SubCategories",
                column: "MiddleCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "AdProducts");

            migrationBuilder.DropTable(
                name: "ContactUsMessages");

            migrationBuilder.DropTable(
                name: "EmailNewsletters");

            migrationBuilder.DropTable(
                name: "ErrorLogs");

            migrationBuilder.DropTable(
                name: "FavoriteProducts");

            migrationBuilder.DropTable(
                name: "MiniSliders");

            migrationBuilder.DropTable(
                name: "ProductPhotos");

            migrationBuilder.DropTable(
                name: "Sliders");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropTable(
                name: "MiddleCategories");

            migrationBuilder.DropTable(
                name: "TopCategories");
        }
    }
}
