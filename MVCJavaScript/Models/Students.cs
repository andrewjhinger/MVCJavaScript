using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCJavaScript.Models
{
    public class Students<Student>
    {
        public IEnumerable<Student> Data { get; set; }
        public int NumberOfPages { get; set; }
        public int CurrentPage { get; set; }
    }
}