// Filemanager ASP.NET MVC connector
// Author: David Hammond <dave@modernsignal.com>
// Based on ASHX connection by Ondřej "Yumi Yoshimido" Brožek | <cholera@hzspraha.cz>

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using Arquitetura.Dominio.ControladorDeSessao;

namespace MyProject.Areas.FilemanagerArea.Controllers
{
    /// <summary>
    /// Filemanager controller
    /// </summary>
    public class FilemanagerController : Controller
    {
        /// <summary>
        /// Root directory for all file uploads [string]
        /// Set in web.config. E.g. <add key="Filemanager_RootPath" value="/uploads/"/>
        /// </summary>
        private string RootPath;// = WebConfigurationManager.AppSettings["Filemanager_RootPath"]; // Root directory for all file uploads [string]

        /// <summary>
        /// Directory for icons. [string]
        /// Set in web.config E.g. <add key="Filemanager_IconDirectory" value="/Scripts/filemanager/images/fileicons/"/>
        /// </summary>
        private string IconDirectory = "/filemanager/images/fileicons/";//WebConfigurationManager.AppSettings["Filemanager_IconDirectory"]; // Icon directory for filemanager. [string]

        /// <summary>
        /// White list of allowed file extensions
        /// </summary>
        private List<string> allowedExtensions = new List<string> { ".txt", ".log", ".conf", ".xls", ".xlsx", ".csv" }; // Only allow these extensions to be uploaded

        /// <summary>
        /// List of image file extensions
        /// </summary>
        private List<string> imgExtensions = new List<string> {  }; // Only allow this image extensions. [string]

        /// <summary>
        /// Serializer for generating json responses
        /// </summary>
        private JavaScriptSerializer json = new JavaScriptSerializer();

        public FilemanagerController()
        {
            RootPath = ObtenhaPastaRaiz();
        }

        /// <summary>
        /// Process file manager action
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index(string mode, string path = null)
        {
            Response.ClearHeaders();
            Response.ClearContent();
            Response.Clear();

            try
            {
                switch (mode)
                {
                    case "getinfo":
                        return Content(GetInfo(path), "application/json", Encoding.UTF8);
                    case "getfolder":
                        return Content(GetFolderInfo(path), "application/json", Encoding.UTF8);
                    case "move":
                        var oldPath = Request.QueryString["old"];
                        var newPath = string.Format("{0}{1}/{2}", Request.QueryString["root"], Request.QueryString["new"], Path.GetFileName(oldPath));
                        return Content(Move(oldPath, newPath), "application/json", Encoding.UTF8);
                    case "rename":
                        return Content(Rename(Request.QueryString["old"], Request.QueryString["new"]), "application/json", Encoding.UTF8);
                    case "replace":
                        return Content(Replace(Request.Form["newfilepath"]), "text/html", Encoding.UTF8);
                    case "delete":
                        return Content(Delete(path), "application/json", Encoding.UTF8);
                    case "addfolder":
                        return Content(AddFolder(path, Request.QueryString["name"]), "application/json", Encoding.UTF8);
                    case "download":
                        if (System.IO.File.Exists(ObtenhaCaminho(path)) && IsInRootPath(path))
                        {
                            FileInfo fi = new FileInfo(ObtenhaCaminho(path));
                            Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlPathEncode(path));
                            Response.AddHeader("Content-Length", fi.Length.ToString());
                            return File(fi.FullName, "application/octet-stream");
                        }
                        else
                        {
                            return new HttpNotFoundResult("Arquivo não encontrado.");
                        }
                    case "add":
                        return Content(AddFile(Request.Form["currentpath"]), "text/html", Encoding.UTF8);
                    case "preview":
                        var fi2 = new FileInfo(Server.MapPath(Request.QueryString["path"]));
                        return new FilePathResult(fi2.FullName, "image/" + fi2.Extension.TrimStart('.'));
                    default:
                        return Content("");
                }
            }
            catch (HttpException he)
            {
                return Content(Error(he.Message), "application/json", Encoding.UTF8);
            }
        }

