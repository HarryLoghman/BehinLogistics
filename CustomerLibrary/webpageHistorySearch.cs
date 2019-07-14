using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerLibrary
{
    public class webpageHistory : SharedLibrary.htmlModel.webpage
    {
        public webpageHistory() : this("http://customers.rai.ir/Wagon_Info/Wagon_Seir_History/searchForm.asp")
        {
            this.sb_createTables();
        }
        public webpageHistory(string url) : base(url)
        {
            this.sb_createTables();
        }
        public webpageHistory(int parentId) : base(parentId)
        {
            this.sb_createTables();
        }

        public webpageHistory(string url, ChromeDriver webBrowser) : base(url, webBrowser)
        {
            this.sb_createTables();
        }
        public webpageHistory(int parentId, ChromeDriver webBrowser) : base(parentId, webBrowser)
        {
            this.sb_createTables();
        }

        private void sb_createTables()
        {

        }
    }
}
