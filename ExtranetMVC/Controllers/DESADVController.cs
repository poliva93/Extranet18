using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Extranet_EF;
using ExtranetMVC.Models;
using AutoMapper;
using System.Web.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using ExtranetMVC.CustomAuthentication;
using System.IO;
using System.Web.UI;

namespace ExtranetMVC.Controllers
{
    [CustomAuthorize(Roles = "Admin,Gruppo_EDP,Deufol")]
    public class DESADVController : Controller
    {
        private DesadvDB db = new DesadvDB();

        // GET: DESADV
        public ActionResult Index()
        {
            var dESADV_TESTATA = db.DESADV_TESTATA.Include(d => d.DESADV_ANAGRAFICHE);
            return View(Mapper.Map<List<DesadvTestata>>(dESADV_TESTATA.ToList()));
        }
        [System.Web.Mvc.HttpGet]
        public ActionResult Create()
        {
            // SelectList clienti = new SelectList(db.DESADV_ANAGRAFICHE, "CLIENTE", "CLIENTE");

            // ViewBag.CLIENTE = new SelectList(Mapper.Map < List <DesadvAnagrafica>> (db.DESADV_ANAGRAFICHE), "CLIENTE", "CLIENTE");
            return View();
        }

        public void Loggamelo(string programma, string avviso, string informazioni = "")
        {

            LogTable log = new LogTable();

            log.Utente = User.Identity.Name;
            log.Orario = DateTime.Now.ToString();
            log.Programma = programma;
            log.Avviso = avviso;
            log.Informazioni = informazioni.ToString();
            db.LogTable.Add(log);
            db.SaveChanges();

        }