        [Authorize]
        public ActionResult Arquivo(string name)
        {
            var path = ObtenhaCaminho(name);
            if (path.Contains("?"))
                path = path.Substring(0, path.IndexOf('?'));

            return File(path, "image/png");
        }
            

        //===================================================================
        //========================== END EDIT ===============================
        //===================================================================       

        /// <summary>
        /// Is the file an image file
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        private bool IsImage(FileInfo fileInfo)
        {
            return imgExtensions.Contains(Path.GetExtension(fileInfo.FullName).ToLower());
        }

        /// <summary>
        /// Is the file in the root path?  Don't allow uploads outside the root path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool IsInRootPath(string path)
        {
            return path != null && Path.GetFullPath(ObtenhaCaminho(path)).StartsWith(Path.GetFullPath(RootPath));
        }

        /// <summary>
        /// Add a file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string AddFile(string path)
        {
            string response;

            if (path.Where(c => c == '/').Count() <= 1)
            {
                response = Error("Sem permissão para gravar nesta pasta.");
            }
            else if (Request.Files.Count == 0 || Request.Files[0].ContentLength == 0)
            {
                response = Error("Nehum arquivo informado.");
            }
            else
            {
                if (!IsInRootPath(path))
                {
                    response = Error("Tentativa de adicionar arquivo fora da pasta raiz.");
                }
                else
                {
                    System.Web.HttpPostedFileBase file = Request.Files[0];
                    //if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                    //{
                    //    response = Error("Tipo de arquivo não permitido.");
                    //}
                    //else
                    {
                        //Only allow certain characters in file names
                        var baseFileName = Regex.Replace(Path.GetFileNameWithoutExtension(file.FileName), @"[^\w_-]", "");
                        var filePath = Path.Combine(path, baseFileName + Path.GetExtension(file.FileName));

                        //Make file name unique
                        var i = 0;
                        while (System.IO.File.Exists(ObtenhaCaminho(filePath)))
                        {
                            i = i + 1;
                            baseFileName = Regex.Replace(baseFileName, @"_[\d]+$", "");
                            filePath = Path.Combine(path, baseFileName + "_" + i + Path.GetExtension(file.FileName));
                        }
                        file.SaveAs(ObtenhaCaminho(filePath));

                        response = json.Serialize(new
                        {
                            Path = path,
                            Name = Path.GetFileName(file.FileName),
                            Error = "No error",
                            Code = 0
                        });
                    }
                }
            }
            return "<textarea>" + response + "</textarea>";
        }

        /// <summary>
        /// Add a folder
        /// </summary>
        /// <param name="path"></param>
        /// <param name="newFolder"></param>
        /// <returns></returns>
        private string AddFolder(string path, string newFolder)
        {
            if (!IsInRootPath(path))
            {
                return Error("Tentativa de adicionar pasta fora da pasta raiz.");
            }

            if (path == "/")
                return Error("Usuário sem permissão.");

            StringBuilder sb = new StringBuilder();
            Directory.CreateDirectory(Path.Combine(ObtenhaCaminho(path), newFolder));

            sb.AppendLine("{");
            sb.AppendLine("\"Parent\": \"" + path + "\",");
            sb.AppendLine("\"Name\": \"" + newFolder + "\",");
            sb.AppendLine("\"Error\": \"No error\",");
            sb.AppendLine("\"Code\": 0");
            sb.AppendLine("}");

            return sb.ToString();
        }

        /// <summary>
        /// Delete a file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string Delete(string path)
        {
            if (!IsInRootPath(path))
            {
                return Error("Tentativa de deletar arquivo fora da raiz da pasta.");
            }
            if (!System.IO.File.Exists(ObtenhaCaminho(path)) && !Directory.Exists(ObtenhaCaminho(path)))
            {
                return Error("Arquivo não encontrado.");
            }

            if (!TemPermissao(path))
                return Error("Usuário sem permissão.");

            FileAttributes attr = System.IO.File.GetAttributes(ObtenhaCaminho(path));

            StringBuilder sb = new StringBuilder();

            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                Directory.Delete(ObtenhaCaminho(path), true);
            }
            else
            {
                System.IO.File.Delete(ObtenhaCaminho(path));
            }

