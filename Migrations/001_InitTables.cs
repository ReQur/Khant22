using FluentMigrator;


namespace dotnetserver
{
    [Migration(001)]
    public class InitTables_001 : Migration
    {
        public override void Down()
        {
            Execute.Script("./Migrations/sql/Down.001.InitTables.sql");
        }
        public override void Up()
        {
            Execute.Script("./Migrations/sql/Up.001.InitTables.sql");
        }
        
    }
}