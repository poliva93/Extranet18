namespace Extranet_EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Newtonsoft.Json;

    public partial class DesadvDB : DbContext
    {
        public DesadvDB()
            : base("name=ExtranetDB")
        {
            
        }
        public virtual DbSet<LogTable> LogTable { get; set; }
        public virtual DbSet<DESADV_ANA_IMBALLI> DESADV_ANA_IMBALLI { get; set; }
        public virtual DbSet<DESADV_ANAGRAFICHE> DESADV_ANAGRAFICHE { get; set; }
        public virtual DbSet<DESADV_ETICHETTE> DESADV_ETICHETTE { get; set; }
        public virtual DbSet<DESADV_IMBALLI> DESADV_IMBALLI { get; set; }
        public virtual DbSet<DESADV_RIGHE> DESADV_RIGHE { get; set; }
        public virtual DbSet<DESADV_TESTATA> DESADV_TESTATA { get; set; }
        public virtual DbSet<DESADV_TRASPORTATORE> DESADV_TRASPORTATORE { get; set; }
        
           
            

        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogTable>();

            modelBuilder.Entity<DESADV_ANA_IMBALLI>()
                .Property(e => e.LUGDESCARGA)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_ANA_IMBALLI>()
                .Property(e => e.ARTCOD)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_ANA_IMBALLI>()
                .Property(e => e.BOXCOD)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_ANAGRAFICHE>()
                .Property(e => e.CLIENTE)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_ANAGRAFICHE>()
                .Property(e => e.TIPO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_ANAGRAFICHE>()
                .Property(e => e.VENDEDOR)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_ANAGRAFICHE>()
                .Property(e => e.CONSIGNATARIO)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_ANAGRAFICHE>()
                .Property(e => e.NOMBRE_CN)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_ANAGRAFICHE>()
                .Property(e => e.LUGDESCARGA)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_ANAGRAFICHE>()
                .Property(e => e.SHIPFROM)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_ANAGRAFICHE>()
                .Property(e => e.FINALDESCARGA)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_ANAGRAFICHE>()
                .HasMany(e => e.DESADV_TESTATA)
                .WithRequired(e => e.DESADV_ANAGRAFICHE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DESADV_ETICHETTE>()
                .Property(e => e.CLIENTE)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_ETICHETTE>()
                .Property(e => e.IDNUMDES)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_ETICHETTE>()
                .Property(e => e.IDEMB)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_ETICHETTE>()
                .Property(e => e.IDETQ)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_ETICHETTE>()
                .Property(e => e.IDETQPACK)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_ETICHETTE>()
                .Property(e => e.NUMETIQUETA);
            //.IsUnicode(false);

            modelBuilder.Entity<DESADV_ETICHETTE>()
                .Property(e => e.NUMETIQUETA2);

            modelBuilder.Entity<DESADV_ETICHETTE>()
                .Property(e => e.NUMPACCOMP)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_ETICHETTE>()
                .Property(e => e.NUMLOTE)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_ETICHETTE>()
                .Property(e => e.HANNUM)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_ETICHETTE>()
                .Property(e => e.AGENCY)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_ETICHETTE>()
                .Property(e => e.HANTYPE)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_IMBALLI>()
                .Property(e => e.CLIENTE)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_IMBALLI>()
                .Property(e => e.IDNUMDES)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_IMBALLI>()
                .Property(e => e.IDEMB)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_IMBALLI>()
                .Property(e => e.CPS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_IMBALLI>()
                .Property(e => e.MEDIOEMB)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_IMBALLI>()
                .Property(e => e.CALMEDIOEMB)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_IMBALLI>()
                .Property(e => e.UMLONGITUD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_IMBALLI>()
                .Property(e => e.UMANCHURA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_IMBALLI>()
                .Property(e => e.UMALTURA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_IMBALLI>()
                .Property(e => e.UNIDMEDCPAC)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_IMBALLI>()
                .Property(e => e.INSTMARCAJE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_IMBALLI>()
                .Property(e => e.IDENTETIQUETA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_IMBALLI>()
                .Property(e => e.CALIDENTETIQUETA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_IMBALLI>()
                .Property(e => e.ETQMAESTRA)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_IMBALLI>()
                .Property(e => e.FECHAFABRIC)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_IMBALLI>()
                .Property(e => e.FECHACADUC)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_IMBALLI>()
                .Property(e => e.NUMETIQUETA)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_IMBALLI>()
                .Property(e => e.NUMPACCOMP)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_IMBALLI>()
                .Property(e => e.NUMLOTE)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_IMBALLI>()
                .Property(e => e.NUMPACPRO)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_IMBALLI>()
                .HasMany(e => e.DESADV_ETICHETTE)
                .WithRequired(e => e.DESADV_IMBALLI)
                .HasForeignKey(e => new { e.CLIENTE, e.IDNUMDES, e.IDEMB, e.CPS })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DESADV_IMBALLI>()
                .HasMany(e => e.DESADV_RIGHE)
                .WithRequired(e => e.DESADV_IMBALLI)
                .HasForeignKey(e => new { e.CLIENTE, e.IDNUMDES, e.IDEMB, e.CPS })
                .WillCascadeOnDelete(false);



            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.CLIENTE)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.IDNUMDES)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.IDEMB)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.NUMLIN)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.REFCOMPRADOR)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.REFPROVEEDOR)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.PESONETO)
                .HasPrecision(18, 3);

            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.UMPESONETO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.UMCANTENT)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.PAISORIG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.TIPOREGIMEN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.TEXTOLIBRE)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.VALORADUANA)
                .HasPrecision(18, 3);

            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.DIVISAVARADU)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.NUMPEDIDO)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.NUMPROGRAMA)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.NUMLINPEDIDO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.INFOADICIONAL)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.NUMSERIE)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.NUMENGINE)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.NUMVEHICLE)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.RFF_AAP)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.RFF_AAE)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.DTM_AAE)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.RFF_IV)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.RFF_IV2)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.FINALDESCARGA)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.APPLICACION)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_RIGHE>()
                .Property(e => e.INVENTARIO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.CLIENTE)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.NUMDES)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.TIPO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.CODTIPO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.FECDES)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.FECENT)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.FECSAL)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.TOTPESBRU)
                .HasPrecision(18, 3);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.UMTOTPESBRU)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.TOTPESNE)
                .HasPrecision(18, 3);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.UMTOTPESNE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.NUMENVCARG)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.VENDEDOR)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.CALVENDEDOR)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.DIRECCION_SE)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.POBLACION_SE)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.NOMBRE_SE)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.CODINTVEND)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.CONSIGNATARIO)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.CALCONSIGNA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.DIRECCION_CN)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.POBLACION_CN)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.NOMBRE_CN)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.LUGDESCARGA)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.EXPEDCARGA)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.CALITERLOCUTOR)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.CALEXPEDIDOR)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.NOMBRE_FW)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.CODINTEXPE)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.ENTREGACODIG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.ENTREGATRANS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.CALPAGO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.TIPTRANS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.CODDESCTRAS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.IDTRANSPORT)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.CODTIPEQUIP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.CODEQUIP)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.RFF_AAJ)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.RFF_AAS)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.RFF_CRN)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.SHIPFROM)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.NUMTRANOLD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.NUMTRANNEW)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.LUGENTREGA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.CONTRATO)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .Property(e => e.STATO)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TESTATA>()
                .HasMany(e => e.DESADV_IMBALLI)
                .WithRequired(e => e.DESADV_TESTATA)
                .HasForeignKey(e => new { e.CLIENTE, e.IDNUMDES })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DESADV_TRASPORTATORE>()
                .Property(e => e.Descrizione)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TRASPORTATORE>()
                .Property(e => e.CLIENTE)
                .IsUnicode(false);

            modelBuilder.Entity<DESADV_TRASPORTATORE>()
                .Property(e => e.Codice)
                .IsUnicode(false);
        }
    }
}
