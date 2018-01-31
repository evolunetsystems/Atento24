using System;
using System.IO;
using Xamarin.Forms;
using atento24.Data.StandarDB;
using atento24.Data.LiteConnection;

[assembly: Dependency(typeof(atento24.Droid.SQLite.LiteConnection.Connection))]
namespace atento24.Droid.SQLite.LiteConnection
{
    public class Connection : IDataBase
    {
        public LocalDB GetDataBase()
        {
            var fileName = Keys.DataBaseName;
            //var internalpath = Android.OS.Environment.ExternalStorageDirectory.Path;
            //var path = Path.Combine(internalpath, fileName);
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, fileName);
            return new LocalDB(path);
        }
    }
}