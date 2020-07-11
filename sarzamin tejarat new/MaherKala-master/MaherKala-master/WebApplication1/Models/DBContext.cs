using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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
        //public DBContext() : base("Data Source=.;Initial Catalog=cp33105_db;Integrated Security=true")
        //{
        //    this.Configuration.ProxyCreationEnabled = false;
        //    this.Configuration.LazyLoadingEnabled = false;
        //}
        //Iran Server
        //public DBContext() : base("Data Source=185.4.31.137,2016;Initial Catalog=atrincom123_shop;User Id=sa;Password=zurEUxXhbAqW8dqHTCcP")
        //{
        //    this.Configuration.ProxyCreationEnabled = false;
        //    this.Configuration.LazyLoadingEnabled = false;
        //}
        //IPAServer
        //public DBContext() : base("Data Source=server;Initial Catalog=test;User Id=sa;Password=@mscdb2")
        //{
        //    this.Configuration.ProxyCreationEnabled = false;
        //    this.Configuration.LazyLoadingEnabled = false;
        //}

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

        public DbSet<CreditPayConditations> creditPayConditations { get; set; }

        
        class configure : System.Data.Entity.Migrations.DbMigrationsConfiguration<DBContext>
        {
            public configure()
            {
                this.AutomaticMigrationsEnabled = true;
                this.AutomaticMigrationDataLossAllowed = true;
            }
            protected override void Seed(WebApplication1.Models.DBContext context)
            {
                
                if (!context.Settings.Any())
                {
                    string _AbutsUs = "<div class=\"col-md-7\" style=\"text-align:right\"><div><strong>فروشگاه اینترنتی سرزمین تجارت</strong></div><div style=\"padding-bottom:20px;\"> وجود معضلاتی چون ترافیک شهری، آلودگی هوا، نگرانی از وجود کالاهای قاچاق و غیر اصل در بازار و یا بعضاً عرضه محصولات با قیمت های غیر واقعی که امروز با آن ها درگیر هستیم همگی اهمیت وجود مراجع معتبری چون جهت ارائه محصولات به صورت مجازی را بیش از پیش نشان می دهد. آشنایی هر چه بیشتر عموم مردم جامعه با مقوله فروشگاه های مجازی و فرهنگ خرید آنلاین در کنار احترام به این حق مسلم مشتری که کالای مورد نظرش را با اطمینان از اصالت و با قیمت مناسب، با برخورداری از گارانتی و خدمات پس از فروش دریافت می کند، از جمله مهم ترین اصول و خط مشی می باشد. رویکرد ما در این باره این است که با ارائه بهترین و با کیفیت ترین خدمات آنلاین به مشتریان گرامی، تجربه یک خرید آنلاین خوب و اطمینان بخش را برای آن ها رقم زنیم</div><div style =\"padding-bottom:20px;\"><hr/><p> مجتبی عسگری برزانی</p></div></div><p>&nbsp;</p>";
                    string _CallUs = "<div class=\"data\" style=\"text-align:right;\"><p> بخش مرکزی ، شهر اصفهان، ارگ عظیم جهان نما-فاز سوم - طبقه سوم - واحد 311 </p><hr/><div class=\"data\" style=\"text-align:right;\"><div class=\"col-md-4\"><div class=\"form-group has-feedback\"><div class=\"data\">&nbsp;</div><div class=\"data\">&nbsp;عسگری 09377903500</div></div></div></div><div class=\"col-md-4\"><div class=\"form-group has-feedback\"><div class=\"data\" dir=\"rtl\">کانال ارتباطی دیگر</div><div class=\"data\" dir=\"rtl\">sarzamin.tejarat @gmail.com</div><div class=\"data\" dir=\"rtl\">&nbsp;</div><div class=\"data\" dir=\"rtl\">&nbsp;</div></div></div></div>";
                    string _HowToWork = "<p style=\"text-align:right\"><span dir=\"rtl\">هدف از تاسیس این سایت همکاری با تولید کنندگان و واردکنندگان اصلی کالاها و همچنین پیشبرد در زمینه تولید محصولات داخلی و صادرات آن می باشند به طوریکه شرکت سرزمین تجارت با کسب نمایندگی بخش وسیعی از این دسته شرکت ها در صدد است تا رشد خوبی در این حوزه بدست آورد تا گامی بزرگ در مسیر پیشرفت و رونق اقتصاد کشور را فراهم سازد.</span></p><p style = \"text-align:right\"><strong><span style = \"font-size:14px\"><span dir = \"rtl\" > قوانین استفاده از سایت در زمینه خرید و فروش</span></span></strong></p><p style = \"text-align:right\"><span dir = \"rtl\"> قوانین سایت در چند دسته کلی تنظیم شده که بند به بند آن در ذیل توضیح داده می شود</span></p><p style = \"text-align:right\" ><span style = \"font-size:14px\"><strong><span dir = \"rtl\"> 1.قوانین ارسال کالا</span></strong></span></p><p style =\"text-align:right\" ><span dir =\"rtl\" > هر کالا ظرف مدت 24 الی 72 ساعت در سرتاسر کشور تحویل داده می شود.</span></p><p style = \"text-align:right\"><span dir = \"rtl\" > خرید های ثبت شده با مبلغ مثبت پنج میلیون ریال شامل ارسال رایگان می شود</span></p><p style =\"text-align:right\"><strong><span style =\"font-size:14px\"><span dir = \"rtl\"> 2.قوانین گارانتی محصول پس از ارسال پست :</span></span></strong></p><p style = \"text-align:right\"><span dir = \"rtl\" > شرکت سرزمین تجارت این امکان را فراهم کرده تا ظرف 48 ساعت بتوانید کالای خود را مرجوع کرده و در صورت تائید مشاوران شرکت با هزینه خود اقدام به ارسال محصول سالم می کند.</span></p><p style = \"text-align:right\" ><strong><span style = \"font-size:14px\" ><span dir = \"rtl\" > 3.تبصره در خصوص گارانتی کالا :</span></span></strong></p><p style = \"text-align:right\"><span dir = \"rtl\" > اگر کالای دارای نقص بعد از گذشت &nbsp; 48 ساعت از دریافت آن خواهان مرجوع شدن آن شدید به هیچ وجه این امکان وجود نداشته و شرکت هیچ مسئولیتی در قبال آن قبول نمی کند.</span></p><p style = \"text-align:right\"><strong><span style = \"font-size:14px\"><span dir = \"rtl\"> تبصره 1 </span></span></strong></p><p dir = \"rtl\" style = \"text-align:justify\" ><span dir = \"rtl\" > تنها کالا هایی حق مرجوعیت دارند که هیچگونه تغییری در باکس آن اعم از(شکستگی ؛ خراشیدگی ؛و یا هرنوع تغییر ظاهری ؛ دستکاری & nbsp;) که در اثر استفاده نادرست مشتری صورت گرفته&nbsp; نداشته درغیر این صورت شامل گارانتی تعویض کالا قرار نمی گیرد </span></p><p dir=\"rtl\"> &nbsp;</p>";
                    context.Settings.AddOrUpdate(
                        p=>p.Id,new Setting {AboutUs=_AbutsUs,CallUs=_CallUs,HowToWork=_HowToWork,FirstCategory=361,SecoundCategory=342,Notificaion=string.Empty,NotificaionUrl=string.Empty,Email= "info@sarzamintejarat.com",EmailPassword= "Q_jfs786",HostName= "mail.sarzamintejarat.com",Domain=null,SiteName=null,TransportationEsfahan=20000,TransportationNajafabad=10000,TransportationOther=3000 } );
                }
                if (!context.PlanTypes.Any())
                {
                    context.PlanTypes.AddOrUpdate(
                        p => p.Id, new PlanType { Id = 1, Name = "نقره ای" },
                        new PlanType { Id = 2, Name = "طلایی" });
                }
                if (!context.Companies.Any())
                {
                    context.Companies.AddOrUpdate(
                        p => p.Id, new Company { Id = 1, Name = "بدون شرکت" , Address="بدون آدرس" , cityName = "بدون شهر", NationalCode=0, PhoneNumber=0, subCityName="بدون شهرستان" }
                        );
                }
                if (!context.Plannns.Any())
                {
                    context.Plannns.AddOrUpdate(
                        p => p.Id, new Plannn { Id = 1, Level = 0, PlanTypeID = 1, }
                        );
                }
                if (!context.AdminUsers.Any())
                {
                    context.AdminUsers.AddOrUpdate(
                        p=>p.Id,new AdminUsers { Id=1,FirstName="atrin",LastName="atrin",Mobile=09136569769,UserName="atrin",Password="123"}
                        );}
                if(!context.adminsRoles.Any() || context.adminsRoles.Count() < 41)
                {
                    context.adminsRoles.AddOrUpdate(
                        p=>p.Id,new adminsRoles { Id=1,RoleName= "دسترسی به همه موارد", RoleDes="a"},
                        new adminsRoles { Id = 2, RoleName = "خریدها", RoleDes = "r1" },
                        new adminsRoles { Id = 3, RoleName = "دسته بندی", RoleDes = "r2" },
                       new adminsRoles { Id = 4, RoleName = "تعیین هزینه ارسال", RoleDes = "r3" },
                       new adminsRoles { Id = 5, RoleName = "تعیین تخفیف برای خرده فروشان تازه کار", RoleDes = "r4" },
                       new adminsRoles { Id = 6, RoleName = "محصولات", RoleDes = "r5" },
                       new adminsRoles { Id = 7, RoleName = "اخبار", RoleDes = "r6" },
                       new adminsRoles { Id = 8, RoleName = "ارسال ایمیل کلی", RoleDes = "r7" },
                       new adminsRoles { Id = 9, RoleName = "ارسال نوتیفیکیشن", RoleDes = "r8" },
                       new adminsRoles { Id = 10, RoleName = "ارسال پیام", RoleDes = "r9" },
                       new adminsRoles { Id = 11, RoleName = "تعریف کارت شارژ", RoleDes = "r10" },
                       new adminsRoles { Id = 12, RoleName = "تایید نظرات", RoleDes = "r11" },
                       new adminsRoles { Id = 13, RoleName = "پورسانت بازاریاب", RoleDes = "r12" },
                       new adminsRoles { Id = 14, RoleName = "فاکتورهای بازاریاب", RoleDes = "r13" },
                       new adminsRoles { Id = 15, RoleName = "مدیریت کاربران آپ", RoleDes = "r14" },
                       new adminsRoles { Id = 16, RoleName = "اخباربازاریاب", RoleDes = "r15" },
                       new adminsRoles { Id = 17, RoleName = "جوایزبازاریاب", RoleDes = "r16" },
                       new adminsRoles { Id = 18, RoleName = "اسلایدربازاریاب", RoleDes = "r17" },
                       new adminsRoles { Id = 19, RoleName = "ایجاد نرخ افزایش زیرمجموعه", RoleDes = "r18" },
                       new adminsRoles { Id = 20, RoleName = "ایجادپلن بازاریاب", RoleDes = "r19" },
                       new adminsRoles { Id = 21, RoleName = "تنظیمات ورودبازاریاب", RoleDes = "r20" },
                       new adminsRoles { Id = 22, RoleName = "تعرفه های ارتقای پلن", RoleDes = "r21" },
                       new adminsRoles { Id = 23, RoleName = "آموزش بازاریاب", RoleDes = "r22" },
                       new adminsRoles { Id = 24, RoleName = "تعویض بازاریاب", RoleDes = "r23" },
                       new adminsRoles { Id = 25, RoleName = "مدیریت افرادی که میخواهند به بازاریاب تبدیل شوند", RoleDes = "r24" },
                       new adminsRoles { Id = 26, RoleName = "تعریف تولیدکنندگان", RoleDes = "r25" },
                       new adminsRoles { Id = 27, RoleName = "تولیدکنندگان در انتظار تایید", RoleDes = "r26" },
                       new adminsRoles { Id = 28, RoleName = "بخش همکاری با ما", RoleDes = "r27" },
                       new adminsRoles { Id = 29, RoleName = "مدیریت کاربران سایت", RoleDes = "r28" },
                       new adminsRoles { Id = 30, RoleName = "هزینه حمل و نقل", RoleDes = "r29" },
                       new adminsRoles { Id = 31, RoleName = "اسلایدر سایت", RoleDes = "r30" },
                       new adminsRoles { Id = 32, RoleName = "اسلایدرموبایل", RoleDes = "r31" },
                       new adminsRoles { Id = 33, RoleName = "مدیریت کالاهای ویژه", RoleDes = "r32" },
                       new adminsRoles { Id = 34, RoleName = "مدیریت ردیف های چرخشی", RoleDes = "r33" },
                       new adminsRoles { Id = 35, RoleName = "مدیریت بنرها", RoleDes = "r34" },
                       new adminsRoles { Id = 36, RoleName = "تعریف کدتخفیف", RoleDes = "r35" },
                       new adminsRoles { Id = 37, RoleName = "مدیریت تماس با ما", RoleDes = "r36" },
                       new adminsRoles { Id = 38, RoleName = "آپلودورژن اندروید", RoleDes = "r37" },
                       new adminsRoles { Id = 39, RoleName = "مدیریت درباره ما", RoleDes = "r38" },
                       new adminsRoles { Id = 40, RoleName = "مدیریت نحوه فعالیت", RoleDes = "r39" },
                       new adminsRoles { Id = 41, RoleName = "مدیریت تنظیم ایمیل", RoleDes = "r40" },
                       new adminsRoles { Id = 42, RoleName = "تنظیم حداقل خرید ها", RoleDes = "r41" },
                       new adminsRoles { Id = 43, RoleName = "ایجادشرایط پرداخت چکی", RoleDes = "r42" },
                       new adminsRoles { Id = 44, RoleName = "مدیریت بانک", RoleDes = "r43" },
                       new adminsRoles { Id = 45, RoleName = "ایجاد شرایط پرداخت اعتباری", RoleDes = "r44" },
                       new adminsRoles { Id = 46, RoleName = "ایجاد چک", RoleDes = "r45" },
                       new adminsRoles { Id = 47, RoleName = " ایجاد سفته", RoleDes = "r46" },
                       new adminsRoles { Id = 48, RoleName = "درخواست های پرداخت چک", RoleDes = "r47" },
                       new adminsRoles { Id = 49, RoleName = "درخواست های پرداخت اعتباری", RoleDes = "r48" }
                       ); }
                if (!context.AdminsInRoles.Any())
                {
                    context.AdminsInRoles.AddOrUpdate(
                        p=>p.Id,new AdminsInRoles { AdminUsersID=1,AdminsRolesID=1}
                        );
                }
                if(context.PaySettings.Where(s=>s.Name== "MinimumForPayInPerson").FirstOrDefault() == null)
                {
                    context.PaySettings.AddOrUpdate(
                        p=>p.Id,new PaySetting { Id=1,Name= "MinimumForPayInPerson",Type=PaySettingType.MinimumForPayInPerson,Value=500000 }
                        );
                }
                if(context.PaySettings.Where(s=>s.Name== "MaximumForPayInPerson").FirstOrDefault()==null)
                {
                    context.PaySettings.AddOrUpdate(
                        p=>p.Id,new PaySetting { Id=2 , Name = "MaximumForPayInPerson", Type=PaySettingType.MaximumForPayInPerson,Value=1000000}
                        );
                }
                if(context.PaySettings.Where(s=>s.Name == "MinimumForCheckPay").FirstOrDefault() == null)
                {
                    context.PaySettings.AddOrUpdate(
                    p => p.Id, new PaySetting { Id = 3, Name = "MinimumForCheckPay", Type = PaySettingType.MinimumForCheckPay, Value = 400000 }
                     );
                }
                if (context.PaySettings.Where(s => s.Name == "MinimumForCreditPay").FirstOrDefault() == null)
                {
                    context.PaySettings.AddOrUpdate(
                    p => p.Id, new PaySetting { Id = 4, Name = "MinimumForCreditPay", Type = PaySettingType.MinimumForCreditPay, Value = 600000 }
                     );
                }
                if (!context.Banks.Any())
                {
                    context.Banks.AddOrUpdate(
                        p => p.Id, new bank { Id = 1,Name="ملی"  },
                        new bank { Id=2 , Name="تجارت"}
                        );
                }
                if(!context.Roles.Any())
				{
                    context.Roles.AddOrUpdate(
                      p => p.Id, new Role { Id = 1, RoleNameFa = "مدیر سیستم",RoleNameEn = "Admin" },
                      new Role { Id = 2, RoleNameFa = "کاربر عادی" , RoleNameEn = "Member" }
                      );
                }
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
		public System.Data.Entity.DbSet<WebApplication1.Models.UserSavedConversionInfo> UserSavedConversionInfo { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.PaySetting> PaySettings { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.BasketPay> BasketPays { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.Customer> Customers { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.CheckPaymentConditaion>  checkPaymentConditaions { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.PaymentCodes> PaymentCodes { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.bank> Banks { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.Check> Checks { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.Promissory> Promissory { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.CheckPaymentRequestAttemp> CheckPaymentRequestAttemps { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.CheckPaymentRequestAttempPictures> CheckPaymentRequestAttempPictures { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.CreditPaymentRequestAttemp> CreditPaymentRequestAttemps { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.CreditPaymentRequestAttempPictures> CreditPaymentRequestAttempPictures { get; set; }
    }
}
