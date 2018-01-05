using DataLayer.ExamModel;
using Exam_Application.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Excel;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;


namespace Exam_Application.Models.BAL
{
    public class WebFunction
    {
        Exam_MasterEntities db = new Exam_MasterEntities();
        public string Storefile(HttpPostedFileBase filepost, int numb)
        {
            try
            {
                if (filepost != null)
                {
                    if (filepost.FileName != "")
                    {
                        DateTime date = DateTime.Now;
                        string dates = date.Day.ToString() + date.Month.ToString() + date.Year.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString() + date.ToString("tt");
                        var path = "";
                        var returnpath = "";
                        if (numb == 1)
                        {
                            returnpath = "/UploadFiles/UserPIC/" + dates + filepost.FileName;
                            path = Path.Combine(HttpContext.Current.Server.MapPath("/UploadFiles/UserPIC/"), dates + filepost.FileName);
                            var filename = HttpContext.Current.Server.MapPath("/UploadFiles/UserPIC/" + dates + filepost.FileName);
                            filepost.SaveAs(path);
                        }
                        if (numb == 2)
                        {
                            returnpath = "/UploadFiles/StudentData/" + dates + filepost.FileName;
                            path = Path.Combine(HttpContext.Current.Server.MapPath("/UploadFiles/StudentData/"), dates + filepost.FileName);
                            var filename = HttpContext.Current.Server.MapPath("/UploadFiles/StudentData/" + dates + filepost.FileName);
                            filepost.SaveAs(path);
                        }
                        if (numb == 3)
                        {
                            returnpath = "/UploadFiles/UploadCertificates/" + dates + filepost.FileName;
                            path = Path.Combine(HttpContext.Current.Server.MapPath("/UploadFiles/UploadCertificates/"), dates + filepost.FileName);
                            var filename = HttpContext.Current.Server.MapPath("/UploadFiles/UploadCertificates/" + dates + filepost.FileName);
                            filepost.SaveAs(path);
                        }
                        return returnpath;
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }
        public void DeleteImage(string file)
        {
            try
            {
              //  file = "" + file.Substring(3, (file.Length - 7)) + "";
                string fullPath = "" + file.Substring(3, (file.Length - 7)) + "";
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            catch
            {

            }
        }
        public string BlockScript(string input)
        {
            try
            {
                bool chk = true;
                if (input.Contains("<script>"))
                    chk = false;
                if (input.Contains("<script type=" + "text/javascript" + ">"))
                    chk = false;
                if (input.Contains("script"))
                    chk = false;
                if (input.Contains("</script>"))
                    chk = false;
                if (chk == false)
                    return "";
                else
                    return input;
            }
            catch
            {
                return "";
            }
        }

        public DataTable GetExcesData(HttpPostedFileBase FileUpload)
        {
            DataTable dt = new DataTable();
            if (FileUpload != null)
            {
                var fileName = Path.GetFileName(FileUpload.FileName);
                var path = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadFiles/ImportExcel/"), fileName);
                FileUpload.SaveAs(path);

                FileStream stream = new FileStream(Path.Combine(HttpContext.Current.Server.MapPath("~/UploadFiles/ImportExcel/"), fileName), FileMode.Open);
                IExcelDataReader excelReader2007 = ExcelReaderFactory.CreateOpenXmlReader(stream);
                //DataSet - The result of each spreadsheet will be created in the result.Tables
                DataSet result = excelReader2007.AsDataSet();
                dt = result.Tables[0];
                File.Delete(Path.Combine(HttpContext.Current.Server.MapPath("~/UploadFiles/"), fileName));
                return dt;
            }
            else { return dt; }

        }
    }
}