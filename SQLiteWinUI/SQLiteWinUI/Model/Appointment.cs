using SQLite;
using System;

namespace SQLiteWinUI
{
    public class Appointment
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }
        public string EventName { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool AllDay { get; set; }
        public string Notes { get; set; }
    }
}
