namespace Extranet_EF
{
    using System.Data.Entity;
    using System.Linq;
    public partial class ExtranetDB : DbContext
    {
        public ExtranetDB()
            : base("name=ExtranetDB")
        {
        }
        public DbSet<TicketWB> TicketWB { get; set; }
        public virtual DbSet<LogTable> LogTable { get; set; }
        public virtual DbSet<EDI_RIGHE> EDI_RIGHE { get; set; }
        public virtual DbSet<EDI_TESTATA> EDI_TESTATA { get; set; }
        public virtual DbSet<FORNITORE> FORNITORE { get; set; }
        public virtual DbSet<UserShares> UserShares { get; set; }
        public virtual DbSet<FTPabilitazioni> FTPabilitazioni { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Shares> Shares { get; set; }
        //public virtual DbSet<LogTable> LogTable { get; set; }

        //public virtual DbSet<Roles> Roles { get; set; }
        //public virtual DbSet<Users> Users { get; set; }
        //public virtual DbSet<ShareRoles> ShareRoles { get; set; }
        //public virtual DbSet<Shares> Shares { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogTable>();

            modelBuilder.Entity<EDI_RIGHE>()
                .Property(e => e.NUMORDINE)
                .IsUnicode(false); 

            modelBuilder.Entity<EDI_RIGHE>()
                .Property(e => e.ARTCOD)
                .IsUnicode(false);

            modelBuilder.Entity<EDI_RIGHE>()
                .Property(e => e.ARTVER)
                .IsUnicode(false);

            modelBuilder.Entity<EDI_RIGHE>()
                .Property(e => e.ARTDES)
                .IsUnicode(false);

            modelBuilder.Entity<EDI_RIGHE>()
                .Property(e => e.ARTQTA)
                .HasPrecision(10, 3);

            modelBuilder.Entity<EDI_RIGHE>()
                .Property(e => e.ARTUM)
                .IsUnicode(false);

            modelBuilder.Entity<EDI_RIGHE>()
                .Property(e => e.LAST_DDT)
                .IsUnicode(false);

            modelBuilder.Entity<EDI_RIGHE>()
                .Property(e => e.LAST_DDT_F)
                .IsUnicode(false);

            modelBuilder.Entity<EDI_RIGHE>()
                .Property(e => e.LAST_QTA)
                .HasPrecision(10, 3);

            modelBuilder.Entity<EDI_RIGHE>()
                .Property(e => e.PROG_ARTICOLO)
                .HasPrecision(10, 3);

            modelBuilder.Entity<EDI_TESTATA>()
                .Property(e => e.NUMORDINE)
                .IsUnicode(false);

            modelBuilder.Entity<EDI_TESTATA>()
                .Property(e => e.CLFCOD)
                .IsUnicode(false);

            modelBuilder.Entity<EDI_TESTATA>()
                .Property(e => e.CLFDES)
                .IsUnicode(false);

            modelBuilder.Entity<EDI_TESTATA>()
                .Property(e => e.CLFIND)
                .IsUnicode(false);

            modelBuilder.Entity<EDI_TESTATA>()
                .Property(e => e.MAGAZZINO)
                .IsUnicode(false);

            modelBuilder.Entity<EDI_TESTATA>()
                .Property(e => e.CONTATTOLOG)
                .IsUnicode(false);

            modelBuilder.Entity<EDI_TESTATA>()
                .Property(e => e.CONTATTOFOR)
                .IsUnicode(false);

            modelBuilder.Entity<EDI_TESTATA>()
                .HasMany(e => e.EDI_RIGHE)
                .WithRequired(e => e.EDI_TESTATA)
                .HasForeignKey(e => new { e.ID_TESTATA, e.NUMORDINE })
                .WillCascadeOnDelete(false);
            
            modelBuilder.Entity<FORNITORE>()
                .Property(e => e.CLFCOD)
                .IsUnicode(false);

            modelBuilder.Entity<FORNITORE>()
                .HasMany(e => e.EDI_TESTATA)
                .WithRequired(e => e.FORNITORE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FORNITORE>()
                .HasMany(e => e.Users)
                .WithOptional(e => e.FORNITORE)
                .HasForeignKey(e => e.CodiceFornitore);

            //MODIFICHE DEL 20181212
            modelBuilder.Entity<FTPabilitazioni>()
               .HasMany(e => e.UserShares)
               .WithRequired(e => e.FTPabilitazioni)
               .HasForeignKey(e => e.username)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Shares>()
               .HasMany(e => e.UserShares)
               .WithRequired(e => e.Shares)
               .HasForeignKey(e => e.shareID)
               .WillCascadeOnDelete(false);
            //

            modelBuilder.Entity<Roles>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Roles)
                .Map(m => m.ToTable("UserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<Users>()
                .Property(e => e.CodiceFornitore)
                .IsUnicode(false);


            //modelBuilder.Entity<Shares>()
            //   .HasMany(e => e.ShareRoles)
            //   .WithRequired(e => e.Shares)
            //   .HasForeignKey(e => e.RoleId);


            //test perchè non funziona sharesRole, originale subito sotto
            //modelBuilder.Entity<Roles>()
            //.HasMany(s => s.Shares)
            //.WithMany(r => r.Roles)
            //.Map(m => m.ToTable("ShareRoles").MapLeftKey("ShareId").MapRightKey("RoleId"));

            modelBuilder.Entity<Roles>()
            .HasMany(s => s.Shares)
            .WithMany(r => r.Roles)
            .Map(m =>
            {
                m.ToTable("ShareRoles");
                m.MapLeftKey("ShareId");
                m.MapRightKey("RoleId");
            });


            //modelBuilder.Entity<UserShares>()
            //    .HasRequired(e => e.Shares)
            //    .WithMany(e => e.UserShares)
            //    .HasForeignKey(e => e.shareID);

            //
            modelBuilder.Entity<Shares>()
            .HasMany(r => r.Roles)
            .WithMany(s => s.Shares)
            .Map(m =>
            {
                m.ToTable("ShareRoles");
                m.MapLeftKey("RoleId");
                m.MapRightKey("ShareId");
            }
            );
            





        }
        static void Main(string[] args)
        {
            using (ExtranetDB dbContext = new ExtranetDB())
            {
                var user = dbContext.Users
                    .Include(ro => ro.Roles)
                    .FirstOrDefault();
                foreach (var ruolo in user.Roles)
                {
                    var condivisione = dbContext.Roles
                         .Include(sh => sh.Shares)
                         .Where(sh => sh.RoleId == ruolo.RoleId)
                         .FirstOrDefault();
                }
                
            }
        }
        
       
    }
}
    