            sb.AppendLine("{");
            sb.AppendLine("\"Error\": \"No error\",");
            sb.AppendLine("\"Code\": 0,");
            sb.AppendLine("\"Path\": \"" + path + "\"");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private bool TemPermissao(string path)
        {
            FileAttributes attr = System.IO.File.GetAttributes(ObtenhaCaminho(path));
            
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                int cont = path.Count(x => x == '/');
                if (cont <= 2)
                    return false;

                return true;
            }

            return true;
        }

        /// <summary>
        /// Generate json for error message
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private string Error(string msg)
        {
            return json.Serialize(new
            {
                Error = msg,
                Code = -1
            });
        }

        /// <summary>
        /// Get folder information
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetFolderInfo(string path)
        {
            if (!IsInRootPath(path))
            {
                return Error("Tentativa de visualizar arquivo fora da pasta raiz.");
            }
            if (!Directory.Exists(ObtenhaCaminho(path)))
            {
                return Error("Diretório não encontrado.");
            }

            DirectoryInfo RootDirInfo = new DirectoryInfo(ObtenhaCaminho(path));
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("{");

            int i = 0;

            foreach (DirectoryInfo DirInfo in RootDirInfo.GetDirectories())
            {
                if (i > 0)
                {
                    sb.Append(",");
                    sb.AppendLine();
                }

                sb.AppendLine("\"" + Path.Combine(path, DirInfo.Name) + "\": {");
                sb.AppendLine("\"Path\": \"" + Path.Combine(path, DirInfo.Name) + "/\",");
                sb.AppendLine("\"ContentPath\": \"" + Url.Action("file", new { name = Path.Combine(path, DirInfo.Name) }) + "/\",");
                sb.AppendLine("\"Filename\": \"" + DirInfo.Name + "\",");
                sb.AppendLine("\"File Type\": \"dir\",");
                sb.AppendLine("\"Preview\": \"" + IconDirectory + "_Open.png\",");
                sb.AppendLine("\"Properties\": {");
                sb.AppendLine("\"Date Created\": \"" + DirInfo.CreationTime.ToString() + "\", ");
                sb.AppendLine("\"Date Modified\": \"" + DirInfo.LastWriteTime.ToString() + "\", ");
                sb.AppendLine("\"Height\": 0,");
                sb.AppendLine("\"Width\": 0,");
                sb.AppendLine("\"Size\": 0 ");
                sb.AppendLine("},");
                sb.AppendLine("\"Error\": \"\",");
                sb.AppendLine("\"Code\": 0	");
                sb.Append("}");

                i++;
            }

            foreach (FileInfo fileInfo in RootDirInfo.GetFiles())
            {
                if (i > 0)
                {
                    sb.Append(",");
                    sb.AppendLine();
                }

                sb.AppendLine("\"" + Path.Combine(path, fileInfo.Name) + "\": {");
                sb.AppendLine("\"Path\": \"" + Path.Combine(path, fileInfo.Name) + "\",");
                sb.AppendLine("\"Filename\": \"" + fileInfo.Name + "\",");
                sb.AppendLine("\"File Type\": \"" + fileInfo.Extension.Replace(".", "") + "\",");

                if (IsImage(fileInfo))
                {
                    sb.AppendLine("\"Preview\": \"" + "/Filemanager/Arquivo?name=" + System.Web.HttpUtility.UrlEncode(Path.Combine(path, fileInfo.Name)) + "?" + fileInfo.LastWriteTime.Ticks.ToString() + "\",");
                }
                else
                {
                    var icon = String.Format("{0}{1}.png", IconDirectory, fileInfo.Extension.Replace(".", ""));
                    if (!System.IO.File.Exists(ObtenhaCaminho(icon)))
                    {
                        icon = String.Format("{0}default.png", IconDirectory);
                    }
                    sb.AppendLine("\"Preview\": \"" + icon + "\",");
                }

                sb.AppendLine("\"Properties\": {");
                sb.AppendLine("\"Date Created\": \"" + fileInfo.CreationTime.ToString() + "\", ");
                sb.AppendLine("\"Date Modified\": \"" + fileInfo.LastWriteTime.ToString() + "\", ");

                if (IsImage(fileInfo))
                {
                    using (System.Drawing.Image img = System.Drawing.Image.FromFile(fileInfo.FullName))
                    {
                        sb.AppendLine("\"Height\": " + img.Height.ToString() + ",");
                        sb.AppendLine("\"Width\": " + img.Width.ToString() + ",");
                    }
                }

                sb.AppendLine("\"Size\": " + fileInfo.Length.ToString() + " ");
                sb.AppendLine("},");
                sb.AppendLine("\"Error\": \"\",");
                sb.AppendLine("\"Code\": 0	");
                sb.Append("}");

                i++;
            }

            sb.AppendLine();
            sb.AppendLine("}");

            return sb.ToString();
        }

        /// <summary>
        /// Get file information
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetInfo(string path)
        {
            if (!IsInRootPath(path))
            {
                return Error("Tentativa de visualizar arquivo fora da pasta raiz.");
            }
            if (!System.IO.File.Exists(ObtenhaCaminho(path)) && !Directory.Exists(ObtenhaCaminho(path)))
            {
                return Error("Arquivo não encontrado.");
            }

            StringBuilder sb = new StringBuilder();

            FileAttributes attr = System.IO.File.GetAttributes(ObtenhaCaminho(path));

            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                DirectoryInfo DirInfo = new DirectoryInfo(ObtenhaCaminho(path));

                sb.AppendLine("{");
                sb.AppendLine("\"Path\": \"" + path + "\",");
                sb.AppendLine("\"Filename\": \"" + DirInfo.Name + "\",");
                sb.AppendLine("\"File Type\": \"dir\",");
                sb.AppendLine("\"Preview\": \"" + IconDirectory + "_Open.png\",");
                sb.AppendLine("\"Properties\": {");
                sb.AppendLine("\"Date Created\": \"" + DirInfo.CreationTime.ToString() + "\", ");
                sb.AppendLine("\"Date Modified\": \"" + DirInfo.LastWriteTime.ToString() + "\", ");
                sb.AppendLine("\"Height\": 0,");
                sb.AppendLine("\"Width\": 0,");
                sb.AppendLine("\"Size\": 0 ");
                sb.AppendLine("},");
                sb.AppendLine("\"Error\": \"\",");
                sb.AppendLine("\"Code\": 0	");
                sb.AppendLine("}");
            }
            else
            {
                FileInfo fileInfo = new FileInfo(ObtenhaCaminho(path));

                sb.AppendLine("{");
                sb.AppendLine("\"Path\": \"" + path + "\",");
                sb.AppendLine("\"Filename\": \"" + fileInfo.Name + "\",");
                sb.AppendLine("\"File Type\": \"" + fileInfo.Extension.Replace(".", "") + "\",");

                if (IsImage(fileInfo))
                {
                    sb.AppendLine("\"Preview\": \"" + "/Filemanager/arquivo?name=" + System.Web.HttpUtility.UrlEncode(path) + "?" + fileInfo.LastWriteTime.Ticks.ToString() + "\",");
                }
                else
                {
                    sb.AppendLine("\"Preview\": \"" + String.Format("{0}{1}.png", IconDirectory, fileInfo.Extension.Replace(".", "")) + "\",");
                }

                sb.AppendLine("\"Properties\": {");
                sb.AppendLine("\"Date Created\": \"" + fileInfo.CreationTime.ToString() + "\", ");
                sb.AppendLine("\"Date Modified\": \"" + fileInfo.LastWriteTime.ToString() + "\", ");

                if (IsImage(fileInfo))
                {
                    using (System.Drawing.Image img = System.Drawing.Image.FromFile(ObtenhaCaminho(path)))
                    {
                        sb.AppendLine("\"Height\": " + img.Height.ToString() + ",");
                        sb.AppendLine("\"Width\": " + img.Width.ToString() + ",");
                    }
                }

                sb.AppendLine("\"Size\": " + fileInfo.Length.ToString() + " ");
                sb.AppendLine("},");
                sb.AppendLine("\"Error\": \"\",");
                sb.AppendLine("\"Code\": 0	");
                sb.AppendLine("}");
            }

            return sb.ToString();

        }


        private string Move(string oldPath, string newPath)
        {
            if (!IsInRootPath(oldPath))
            {
                return Error("Tentativa de modificar arquivo fora da pasta raiz.");
            }
            else if (!IsInRootPath(newPath))
            {
                return Error("Tentativa de mover arquivo fora da pasta raiz.");
            }
            else if (!System.IO.File.Exists(ObtenhaCaminho(oldPath)) && !Directory.Exists(ObtenhaCaminho(oldPath)))
            {
                return Error("Arquivo não encontrado.");
            }

            if (!TemPermissao(oldPath))
                return Error("Usuário sem permissão.");

            FileAttributes attr = System.IO.File.GetAttributes(ObtenhaCaminho(oldPath));

            StringBuilder sb = new StringBuilder();

            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                DirectoryInfo oldDir = new DirectoryInfo(ObtenhaCaminho(oldPath));
                newPath = Path.Combine(newPath, oldDir.Name);
                Directory.Move(ObtenhaCaminho(oldPath), ObtenhaCaminho(newPath));
                DirectoryInfo newDir = new DirectoryInfo(ObtenhaCaminho(newPath));

                sb.AppendLine("{");
                sb.AppendLine("\"Error\": \"No error\",");
                sb.AppendLine("\"Code\": 0,");
                sb.AppendLine("\"Old Path\": \"" + oldPath + "\",");
                sb.AppendLine("\"Old Name\": \"" + oldDir.Name + "\",");
                sb.AppendLine("\"New Path\": \"" + newDir.FullName.Replace(HttpRuntime.AppDomainAppPath, "/").Replace(Path.DirectorySeparatorChar, '/') + "\",");
                sb.AppendLine("\"New Name\": \"" + newDir.Name + "\"");
                sb.AppendLine("}");
            }
            else
            {
                FileInfo oldFile = new FileInfo(ObtenhaCaminho(oldPath));
                FileInfo newFile = new FileInfo(ObtenhaCaminho(newPath));
                if (newFile.Extension != oldFile.Extension)
                {
                    //Don't allow extension to be changed
                    newFile = new FileInfo(Path.ChangeExtension(newFile.FullName, oldFile.Extension));
                }
                System.IO.File.Move(oldFile.FullName, newFile.FullName);

                sb.AppendLine("{");
                sb.AppendLine("\"Error\": \"No error\",");
                sb.AppendLine("\"Code\": 0,");
                sb.AppendLine("\"Old Path\": \"" + oldPath.Replace(oldFile.Name, "") + "\",");
                sb.AppendLine("\"Old Name\": \"" + oldFile.Name + "\",");
                sb.AppendLine("\"New Path\": \"" + newFile.FullName.Replace(HttpRuntime.AppDomainAppPath, "/").Replace(Path.DirectorySeparatorChar, '/') + "\",").Replace(newFile.Name, "");
                sb.AppendLine("\"New Name\": \"" + newFile.Name + "\"");
                sb.AppendLine("}");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Rename a file or directory
        /// </summary>
        /// <param name="path"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        private string Rename(string path, string newName)
        {
            if (!IsInRootPath(path))
            {
                return Error("Tentativa de modificar arquivo fora da pasta raiz.");
            }
            if (!System.IO.File.Exists(ObtenhaCaminho(path)) && !Directory.Exists(ObtenhaCaminho(path)))
            {
                return Error("Arquivo não encontrado.");
            }

            if (!TemPermissao(path))
                return Error("Usuário sem permissão.");

            FileAttributes attr = System.IO.File.GetAttributes(ObtenhaCaminho(path));

            StringBuilder sb = new StringBuilder();

            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                DirectoryInfo oldDir = new DirectoryInfo(ObtenhaCaminho(path));
                Directory.Move(ObtenhaCaminho(path), Path.Combine(oldDir.Parent.FullName, newName));
                DirectoryInfo newDir = new DirectoryInfo(Path.Combine(oldDir.Parent.FullName, newName));

                sb.AppendLine("{");
                sb.AppendLine("\"Error\": \"No error\",");
                sb.AppendLine("\"Code\": 0,");
                sb.AppendLine("\"Old Path\": \"" + path + "\",");
                sb.AppendLine("\"Old Name\": \"" + oldDir.Name + "\",");
                sb.AppendLine("\"New Path\": \"" + newDir.FullName.Replace(HttpRuntime.AppDomainAppPath, "/").Replace(Path.DirectorySeparatorChar, '/') + "\",");
                sb.AppendLine("\"New Name\": \"" + newDir.Name + "\"");
                sb.AppendLine("}");
            }
            else
            {
                FileInfo oldFile = new FileInfo(ObtenhaCaminho(path));
                //Don't allow extension to be changed
                newName = Path.GetFileNameWithoutExtension(newName) + oldFile.Extension;
                FileInfo newFile = new FileInfo(Path.Combine(oldFile.Directory.FullName, newName));
                System.IO.File.Move(oldFile.FullName, newFile.FullName);

                sb.AppendLine("{");
                sb.AppendLine("\"Error\": \"No error\",");
                sb.AppendLine("\"Code\": 0,");
                sb.AppendLine("\"Old Path\": \"" + path + "\",");
                sb.AppendLine("\"Old Name\": \"" + oldFile.Name + "\",");
                sb.AppendLine("\"New Path\": \"" + newFile.FullName.Replace(HttpRuntime.AppDomainAppPath, "/").Replace(Path.DirectorySeparatorChar, '/') + "\",");
                sb.AppendLine("\"New Name\": \"" + newFile.Name + "\"");
                sb.AppendLine("}");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Replace a file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string Replace(string path)
        {
            if (Request.Files.Count == 0 || Request.Files[0].ContentLength == 0)
            {
                return Error("Nenhum arquivo informado.");
            }
            else if (!IsInRootPath(path))
            {
                return Error("Tentativa de substituir arquivo fora da pasta raiz.");
            }
            else
            {
                var fi = new FileInfo(ObtenhaCaminho(path));
                HttpPostedFileBase file = Request.Files[0];
                if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                {
                    return Error("Tipo de arquivo não permitido.");
                }
                else if (!Path.GetExtension(file.FileName).Equals(fi.Extension))
                {
                    return Error("O arquivo substituito deve ter a mesma extensão do arquivo substituído.");
                }
                else if (!fi.Exists)
                {
                    return Error("Arquivo para ser substituido não encontrado.");
                }
                else
                {
                    file.SaveAs(fi.FullName);

                    return "<textarea>" + json.Serialize(new
                    {
                        Path = path.Replace("/" + fi.Name, ""),
                        Name = fi.Name,
                        Error = "No error",
                        Code = 0
                    }) + "</textarea>";
                }
            }
        }

        private string ObtenhaPastaRaiz()
        {
            return ControladorDeSessao.GetPastaRaiz();
        }

        private string ObtenhaCaminho(string path)
        {
            //caminho absoluto
            if (path != null)
            {
                if (path.StartsWith("/"))
                {
                    path = path.Substring(1);
                }

                path = path.Replace("/", "\\");
                path = Path.Combine(ObtenhaPastaRaiz(), path);
            }

            return path;

            //caminho virtual
            //return Server.MapPath(path);
        }
    }
}