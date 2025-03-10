using FluentMigrator;

namespace Student_API.Migrations
{
    [Migration(202503052300)] 
    public class Migration202503052300_Student_Address : Migration 
    {
        public override void Up()
        {
            Create.Table("Address")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Barangay").AsString(255).NotNullable()
                .WithColumn("Province").AsString(255).NotNullable()
                .WithColumn("Municipality").AsString(255).NotNullable()
                .WithColumn("Country").AsString(255).NotNullable();

            Create.Table("Students")
                .WithColumn("StudentId").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("Birthday").AsDateTime().Nullable()
                .WithColumn("Email").AsString(255).NotNullable()
                .WithColumn("PhoneNumber").AsString(50).Nullable()
                .WithColumn("CreatedDate").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
                .WithColumn("AddressId").AsInt32().NotNullable()
                .ForeignKey("FK_Students_Address", "Address", "Id")
                .OnDelete(System.Data.Rule.Cascade);

            Create.Index("IX_Students_AddressId").OnTable("Students").OnColumn("AddressId");
        }

        public override void Down()
        {
            Delete.Table("Students");
            Delete.Table("Address");
        }
    }
}
