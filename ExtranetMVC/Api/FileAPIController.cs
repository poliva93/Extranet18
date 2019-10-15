using AutoMapper;
using Extranet_EF;
using ExtranetMVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using System.Data.Entity;
using System.Web.Http;
using ExtranetMVC.CustomAuthentication;
using System.Net.Http.Headers;
using System.Web.Hosting;
using System.IO;
using Ionic.Zip;
using Newtonsoft;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ExtranetMVC.Api
{
    [CustomAuthorize(Roles = "Admin,Gruppo_EDP")]
    public class FileAPIcontroller : ApiController
    {

        string ftpRoot = "C:\\inetpub\\wwwroot\\FTP";

        private ExtranetDB db = new ExtranetDB();

        [HttpGet]
        [Route("api/file/load/{*folder?}")]//{username}")]
        public IHttpActionResult Load(string folder = "")

        {
            FTPabilitazioni utente = db.FTPabilitazioni.Where(e => e.ftpUser == User.Identity.Name).FirstOrDefault();
            Boolean bCheck = false;
            if (utente == null)
            {
                return Content(HttpStatusCode.NoContent, "Abilitazione non trovata");
            }
            var nodes = new List<FtpModel>();
            string sDir = "C:\\inetpub\\wwwroot\\FTP";
            if (folder == "")
            {
                //FTPabilitazioni utente = db.FTPabilitazioni.Where(e => e.ftpUser == User.Identity.Name).FirstOrDefault();

                nodes.Add(new FtpModel() { FileName = "FTP", Folder = "#", Type = "Folder"  , ReadWrite="R" });
                var data = DirSearch(sDir, nodes);
            }
            else
            {
                folder = folder.Replace("#/", "");
                folder = folder.Replace('/', Path.DirectorySeparatorChar);
                string cartella = (sDir + "\\" + folder);
                string cartellaAttuale = cartella.Substring(sDir.Length + 1);
                string[] percorso = cartellaAttuale.Split(Path.DirectorySeparatorChar);
                string sRight;
                using (ExtranetDB dbContext = new ExtranetDB())
                {
                    string toCheck = percorso[0];
                    UserShares abilitazioni = dbContext.UserShares.Where(e => e.username == User.Identity.Name && e.SharePath == toCheck).FirstOrDefault();
                    if (abilitazioni != null) bCheck = true;
                    sRight = abilitazioni.abilitazione;
                }
                if (bCheck == false)
                {
                    return Content(HttpStatusCode.NoContent, "404");
                }
                DirectoryInfo nomeFolder = new DirectoryInfo(cartella);
                nodes.Add(new FtpModel() { FileName = nomeFolder.Name,  Folder = folder, Type = "Folder", Size = GetDirectorySize(nomeFolder.FullName).ToString(), ReadWrite=sRight });
                var data = DirSearch(cartella, nodes);
            }
            return Ok(nodes);//, System.Web.Mvc.Jso.nRequestBehavior.AllowGet);
        }

        [HttpGet]
        public object DirSearch(string sDir, List<FtpModel> nodes)
        {
            string ftpRoot = "C:\\inetpub\\wwwroot\\FTP";
            string cartellaAttuale = "";
            if (ftpRoot != sDir)
            {
                cartellaAttuale = sDir.Substring(ftpRoot.Length + 1);
            }

            try
            {
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    Boolean bCheck = true;

                    //string oldFolder = d;

                    DirectoryInfo nomeFolder = new DirectoryInfo(d);
                    if (sDir == ftpRoot)
                    {
                        bCheck = false;
                        //var user = (CustomMembershipUser)Membership.GetUser(LoginView.UserName, false);
                        var user = User.Identity.Name;
                        using (ExtranetDB dbContext = new ExtranetDB())
                        {
                            //var utente = dbContext.Users.Include("UserShares");
                            //FTPabilitazioni[] abilitazioni = dbContext.FTPabilitazioni.Include("UserShares").Where(e => e.ftpUser == User.Identity.Name && e.UserShares).FirstOrDefault();
                            UserShares abilitazioni = dbContext.UserShares.Where(e => e.username == User.Identity.Name && e.SharePath == nomeFolder.Name).FirstOrDefault();
                            if (abilitazioni != null)
                            {
                                bCheck = true;
                            }
                        }
                        if (bCheck == true)
                        {
                            nodes.Add(new FtpModel() { FileName = nomeFolder.Name, Folder = "#" /*nomeFolder.Parent.Name*/, Type = "Folder", LastEdit = nomeFolder.LastWriteTimeUtc.ToLocalTime().ToString(), Size = GetDirectorySize(nomeFolder.FullName).ToString() + "Kb" });

                        }
                    }
                    else
                    {
                        nodes.Add(new FtpModel() { FileName = nomeFolder.Name, Folder = cartellaAttuale, Type = "Folder", LastEdit = nomeFolder.LastWriteTimeUtc.ToLocalTime().ToString(), Size = GetDirectorySize(nomeFolder.FullName).ToString() + "Kb" });


                    }
                }
                if (cartellaAttuale != "")
                {
                    DirectoryInfo nomeFolder2 = new DirectoryInfo(sDir);
                    //var cartella = sDir.Substring(ftpRoot.Length+1);
                    foreach (var f in nomeFolder2.GetFiles())
                    {

                        nodes.Add(new FtpModel() { FileName = f.Name, Folder = cartellaAttuale, Type = f.Extension, LastEdit = f.LastWriteTimeUtc.ToLocalTime().ToString(), Size = f.Length.ToString() + "Kb" });
                    }
                }
                return Ok("");
            }



            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.InnerException);
                return InternalServerError(excpt.InnerException);
            }
        }
        [HttpGet]
        private static long GetDirectorySize(string folderPath)
        {
            DirectoryInfo di = new DirectoryInfo(folderPath);
            return di.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);
        }

        [HttpGet]
        [Route("api/file/download/{sTipo}/{*sFile?}")]//{username}")]
        public HttpResponseMessage Download(string sTipo, string sFile)

        {
            try
            {
                sFile = sFile.Substring(0, sFile.Length - 1);
                string filePath = ftpRoot + "\\" + sFile + "." + sTipo;
                //filePath = filePath.Substring(5);
                filePath = filePath.Replace('/', '\\');
                
                string[] sFolder = sFile.Split('/');
                Boolean bCheck = false;
                var user = User.Identity.Name;
                using (ExtranetDB dbContext = new ExtranetDB())
                {
                    var sPathToCheck = sFolder[0];
                    //var utente = dbContext.Users.Include("UserShares");
                    //FTPabilitazioni[] abilitazioni = dbContext.FTPabilitazioni.Include("UserShares").Where(e => e.ftpUser == User.Identity.Name && e.UserShares).FirstOrDefault();
                    UserShares abilitazioni = dbContext.UserShares.Where(e => e.username == User.Identity.Name && e.SharePath == sPathToCheck).FirstOrDefault();
                    if (abilitazioni != null)
                    {
                        bCheck = true;
                    }
                }
                if (bCheck == false)
                {
                    //var resultB = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    //resultB.ReasonPhrase = "Not permitted";
                    var resultB = Request.CreateResponse(HttpStatusCode.BadRequest, "Not allowed");

                    return resultB;
                }
                var dataBytes = File.ReadAllBytes(filePath);
                var sName = filePath.Split('\\').Last();
                HttpContext.Current.Response.BufferOutput = false;
                //adding bytes to memory stream   
                var dataStream = new MemoryStream(dataBytes);
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    //Content = new ByteArrayContent(dataStream.ToArray())
                    Content = new ByteArrayContent(dataStream.ToArray())
                };
                
                //.BufferOutput = false;
                
                result.Content.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = sName
                };
                result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");
                

                return result;
                // System.Diagnostics.Debugger.Break();
                //byte[] fileBytes = System.IO.File.ReadAllBytes(@"c:\folder\myfile.ext");
                //byte[] fileBytes = System.IO.File.ReadAllBytes(@"C:\\inetpub\\wwwroot\\Condivisione\\" + filePath);
                //string fileName = filePath.Substring(filePath.LastIndexOf('\\') + 1);
                //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                //var result = new HttpResponseMessage(HttpStatusCode.BadRequest);
                //result.ReasonPhrase = e.Message;
                var result = Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
                return result;

            }
        }

        //DOWNLOAD MULTIPLO
        
        [Route("api/file/downloadM/")]//{username}")]
        [HttpPost]
        public HttpResponseMessage DownloadM(DownloadUrl t)
        {
            if( t==null )
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
           
            HttpResponseMessage result = null;
            
            try
            {
                var httpRequest = HttpContext.Current.Request;

                Boolean bCheck = false;
                var user = User.Identity.Name;
                string sRight = "R";




                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                    zip.AddDirectoryByName("Files");


                    foreach (var percorso in t.Urls)
                    {
                        bCheck = false;

                        //var user = User.Identity.Name;
                        using (ExtranetDB dbContext = new ExtranetDB())
                        {
                            var sPath = percorso.Split('/');
                            var sPathToCheck = sPath[0];
                            //var utente = dbContext.Users.Include("UserShares");
                            //FTPabilitazioni[] abilitazioni = dbContext.FTPabilitazioni.Include("UserShares").Where(e => e.ftpUser == User.Identity.Name && e.UserShares).FirstOrDefault();
                            UserShares abilitazioni = dbContext.UserShares.Where(e => e.username == User.Identity.Name && e.SharePath == sPathToCheck).FirstOrDefault();
                            if (abilitazioni != null)
                            {
                                bCheck = true;
                            }
                        }
                        if (bCheck == false)
                        {
                            var resultB = new HttpResponseMessage(HttpStatusCode.BadRequest);
                            return resultB;
                        }

                        var sPercorso= ftpRoot + "\\" + percorso;
                        sPercorso = sPercorso.Replace("/", "\\");
                        zip.AddFile(sPercorso.ToString(), "Files");

                    }
                    return ZipContentResult(zip);
                   

                }
            
            }

            catch (Exception e)
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
            return result;
        }

        protected HttpResponseMessage ZipContentResult(ZipFile zipFile)
        {
            Boolean bCorretto = true;
                var pushStreamContent = new PushStreamContent((stream, content, context) =>
                {
                    try { 
                    zipFile.Save(stream);
                    stream.Close();
                    }
                    catch (Exception e)
                    {
                        bCorretto = false;
                    }
                }, "application/zip");
            if (bCorretto==false) 
                    {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
                var esito = new HttpResponseMessage(HttpStatusCode.OK);
                esito.Content = pushStreamContent;
                // esito.Headers.Add("Content-Disposition", "attachment; filename=" + zipFile.Name);
                esito.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "DO_Files_" + DateTime.Now.ToShortDateString()
                };

                return esito;
            
        }

        [HttpPost]
        [Route("api/file/upload/")]//{username}")]
        public HttpResponseMessage Upload()//non più usata, ora è in FileController
        {

            HttpResponseMessage result = null;
            try
            {
                var httpRequest = HttpContext.Current.Request;
                var filecontent = httpRequest.Files[0];
                var sFolder = httpRequest["PathToSave"].ToString().TrimEnd();

                string[] sDir = sFolder.Split('\\');
                Boolean bCheck = false;
                var user = User.Identity.Name;
                string sRight="R";
                using (ExtranetDB dbContext = new ExtranetDB())
                {
                    var sPathToCheck = sDir[0];
                    //var utente = dbContext.Users.Include("UserShares");
                    //FTPabilitazioni[] abilitazioni = dbContext.FTPabilitazioni.Include("UserShares").Where(e => e.ftpUser == User.Identity.Name && e.UserShares).FirstOrDefault();
                    UserShares abilitazioni = dbContext.UserShares.Where(e => e.username == User.Identity.Name && e.SharePath == sPathToCheck).FirstOrDefault();
                    if (abilitazioni != null)
                    {
                        sRight = abilitazioni.abilitazione;
                        bCheck = true;
                    }
                }

                if (bCheck == false || sRight=="R")
                {
                    result = Request.CreateResponse(HttpStatusCode.Forbidden,"Forbidden");
                    return result;
                }

                // var a=httpRequest.Files["uFile"];
                if (httpRequest.Files.Count > 0)
                {


                    if (sFolder != "#" && sFolder != "FTP" && sFolder != "")
                    {
                        var docfiles = new List<string>();
                        foreach (string file in httpRequest.Files)
                        {
                            var postedFile = httpRequest.Files[file]; //FTPROOT
                            var filePath = HostingEnvironment.MapPath("~/FTP")  + '\\' + sFolder + '\\' + postedFile.FileName;
                            postedFile.SaveAs(filePath);
                            docfiles.Add(filePath);
                        }
                        result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
                    }
                    else
                    {
                        result = Request.CreateResponse(HttpStatusCode.BadRequest, "Cannot upload in this folder");
                        return result;
                    }

                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.BadRequest, "File non allegato");
                }
                return result;
            }

            catch (Exception e)
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
            return result;
        }
        
    }
    }
