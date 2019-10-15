using ExtranetMVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExtranetMVC.CustomAuthentication;
using System.Web.Security;
using System.Data.Entity;
using Extranet_EF;
using System.Web.Hosting;
using System.Text.RegularExpressions;
using System.Net.Http;

namespace ExtranetMVC.Controllers
{
    [CustomAuthorize(Roles = "Admin,Gruppo_EDP")]
    public class FileController : Controller
    {
        public int? id;
        string ftpRoot = "C:\\inetpub\\wwwroot\\FTP";
        [HttpGet]
        public ActionResult bubu()
        {
            return View();
        }
        // GET: File
        [HttpGet]
        public ActionResult Documenti()
        {
            return View();
        }
        public ActionResult p1Lista()
        {
            //string sDir = "C:\\Users\\paolo.oliva\\source\\repos\\test root";
            string sDir = "C:\\inetpub\\wwwroot\\Condivisione";
            var nodes = new List<JsTreeModel>();
            nodes.Add(new JsTreeModel() { id = "1", parent = "#", text = "root" });
            var data = dirSearch(sDir, 1, nodes);
            //System.Diagnostics.Debugger.Break();
            return Json(data, JsonRequestBehavior.AllowGet);
            //  return Json(nodes, JsonRequestBehavior.AllowGet);
        }

