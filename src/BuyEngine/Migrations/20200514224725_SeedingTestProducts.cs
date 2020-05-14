using Microsoft.EntityFrameworkCore.Migrations;

namespace BuyEngine.Migrations
{
    public partial class SeedingTestProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandId",
                schema: "BuyEngine",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Suppliers_SupplierId",
                schema: "BuyEngine",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "SupplierId",
                schema: "BuyEngine",
                table: "Products",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BrandId",
                schema: "BuyEngine",
                table: "Products",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                schema: "BuyEngine",
                table: "Brands",
                columns: new[] { "Id", "Name", "Notes", "WebsiteUrl" },
                values: new object[,]
                {
                    { 1, "Apple", "Makes overpriced phones that sell very well.", "https://www.apple.com" },
                    { 2, "Samsung", null, "https://www.samsung.com/us" },
                    { 3, "Google", null, "https://store.google.com/us/category/phones" }
                });

            migrationBuilder.InsertData(
                schema: "BuyEngine",
                table: "Suppliers",
                columns: new[] { "Id", "Name", "Notes", "WebsiteUrl" },
                values: new object[,]
                {
                    { 1, "Apple", null, "https://www.apple.com" },
                    { 2, "Samsung", null, "https://www.samsung.com/us" },
                    { 3, "Ebay", null, "https://www.ebay.com" },
                    { 4, "Alibaba", null, "https://www.alibaba.com" }
                });

            migrationBuilder.InsertData(
                schema: "BuyEngine",
                table: "Products",
                columns: new[] { "Id", "BrandId", "Description", "Enabled", "Name", "Price", "Quantity", "Sku", "SupplierId" },
                values: new object[,]
                {
                    { 1, 1, null, true, "iPhone 11", 699m, 75, "APP-IPH-001", 1 },
                    { 2, 1, null, true, "iPhone 11 Pro", 999m, 125, "APP-IPH-002", 1 },
                    { 3, 2, null, true, "Samsung S20", 999m, 15, "SAM-S20-001", 2 },
                    { 4, 2, null, true, "Samsung S20+", 1199m, 25, "SAM-S20-002", 2 },
                    { 5, 2, null, true, "Samsung S20 Ultra", 1399m, 20, "SAM-S20-003", 2 },
                    { 6, 3, null, true, "Google Pixel 4", 799m, 25, "GGL-PXL-001", 3 },
                    { 7, 3, null, true, "Google Pixel 4 XL", 899m, 20, "GGL-PXL-002", 4 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandId",
                schema: "BuyEngine",
                table: "Products",
                column: "BrandId",
                principalSchema: "BuyEngine",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Suppliers_SupplierId",
                schema: "BuyEngine",
                table: "Products",
                column: "SupplierId",
                principalSchema: "BuyEngine",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandId",
                schema: "BuyEngine",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Suppliers_SupplierId",
                schema: "BuyEngine",
                table: "Products");

            migrationBuilder.DeleteData(
                schema: "BuyEngine",
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "BuyEngine",
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "BuyEngine",
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "BuyEngine",
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "BuyEngine",
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "BuyEngine",
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "BuyEngine",
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "BuyEngine",
                table: "Brands",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "BuyEngine",
                table: "Brands",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "BuyEngine",
                table: "Brands",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "BuyEngine",
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "BuyEngine",
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "BuyEngine",
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "BuyEngine",
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.AlterColumn<int>(
                name: "SupplierId",
                schema: "BuyEngine",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "BrandId",
                schema: "BuyEngine",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandId",
                schema: "BuyEngine",
                table: "Products",
                column: "BrandId",
                principalSchema: "BuyEngine",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Suppliers_SupplierId",
                schema: "BuyEngine",
                table: "Products",
                column: "SupplierId",
                principalSchema: "BuyEngine",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
