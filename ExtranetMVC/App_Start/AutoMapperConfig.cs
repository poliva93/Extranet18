using AutoMapper;
using Extranet_EF;
using ExtranetMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtranetMVC
{
    public class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                /*EDI Piano Fornitore*/
                cfg.CreateMap<EDI_TESTATA, EdiTestata>()
                    .ForMember(t => t.Righe, opt => opt.MapFrom(s => s.EDI_RIGHE));
                cfg.CreateMap<EDI_RIGHE, EdiRighe>();
                cfg.CreateMap<Users, User>();
                cfg.CreateMap<Roles, Role>();
                /*fine sezione EDI*/
                cfg.CreateMap<DESADV_ANAGRAFICHE, DesadvAnagrafica>()
                    .ForMember(t => t.Testata, opt => opt.MapFrom(s => s.DESADV_TESTATA));
                cfg.CreateMap<DESADV_ANA_IMBALLI, DesadvAnagraficaImballi>();
                cfg.CreateMap<DESADV_TESTATA, DesadvTestata>()
                    .ForMember(t => t.Anagrafica, opt => opt.MapFrom(s => s.DESADV_ANAGRAFICHE))
                    .ForMember(t => t.Imballi, opt => opt.MapFrom(s => s.DESADV_IMBALLI));
                cfg.CreateMap<DESADV_TRASPORTATORE, DesadvTrasportatore>();
                cfg.CreateMap<DESADV_ETICHETTE, DesadvEtichette>()
                    .ForMember(t => t.Imballi, opt => opt.MapFrom(s => s.DESADV_IMBALLI));
                cfg.CreateMap<DESADV_IMBALLI, DesadvImballi>()
                    .ForMember(t => t.Testata, opt => opt.MapFrom(s => s.DESADV_TESTATA))
                    .ForMember(t => t.Righe, opt => opt.MapFrom(s => s.DESADV_RIGHE))
                    .ForMember(t => t.Etichette, opt => opt.MapFrom(s => s.DESADV_ETICHETTE));
                cfg.CreateMap<DESADV_RIGHE, DesadvRighe>()
                    .ForMember(t => t.Imballi, opt => opt.MapFrom(s => s.DESADV_IMBALLI));
                /*fine sezione DESADV*/

            });
        }
    }
}