        [System.Web.Http.HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(string cliente, string bolla, string bollaEffettiva, string trasportatore, string partenza, string Imballo)  //([Bind(Include = "CLIENTE,NUMDES,TIPO,CODTIPO,FECDES,FECENT,FECSAL,TOTPESBRU,UMTOTPESBRU,TOTPESNE,UMTOTPESNE,NUMENVCARG,VENDEDOR,CALVENDEDOR,DIRECCION_SE,POBLACION_SE,NOMBRE_SE,CODINTVEND,CONSIGNATARIO,CALCONSIGNA,DIRECCION_CN,POBLACION_CN,NOMBRE_CN,LUGDESCARGA,EXPEDCARGA,CALITERLOCUTOR,CALEXPEDIDOR,NOMBRE_FW,CODINTEXPE,ENTREGACODIG,ENTREGATRANS,CALPAGO,TIPTRANS,CODDESCTRAS,IDTRANSPORT,CODTIPEQUIP,CODEQUIP,RFF_AAJ,RFF_AAS,RFF_CRN,SHIPFROM,NUMTRANOLD,NUMTRANNEW,LUGENTREGA,CONTRATO")] DESADV_TESTATA dESADV_TESTATA)
        {



            //var isNumeric = int.TryParse(cliente, out int n);
            DESADV_ANAGRAFICHE testCliente = db.DESADV_ANAGRAFICHE.Find(cliente);

            if (testCliente == null)
            {
                var risposta = new { status = "400", data = "Cliente non trovato" };
                return Json(risposta);
                // return Content(HttpStatusCode.NoContent, "Cliente non trovato");
            }

            string EDI;
            EDI = testCliente.EDI.ToString();

            string sESECOD = bolla.Substring(0, 4);
            string sBAMSEZ = bolla.Substring(4, 2);
            string sBAMNRR = bolla.Substring(6);
            string sNumero = "";
            string sFinalD = "";
            string strConnAlnus = "";
            int iASN;
            ConnectionStringSettings cnnString = ConfigurationManager.ConnectionStrings["ExtranetDB"];
            string strConn = cnnString.ConnectionString;
            //vado a prendere i dati di anagrafica e l'ultimo numero di spedizione
            var testata = new DESADV_TESTATA();
            using (SqlConnection sqlConnection = new SqlConnection(strConn))
            {
                
                sqlConnection.Open();
                string StrSql = "select *, isnull((select MAX(ID_ASN)+1 from DESADV_TESTATA where Cliente='" + cliente + "')  ,0 ) maxASN, isnull((select MAX(cast(NUMDES as integer)) FROM DESADV_TESTATA d where  d.CLIENTE=a.CLIENTE),0) numero from DESADV_ANAGRAFICHE a where a.CLIENTE='" + cliente + "'";
                SqlCommand sqlCmd = new SqlCommand(StrSql, sqlConnection);
                SqlDataReader reader = sqlCmd.ExecuteReader();
                reader.Read();
                iASN = Int32.Parse(reader["maxASN"].ToString());
                testata.ID_ASN = iASN;
                sFinalD = reader["FINALDESCARGA"].ToString();
                testata.CLIENTE = cliente;
                sNumero = (Int32.Parse(reader["numero"].ToString()) + 1).ToString();
                testata.NUMDES = sNumero;
                testata.TIPO = reader["TIPO"].ToString();
                testata.CODTIPO = "";
                testata.FECDES = DateTime.Now.ToString("yyyyMMddHHmm");
                //DateTime parsed = DateTime.ParseExact(DateTime.Now.ToString(), "yyyyMMddHHmm",
                //                      CultureInfo.InvariantCulture);
                testata.FECENT = DateTime.Now.ToString("yyyyMMddHHmm"); //viene aggiornato all'inserimento delle righe
                testata.FECSAL = DateTime.Now.ToString("yyyyMMddHHmm");
                string sPeso = "", sPesoNetto = "";
                ConnectionStringSettings cnnString2 = ConfigurationManager.ConnectionStrings["AlnusConnection"];
                strConnAlnus = cnnString2.ConnectionString;
                using (SqlConnection sqlConnectionAlnus = new SqlConnection(strConnAlnus))
                {
                    string strSql2 = "select distinct BAMPLR  from BAMTES00 where BAMTES00.ESECOD=" + sESECOD + " and BAMTES00.BAMSEZ= '" + sBAMSEZ + "' and BAMTES00.BAMNRR=" + sBAMNRR;

                    SqlCommand sqlCmdAlnus = new SqlCommand(strSql2, sqlConnectionAlnus);
                    sqlConnectionAlnus.Open();
                    sPeso = sqlCmdAlnus.ExecuteScalar().ToString();
                    //sPeso = sPeso.Replace(",", ".");
                    sqlConnectionAlnus.Close();

                    strSql2 = "select distinct BAMPNE  from BAMTES00 where BAMTES00.ESECOD=" + sESECOD + " and BAMTES00.BAMSEZ= '" + sBAMSEZ + "' and BAMTES00.BAMNRR=" + sBAMNRR;

                    sqlCmdAlnus = new SqlCommand(strSql2, sqlConnectionAlnus);
                    sqlConnectionAlnus.Open();
                    sPesoNetto = sqlCmdAlnus.ExecuteScalar().ToString();
                    //sPeso = sPeso.Replace(",", ".");
                    sqlConnectionAlnus.Close();

                }
                //porsche pretende che i pesi non siano uguali
                if (sPeso == sPesoNetto)
                {
                    testata.TOTPESBRU = (decimal.Parse(sPeso) + 1000) / 1000;
                }
                else
                {
                    testata.TOTPESBRU = (decimal.Parse(sPeso)) / 1000;
                }
                testata.UMTOTPESBRU = "KGM";
                testata.TOTPESNE = decimal.Parse(sPesoNetto) / 1000;
                testata.UMTOTPESNE = "KGM";
                testata.NUMENVCARG = "";
                testata.VENDEDOR = reader["VENDEDOR"].ToString();
                testata.CALVENDEDOR = "";
                testata.DIRECCION_SE = "";
                testata.POBLACION_SE = "";
                testata.NOMBRE_SE = "";
                testata.CODINTVEND = "";
                testata.CONSIGNATARIO = reader["CONSIGNATARIO"].ToString();
                testata.CALCONSIGNA = "";
                testata.DIRECCION_CN = "";
                testata.POBLACION_CN = "";
                testata.NOMBRE_CN = "";
                testata.LUGDESCARGA = reader["LUGDESCARGA"].ToString();
                testata.EXPEDCARGA = trasportatore;
                testata.CALITERLOCUTOR = "";
                testata.CALEXPEDIDOR = "";
                testata.NOMBRE_FW = trasportatore;
                testata.CODINTEXPE = db.DESADV_TRASPORTATORE.Where(a => a.CLIENTE == cliente && a.Descrizione == trasportatore).Select(a => a.Codice).FirstOrDefault();
                testata.ENTREGACODIG = "01"; // DA GESTIRE
                testata.ENTREGATRANS = "01"; //valori possibili da 01 a 05
                testata.CALPAGO = "";
                testata.TIPTRANS = "6"; //1 targa //2dich di beni //6 n generale del carico //7consegna express //8vagone//9pacch postale//10 n volo aereo//11 nome imbarcazione
                testata.CODDESCTRAS = "";
                string[] numbers = Regex.Split(bollaEffettiva, @"\D+");  //è per porsche, se altri clienti vogliono tutto il numero della bolla è da gestire
                string bollaNumero = numbers[0].ToString() + numbers[1].ToString();
                testata.IDTRANSPORT = bollaNumero;
                testata.CODTIPEQUIP = "TE";
                testata.CODEQUIP = bollaEffettiva;
                testata.RFF_AAJ = "";
                testata.RFF_AAS = bollaEffettiva.Substring(bollaEffettiva.Length- 8);
                testata.RFF_CRN = testata.NUMDES;
                testata.SHIPFROM = reader["VENDEDOR"].ToString(); //reader["SHIPFROM"].ToString(); //Marcin ha detto di mettere il supplier code
                string strNumeroOld = (Int32.Parse(testata.NUMDES) - 1).ToString();
                //if (strNumeroOld == "0") strNumeroOld = "";
                if (strNumeroOld == "0")
                {
                    strNumeroOld = "00001";
                }
                testata.NUMTRANOLD = strNumeroOld;
                testata.NUMTRANNEW = testata.NUMDES.PadLeft(5, '0');

                testata.LUGENTREGA = reader["FINALDESCARGA"].ToString();
                testata.CONTRATO = "";
                testata.STATO = "APERTO";



                //if (ModelState.IsValid)
                //{
                //    db.DESADV_TESTATA.Add(dESADV_TESTATA);
                //    db.SaveChanges();
                //    return RedirectToAction("Index");
                //}

                //ViewBag.CLIENTE = new SelectList(db.DESADV_ANAGRAFICHE, "CLIENTE", "TIPO", dESADV_TESTATA.CLIENTE);
                //return View(dESADV_TESTATA);


            }

            db.DESADV_TESTATA.Add(testata);
            db.SaveChanges();

            Loggamelo("NewDESADV", "Creata la testata");

            //************************************************************************************************************
            //CARICO I DATI DELLE RIGHE DELLA BOLLA
            //************************************************************************************************************
            //ConnectionStringSettings cnnString2 = ConfigurationManager.ConnectionStrings["AlnusConnection"];
            //string strConnAlnus = cnnString2.ConnectionString;

            using (SqlConnection sqlConnectionAlnus = new SqlConnection(strConnAlnus))
            {
                //string strSql2 = "select t.*,d.*,a.ARTDES, a.ARTCO1, o.ORCRIF, o.ORCTSSREV  from BAMTES00 t inner join BAMDET00 d on t.ESECOD=d.ESECOD and t.BAMSEZ=d.BAMSEZ and t.BAMNRR=d.BAMNRR  inner join ARTANA a on d.ARTCOD=a.ARTCO1 inner join ORCTES00 o on d.BAMESE=o.ESECOD and d.BAMSOR= o.ORCTSZ and d.BADNRO=o.ORCTNR " +
                //    " where t.ESECOD=" + bolla.Substring(0, 4).ToString() + " and t.BAMSEZ='" + bolla.Substring(4, 2).ToString() + "' AND t.BAMNRR=" + bolla.Substring(6).ToString();
                //MODIFICATO CAUSA SPEDIZIONE DI STESSO ARTICOLO CON 2 RIGHE DIVERSE, 



                string strSql2 = "select max(BADNRD) BADNRD,d.ARTCOD, ORCRIF,MAX(ORDDCOREV) dataConsegna, sum(BAMEOR) BAMEOR, BARCFO,  ARTDES from BAMTES00 t inner join BAMDET00 d on t.ESECOD=d.ESECOD and t.BAMSEZ=d.BAMSEZ and t.BAMNRR=d.BAMNRR  inner join ARTANA a on d.ARTCOD = a.ARTCO1  inner join ORCDET00 od on od.ESECOD = d.BAMESE and od.ORDSEZ = d.BAMSOR and od.ORDNRR = d.BADNRO and d.BAMROR = od.ORDRIG inner join ORCTES00 o on od.ESECOD = o.ESECOD and od.ORDSEZ = o.ORCTSZ and od.ORDNRR = o.ORCTNR   where   t.ESECOD=" + bolla.Substring(0, 4).ToString() + " and t.BAMSEZ='" + bolla.Substring(4, 2).ToString() + "' AND t.BAMNRR=" + bolla.Substring(6).ToString() + " GROUP BY d.ARTCOD, ORCRIF,ORCTSSREV, BARCFO, ARTDES";

                SqlCommand sqlCmdAlnus = new SqlCommand(strSql2, sqlConnectionAlnus);
                sqlConnectionAlnus.Open();
                Loggamelo("NewDESADV", "Apro connessione Alnus");
                SqlDataReader readerAlnus = sqlCmdAlnus.ExecuteReader();
                Loggamelo("NewDESADV", "Letti i dati di Alnus");

                //sPeso = sPeso.Replace(",", ".");
                //

                int iCPS = 0;
                int iRiga = 0;
                Boolean btest = true;
                int iNumeroScatole = 0;
                string sCodImb = "";
                int iPezziImballaggio = 0;
                string sCodPallet = "";
                string sCodPalletDO = "";
                string sCodImbDO = "";
                string dataPrevista = "";
                int iMlabel, iSlabel;
                 iMlabel = 0;
                var vMax = 0;
                int? iEtichetta; //implementare progressivo come da richiesta di corinna 20190509 TDL
                                 //if (cliente == "310310") //ad oggi solo porsche vuole etichette sempre diverse
                                 //{
                                 //Int32.TryParse(db.DESADV_ETICHETTE.Where(s => s.CLIENTE == cliente).Max(s => s.NUMETIQUETA2), out iEtichetta);
                //VDA M Label
                if (EDI == "VDA")
                {
                   iEtichetta = db.DESADV_ETICHETTE.Where(s => s.CLIENTE == cliente).Max(s => s.NUMETIQUETA2);
                   
                    if (iEtichetta > 900000000)
                    {
                        iMlabel = iEtichetta.Value;
                    }
                    else
                    {
                        iMlabel = 900000000;
                    }
                }
                //EDIFACT M label
                if (EDI == "EDIFACT")
                {
                    Int32.TryParse(db.DESADV_IMBALLI.Where(s => s.CLIENTE == cliente).Max(s => s.ETQMAESTRA), out iMlabel);
                    if (iMlabel < 900000000)
                    {
                        iMlabel = 900000000;
                    }
                }
                //Int32.TryParse(db.DESADV_ETICHETTE.Where(s => s.CLIENTE == cliente && !s.NUMETIQUETA2.StartsWith("9") ).Max(s => s.NUMETIQUETA2), out iSlabel);
                //S Label
                iEtichetta = db.DESADV_ETICHETTE.Where(s => s.CLIENTE == cliente && s.NUMETIQUETA2<900000000).Max(s=> (int?)s.NUMETIQUETA2 ) ?? 0;
                //iSlabel = db.DESADV_ETICHETTE.Where(s => s.CLIENTE == cliente && s._INumetiqueta2<900000000 ).Max(s => s._INumetiqueta2);

                if (iEtichetta != null)
                {
                    iSlabel = iEtichetta.Value;
                }
                else
                {
                    iSlabel = 0;
                }


                //}
                int iMaxEtichetta = 0;
                Loggamelo("NewDESADV", "Pronto a utilizzare dati Alnus");
                do
                {
                    if (readerAlnus.Read().ToString() == "True")
                    {
                        Loggamelo("NewDESADV", "Risultato positivo: inizio");
                        string sArticolo;
                        sArticolo = readerAlnus["ARTCOD"].ToString().Trim();
                        //sistemo la testata col LUDESCARGA che cambia per articolo..... sarà da rivedere in caso di bolle con più articoli
                        //---------------------------------------------------------------------------------------------------------
                        DESADV_TESTATA testataUPD = db.DESADV_TESTATA.Find(cliente, sNumero);
                        testataUPD.LUGDESCARGA = db.DESADV_ANA_IMBALLI.Where(e => e.CLIENTE == cliente && e.ARTCOD == sArticolo && e.TIPOLOGIA == "PALLET").Select(e => e.LUGDESCARGA).FirstOrDefault();
                        testataUPD.CONTRATO = readerAlnus["ORCRIF"].ToString().TrimEnd();

                        if (testataUPD.CONTRATO.Length > 12)
                        {
                            testataUPD.CONTRATO = testataUPD.CONTRATO.Substring(0, 12);
                        }
                        DateTime dataRichiesta = DateTime.ParseExact(readerAlnus["dataConsegna"].ToString() + "1200", "yyyyMMddHHmm",
                                              CultureInfo.InvariantCulture);

                        testataUPD.FECENT = dataRichiesta.ToString("yyyyMMddHHmm");
                        db.Entry(testataUPD).State = EntityState.Modified;
                        db.SaveChanges();
                        Loggamelo("NewDESADV", "Provo ad aggiornare la testata con la data di richiesta di arrivo");
                        Loggamelo("NewDESADV", "Testata aggiornata");
                        //--------------------------------------------------------------------------------------------------------------
                        //DESADV_ANA_IMBALLI anaImballi = db.DESADV_ANA_IMBALLI.Find(readerAlnus["ARTCOD"].ToString().Trim());
                        // DESADV_ANA_IMBALLI[]   .array()

                        //----------------------------------------------------------------------------------------------------------------
                        //Gestisco tutte le righe degli imballi
                        //------------------------------------------------------------------------------------------------------------------
                        Loggamelo("NewDESADV", "Inizio calcolo pezzi per bancale");
                        int pezziBancale = Int32.Parse(db.DESADV_ANA_IMBALLI.Where(e => e.ARTCOD == sArticolo && e.TIPOLOGIA == "PALLET" && e.imbStandard == Imballo && e.CLIENTE == cliente).Select(e => e.PARTXBOX).FirstOrDefault().ToString());
                        Loggamelo("NewDESADV", "Pezzi da inviare. Valore originale" + readerAlnus["BAMEOR"].ToString());
                        int pezziInviare;
                        pezziInviare = 0;
                        if (readerAlnus["BAMEOR"].ToString().Contains('.'))
                        {
                            pezziInviare = Int32.Parse(readerAlnus["BAMEOR"].ToString().Replace(".000", ""));
                        }
                        if (readerAlnus["BAMEOR"].ToString().Contains(','))
                        {
                            pezziInviare = Int32.Parse(readerAlnus["BAMEOR"].ToString().Replace(",000", ""));
                        }
                        Loggamelo("NewDESADV", "Pezzi da inviare:" + pezziInviare.ToString());
                        //numeroImballi indica quanti pallet sono.
                        //per ogni pallet vanno create le righe di tutti gli imballaggi (pallet, coperchio, box) con cps ad aumentare e IDEMB fisso per bancale
                        Loggamelo("NewDESADV", "Inizio calcolo numero imballi");
                        int numeroImballi = (int)Math.Ceiling((double)pezziInviare / pezziBancale);
                        Loggamelo("NewDESADV", "Numero: " + numeroImballi.ToString());
                        int iScatolePiene = 0;
                        int iScatoleTotali = 0;

                        for (int i = 1; i <= numeroImballi; i++) //ciclo x qta di imballi fisica. ES: 40 pezzi e pallet da 20 -> 2
                        {
                            iRiga += 1;
                            //----------------------------------------------------------
                            //seleziono le anagrafiche imballi necessarie per il bancale
                            //----------------------------------------------------------
                            Loggamelo("NewDESADV", "Inizio FOR da 1 fino a numero imballi");
                            DESADV_ANA_IMBALLI[] anaImballi = db.DESADV_ANA_IMBALLI.Where(e => e.ARTCOD == sArticolo && e.imbStandard == Imballo && e.CLIENTE == cliente).OrderBy(x => x.Sort).ToArray(); //v1.8 inserito l'ordinamento

                            //--------------------------------------------------------------------
                            //per ogni tipo di imballo inserisco la riga
                            //--------------------------------------------------------------------

                            if (EDI == "VDA")
                            {
                                for (int imb = 0; imb < anaImballi.Length; imb++) //creo una riga in  EMBALB per ogni imballo associato al codice. 
                                {
                                    string ImbType = anaImballi[imb].TIPOLOGIA;
                                    Loggamelo("NewDESADV", "FOR nuovo Imballo", "VDA");

                                    iCPS += 1;
                                    //if (ImbType == "PALLET" || ImbType == "BOX") { iEtichetta += 1; } 20191007
                                    if (ImbType == "PALLET")
                                    {
                                        iMlabel += 1;
                                    }
                                    if (ImbType == "BOX")
                                    {
                                        iSlabel += 1;
                                    }
                                    //   iEtichetta += 1;
                                    var rigaImb = new DESADV_IMBALLI();
                                    rigaImb.CLIENTE = cliente;
                                    rigaImb.IDNUMDES = sNumero;
                                    rigaImb.IDEMB = iCPS.ToString(); //i.ToString(); //v1.7 Donatiello ha detto di mettere IDEMB=CPS
                                    rigaImb.CPS = iCPS.ToString();

                                    //qua va gestito in caso di bancale non pieno per le scatole...x ford le scatole sono fisse
                                    sCodImb = anaImballi[imb].BOXCOD;
                                    //-------------------------------------------------------------------------------------------

                                    if (ImbType == "PALLET")
                                    {
                                        iPezziImballaggio = anaImballi[imb].PARTXBOX ?? default(int);
                                        iScatoleTotali = anaImballi[imb].BOXNUM ?? default(int);
                                        if (i < (numeroImballi)) // i < (numeroImballi - 1) 
                                        {
                                            iPezziImballaggio = anaImballi[imb].PARTXBOX ?? default(int);
                                            iNumeroScatole = anaImballi[imb].BOXNUM ?? default(int);

                                            //iScatolePiene = anaImballi[imb].BOXNUM ?? default(int);
                                        }
                                        else
                                        {
                                            iNumeroScatole = anaImballi[imb].BOXNUM ?? default(int);
                                            //se i pezzi inviati sono diversi dal prodotto significa che l'ultimo non è completo. es. 100pz a bancale, spedizione 320pz. N imballi=4 * pezziBancale=100 farebbe 400
                                            if (pezziInviare != numeroImballi * pezziBancale)
                                            {
                                                if (anaImballi[imb].PalletCompleto == "S") //v1.8 devo tenere traccia di quante scatole piene ci sono sul bancale
                                                {
                                                    iScatolePiene = (pezziInviare - (iPezziImballaggio * (numeroImballi - 1))) / (iPezziImballaggio / iNumeroScatole);
                                                    iNumeroScatole = (pezziInviare - (iPezziImballaggio * (numeroImballi - 1))) / (iPezziImballaggio / iNumeroScatole);
                                                }
                                                else
                                                //if (anaImballi[imb].PalletCompleto != "S") //se il pallet non viene completato devo calcolare il numero di scatole rimanenti
                                                {
                                                    iNumeroScatole = (pezziInviare - (iPezziImballaggio * (numeroImballi - 1))) / (iPezziImballaggio / iNumeroScatole);
                                                }
                                                iPezziImballaggio = pezziInviare - (pezziBancale * (numeroImballi - 1));

                                            }
                                            else
                                            {
                                                iPezziImballaggio = anaImballi[imb].PARTXBOX ?? default(int);
                                                //iScatolePiene= anaImballi[imb].BOXNUM ?? default(int);
                                            }

                                        }

                                        iMaxEtichetta = iNumeroScatole; //v1.7: numero di etichette da riportare nel record delle scatole
                                    }
                                    else
                                    {
                                        iPezziImballaggio = anaImballi[imb].PARTXBOX ?? default(int);
                                    }

                                    sCodImbDO = anaImballi[imb].BOXCODDO;
                                    if (ImbType == "PALLET" || ImbType == "COPERCHIO")
                                    {
                                        rigaImb.CANTEMB = 1;
                                    }
                                    else
                                    {
                                        if (anaImballi[imb].PalletCompleto == "S") //v1.8 devo tenere traccia di quante scatole piene ci sono sul bancale 
                                        {
                                            if (iScatolePiene != 0)
                                            {
                                                rigaImb.CANTEMB = iScatolePiene;
                                            }
                                            else
                                            {
                                                rigaImb.CANTEMB = iNumeroScatole;
                                            }
                                        }
                                        else
                                        {
                                            rigaImb.CANTEMB = iNumeroScatole;
                                        }
                                    }
                                    rigaImb.MEDIOEMB = sCodImb;
                                    rigaImb.CALMEDIOEMB = "";
                                    rigaImb.LONGITUD = 0;
                                    rigaImb.UMLONGITUD = "";
                                    rigaImb.ANCHURA = 0;
                                    rigaImb.UMANCHURA = "";
                                    rigaImb.ALTURA = 0;
                                    rigaImb.UMALTURA = "";
                                    if (ImbType == "PALLET" || ImbType == "COPERCHIO")
                                    {
                                        rigaImb.CANTPAQUETE = 0;
                                    }
                                    else
                                    {
                                        rigaImb.CANTPAQUETE = iPezziImballaggio;
                                    }

                                    rigaImb.UNIDMEDCPAC = "PCE";
                                    rigaImb.INSTMARCAJE = "";
                                    if (ImbType == "PALLET")
                                    {
                                        rigaImb.IDENTETIQUETA = "M";
                                    }
                                    else if (ImbType == "BOX")
                                    {
                                        rigaImb.IDENTETIQUETA = "S";
                                    }
                                    else
                                    {
                                        rigaImb.IDENTETIQUETA = "0";
                                    }
                                    rigaImb.CALIDENTETIQUETA = "";
                                    rigaImb.ETQMAESTRA = "";//sCodPallet;
                                    rigaImb.FECHAFABRIC = "";
                                    rigaImb.FECHACADUC = "";
                                    if (ImbType == "COPERCHIO")
                                    {
                                        rigaImb.NUMETIQUETA = "";

                                    }
                                    else
                                    {
                                        if (ImbType == "PALLET")
                                        {
                                            rigaImb.NUMETIQUETA = iMlabel.ToString();
                                        }
                                        if (ImbType == "BOX")
                                        {
                                            rigaImb.NUMETIQUETA = iSlabel.ToString();
                                        }

                                        //rigaImb.NUMETIQUETA = iEtichetta.ToString(); 20191007
                                    }
                                    rigaImb.NUMPACCOMP = sCodImb;
                                    rigaImb.NUMLOTE = "";
                                    rigaImb.NUMPACPRO = sCodImbDO;
                                    rigaImb.CAPACITA = iPezziImballaggio.ToString();
                                    //rigaImb.CAPACITA = "0";
                                    rigaImb.DESCEMB = ImbType; //va inserita la tipologia dell'imballo
                                    rigaImb.TIPOENV = "";
                                    rigaImb.CONDIMB = "";
                                    //*****************************************
                                    //RIGHE
                                    //*****************************************
                                    Loggamelo("NewDESADV", "Creo le righe");
                                    var riga = new DESADV_RIGHE();
                                    riga.CLIENTE = cliente;
                                    riga.IDNUMDES = sNumero;
                                    riga.IDEMB = iCPS.ToString();
                                    riga.CPS = iCPS.ToString();
                                    riga.NUMLIN = iCPS.ToString();
                                    riga.REFCOMPRADOR = readerAlnus["BARCFO"].ToString();
                                    riga.NIVELCONFIG = 0;
                                    riga.REFPROVEEDOR = sArticolo;
                                    riga.PESONETO = 0; //DA AGGIUNGERE
                                    riga.UMPESONETO = ""; //DA METTERE
                                    string sBAMEOR = ""; int sBAMEORI;
                                    Loggamelo("NewDESADV", "Gestione BAMEOR");
                                    sBAMEORI = 0;
                                    sBAMEOR = readerAlnus["BAMEOR"].ToString();
                                    if (readerAlnus["BAMEOR"].ToString().Contains('.'))
                                    {
                                        sBAMEORI = sBAMEOR.IndexOf(".");
                                    }
                                    if (readerAlnus["BAMEOR"].ToString().Contains(','))
                                    {
                                        sBAMEORI = sBAMEOR.IndexOf(",");
                                    }


                                    if (sBAMEORI > 0)
                                    {
                                        sBAMEOR = sBAMEOR.Substring(0, sBAMEORI);
                                    }
                                    Loggamelo("NewDESADV", "Fine Gestione BAMEOR");

                                    //-------------------------------------------------------------------------------------------
                                    //GESTIONE PEZZI X BANCALE - per tutto il resto CANTENT=0
                                    //-------------------------------------------------------------------------------------------
                                    //in caso ci sono più bancali l'ultimo non è sicuro contenga tutta la quantità, ma se sono 3 bancali sicuramente i primi due sono pieni
                                    //-------------------------------------------------------------------------------------------
                                    if (ImbType == "PALLET")
                                    {
                                        if (i < numeroImballi) //if (i < numeroImballi - 1)
                                        {
                                            riga.CANTENT = pezziBancale;
                                        }
                                        else
                                        {
                                            //se i pezzi inviati sono diversi dal prodotto significa che l'ultimo non è completo. es. 100pz a bancale, spedizione 320pz. N imballi=4 * pezziBancale=100 farebbe 400
                                            if (Int32.Parse(sBAMEOR) != numeroImballi * pezziBancale)
                                            {
                                                riga.CANTENT = Int32.Parse(sBAMEOR) - ((numeroImballi - 1) * pezziBancale);
                                            }
                                            else
                                            {
                                                riga.CANTENT = pezziBancale;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        riga.CANTENT = 0;
                                    }
                                    //riga.CANTENT = Int32.Parse(sBAMEOR); //da sistemare con qta x imballo iPezziImballaggio
                                    riga.UMCANTENT = "PCE";
                                    riga.PAISORIG = "IT";
                                    riga.TIPOREGIMEN = "X"; //X sempre buono, G=origin in EU W= europa EFTA
                                    riga.TEXTOLIBRE = readerAlnus["ARTDES"].ToString();
                                    riga.VALORADUANA = 0;
                                    riga.DIVISAVARADU = "";
                                    //ordine loro
                                    riga.NUMPEDIDO = readerAlnus["ORCRIF"].ToString();
                                    //va inserito anche nella testata
                                    riga.NUMPROGRAMA = "";
                                    riga.NUMLINPEDIDO = "";
                                    riga.INFOADICIONAL = "";
                                    riga.NUMSERIE = "";
                                    riga.NUMENGINE = "";
                                    riga.NUMVEHICLE = "";
                                    riga.RFF_AAP = ""; //ho saltato i campi in fondo blanks per vedere se posso evitare
                                    riga.RFF_AAE = "";
                                    riga.DTM_AAE = "";
                                    riga.RFF_IV = "";
                                    riga.RFF_IV2 = "";
                                    riga.FINALDESCARGA = sFinalD;
                                    riga.NUMLINALB = Int32.Parse(readerAlnus["BADNRD"].ToString());

                                    if (ImbType == "PALLET")
                                    {
                                        riga.APPLICACION = "M";//  v1.7: messo uguale a tipo etichetta  "S" per la serie; //qua forse va gestita la tipologia
                                    }
                                    else if (ImbType == "BOX")
                                    {
                                        riga.APPLICACION = "S";
                                    }
                                    else
                                    {
                                        riga.APPLICACION = " ";
                                    }

                                    riga.INVENTARIO = "";
                                    Loggamelo("NewDESADV", "Aggiungo la riga al DB");
                                    db.DESADV_RIGHE.Add(riga);

                                    //db.SaveChanges();
                                    if (ImbType == "PALLET")
                                    {
                                        //rigaImb.NUMETIQUETA2 = iEtichetta.ToString(); //nel caso del pallet l'etichetta è una sola
                                        var etichetta = new DESADV_ETICHETTE();
                                        etichetta.CLIENTE = cliente;
                                        etichetta.IDNUMDES = sNumero;
                                        etichetta.IDEMB = iCPS.ToString();
                                        etichetta.CPS = iCPS.ToString();
                                        //etichetta.IDETQ = iEtichetta.ToString(); 20191007
                                        etichetta.IDETQ = iMlabel.ToString();
                                        etichetta.IDETQPACK = "";
                                        //etichetta.NUMETIQUETA = iEtichetta.ToString(); 20191007
                                        etichetta.NUMETIQUETA = iMlabel;//.ToString();
                                        etichetta.NUMPACCOMP = "";
                                        etichetta.NUMLOTE = "";
                                        etichetta.HANNUM = "";
                                        etichetta.AGENCY = "";
                                        etichetta.HANTYPE = "M";// v1.7: metto M per i pallet //"1J"; //HANTYPE 1J = S-LABEL ->5J = G-LABEL -> 6J = M-LABEL
                                        //etichetta.NUMETIQUETA2 = iEtichetta.ToString(); 20191007
                                        etichetta.NUMETIQUETA2 = iMlabel;//.ToString();
                                        db.DESADV_ETICHETTE.Add(etichetta);
                                        Loggamelo("NewDESADV", "Riga salvata e etichette  PALLET");
                                        db.SaveChanges();
                                    }
                                    else if (ImbType == "BOX")
                                    {

                                        //********************************
                                        //ETICHETTE
                                        //********************************
                                        var etichetta = new DESADV_ETICHETTE();
                                        etichetta.CLIENTE = cliente;
                                        etichetta.IDNUMDES = sNumero;
                                        etichetta.IDEMB = iCPS.ToString();
                                        etichetta.CPS = iCPS.ToString();
                                        //etichetta.IDETQ = iEtichetta.ToString(); 20191007
                                        etichetta.IDETQ = iSlabel.ToString();
                                        etichetta.IDETQPACK = "";
                                        //etichetta.NUMETIQUETA = iEtichetta.ToString(); 20191007
                                        etichetta.NUMETIQUETA = iSlabel;//.ToString();
                                        etichetta.NUMPACCOMP = "";
                                        etichetta.NUMLOTE = "";
                                        etichetta.HANNUM = "";
                                        etichetta.AGENCY = "";
                                        etichetta.HANTYPE = "S"; //v1.7: metto S per le scatole //"1J"; //HANTYPE 1J = S-LABEL ->5J = G-LABEL -> 6J = M-LABEL
                                        //iEtichetta = (iEtichetta + iMaxEtichetta - 1); 20191007
                                        iSlabel = (iSlabel + iMaxEtichetta - 1);
                                        //etichetta.NUMETIQUETA2 = iEtichetta.ToString(); 20191007
                                        etichetta.NUMETIQUETA2 = iSlabel;//.ToString();
                                        db.DESADV_ETICHETTE.Add(etichetta);
                                        Loggamelo("NewDESADV", "Salvata riga e etichette SCATOLE");
                                        db.SaveChanges();

                                    }
                                    else //COPERCHIO
                                    {
                                        var etichetta = new DESADV_ETICHETTE();
                                        etichetta.CLIENTE = cliente;
                                        etichetta.IDNUMDES = sNumero;
                                        etichetta.IDEMB = iCPS.ToString();
                                        etichetta.CPS = iCPS.ToString();
                                        etichetta.IDETQ = "0";
                                        etichetta.IDETQPACK = "";
                                        etichetta.NUMETIQUETA = 0;
                                        etichetta.NUMPACCOMP = "";
                                        etichetta.NUMLOTE = "";
                                        etichetta.HANNUM = "";
                                        etichetta.AGENCY = "";
                                        etichetta.HANTYPE = "0";// v1.7: metto M per i pallet //"1J"; //HANTYPE 1J = S-LABEL ->5J = G-LABEL -> 6J = M-LABEL
                                        etichetta.NUMETIQUETA2 = 0;
                                        db.DESADV_ETICHETTE.Add(etichetta);
                                        Loggamelo("NewDESADV", "Salvata riga e etichette COPERCHIO");
                                        db.SaveChanges();
                                    }
                                    if (ImbType == "COPERCHIO")
                                    {
                                        rigaImb.NUMETIQUETA2 = " ";
                                    }
                                    else
                                    {
                                        if (ImbType == "BOX")
                                        {
                                            rigaImb.NUMETIQUETA2 = iSlabel.ToString();
                                        }
                                        if (ImbType == "PALLET")
                                        {
                                            rigaImb.NUMETIQUETA2 = iMlabel.ToString();
                                        }

                                    }
                                    Loggamelo("NewDESADV", "Aggiungo riga imballi");
                                    db.DESADV_IMBALLI.Add(rigaImb);
                                    //***************************************************************
                                    //SCATOLE VUOTE
                                    //***************************************************************
                                    //v1.8 gestione righe delle scatole vuote...may god forgive me
                                    if (iScatolePiene != 0 && ImbType == "BOX")
                                    {
                                        var imbVuoti = new DESADV_IMBALLI();
                                        iCPS += 1;
                                        imbVuoti.CLIENTE = cliente;
                                        imbVuoti.IDNUMDES = sNumero;
                                        imbVuoti.IDEMB = iCPS.ToString(); //i.ToString(); //v1.7 Donatiello ha detto di mettere IDEMB=CPS
                                        imbVuoti.CPS = iCPS.ToString();
                                        sCodImb = anaImballi[imb].BOXCOD;
                                        imbVuoti.CANTEMB = iScatoleTotali - iScatolePiene;
                                        imbVuoti.MEDIOEMB = sCodImb;
                                        imbVuoti.CALMEDIOEMB = "";
                                        imbVuoti.LONGITUD = 0;
                                        imbVuoti.UMLONGITUD = "";
                                        imbVuoti.ANCHURA = 0;
                                        imbVuoti.UMANCHURA = "";
                                        imbVuoti.ALTURA = 0;
                                        imbVuoti.UMALTURA = "";
                                        imbVuoti.CANTPAQUETE = 0;
                                        imbVuoti.UNIDMEDCPAC = "PCE";
                                        imbVuoti.INSTMARCAJE = "";
                                        imbVuoti.IDENTETIQUETA = "0";
                                        imbVuoti.CALIDENTETIQUETA = "";
                                        imbVuoti.ETQMAESTRA = "";//sCodPallet;
                                        imbVuoti.FECHAFABRIC = "";
                                        imbVuoti.FECHACADUC = "";
                                        imbVuoti.NUMETIQUETA = "0";
                                        imbVuoti.NUMPACCOMP = sCodImb;
                                        imbVuoti.NUMLOTE = "";
                                        imbVuoti.NUMPACPRO = sCodImbDO;
                                        imbVuoti.CAPACITA = iPezziImballaggio.ToString();
                                        imbVuoti.DESCEMB = ImbType;
                                        imbVuoti.TIPOENV = "";
                                        imbVuoti.CONDIMB = "";
                                        imbVuoti.NUMETIQUETA2 = "0";
                                        //******INIZIO ABORTO***
                                        var rigaVuota = new DESADV_RIGHE();
                                        rigaVuota.CLIENTE = cliente;
                                        rigaVuota.IDNUMDES = sNumero;
                                        rigaVuota.IDEMB = iCPS.ToString();
                                        rigaVuota.CPS = iCPS.ToString();
                                        rigaVuota.NUMLIN = iCPS.ToString();
                                        rigaVuota.REFCOMPRADOR = readerAlnus["BARCFO"].ToString();
                                        rigaVuota.NIVELCONFIG = 0;
                                        rigaVuota.REFPROVEEDOR = sArticolo;
                                        rigaVuota.PESONETO = 0; //DA AGGIUNGERE
                                        rigaVuota.UMPESONETO = ""; //DA METTERE
                                        sBAMEOR = readerAlnus["BAMEOR"].ToString();
                                        sBAMEORI = sBAMEOR.IndexOf(",");
                                        if (sBAMEORI > 0)
                                        {
                                            sBAMEOR = sBAMEOR.Substring(0, sBAMEORI);
                                        }
                                        rigaVuota.CANTENT = 0;
                                        rigaVuota.UMCANTENT = "PCE";
                                        rigaVuota.PAISORIG = "IT";
                                        rigaVuota.TIPOREGIMEN = "X"; //X sempre buono, G=origin in EU W= europa EFTA
                                        rigaVuota.TEXTOLIBRE = readerAlnus["ARTDES"].ToString();
                                        rigaVuota.VALORADUANA = 0;
                                        rigaVuota.DIVISAVARADU = "";
                                        //ordine loro
                                        rigaVuota.NUMPEDIDO = readerAlnus["ORCRIF"].ToString();
                                        rigaVuota.NUMPROGRAMA = "";
                                        rigaVuota.NUMLINPEDIDO = "";
                                        rigaVuota.INFOADICIONAL = "";
                                        rigaVuota.NUMSERIE = "";
                                        rigaVuota.NUMENGINE = "";
                                        rigaVuota.NUMVEHICLE = "";
                                        rigaVuota.RFF_AAP = ""; //ho saltato i campi in fondo blanks per vedere se posso evitare
                                        rigaVuota.RFF_AAE = "";
                                        rigaVuota.DTM_AAE = "";
                                        rigaVuota.RFF_IV = "";
                                        rigaVuota.RFF_IV2 = "";
                                        rigaVuota.FINALDESCARGA = sFinalD;
                                        rigaVuota.NUMLINALB = Int32.Parse(readerAlnus["BADNRD"].ToString());
                                        rigaVuota.APPLICACION = "0";
                                        rigaVuota.INVENTARIO = "";
                                        db.DESADV_RIGHE.Add(rigaVuota);
                                        //db.SaveChanges();
                                        var etichetta = new DESADV_ETICHETTE();
                                        etichetta.CLIENTE = cliente;
                                        etichetta.IDNUMDES = sNumero;
                                        etichetta.IDEMB = iCPS.ToString();
                                        etichetta.CPS = iCPS.ToString();
                                        etichetta.IDETQ = "0";
                                        etichetta.IDETQPACK = "";
                                        etichetta.NUMETIQUETA = 0;
                                        etichetta.NUMPACCOMP = "";
                                        etichetta.NUMLOTE = "";
                                        etichetta.HANNUM = "";
                                        etichetta.AGENCY = "";
                                        etichetta.HANTYPE = "0";// v1.7: metto M per i pallet //"1J"; //HANTYPE 1J = S-LABEL ->5J = G-LABEL -> 6J = M-LABEL
                                        etichetta.NUMETIQUETA2 = 0;
                                        db.DESADV_ETICHETTE.Add(etichetta);
                                        db.DESADV_IMBALLI.Add(imbVuoti);
                                        Loggamelo("NewDESADV", "Salvo etichette e riga VUOTI");
                                        db.SaveChanges();
                                        //*****FINE ABORTO****
                                    }
                                    //
                                    db.SaveChanges();
                                }
                            }
                            else if (EDI == "EDIFACT")
                            {
                                Loggamelo("NewDESADV", "Nuovo Imballo", "EDIFACT");
                                DESADV_ANA_IMBALLI pallet = db.DESADV_ANA_IMBALLI.Where(e => e.ARTCOD == sArticolo && e.TIPOLOGIA == "PALLET" && e.imbStandard == Imballo && e.CLIENTE == cliente).FirstOrDefault();
                                pezziBancale = Int32.Parse(pallet.PARTXBOX.ToString());
                                //pezziBancale = Int32.Parse(db.DESADV_ANA_IMBALLI.Where(e => e.ARTCOD == sArticolo && e.TIPOLOGIA == "PALLET" && e.imbStandard == Imballo).Select(e => e.PARTXBOX).FirstOrDefault().ToString());

                                if (readerAlnus["BAMEOR"].ToString().Contains('.'))
                                {
                                    pezziInviare = Int32.Parse(readerAlnus["BAMEOR"].ToString().Replace(".000", ""));
                                }
                                if (readerAlnus["BAMEOR"].ToString().Contains(','))
                                {
                                    pezziInviare = Int32.Parse(readerAlnus["BAMEOR"].ToString().Replace(",000", ""));
                                }

                                //numeroImballi indica quanti pallet sono.
                                //per ogni pallet vanno create le righe di tutti gli imballaggi (pallet, coperchio, box) con cps ad aumentare e IDEMB fisso per bancale


                                //influisce col for iniziale altrimenti

                                //numeroImballi = (int)Math.Ceiling((double)pezziInviare / pezziBancale); 

                                //IMBALLI
                                iCPS += 1;
                                var rigaImb = new DESADV_IMBALLI();
                                rigaImb.CLIENTE = cliente;
                                rigaImb.IDNUMDES = sNumero;
                                rigaImb.IDEMB = i.ToString(); //i.ToString(); //v1.7 Donatiello ha detto di mettere IDEMB=CPS
                                rigaImb.CPS = iCPS.ToString();

                                //qua va gestito in caso di bancale non pieno per le scatole...x ford le scatole sono fisse
                                sCodImb = pallet.BOXCOD;

                                iPezziImballaggio = pallet.PARTXBOX ?? default(int);
                                iScatoleTotali = pallet.BOXNUM ?? default(int);
                                iNumeroScatole = pallet.BOXNUM ?? default(int);
                                if (i < (numeroImballi)) // i < (numeroImballi - 1) 
                                {
                                    iPezziImballaggio = pallet.PARTXBOX ?? default(int);
                                }
                                else
                                {
                                    //se i pezzi inviati sono diversi dal prodotto significa che l'ultimo non è completo. es. 100pz a bancale, spedizione 320pz. N imballi=4 * pezziBancale=100 farebbe 400
                                    if (pezziInviare != numeroImballi * pezziBancale)
                                    {
                                        if (pallet.PalletCompleto == "S") //v1.8 devo tenere traccia di quante scatole piene ci sono sul bancale
                                        {
                                            iScatolePiene = (pezziInviare - (iPezziImballaggio * (numeroImballi - 1))) / (iPezziImballaggio / iNumeroScatole);
                                            iNumeroScatole = (pezziInviare - (iPezziImballaggio * (numeroImballi - 1))) / (iPezziImballaggio / iNumeroScatole);
                                        }
                                        else
                                        //if (anaImballi[imb].PalletCompleto != "S") //se il pallet non viene completato devo calcolare il numero di scatole rimanenti
                                        {
                                            iNumeroScatole = (pezziInviare - (iPezziImballaggio * (numeroImballi - 1))) / (iPezziImballaggio / iNumeroScatole);
                                        }
                                        iPezziImballaggio = pezziInviare - (pezziBancale * (numeroImballi - 1));

                                    }
                                    else
                                    {
                                        iPezziImballaggio = pallet.PARTXBOX ?? default(int);
                                        //iScatolePiene= anaImballi[imb].BOXNUM ?? default(int);
                                    }

                                }
                                iMlabel += 1;
                                //iMaxEtichetta += iNumeroScatole; 20191007
                                //if (iEtichetta > 0) { iMaxEtichetta++; } 20191007

                                sCodImbDO = pallet.BOXCODDO;
                                rigaImb.CANTEMB = iNumeroScatole;
                                rigaImb.MEDIOEMB = sCodImb;//secondo me qui vanno i codici delle scatole
                                rigaImb.CALMEDIOEMB = "";
                                rigaImb.LONGITUD = 0;
                                rigaImb.UMLONGITUD = "";
                                rigaImb.ANCHURA = 0;
                                rigaImb.UMANCHURA = "";
                                rigaImb.ALTURA = 0;
                                rigaImb.UMALTURA = "";
                                //rigaImb.CANTPAQUETE = 0;
                                rigaImb.CANTPAQUETE = Int32.Parse(db.DESADV_ANA_IMBALLI.Where(f => f.TIPOLOGIA == "BOX" && f.imbStandard == Imballo && f.ARTCOD == sArticolo && f.CLIENTE == cliente).Select(e => e.PARTXBOX).FirstOrDefault().ToString()); //iPezziImballaggio.ToString();//iPezziImballaggio;
                                rigaImb.UNIDMEDCPAC = "PCE";
                                rigaImb.INSTMARCAJE = "";
                                rigaImb.IDENTETIQUETA = "M"; //x me dovrebbe essere M, ma nell'esempio c'è S
                                rigaImb.CALIDENTETIQUETA = "";
                                rigaImb.ETQMAESTRA = iMlabel.ToString();//(iMaxEtichetta+1).ToString();//sCodPallet; 20191007
                                rigaImb.FECHAFABRIC = "";
                                rigaImb.FECHACADUC = "";
                                rigaImb.NUMETIQUETA = ""; //iEtichetta.ToString();
                                rigaImb.NUMPACCOMP = sCodImb;
                                rigaImb.NUMLOTE = "";
                                rigaImb.NUMPACPRO = sCodImbDO;
                                rigaImb.CAPACITA = db.DESADV_ANA_IMBALLI.Where(f => f.TIPOLOGIA == "BOX" && f.imbStandard == Imballo && f.ARTCOD == sArticolo && f.CLIENTE == cliente).Select(e => e.PARTXBOX).FirstOrDefault().ToString(); //iPezziImballaggio.ToString();
                                //rigaImb.CAPACITA = "0";
                                rigaImb.DESCEMB = "PALLET"; //va inserita la tipologia dell'imballo
                                rigaImb.TIPOENV = "";
                                rigaImb.CONDIMB = "";
                                rigaImb.NUMETIQUETA2 = "";
                                db.DESADV_IMBALLI.Add(rigaImb);
                                db.SaveChanges();

                                //*****************************************
                                //RIGHE
                                //*****************************************
                                Loggamelo("NewDESADV", "Creo le righe", "EDIFACT");
                                var riga = new DESADV_RIGHE();
                                riga.CLIENTE = cliente;
                                riga.IDNUMDES = sNumero;
                                riga.IDEMB = i.ToString();
                                riga.CPS = iCPS.ToString();
                                riga.NUMLIN = iCPS.ToString();
                                riga.REFCOMPRADOR = readerAlnus["BARCFO"].ToString();
                                riga.NIVELCONFIG = 0;
                                riga.REFPROVEEDOR = sArticolo;
                                riga.PESONETO = 0; //DA AGGIUNGERE
                                riga.UMPESONETO = ""; //DA METTERE
                                riga.CANTENT = iPezziImballaggio;
                                riga.UMCANTENT = "PCE";
                                riga.PAISORIG = pallet.Origine; //qua deve andare india
                                riga.TIPOREGIMEN = "X";
                                riga.TEXTOLIBRE = readerAlnus["ARTDES"].ToString();
                                riga.VALORADUANA = 0;
                                riga.DIVISAVARADU = "";
                                //ordine loro
                                riga.NUMPEDIDO = readerAlnus["ORCRIF"].ToString();
                                //va inserito anche nella testata
                                riga.NUMPROGRAMA = "";
                                riga.NUMLINPEDIDO = "";
                                riga.INFOADICIONAL = "";
                                riga.NUMSERIE = "";
                                riga.NUMENGINE = "";
                                riga.NUMVEHICLE = "";
                                riga.RFF_AAP = ""; //ho saltato i campi in fondo blanks per vedere se posso evitare
                                riga.RFF_AAE = "";
                                riga.DTM_AAE = "";
                                riga.RFF_IV = "";
                                riga.RFF_IV2 = "";
                                riga.FINALDESCARGA = sFinalD;
                                riga.NUMLINALB = Int32.Parse(readerAlnus["BADNRD"].ToString());
                                riga.APPLICACION = "S";
                                riga.INVENTARIO = "";
                                Loggamelo("NewDESADV", "Aggiungo la riga al DB");
                                db.DESADV_RIGHE.Add(riga);
                                db.SaveChanges();
                                ////********************************
                                ////ETICHETTE
                                ////********************************

                                iMaxEtichetta = iSlabel + iNumeroScatole;
                                //for (iEtichetta = iEtichetta+1; iEtichetta <= iMaxEtichetta; iEtichetta++) 20191007
                                for (iSlabel = iSlabel + 1; iSlabel <= iMaxEtichetta; iSlabel++)
                                {
                                    var etichetta = new DESADV_ETICHETTE();
                                    etichetta.CLIENTE = cliente;
                                    etichetta.IDNUMDES = sNumero;
                                    etichetta.IDEMB = i.ToString();
                                    etichetta.CPS = iCPS.ToString();
                                    //etichetta.IDETQ = iEtichetta.ToString(); 20191007
                                    etichetta.IDETQ = iSlabel.ToString();
                                    etichetta.IDETQPACK = "";
                                    //etichetta.NUMETIQUETA = iEtichetta.ToString(); 20191007
                                    etichetta.NUMETIQUETA = iSlabel;
                                    etichetta.NUMPACCOMP = "";
                                    etichetta.NUMLOTE = "";
                                    etichetta.HANNUM = "";
                                    etichetta.AGENCY = "";
                                    etichetta.HANTYPE = "1J"; //HANTYPE 1J = S-LABEL ->5J = G-LABEL -> 6J = M-LABEL
                                    //etichetta.NUMETIQUETA2 = iEtichetta.ToString(); 20191007
                                    etichetta.NUMETIQUETA2 = iSlabel;
                                    db.DESADV_ETICHETTE.Add(etichetta);
                                    db.SaveChanges();
                                }
                                //iEtichetta ++;  20191007
                            }
                        }

                    }
                    else
                    {
                        btest = false;
                    }

                } while (btest == true);
                sqlConnectionAlnus.Close();
            }
            //return RedirectToAction("Index");
            var rispostaOK = new { status = "201", data = "Spedizione creata. E' stato assegnato l'ID: " + sNumero, ID = sNumero };
            return Json(rispostaOK);
        }





        public JsonResult listaINVIO(string Cliente)
        {
            //var query = db.DESADV_TESTATA.Where(a => a.STATO == "APERTO" && a.CLIENTE == Cliente).Select(a => a.NUMDES);
            //SelectList Testate = new SelectList(query.ToList());
            //var lista = Testate.Select(a => a.Text).ToArray();
            //return Json(lista, JsonRequestBehavior.AllowGet);

            //20190410 modifica: per visualizzazione deufol aggiungo come testo il codice della bolla
            var query = db.DESADV_TESTATA.Where(a => a.STATO == "APERTO" && a.CLIENTE == Cliente).Select(a => new { valore = a.NUMDES, testo = a.CODEQUIP });
            SelectList Testate = new SelectList(query, "valore", "testo");
            var lista = Testate.ToArray();
            return Json(lista, JsonRequestBehavior.AllowGet);



        }

        [System.Web.Mvc.HttpGet]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public ActionResult InvioEDI(string cliente, int? id)
        {
            //ViewBag.CLIENTE = new SelectList(db.DESADV_ANAGRAFICHE, "CLIENTE", "CLIENTE");
            if (cliente != null)
            {
                ViewBag.Cliente = '_' + cliente;
            }
            if (id != null)
            {
                ViewBag.Testata = '_' + "" + id.Value;
            }
            return View();
        }

        [System.Web.Http.HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult InvioEDI(string Cliente, string sNumero)
        {
            //*************************************************************
            //CARICO IL TESTO DI CABALB
            //*************************************************************
            try
            {

                //throw new Exception("Test Exception");
                string sPath = @"C:\inetpub\wwwroot\DESADV\";
            string strNomeFile = sPath + @"CABALB - " + Cliente + sNumero + ".txt";
            string sMessaggio = "";
            string sID = "";

            DESADV_ANAGRAFICHE testCliente = db.DESADV_ANAGRAFICHE.Find(Cliente);

            if (testCliente == null)
            {
                var risposta = new { status = "400", data = "Cliente non trovato" };
                return Json(risposta);
            }

            LogTable log = new LogTable();

            log.Utente = User.Identity.Name;
            log.Orario = DateTime.Now.ToString();
            log.Programma = "NewDESADV-Invio";
            log.Avviso = "procedo a eliminare il file se esiste";
            db.LogTable.Add(log);
            db.SaveChanges();



            if (System.IO.File.Exists(strNomeFile))
            {
                log.Utente = User.Identity.Name;
                log.Orario = DateTime.Now.ToString();
                log.Programma = "NewDESADV-Invio";
                log.Avviso = "file trovato";
                db.LogTable.Add(log);
                db.SaveChanges();

                System.IO.File.Delete(strNomeFile);

                log.Utente = User.Identity.Name;
                log.Orario = DateTime.Now.ToString();
                log.Programma = "NewDESADV-Invio";
                log.Avviso = "eliminato";
                db.LogTable.Add(log);
                db.SaveChanges();
            }
            DESADV_TESTATA testata = db.DESADV_TESTATA.Find(Cliente, sNumero);
                //RICHIESTA DA PARTE DI JUAN: INIZIARE SEMPRE CON 1. TOLGO IL CLIENTE PRIMA DEL NUMERO. ANULLATA
                sID = testCliente.ID.ToString() + testata.ID_ASN.ToString();
            //sID =  testata.NUMDES.ToString();
            sMessaggio = "";
            sMessaggio += sID.ToString().PadRight(35, ' ')
               + testata.TIPO.PadRight(3, ' ')
               + testata.CODTIPO.PadRight(3, ' ')
               + testata.FECDES.PadRight(12, ' ')
               + testata.FECENT.PadRight(12, ' ')
               + testata.FECSAL.PadRight(12, ' ')
               + testata.TOTPESBRU.ToString().PadLeft(18, '0');
            sMessaggio += testata.UMTOTPESBRU.PadRight(3, ' ')
                + testata.TOTPESNE.ToString().PadLeft(18, '0')
                + testata.UMTOTPESNE.PadRight(3, ' ')
                + testata.NUMENVCARG.PadRight(35, ' ')
                + testata.VENDEDOR.PadRight(35, ' ')
                + testata.CALVENDEDOR.PadRight(3, ' ')
                + testata.DIRECCION_SE.PadRight(35, ' ')
                + testata.POBLACION_SE.PadRight(35, ' ')
                + testata.NOMBRE_SE.PadRight(35, ' ')
                + testata.CODINTVEND.PadRight(35, ' ');
            sMessaggio += testata.CONSIGNATARIO.PadRight(35, ' ')
                + testata.CALCONSIGNA.PadRight(3, ' ')
                + testata.DIRECCION_CN.PadRight(35, ' ')
                + testata.POBLACION_CN.PadRight(35, ' ')
                + testata.NOMBRE_CN.PadRight(35, ' ')
                + testata.LUGDESCARGA.PadRight(25, ' ')
                + testata.EXPEDCARGA.PadRight(35, ' ')
                + testata.CALITERLOCUTOR.PadRight(3, ' ')
                + testata.CALEXPEDIDOR.PadRight(3, ' ')
                + testata.NOMBRE_FW.PadRight(35, ' ')
                + testata.CODINTEXPE.PadRight(35, ' ')
                + testata.ENTREGACODIG.PadRight(3, ' ')
                + testata.ENTREGATRANS.PadRight(3, ' ')
                + testata.CALPAGO.PadRight(3, ' ')
                + testata.TIPTRANS.PadRight(3, ' ');
            sMessaggio += testata.CODDESCTRAS.PadRight(8, ' ')
                + testata.IDTRANSPORT.PadRight(35, ' ')
                + testata.CODTIPEQUIP.PadRight(3, ' ')
                + testata.CODEQUIP.PadRight(17, ' ')
                + testata.RFF_AAJ.PadRight(35, ' ')
                + testata.RFF_AAS.PadRight(35, ' ')
                + testata.RFF_CRN.PadRight(35, ' ')
                + testata.SHIPFROM.PadRight(35, ' ')
                + testata.NUMTRANOLD.PadLeft(5, '0')
                + testata.NUMTRANNEW.PadLeft(5, '0')
                + testata.LUGENTREGA.PadRight(3, ' ')
                + testata.CONTRATO.PadRight(12, ' ');

            //**************************************************
            //SCRIVO CABALB
            //**************************************************
            log.Utente = User.Identity.Name;
            log.Orario = DateTime.Now.ToString();
            log.Programma = "NewDESADV-Invio";
            log.Avviso = "scrivo la testata";
            db.LogTable.Add(log);
            db.SaveChanges();
            try
            {
                //strNomeFile = @"\\192.168.99.100\c\dati\test.txt";
                using (System.IO.FileStream fs = System.IO.File.Create(strNomeFile))
                {
                    log.Utente = User.Identity.Name;
                    log.Orario = DateTime.Now.ToString();
                    log.Programma = "NewDESADV-Invio";
                    log.Avviso = "file creato";
                    db.LogTable.Add(log);
                    db.SaveChanges();

                    Byte[] info = new UTF8Encoding(true).GetBytes(sMessaggio);
                    fs.Write(info, 0, info.Length);
                    log.Utente = User.Identity.Name;
                    log.Orario = DateTime.Now.ToString();
                    log.Programma = "NewDESADV-Invio";
                    log.Avviso = "scrivo";
                    db.LogTable.Add(log);
                    db.SaveChanges();
                }
            }
            catch (Exception exc)
            {
                log.Utente = User.Identity.Name;
                log.Orario = DateTime.Now.ToString();
                log.Programma = "NewDESADV-Invio";
                log.Avviso = exc.Message.ToString();
                //log.Informazioni = exc.InnerException.Message.ToString();
                db.LogTable.Add(log);
                db.SaveChanges();
                var rispostaErrore = new { status = "500", data = "ERRORE. Contattare edp@dellorto.it" };
                return Json(rispostaErrore);
            }
            //***************************************************
            //EMBALB
            //***************************************************
            sMessaggio = "";
            strNomeFile = sPath + @"EMBALB - " + Cliente + sNumero + ".txt";

            foreach (DESADV_IMBALLI imballo in db.DESADV_IMBALLI.Where(a => a.CLIENTE == Cliente && a.IDNUMDES == sNumero))
            {
                //DESADV_IMBALLI imballo = db.DESADV_IMBALLI.Find(Cliente, sNumero);
                sMessaggio += sID.PadRight(35, ' ') //imballo.IDNUMDES.PadRight(35, ' ')
                    + imballo.IDEMB.PadRight(12, ' ')
                    + imballo.CPS.PadRight(3, ' ')
                    + imballo.CANTEMB.ToString().PadLeft(8, '0')
                    + imballo.MEDIOEMB.PadRight(17, ' ')
                    + imballo.CALMEDIOEMB.PadRight(3, ' ')
                    + imballo.LONGITUD.ToString().PadLeft(18, '0')
                    + imballo.UMLONGITUD.PadRight(3, ' ')
                    + imballo.ANCHURA.ToString().PadLeft(18, '0')
                    + imballo.UMANCHURA.PadRight(3, ' ')
                    + imballo.ALTURA.ToString().PadLeft(18, '0')
                    + imballo.UMALTURA.PadRight(3, ' ')
                    + imballo.CANTPAQUETE.ToString().PadLeft(15, '0')
                    + imballo.UNIDMEDCPAC.PadRight(3, ' ')
                    + imballo.INSTMARCAJE.PadRight(3, ' ')
                    + imballo.IDENTETIQUETA.PadRight(3, ' ')
                    + imballo.CALIDENTETIQUETA.PadRight(3, ' ')
                    + imballo.ETQMAESTRA.PadRight(35, ' ')
                    + imballo.FECHAFABRIC.PadRight(12, ' ')
                    + imballo.FECHACADUC.PadRight(12, ' ')
                    + imballo.NUMETIQUETA.PadRight(35, ' ')
                    + imballo.NUMPACCOMP.PadRight(35, ' ')
                    + imballo.NUMLOTE.PadRight(35, ' ')
                    + imballo.NUMPACPRO.PadRight(22, ' ')
                    + imballo.CAPACITA.PadRight(13, ' ')
                    + imballo.DESCEMB.PadRight(35, ' ')
                    + imballo.TIPOENV.PadRight(3, ' ')
                    + imballo.CONDIMB.PadRight(3, ' ')
                    + imballo.NUMETIQUETA2.PadRight(35, ' ');
                sMessaggio += System.Environment.NewLine;
            }
            //*******************************************************
            //SCRIVO EMBALB
            //*******************************************************
            using (System.IO.FileStream fs = System.IO.File.Create(strNomeFile))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(sMessaggio);
                fs.Write(info, 0, info.Length);
            }
            //********************************************************
            //ETQPACK
            //********************************************************
            sMessaggio = "";
            strNomeFile = sPath + @"ETQPACK - " + Cliente + sNumero + ".txt";
                int testEtichetta;
            foreach (DESADV_ETICHETTE etichetta in db.DESADV_ETICHETTE.Where(a => a.CLIENTE == Cliente && a.IDNUMDES == sNumero))
            {
                sMessaggio += sID.PadRight(35, ' ')
                    + etichetta.IDEMB.PadRight(12, ' ')
                    + etichetta.IDETQ.PadRight(12, ' ')
                    + etichetta.IDETQPACK.PadRight(12, ' ');
                if (etichetta.NUMETIQUETA == 0)
                {
                    sMessaggio += "".PadRight(35, ' ');
                }
                else
                {
                    sMessaggio += Convert.ToString(etichetta.NUMETIQUETA).PadRight(35, ' ');
                }

               // + Convert.ToString(etichetta.NUMETIQUETA).PadRight(35, ' ')
                 sMessaggio+= etichetta.NUMPACCOMP.PadRight(35, ' ')
                    + etichetta.NUMLOTE.PadRight(35, ' ')
                    + etichetta.HANNUM.PadRight(35, ' ')
                    + etichetta.AGENCY.PadRight(35, ' ')
                    + etichetta.HANTYPE.PadRight(35, ' ');
                //commento di test
                if (etichetta.NUMETIQUETA2.HasValue == false)
                {
                    sMessaggio += "".PadRight(35, ' ');
                }
                else
                {
                    sMessaggio += Convert.ToString(etichetta.NUMETIQUETA2.Value).PadRight(35, ' ');
                }
                    sMessaggio += System.Environment.NewLine;

            }

            //*******************************************************
            //SCRIVO ETQPACK
            //*******************************************************
            using (System.IO.FileStream fs = System.IO.File.Create(strNomeFile))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(sMessaggio);
                fs.Write(info, 0, info.Length);
            }

            //********************************************************
            //LINALB 
            //********************************************************
            sMessaggio = "";
            strNomeFile = sPath + @"LINALB - " + Cliente + sNumero + ".txt";
            foreach (DESADV_RIGHE riga in db.DESADV_RIGHE.Where(a => a.CLIENTE == Cliente && a.IDNUMDES == sNumero))
            {
                sMessaggio += sID.PadRight(35, ' ')
                    + riga.IDEMB.PadRight(12, ' ')
                    + riga.NUMLIN.PadRight(6, ' ')
                    + riga.REFCOMPRADOR.PadRight(35, ' ')
                    + riga.NIVELCONFIG.ToString().PadLeft(2, '0')
                    + riga.REFPROVEEDOR.PadRight(35, ' ')
                    + riga.PESONETO.ToString().PadLeft(18, '0')
                    + riga.UMPESONETO.PadRight(3, ' ')
                    + riga.CANTENT.ToString().PadLeft(15, '0')
                    + riga.UMCANTENT.PadRight(3, ' ')
                    + riga.PAISORIG.PadRight(3, ' ')
                    + riga.TIPOREGIMEN.PadRight(3, ' ')
                    + riga.TEXTOLIBRE.PadRight(70, ' ')
                    + riga.VALORADUANA.ToString().PadLeft(18, '0')
                    + riga.DIVISAVARADU.PadRight(3, ' ')
                    + riga.NUMPEDIDO.PadRight(35, ' ')
                    + riga.NUMPROGRAMA.PadRight(35, ' ')
                    + riga.NUMLINPEDIDO.PadRight(6, ' ')
                    + riga.INFOADICIONAL.PadRight(35, ' ')
                    + riga.NUMSERIE.PadRight(35, ' ')
                    + riga.NUMENGINE.PadRight(35, ' ')
                    + riga.NUMVEHICLE.PadRight(35, ' ')
                    + riga.RFF_AAP.PadRight(35, ' ')
                    + riga.RFF_AAE.PadRight(35, ' ')
                    + riga.DTM_AAE.PadRight(35, ' ')
                    + riga.RFF_IV.PadRight(35, ' ')
                    + riga.RFF_IV2.PadRight(35, ' ')
                    + riga.FINALDESCARGA.PadRight(25, ' ')
                    + riga.NUMLINALB.ToString().PadLeft(3, '0')
                    + riga.APPLICACION.PadRight(1, ' ')
                    + riga.INVENTARIO.PadRight(1, ' ');
                sMessaggio += System.Environment.NewLine;
            }

            //*******************************************************
            //SCRIVO ETQPACK
            //*******************************************************
            using (System.IO.FileStream fs = System.IO.File.Create(strNomeFile))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(sMessaggio);
                fs.Write(info, 0, info.Length);
            }

            var rispostaOK = new { status = "200", data = "Spedizione effettuata." };
            testata.STATO = "CHIUSO";
            db.Entry(testata).State = EntityState.Modified;
            db.SaveChanges();
            return Json(rispostaOK);
            }
            catch( Exception ex)
            {
                var Errore = new { status = "500", data = ex.Message };
                Loggamelo("NewDESADV",ex.Message.ToString(),"ERRORE");
                if (ex.InnerException != null)
                {
                    Loggamelo("NewDESADV", ex.InnerException.Message.ToString(), "ERRORE");
                }

                return Json(Errore);
            }

        }
    }
}