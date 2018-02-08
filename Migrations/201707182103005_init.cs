namespace FormToPDF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Forms",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(),
                        Date = c.DateTime(nullable: false),
                        FormType = c.String(),
                        FormName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Forms");
        }
    }
}
