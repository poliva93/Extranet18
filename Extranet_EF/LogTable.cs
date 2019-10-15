
namespace Extranet_EF

{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("LogTable")]
    public class LogTable
    {
        [Key]
       public int ID { get; set; }
        [StringLength(50)]
      public  string Orario { get; set; }

        [StringLength(20)]
      public  string Programma { get; set; }

        [StringLength(35)]
      public  string Utente { get; set; }
        
      public  string Avviso { get; set; }

      public  string Informazioni { get; set; }

    
    }
}
