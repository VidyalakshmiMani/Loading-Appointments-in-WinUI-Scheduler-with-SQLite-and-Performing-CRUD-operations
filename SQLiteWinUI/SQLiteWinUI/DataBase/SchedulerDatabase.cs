using SQLite;
using System;
using System.Collections.Generic;

namespace SQLiteWinUI
{
    public class SchedulerDatabase 
    {
        readonly SQLiteConnection sqLiteConnection;
        public SchedulerDatabase(string dbPath)
        {
            sqLiteConnection = new SQLiteConnection(dbPath);
            sqLiteConnection.CreateTable<Appointment>();
        }

        //Get the list of appointments from the SQLiteConnection
        public List<Appointment> GetSchedulerAppointments()
        {
            return sqLiteConnection.Table<Appointment>().ToList();
        }

        //Insert an appointment in the SQLiteConnection
        public int SaveSchedulerAppointment(Appointment appointment)
        {
            return sqLiteConnection.InsertOrReplace(appointment);
        }

        //Delete an appointment in the SQLiteConnection 
        public int DeleteSchedulerAppointment(Appointment appointment)
        {
            return sqLiteConnection.Delete(appointment);
        }
    }
}
