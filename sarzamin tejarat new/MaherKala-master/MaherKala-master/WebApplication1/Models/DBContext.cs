using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class DBContext: DbContext
    {


        //public DBContext() : base("Data Source=.;Initial Catalog=cp33105_db;Integrated Security=true")
        //public DBContext() : base("Data Source=.;Initial Catalog=sarzami1_shop;User Id=sarzami1_shopusr;Password=Amir@amir$amir2;")
        //Db
        public DBContext() : base("Data Source=95.216.56.89,2016;Initial Catalog=atrincom123_shop;User Id=atrincom123_shop;Password=26cne3D&")
        {
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;   
        }
        static DBContext()
        {
           Database.SetInitializer<DBContext>(new MigrateDatabaseToLatestVersion<DBContext,configure>());
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<AndroidApp> AndroidApps { get; set; }
        public DbSet<PocketBook> PocketBooks { get; set; }

        public DbSet<Product> Products{ get; set; }
        public DbSet<Factor> Factors{ get; set; }
        public DbSet<FactorItem> FactorItems{ get; set; }

        public DbSet<User> Users{ get; set; }
        public DbSet<MobileSlider> MobileSliders{ get; set; }
        public DbSet<SiteSlider> SiteSliders { get; set; }
        public DbSet<SpecialProduct> SpecialProducts{ get; set; }
        public DbSet<Banner> Banners{ get; set; }
        public DbSet<Member> Members{ get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<DiscountCode> DiscountCode { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Role> Roles{ get; set; }
        public DbSet<UserProduct> UserProducts{ get; set; }
        public DbSet<UserComment> UserComments { get; set; }
        public DbSet<WholeSaler> WholeSalers{ get; set; }
        public DbSet<Email> Emails{ get; set; }
        public DbSet<ConfirmEmail> ConfirmEmails{ get; set; }
        public DbSet<UserRecover> UserRecover{ get; set; }
        public DbSet<Complaint> Complaints { get; set; }


        public DbSet<MarketerSlider> MarketerSliders{ get; set; }
        public DbSet<MarketerUser> MarketerUsers { get; set; }

        public DbSet<MarketerNews> MarketerNews { get; set; }
        public DbSet<MarketerPrize> MarketerPrizes{ get; set; }
        public DbSet<MarketerChat> MarketerChats { get; set; }
        public DbSet<MarketerFactor> MarketerFactor { get; set; }
        public DbSet<MarketerFactorItem> MarketerFactorItem { get; set; }
        public DbSet<Commission> Commission { get; set; }
        public DbSet<ProductPresent> ProductPresent { get; set; }
        public DbSet<Payment> Payments{ get; set; }
        public DbSet<Update> Updates { get; set; }
        public DbSet<MarketerPVTicketChat> MarketerPVTicketChats { get; set; }
    

        class configure : System.Data.Entity.Migrations.DbMigrationsConfiguration<DBContext>
        {
            public configure()
            {
                this.AutomaticMigrationsEnabled = true;
                this.AutomaticMigrationDataLossAllowed = true;
            }
        }

        

        public System.Data.Entity.DbSet<WebApplication1.Models.Student> Students { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.PricePointForAddSubset> pricePointForAddSubsets { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.RateOfAddSubSet> RateOfAddSubSets { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.MarketerUserPoints> MarketerUserPoints { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.PlanType> PlanTypes { get; set; }

     
        public System.Data.Entity.DbSet<WebApplication1.Models.Plannn> Plannns { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.Plan> Plans { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.MarketerActiveAccountTicket> MarketerActiveAccountTickets { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.MarketerLimitSale> MarketerLimitSale { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.MarketerImprovePlan> MarketerImprovePlans { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.MarketerTutorial> MarketerTutorials { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.MarketerTutorialFiles> MarketerTutorialFiles { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.PriceForTranslate> PriceForTranslates { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.RequestForTransfer> RequestForTransfers { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.RetailerFirstFactorDiscount> RetailerFirstFactorDiscounts { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.SendMessage> SendMessages { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.CartSharj> CartSharjs { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.Company> Companies { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.CompanyAgent> CompanyAgents { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.AdminUsers> AdminUsers { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.adminsRoles> adminsRoles { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.AdminsInRoles> AdminsInRoles { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.CompanyAjentProduct> CompanyAjentProducts { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.UserAnswer> UserAnswers { get; set; }

        
        public System.Data.Entity.DbSet<WebApplication1.Models.SaledProducts> SaledProducts { get; set; }


        public System.Data.Entity.DbSet<WebApplication1.Models.Converstions> Converstions { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.UserConversions> UserConversions { get; set; }
		public System.Data.Entity.DbSet<WebApplication1.Models.CartSharjType> CartSharjType { get; set; }

		public System.Data.Entity.DbSet<WebApplication1.Models.PlanDateregister> PlanDateregister { get; set; }

	}
}
