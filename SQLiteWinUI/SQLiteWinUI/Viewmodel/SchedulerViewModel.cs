using Microsoft.UI.Xaml.Controls;
using Syncfusion.UI.Xaml.Scheduler;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SQLiteWinUI
{
    public class SchedulerViewModel
    {
        private ScheduleAppointment appointment;
        public ObservableCollection<ScheduleAppointment> Appointments { get; set; }
        public SchedulerViewModel()
        {
            Appointments = new ObservableCollection<ScheduleAppointment>();
            var dataBaseAppointments = App.Database.GetSchedulerAppointments();
            if (dataBaseAppointments != null && dataBaseAppointments.Count > 0)
            {
                foreach (Appointment appointment in dataBaseAppointments)
                {
                    Appointments.Add(new ScheduleAppointment()
                    {
                        StartTime = appointment.From,
                        EndTime = appointment.To,
                        Subject = appointment.EventName,
                        IsAllDay = appointment.AllDay,
                        Id = appointment.Id
                    });
                }
            }
            else
            {
                this.GenerateAppointments();
            }
        }
        internal async void AddAppointmentDetails(string action, ScheduleAppointment scheduleAppointment)
        {
            appointment = scheduleAppointment;
            var endDate = appointment.ActualEndTime.Date;
            var startDate = appointment.ActualStartTime.Date;
            var endTime = appointment.ActualEndTime.TimeOfDay;
            var startTime = appointment.ActualStartTime.TimeOfDay;

            if (endDate < startDate)
            {
                await ShowDisplayAlert("End date should be greater than start date");
            }
            else if (!appointment.IsAllDay && endDate == startDate)
            {
                if (endTime <= startTime)
                {
                    await ShowDisplayAlert("End time should be greater than start time");
                }
                else
                {
                    AppointmentDetails(action, appointment);
                }
            }
            else
            {
                AppointmentDetails(action, appointment);
            }
        }
        private static async Task ShowDisplayAlert(string alert)
        {
            ContentDialog displayAlert = new ContentDialog
            {
                Title = "Alert",
                Content = alert,
                CloseButtonText = "OK"
            };

            await displayAlert.ShowAsync();
        }
        private void AppointmentDetails(string action, ScheduleAppointment scheduleAppointment)
        {
            if (action == "Add" && this.Appointments != null)
            {
                appointment.Id = Appointments.Count;
                //// Add the appointments in the Scheduler.
                Appointments.Add(appointment);
            }
            SaveSchedulerAppointmentAsync();
        }
        private void GenerateAppointments()
        {
            ScheduleAppointment appointment1 = new ScheduleAppointment() { StartTime = DateTime.Now.Date.AddHours(9), EndTime = DateTime.Now.Date.AddHours(10), Subject = "Meeting" };
            ScheduleAppointment appointment2 = new ScheduleAppointment() { StartTime = DateTime.Now.Date.AddDays(1).AddHours(9), EndTime = DateTime.Now.Date.AddDays(1).AddHours(10), Subject = "Meeting" };
            this.Appointments.Add(appointment1);
            this.Appointments.Add(appointment2);

            var editAppointment = new Appointment() { From = appointment1.StartTime, To = appointment1.EndTime, AllDay = appointment1.IsAllDay, Notes = appointment1.Notes, EventName = appointment1.Subject, Id = (int)appointment1.Id };
            var editAppointment1 = new Appointment() { From = appointment2.StartTime, To = appointment2.EndTime, AllDay = appointment1.IsAllDay, Notes = appointment2.Notes, EventName = appointment2.Subject, Id = (int)appointment2.Id };

            App.Database.SaveSchedulerAppointment(editAppointment);
            App.Database.SaveSchedulerAppointment(editAppointment1);
        }
        internal void DeleteSchedulerAppointment(ScheduleAppointment scheduleAppointment)
        {
            if (appointment == null)
            {
                appointment = scheduleAppointment;
            }

            //// Remove the appointments in the Scheduler.
            Appointments.Remove(this.appointment);
            //// Delete appointment in the database
            var deleteAppointment = new Appointment() { From = appointment.StartTime, To = appointment.EndTime, AllDay = appointment.IsAllDay, Notes = appointment.Notes, EventName = appointment.Subject, Id = (int)appointment.Id };
            App.Database.DeleteSchedulerAppointment(deleteAppointment);
        }
        private void SaveSchedulerAppointmentAsync()
        {
            //// - add or edit the appointment in the database collection
            var editAppointment = new Appointment() { From = appointment.StartTime, To = appointment.EndTime, AllDay = appointment.IsAllDay, Notes = appointment.Notes, EventName = appointment.Subject, Id = (int)appointment.Id };
            App.Database.SaveSchedulerAppointment(editAppointment);
        }
    }
}
