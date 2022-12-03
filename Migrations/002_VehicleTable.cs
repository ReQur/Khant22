using FluentMigrator;


namespace dotnetserver
{
    [Migration(002)]
    public class VehicleTable_002 : Migration
    {
        public override void Down()
        {
            Execute.Script("./Migrations/sql/Down.002.VehicleTable.sql");
        }
        public override void Up()
        {
            Execute.Script("./Migrations/sql/Up.002.VehicleTable.sql");
        }
        
    }
}