        public object dirSearch(string sDir, int? idParent, List<JsTreeModel> nodes)
        {
            if (sDir.ToString() == null)
            {
                // sDir = "C:\\Users\\paolo.oliva\\source\\repos\\test root";
            }
            if (id.ToString() == "")
            {
                id = 1;
            }
            if (idParent.ToString() == "")
            {
                idParent = 0;
            }
            if (nodes.Count() == 0)
            {
                nodes = new List<JsTreeModel>();
            }
            try
            {
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    Boolean bCheck = true;
                    string oldFolder = d;
                    DirectoryInfo nomeFolder = new DirectoryInfo(d);
                    if (idParent == 1)
                    {
                        bCheck = false;
                        //var user = (CustomMembershipUser)Membership.GetUser(LoginView.UserName, false);
                        var user = User.Identity.Name;
                        using (ExtranetDB dbContext = new ExtranetDB())
                        {
                            var selected = (from us in dbContext.Users.Include("Roles")
                                            where string.Compare(us.Username, user, StringComparison.OrdinalIgnoreCase) == 0
                                            select us).FirstOrDefault();
                            var yoghi = (from us in dbContext.Shares.Include("Roles") select us).FirstOrDefault();

                            var bubu = (from us in dbContext.Roles.Include("Shares") select us).FirstOrDefault();


                            var user2 = dbContext.Users.Include(us => us.Roles).Where(sh => sh.Username == user)
                                .FirstOrDefault();
                            //.FirstOrDefault();
                            foreach (var ruolo in user2.Roles)
                            {
                                var condivisione = dbContext.Roles
                                     .Include(sh => sh.Shares)
                                     .Where(sh => sh.RoleId == ruolo.RoleId)
                                     .FirstOrDefault();
                                //Console.WriteLine(condivisione.ToString());
                                foreach (var r in condivisione.Shares.Select(s => s.SharePath))
                                {
                                    if (r == nomeFolder.Name)
                                    {
                                        bCheck = true;
                                        break;
                                    }
                                }
                                //if(condivisione.Shares.Select( s => s.SharePath ).ToString() ==nomeFolder.Name)
                                //{
                                //    bCheck = true;
                                //}
                                //var condivisione2 = condivisione.Shares.Select(s => s.SharePath).Take(1);
                                if (bCheck == true) break;
                            }
                            // var test = User.IsInRole("India"); //  CustomRole.isShareForRoles(Membership.GetUser().ToString() , nomeFolder.ToString()).ToString();
                        }
                    }
                    int? oldId = idParent;
                    if (bCheck == true)
                    {
                        id++;
                        //if (idParent > 0) { idParent--; }
                        nodes.Add(new JsTreeModel() { id = id.ToString(), parent = idParent.ToString(), text = nomeFolder.Name });
                        //int? oldId = idParent;
                        idParent = id;
                        //idParent++;
                        foreach (var f in nomeFolder.GetFiles())
                        {
                            string sIcon = "jstree-file";
                            id++;
                            nodes.Add(new JsTreeModel() { id = id.ToString(), parent = idParent.ToString(), text = f.Name, icon = sIcon });
                        }
                    }
                    //}
                    dirSearch(d, idParent, nodes);
                    idParent = oldId;
                    //idParent-- ;
                    //id=oldId;
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
            //return Json(nodes, JsonRequestBehavior.AllowGet);
            return nodes;
        }
        //public ActionResult Download(string id)
        //{
        //    try
        //    {
        //        string filePath = id;
        //        filePath = filePath.Substring(5);
        //        filePath = filePath.Replace('/', '\\');
        //        // System.Diagnostics.Debugger.Break();
        //        //byte[] fileBytes = System.IO.File.ReadAllBytes(@"c:\folder\myfile.ext");
        //        byte[] fileBytes = System.IO.File.ReadAllBytes(@"C:\\inetpub\\wwwroot\\Condivisione\\" + filePath);
        //        string fileName = filePath.Substring(filePath.LastIndexOf('\\') + 1);
        //        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        //    }
        //    catch (System.Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        return View();
        //    }
        //}
        public ActionResult FTPmain()
        {
            using (ExtranetDB dbContext = new ExtranetDB())
            {
                FTPabilitazioni utente = dbContext.FTPabilitazioni.Where(e => e.ftpUser == User.Identity.Name).FirstOrDefault();
                //ICollection<UserShares> test = dbContext.UserShares.Where(e => e.username == utente.ftpUser).ToList();
                //ICollection<UserShares> test2 = dbContext.UserShares.Where(e => e.username == User.Identity.Name).ToList();
                var bubu = utente.UserShares;
                var tt = dbContext.Shares.Where(e => e.ShareID == 4).Select(e => e.UserShares).ToList();
                //var bubu7 = dbContext.UserShares.Where(e => e.username == "paolo.oliva").ToList();
                var yoghi = bubu.Select(e=> e.Shares.SharePath).ToList();
            }
            return View();
            
        }
        public ActionResult FTP()
        {
            //using (ExtranetDB dbContext = new ExtranetDB())
            //{
            //    FTPabilitazioni utente = dbContext.FTPabilitazioni.Where(e => e.ftpUser == User.Identity.Name).FirstOrDefault();
            //    //ICollection<UserShares> test = dbContext.UserShares.Where(e => e.username == utente.ftpUser).ToList();
            //    //ICollection<UserShares> test2 = dbContext.UserShares.Where(e => e.username == User.Identity.Name).ToList();
            //    var bubu = utente.UserShares;
            //    var tt = dbContext.Shares.Where(e => e.ShareID == 4).Select(e => e.UserShares).ToList();
            //    //var bubu7 = dbContext.UserShares.Where(e => e.username == "paolo.oliva").ToList();
            //    var yoghi = bubu.Select(e => e.Shares.SharePath).ToList();
            //}
            return View();

        }
        [HttpPost]
        [Route("/Files/MultiUpload/")]//{username}")]
        public string MultiUpload() 
        {
            //HttpResponseMessage result = null;
            try
            {
                var httpRequest = HttpContext.Request;
                //var filecontent = httpRequest.Files[0];
                //var httpRequest = Request;
                //var filecontent = Request.Files[0];
                string filename = System.Uri.UnescapeDataString( Request.Headers["x-filename"].ToString().TrimEnd());

                var sFolder= System.Uri.UnescapeDataString (Request.Headers["PathToSave"].ToString().TrimEnd());

                string[] sDir = sFolder.Split('\\');
                Boolean bCheck = false;
                var user = User.Identity.Name;
                string sRight = "R";
                using (ExtranetDB dbContext = new ExtranetDB())
                {
                    var sPathToCheck = sDir[0];
                    UserShares abilitazioni = dbContext.UserShares.Where(e => e.username == User.Identity.Name && e.SharePath == sPathToCheck).FirstOrDefault();
                    if (abilitazioni != null)
                    {
                        sRight = abilitazioni.abilitazione;
                        bCheck = true;
                    }
                }

                if (bCheck == false || sRight == "R")
                {
                    //result = Request.CreateResponse(HttpStatusCode.Forbidden, "Forbidden");
                    return "Forbidden";
                }

                var chunks = Request.InputStream;
                
                string path = HostingEnvironment.MapPath("~/FTP/upload"); //test
                //var filePath = HostingEnvironment.MapPath("~/FTP")
               
                string nome = filename + ".tmp" + Request.Headers["completed"].ToString().TrimEnd().PadLeft(4, '0') ;

                //path = ftpRoot + '\\' + sFolder; //20190830: utilizzo cartella temp x upload
                path = ftpRoot + "\\UPLOAD";
                if (sFolder != "#" && sFolder != "FTP" && sFolder != "")
                {
                    string newpath = Path.Combine(path, nome);

                    using (System.IO.FileStream fs = System.IO.File.Create(newpath))
                    {
                        byte[] bytes = new byte[1024000];//[77570];

                        int bytesRead;
                        //var httpRequest = HttpContext.Request;
                        //var filecontent = httpRequest.Files[0];
                        while ((bytesRead = Request.InputStream.Read(bytes, 0, bytes.Length)) > 0)
                        {
                            fs.Write(bytes, 0, bytesRead);
                        }
                    }
                    return "test";
                }
                else
                {
                    return "Cannot upload in this folder";
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        [HttpPost]
        [Route("/Files/UploadComplete")]
        public string UploadComplete(string fileName, bool completed,string PathToSave)
        {
            if (completed)
            {
                var httpRequest = Request;
                var sFolder = PathToSave;
                string[] sDir = sFolder.Split('\\');

                string path = Server.MapPath("~/FTP/upload"); //test
                                                              //path = ftpRoot + '\\' + sFolder; //20190830: utilizzo cartella temp x upload
                path = ftpRoot + "\\UPLOAD";
                string newpath = Path.Combine(ftpRoot + '\\'+ sFolder, fileName);
                //FileInfo[] files = Directory.GetFiles(path).OrderBy(n => Regex.Replace(n.name, @"\d+", n => n.Value.PadLeft(4, '0')));
                string[] filePaths = Directory.GetFiles(path,fileName + ".tmp*");

                //filePaths.OrderBy(e=> e.)
                //DirectoryInfo di = new DirectoryInfo(path);
                //FileSystemInfo[] files = di.GetFileSystemInfos();
                //var orderedFiles = files.OrderBy(f => Regex.Replace(f.Name, @"\d+", n => f..PadLeft(4, '0'))).Select(f=>f.FullName).ToList();
                int cont = 0;

                if (System.IO.File.Exists(newpath))
                {
                    System.IO.File.Delete(newpath);
                }


                foreach (string item in filePaths)
                {
                    MergeFiles(newpath, item);
                }
            }
            return "success";
        }

        private static void MergeFiles(string file1, string file2)
        {
            FileStream fs1 = null;
            FileStream fs2 = null;
            try
            {
                fs1 = System.IO.File.Open(file1, FileMode.Append);
                fs2 = System.IO.File.Open(file2, FileMode.Open);
                byte[] fs2Content = new byte[fs2.Length];
                fs2.Read(fs2Content, 0, (int)fs2.Length);
                fs1.Write(fs2Content, 0, (int)fs2.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " : " + ex.StackTrace);
            }
            finally
            {
                fs1.Close();
                fs2.Close();
                System.IO.File.Delete(file2);
            }
        }

        //[Route("/Files/DownloadTest")]
        //public ActionResult DownloadTest()
        //{
        //    string file = "";
        //    try
        //    {
        //         file = System.Uri.UnescapeDataString(Request.Headers["x-filename"].ToString().TrimEnd());
        //    }
        //    catch(Exception ex)
        //    {
        //        Response.StatusCode = 400;
        //        Response.StatusDescription = "File request error";
        //        Response.End();
        //    }




        //    if(file=="")
        //    {
        //        Response.StatusCode = 400;
        //        Response.StatusDescription = "File request error";
        //        Response.End();

        //    }




        //    //var filePath = @"C:\inetpub\wwwroot\FTP\IDIADA\20190706 EPOLE ERIC GRANADO.mp4";
        //    //var filePath = ftpRoot + file;
        //    var sFile = "";
        //    sFile = file;
        //    //sFile = sFile.Substring(0, sFile.Length - 1);

        //    string filePath = ftpRoot + "\\" + sFile;
        //    //filePath = filePath.Substring(5);
        //    filePath = filePath.Replace('/', '\\');

        //    string[] sFolder = sFile.Split('/');
        //    Boolean bCheck = false;
        //    var user = User.Identity.Name;
        //    using (ExtranetDB dbContext = new ExtranetDB())
        //    {
        //        var sPathToCheck = sFolder[0];
        //        //var utente = dbContext.Users.Include("UserShares");
        //        //FTPabilitazioni[] abilitazioni = dbContext.FTPabilitazioni.Include("UserShares").Where(e => e.ftpUser == User.Identity.Name && e.UserShares).FirstOrDefault();
        //        UserShares abilitazioni = dbContext.UserShares.Where(e => e.username == User.Identity.Name && e.SharePath == sPathToCheck).FirstOrDefault();
        //        if (abilitazioni != null)
        //        {
        //            bCheck = true;
        //        }
        //    }
        //    if (bCheck == false)
        //    {
        //        //var resultB = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //        //resultB.ReasonPhrase = "Not permitted";
        //       Response.StatusCode = 400;
        //        Response.StatusDescription = "File request error";
        //        Response.End();
                

        //        //return resultB;
        //    }




        //    FileInfo OutFile=new FileInfo(filePath);

        //    //try
        //    //{
        //    //     OutFile = new FileInfo(filePath);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    Response.StatusCode = 400;
        //    //    Response.StatusDescription = "File not found";
        //    //    Response.End();
            
        //    //}


        //    Response.Clear();
        //    Response.ContentType = "application/octet-stream";
        //    Response.Buffer = false;
            
        //    Response.BufferOutput = false;
        //    //Response.AppendHeader("Content-Lenght", OutFile.Length.ToString());
        //    filePath = OutFile.FullName;
        //    string fileName = OutFile.Name;
        //    //filePath = @"C:\inetpub\wwwroot\FTP\IDIADA\20190706 EPOLE ERIC GRANADO.mp4";
        //    Response.AppendHeader("Content-Disposition", "filename=" + System.Uri.EscapeDataString(fileName));
        //    Response.TransmitFile(filePath,0,OutFile.Length);
            

        //    Response.End();
        //    //return null;
        //    //return FTP();
        //    return FTP();
        //}



        //x debug



        [Route("file/download/{*file?}")]
        public ActionResult Download(string file)
        {
            var sFile = "";
            sFile = ftpRoot + "/" + System.Uri.UnescapeDataString(file);
            //string file = @"C:\inetpub\wwwroot\FTP\IDIADA\20190706 EPOLE ERIC GRANADO.mp4"; 
            //file = ftpRoot+"/" + System.Uri.UnescapeDataString(file);
            //var filePath = @"C:\inetpub\wwwroot\FTP\IDIADA\20190706 EPOLE ERIC GRANADO.mp4";
            //var filePath = ftpRoot + file;

            if (sFile == "")
            {
                Response.StatusCode = 400;
                Response.StatusDescription = "File request error";
                Response.End();
                return FTP();
            }

            
            sFile = file;
            //sFile = sFile.Substring(0, sFile.Length - 1);

            string filePath =  sFile;
            //filePath = filePath.Substring(5);
            filePath = filePath.Replace('/', '\\');
            sFile=sFile.Replace('/', '\\');
            string[] sFolder = sFile.Split('\\');

            
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
                Response.StatusCode = 400;
                Response.StatusDescription = "File request error";
                Response.End();

                return FTP();
                //return resultB;
            }

            filePath = ftpRoot + "\\" + filePath;

            FileInfo OutFile = new FileInfo(filePath);

  


            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.Buffer = false;

            Response.BufferOutput = false;
            //Response.AppendHeader("Content-Lenght", OutFile.Length.ToString());
            filePath = OutFile.FullName;
            string fileName = OutFile.Name;
            //filePath = @"C:\inetpub\wwwroot\FTP\IDIADA\20190706 EPOLE ERIC GRANADO.mp4";
            Response.AppendHeader("Content-Disposition", "filename=" + System.Uri.EscapeDataString(fileName));
            Response.TransmitFile(filePath, 0, OutFile.Length);


            Response.End();
            //return null;
            //return FTP();
            return FTP();
        }





    }
}