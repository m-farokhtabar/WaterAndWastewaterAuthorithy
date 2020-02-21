using System.Windows;
using System.Threading;

namespace WaterAndWastewaterAuthorithy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {

            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fa-IR");
            /*Database.DefaultConnectionFactory.CreateConnection("Context");
            Database.SetInitializer(new DropCreateDatabaseAlways<Context>());
            Context Db = new Context();
            Db.Customers.Add(new DomainClasses.CustomersTb { Id=1, MeliCode="1010",Name="Mehdi",Family="razavi",Father="nader"
                                                            , Phone="0311", CellPhone="0913", Address="Isf", Description="Test" });*/
        }        
    }
}
