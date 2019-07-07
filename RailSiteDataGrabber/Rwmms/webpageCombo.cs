using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace RailSiteDataGrabber.rwmms
{
    class webpageCombo : webpageControl
    {
        //public webpageCombo(ChromeDriver webbrowser, string name, string alias, string controlId
        //    , enumControlType controlType) : this(webbrowser, name, alias, controlId, controlType)
        //{

        //}
        //public webpageCombo(ChromeDriver webbrowser, string name, string alias, string controlId
        //  , enumControlType controlType)
        //    : this(webbrowser, name, alias, controlId, controlType, null)
        //{

        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="webbrowser"></param>
        /// <param name="name"></param>
        /// <param name="alias"></param>
        /// <param name="controlId">controlId or xpath should be specified</param>
        /// <param name="xpath">controlId or xpath should be specified</param>
        /// <param name="controlAtt">text or value</param>
        /// <param name="controlType"></param>
        /// <param name="tableSource"></param>
        public webpageCombo(ChromeDriver webbrowser, webpage page, string name, string alias, By controlBy
          , string controlAtt, enumControlType controlType
           , Model.htmlTable tableSource)
            : base(webbrowser,page, name, alias, controlBy, controlAtt, controlType)
        {
            this.prp_tableSource = tableSource;
        }
        /// <summary>
        ///set selectedText for prp_value
        /// </summary>
        /// <returns></returns>
        public override void sb_setValue()
        {
            IWebElement element = null;
            //if (!string.IsNullOrEmpty(this.prp_controlId))
            //{
            //    element = this.prp_webBrowser.FindElement(By.Id(this.prp_controlId));

            //}
            //else if (!string.IsNullOrEmpty(this.prp_xpath))
            //{
            //    element = this.prp_webBrowser.FindElement(By.XPath(this.prp_controlId));
            //}
            element = this.prp_webBrowser.FindElement(this.prp_controlBy);
            if (element != null)
            {
                SelectElement selectedItem = new SelectElement(element);
                if (string.IsNullOrEmpty(this.prp_controlAtt) || this.prp_controlAtt.ToLower() == "text")
                {
                    this.prp_value = selectedItem.SelectedOption.Text.Replace("&nbsp;", " ").TrimStart().TrimEnd();
                }
                else
                {
                    this.prp_value = selectedItem.SelectedOption.GetAttribute(this.prp_controlAtt).Replace("&nbsp;", " ").TrimStart().TrimEnd();
                }
            }
            //return null;
        }

        /// <summary>
        /// call sb_setListItems before get this property
        /// </summary>
        public ListItemCollection prp_listItems { get; set; }

        public void sb_setListItems()
        {
            IWebElement element = null;
            //if (!string.IsNullOrEmpty(this.prp_controlId))
            //{
            //    element = this.prp_webBrowser.FindElement(By.Id(this.prp_controlId));
            //}
            //else if (!string.IsNullOrEmpty(this.prp_xpath))
            //{
            //    element = this.prp_webBrowser.FindElement(By.XPath(this.prp_controlId));
            //}
            element = this.prp_webBrowser.FindElement(this.prp_controlBy);
            if (element != null)
            {
                var options = element.FindElements(By.TagName("option"));

                ListItemCollection itemCollection = new ListItemCollection();
                ListItem item;
                for (int i = 0; i <= options.Count - 1; i++)
                {
                    item = new ListItem();
                    item.Text = options[i].Text;
                    item.Value = options[i].GetAttribute("value");
                    try
                    {
                        if (Functions.IsNull(options[i].GetAttribute("selected")))
                        {
                            item.Selected = true;
                        }
                    }
                    catch
                    {
                        item.Selected = false;
                    }
                    itemCollection.Add(item);
                }

                this.prp_listItems = itemCollection;
            }


        }

        public Model.htmlTable prp_tableSource
        {
            get; set;

        }

        public bool fnc_saveTableSource()
        {
            if (this.prp_tableSource == null)
                throw new Exception("tableSource is not specified");
            var element = this.prp_webBrowser.FindElement(this.prp_controlBy);
            var items = element.FindElements(By.TagName("option"));
            return this.prp_tableSource.fnc_save(items);
        }
    }
}
