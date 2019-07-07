using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailSiteDataGrabber.rwmms
{
    static class SharedFunctions
    {
        static TimeSpan v_waitForElement = TimeSpan.FromSeconds(10);
        public static void sb_waitForReady(IWebDriver webDriver)
        {
            WebDriverWait wait = new WebDriverWait(webDriver, v_waitForElement);

            //wait.Until(driver =>
            //{
            //    bool isAjaxFinished = (bool)((IJavaScriptExecutor)driver).
            //        ExecuteScript("return jQuery.active == 0");
            //    /*bool isLoaderHidden = (bool)((IJavaScriptExecutor)driver).
            //        ExecuteScript("return $('.spinner').is(':visible') == false");*/
            //    return isAjaxFinished;// & isLoaderHidden;
            //});
            try
            {
                wait.Until(driver =>
                {
                    bool isAjaxFinished = (bool)((IJavaScriptExecutor)driver).
                        ExecuteScript("return Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack() == false;");
                    /*bool isLoaderHidden = (bool)((IJavaScriptExecutor)driver).
                        ExecuteScript("return $('.spinner').is(':visible') == false");*/
                    return isAjaxFinished;// & isLoaderHidden;
                });
            }
            catch (Exception ex)
            {

            }
            //selenium.waitForCondition(“selenium.browserbot.getUserWindow().Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack() == false;”, “10000″);

        }
    }
}
