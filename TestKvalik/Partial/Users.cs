using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestKvalik
{
    public partial class Users
    {
        public string LastVisit
        {
            get
            {
                var works = Work.ToList();
                DateTime lastVisit = new DateTime();
                string lastVisitString = "-";

                foreach (var work in works)
                    if (work.Date > lastVisit)
                    {
                        lastVisit = work.Date;
                        lastVisitString = lastVisit.ToString();
                    }

                return lastVisitString;
            }
        }

        public int VisitCount
        {
            get
            {
                return Work.Count;
            }
        }
    }
}
