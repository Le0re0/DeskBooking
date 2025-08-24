using System.Configuration;
using System.Data;
using System.Windows;
using DeskBooking.Data;
using DeskBooking.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DeskBooking
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            using (var db = new AppDbContext())
            {
                db.Database.Migrate();
                DbSeeder.Seed(db);
            }
        }
    }
}
