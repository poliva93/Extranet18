namespace Extranet_EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DESADV_TRASPORTATORE
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(35)]
        public string Descrizione { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string CLIENTE { get; set; }

        [StringLength(35)]
        public string Codice { get; set; }
    }
}
