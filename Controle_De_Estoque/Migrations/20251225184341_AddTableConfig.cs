using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Controle_De_Estoque.Migrations
{
    /// <inheritdoc />
    public partial class AddTableConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ConfirmPassword = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    permalink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stock_quantity = table.Column<int>(type: "int", nullable: true),
                    sku = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    variations = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Productsid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.id);
                    table.ForeignKey(
                        name: "FK_Products_Products_Productsid",
                        column: x => x.Productsid,
                        principalTable: "Products",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "UserConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeliClientId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeliClientSecret = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    redirect_uri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeliRefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WooUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WooConsumerKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WooConsumerSecret = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserMeliToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    access_token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    refreshtoken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Expire_in = table.Column<int>(type: "int", nullable: true),
                    datacriacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMeliToken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttributeWoo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    variation = table.Column<bool>(type: "bit", nullable: false),
                    options = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Productsid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeWoo", x => x.id);
                    table.ForeignKey(
                        name: "FK_AttributeWoo_Products_Productsid",
                        column: x => x.Productsid,
                        principalTable: "Products",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "SerchProduct",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    domain_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    domain_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    category_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    category_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    available_quantity = table.Column<int>(type: "int", nullable: true),
                    userMeliTokenId = table.Column<int>(type: "int", nullable: true),
                    userConfigId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SerchProduct", x => x.id);
                    table.ForeignKey(
                        name: "FK_SerchProduct_UserConfig_userConfigId",
                        column: x => x.userConfigId,
                        principalTable: "UserConfig",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SerchProduct_UserMeliToken_userMeliTokenId",
                        column: x => x.userMeliTokenId,
                        principalTable: "UserMeliToken",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AttributeEan",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    value_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    value_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerchProductid = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeEan", x => x.id);
                    table.ForeignKey(
                        name: "FK_AttributeEan_SerchProduct_SerchProductid",
                        column: x => x.SerchProductid,
                        principalTable: "SerchProduct",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Estoque",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantidade_Estoque = table.Column<int>(type: "int", nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    produtos_WooCommerceid = table.Column<int>(type: "int", nullable: true),
                    produtos_MercadoLivreid = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estoque", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Estoque_Products_produtos_WooCommerceid",
                        column: x => x.produtos_WooCommerceid,
                        principalTable: "Products",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Estoque_SerchProduct_produtos_MercadoLivreid",
                        column: x => x.produtos_MercadoLivreid,
                        principalTable: "SerchProduct",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Variations",
                columns: table => new
                {
                    variationsId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    available_quantity = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    seller_custom_field = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerchProductid = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Variations", x => x.variationsId);
                    table.ForeignKey(
                        name: "FK_Variations_SerchProduct_SerchProductid",
                        column: x => x.SerchProductid,
                        principalTable: "SerchProduct",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttributeEan_SerchProductid",
                table: "AttributeEan",
                column: "SerchProductid");

            migrationBuilder.CreateIndex(
                name: "IX_AttributeWoo_Productsid",
                table: "AttributeWoo",
                column: "Productsid");

            migrationBuilder.CreateIndex(
                name: "IX_Estoque_produtos_MercadoLivreid",
                table: "Estoque",
                column: "produtos_MercadoLivreid");

            migrationBuilder.CreateIndex(
                name: "IX_Estoque_produtos_WooCommerceid",
                table: "Estoque",
                column: "produtos_WooCommerceid");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Productsid",
                table: "Products",
                column: "Productsid");

            migrationBuilder.CreateIndex(
                name: "IX_SerchProduct_userConfigId",
                table: "SerchProduct",
                column: "userConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_SerchProduct_userMeliTokenId",
                table: "SerchProduct",
                column: "userMeliTokenId");

            migrationBuilder.CreateIndex(
                name: "IX_Variations_SerchProductid",
                table: "Variations",
                column: "SerchProductid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttributeEan");

            migrationBuilder.DropTable(
                name: "AttributeWoo");

            migrationBuilder.DropTable(
                name: "Estoque");

            migrationBuilder.DropTable(
                name: "Logins");

            migrationBuilder.DropTable(
                name: "Variations");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "SerchProduct");

            migrationBuilder.DropTable(
                name: "UserConfig");

            migrationBuilder.DropTable(
                name: "UserMeliToken");
        }
    }
}
