using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFでLocalDB接続のCRUD
{
    /// <summary>
    /// Productクラス
    /// http://blogs.wankuma.com/kazuki/archive/2009/02/23/168586.aspx
    /// </summary>
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Result { get; set; }
        public string Message { get; set; }
    }  
}
