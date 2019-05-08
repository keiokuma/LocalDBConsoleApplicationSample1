using System.IO;

namespace LocalDB接続のサンプル
{
    public static class LocalDBAdapter
    {
        /// <summary>
        /// 専用クラスでLocalDBの接続文字列を得る
        /// http://overmorrow.hatenablog.com/entry/2015/04/01/221601
        /// http://stackoverflow.com/questions/22963952/connection-string-for-localdb-mdf-not-working-on-wpf
        /// </summary>
        /// <param name="mdfFileName">使用するMDFファイル名</param>
        /// <returns></returns>
        public static string GetLocalDBConnectionString(string mdfFileName)
        {
            //自分自身の実行ファイルのパスを取得する
            string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            // LocalDBのパスを取得する
            var rootPath = new DirectoryInfo(appPath);
            var newPath = rootPath.Parent.Parent;
            var mdfFile = new FileInfo(mdfFileName);
            var absoluteFile = new DirectoryInfo(Path.Combine(rootPath.FullName, mdfFile.FullName));

            //string constr = @"Data Source=(LocalDb)\MSSQLLocalDB;Integrated Security=True;AttachDbFileName=C:\Users\kei.okuma\Documents\visual studio 2015\Projects\LocalDBConsoleApplicationSample1\LocalDBConsoleApplicationSample1\Database1.mdf";
            return string.Format(@"Data Source=(LocalDb)\MSSQLLocalDB;
                                  Integrated Security=True;
                                  AttachDbFileName={0}",
                                  absoluteFile);
        }
    }
}
