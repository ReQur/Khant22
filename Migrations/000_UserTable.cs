using FluentMigrator;


namespace dotnetserver
{
    [Migration(000)]
    public class UserTable_000 : Migration
    {
        public override void Down()
        {
            Execute.Script("./Migrations/sql/Down.000.UserTable.sql");
        }
        public override void Up()
        {
            Execute.Script("./Migrations/sql/Up.000.UserTable.sql");
        }
        
    }
}