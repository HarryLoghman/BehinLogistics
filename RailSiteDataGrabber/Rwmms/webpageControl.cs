using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailSiteDataGrabber.rwmms
{
    class webpageControl
    {
        //public webpageControl(ChromeDriver webbrowser, string name, string alias, string controlId
        //    , enumControlType controlType) : this(webbrowser, name, alias, controlId, controlType)
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
        /// <param name="controlAtt"></param>
        /// <param name="controlType"></param>
        public webpageControl(ChromeDriver webbrowser, webpage page, string name, string alias, By controlBy, string controlAtt, enumControlType controlType)
        {
            if (controlBy == null)
                throw new Exception("controlBy is not specified");
            this.prp_controlBy = controlBy;
            this.prp_controlType = controlType;
            this.prp_name = name;
            this.prp_alias = alias;
            this.prp_controlAtt = controlAtt;
            this.prp_webBrowser = webbrowser;
            this.prp_webpage = page;

        }

        public webpage prp_webpage { get; set; }
        public enumControlType prp_controlType { get; set; }
        public By prp_controlBy { get; }
        public string prp_name { get; set; }
        public string prp_alias { get; set; }
        public string prp_controlAtt { get; set; }
        protected ChromeDriver prp_webBrowser { get; set; }

        /// <summary>
        /// call sb_setValue before calling this property
        /// </summary>
        public object prp_value { get; set; }
        public virtual void sb_setValue()
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
                if (string.IsNullOrEmpty(this.prp_controlAtt) || this.prp_controlAtt.ToLower() == "text")
                {
                    this.prp_value = element.Text.Replace("&nbsp;", " ").TrimStart().TrimEnd();
                }
                else
                {
                    this.prp_value = element.GetAttribute(this.prp_controlAtt).Replace("&nbsp;", " ").TrimStart().TrimEnd();
                }
                return;
            }
            //return null;
        }

        public IWebElement fnc_getHtmlElement()
        {
            //if (!string.IsNullOrEmpty(this.prp_controlId))
            //{
            //    this.prp_webBrowser.FindElement(By.Id(this.prp_controlId));
            //}
            //else if (!string.IsNullOrEmpty(this.prp_xpath))
            //{
            //    this.prp_webBrowser.FindElement(By.XPath(this.prp_xpath));
            //}
            return this.prp_webBrowser.FindElement(this.prp_controlBy);
            return null;
        }


    }

    //class webpageGrid : webpageControl
    //{
    //    public webpageGrid(string name, string alias, string controlId, int? controlIndex, enumControlType controlType) : base(name, alias, controlId, controlIndex, controlType)
    //    {

    //    }
    //    public List<webpageControl> prp_controls { get; set; }
    //}

    //class webpageButton : webpageControl
    //{
    //    public webpageButton(string name, string alias, string controlId, int? controlIndex, enumControlType controlType) : base(name, alias, controlId, controlIndex, controlType)
    //    {

    //    }

    //    //public webpageButton(string controlId, int? controlIndex, enumControlType controlType,webpage webpage) : base(controlId, controlIndex, controlType)
    //    //{
    //    //    this.prp_webpage = webpage;
    //    //}
    //    public webpage prp_webpage { get; set; }
    //}


    internal enum enumControlType
    {
        grid,
        textbox,
        button,
        combo
    }
}
