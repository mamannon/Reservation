using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Reservation
{
    class Database : DbContext
    {

        private int[] mKeys = new int[4];

        public Database(ref string[] rooms, ref string[] seats, ref string[] addresses)
        {
            // Let's put room information into database
            using (meeting_roomsEntities db = new meeting_roomsEntities())
            {

                // First clear the tables
                db.meeting_room.RemoveRange(db.meeting_room.Where(c => true));
                db.reservations.RemoveRange(db.reservations.Where(c => true));
                db.SaveChanges();

                // Then add the rooms
                IList<meeting_room> mr = new List<meeting_room>()
                {
                    new meeting_room() { name = rooms[0], location = addresses[0], seats = Int32.Parse(seats[0]) },
                    new meeting_room() { name = rooms[1], location = addresses[1], seats = Int32.Parse(seats[1]) },
                    new meeting_room() { name = rooms[2], location = addresses[2], seats = Int32.Parse(seats[2]) },
                    new meeting_room() { name = rooms[3], location = addresses[3], seats = Int32.Parse(seats[3]) }
                };
                db.meeting_room.AddRange(mr);
                db.SaveChanges();

                // Finally store meeting room keys
                var result = db.meeting_room.Where(q => true);
                int i = 0;
                foreach (var item in result)
                {
                    mKeys[i] = item.id;
                    i++;
                }

            }
        }

        // This method is called for every single hour to reserve
        public bool IsFree(ref int row, int room)
        {

            // We don't care about days...
            DateTime from = new DateTime(2020, 1, 1, row, 0, 0);

            // Let's search if we find some reservation
            using (meeting_roomsEntities db = new meeting_roomsEntities())
            {
                int shift = mKeys[room];
                var result = db.reservations.Where(q => q.meeting_room_id == shift)
                    .Where(q => q.time_from <= from)
                    .Where(q => q.time_to >= from);

                // We didn't find any
                if (result.Count() == 0) return true;
            }

            // This hour is already reserved
            return false;
        }

        // This method is called to put a reservation into a database
        public void Reserve(string name, ref int row1, ref int row2, int room)
        {

            // We don't care about days...
            DateTime from = new DateTime(2020, 1, 1, row1, 0, 0);
            DateTime to = new DateTime(2020, 1, 1, row1, 0, 0);

            using (meeting_roomsEntities db = new meeting_roomsEntities())
            {

                // To make a successful reservation, we need the key of the meeting_room
                reservation res = new reservation() { host = name, time_from = from, time_to = to, meeting_room_id = mKeys[room] };
                db.reservations.Add(res);
                db.SaveChanges();
            }
        }
    }
}
