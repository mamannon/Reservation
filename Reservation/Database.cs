using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation
{
    class Database : DbContext
    {

        public Database()
        {
 //           DataContext db = new DataContext()
        }

        public bool IsFree(ref int row, ref int column)
        {
            return true;
        }

        public void Reserve(string name, ref int row1, ref int row2, ref int column)
        {

        }

    }

}
