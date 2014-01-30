using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using MVCJavaScript.Models;

namespace MVCJavaScript.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }


        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        public ActionResult GetContent(string format = "JSON")
        {
            Person[] persons = new Person[] { new Person { Name = "Tom Jones", Age = 21 }, new Person { Name = "Bob Lee", Age = 24 } };
            if (format.ToLower().IndexOf("json") >= 0)
                return Json(persons, JsonRequestBehavior.AllowGet);
            else if (format.ToLower().IndexOf("xml") >= 0)
            {
                string xml = ObjectToXML(persons);
                return this.Content(xml, "text/xml");
            }
            else
                return this.Content("Invalid format", "text/html");
        }

        private string ObjectToXML(object obj)
        {
            StringBuilder strXML = new StringBuilder();
            try
            {
                Type objectType = obj.GetType();
                XmlSerializer xmlSerializer = new XmlSerializer(objectType);
                MemoryStream memoryStream = new MemoryStream();
                try
                {
                    using (XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8) { Formatting = Formatting.Indented })
                    {
                        xmlSerializer.Serialize(xmlTextWriter, obj);
                        memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                        strXML.Append(new UTF8Encoding().GetString(memoryStream.ToArray()));
                    }
                }
                finally
                {
                    memoryStream.Dispose();
                }
            }
            catch (Exception ex)
            {
                strXML.Clear();
                strXML.Append("<Error>" + ex.Message + "</Error>\n");
            }
            return strXML.ToString();
        }

        private Student[] studentData = new Student[] 
        {
            new Student { FirstName = "Tom", LastName = "Jones", Age = 21 }, 
            new Student { FirstName = "Bob", LastName = "Lee", Age = 24 } ,
            new Student { FirstName = "A", LastName = "Lee", Age = 25 }, 
            new Student { FirstName = "B", LastName = "Lee", Age = 26 }, 
            new Student { FirstName = "C", LastName = "Lee", Age = 27 }, 
            new Student { FirstName = "D", LastName = "Lee", Age = 28 }, 
            new Student { FirstName = "E", LastName = "Lee", Age = 29 }, 
            new Student { FirstName = "F", LastName = "Lee", Age = 30 }, 
            new Student { FirstName = "G", LastName = "Lee", Age = 31 }, 
            new Student { FirstName = "H", LastName = "Lee", Age = 32 }, 
            new Student { FirstName = "I", LastName = "Lee", Age = 33 }, 
            new Student { FirstName = "J", LastName = "Lee", Age = 34 }, 
            new Student { FirstName = "K", LastName = "Lee", Age = 35 }, 
            new Student { FirstName = "L", LastName = "Lee", Age = 36 } 
        };

        private const int PageSize = 5;

        public ActionResult Student()
        {
            return View(GetStudents(1));
        }

        public ActionResult PartialStudent(int pageNumber = 1)
        {
            return PartialView(GetStudents(pageNumber));
        }

        private Students<Student> GetStudents(int pageNumber)
        {
            var students = new Students<Student>();
            students.Data = studentData.OrderBy(p => p.LastName).Skip(PageSize * (pageNumber - 1)).Take(PageSize).ToList();
            students.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)studentData.Count() / PageSize));
            students.CurrentPage = pageNumber;
            return students;
        }
    }
}
