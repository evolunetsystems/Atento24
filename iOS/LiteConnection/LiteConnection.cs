
using System;
using System.IO;
using atento24.Data.LiteConnection;
using atento24.Data.StandarDB;

[assembly: Xamarin.Forms.Dependency(typeof(atento24.iOS.LiteConnection.Connection))]
namespace atento24.iOS.LiteConnection
{
    public class Connection : IDataBase
    {
        public LocalDB GetDataBase()
        {
            string personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libraryFolder = Path.Combine(personalFolder, "..", "Library");
            var path = Path.Combine(libraryFolder, Keys.DataBaseName);
            return new LocalDB(path);
        }
    }
}