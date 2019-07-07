using OpenQA.Selenium;
using SQLHelper.Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharedLibrary
{
    public static class Functions
    {

        public static bool IsNull(object value)
        {
            if (value == null || value == DBNull.Value || string.IsNullOrEmpty(value.ToString())
                || value.ToString() == "[هیج مقداری انتخاب نشده]") return true;
            return false;
        }

        public static object IsNull(object value, object retValue)
        {
            if (IsNull(value)) return retValue;
            else return value;
        }

        public static TimeSpan? fnc_convertTimeLongToTimespan(long? timeLong)
        {
            if (!timeLong.HasValue) return null;
            string timeStr = timeLong.ToString();

            if (timeStr.Length <= 2) return new TimeSpan(0, int.Parse(timeStr), 0);
            if (timeStr.Length == 3) return new TimeSpan(0, int.Parse(timeStr.Substring(0, 1)), int.Parse(timeStr.Substring(1, 2)));
            if (timeStr.Length == 4) return new TimeSpan(0, int.Parse(timeStr.Substring(0, 2)), int.Parse(timeStr.Substring(2, 2)));
            if (timeStr.Length == 5) return new TimeSpan(int.Parse(timeStr.Substring(0, 1)), int.Parse(timeStr.Substring(1, 2)), int.Parse(timeStr.Substring(3, 1)));
            if (timeStr.Length == 6) return new TimeSpan(int.Parse(timeStr.Substring(0, 2)), int.Parse(timeStr.Substring(2, 2)), int.Parse(timeStr.Substring(4, 2)));
            if (timeStr.Length == 7) return new TimeSpan(int.Parse(timeStr.Substring(0, 2)), int.Parse(timeStr.Substring(2, 2)), int.Parse(timeStr.Substring(4, 2)), int.Parse(timeStr.Substring(6, 1)));
            if (timeStr.Length == 8) return new TimeSpan(int.Parse(timeStr.Substring(0, 2)), int.Parse(timeStr.Substring(2, 2)), int.Parse(timeStr.Substring(4, 2)), int.Parse(timeStr.Substring(6, 2)));
            return new TimeSpan(long.Parse(timeStr));
        }

        public static DateTime? fnc_convertSolarDateAndTimeToDateTime(string solarDateWithOutColon, long? timeInt)
        {
            if (string.IsNullOrEmpty(solarDateWithOutColon) && !timeInt.HasValue) return null;
            TimeSpan? timeSpan = fnc_convertTimeLongToTimespan(timeInt);
            string timeStr;
            if (!timeSpan.HasValue)
            {
                timeStr = "00:00:00";
            }
            else
            {
                timeStr = timeSpan.Value.ToString("c");
            }
            DateTime dateTime;
            if (string.IsNullOrEmpty(solarDateWithOutColon))
            {
                dateTime = DateTime.Parse(DateTime.MinValue.ToString("yyyy/MM/dd") + " " + timeStr);
            }
            else
            {
                if (!solarDateWithOutColon.Contains("/"))
                {
                    if (solarDateWithOutColon.Length == 8)
                    {
                        solarDateWithOutColon = solarDateWithOutColon.Substring(0, 4) + "/" + solarDateWithOutColon.Substring(4, 2) + "/" + solarDateWithOutColon.Substring(7, 2);
                    }
                    else if (solarDateWithOutColon.Length == 7)
                    {
                        solarDateWithOutColon = solarDateWithOutColon.Substring(0, 3) + "/" + solarDateWithOutColon.Substring(3, 2) + "/" + solarDateWithOutColon.Substring(6, 2);
                    }
                    else if (solarDateWithOutColon.Length == 6)
                    {
                        solarDateWithOutColon = solarDateWithOutColon.Substring(0, 2) + "/" + solarDateWithOutColon.Substring(2, 2) + "/" + solarDateWithOutColon.Substring(4, 2);
                    }
                    else if (solarDateWithOutColon.Length == 5)
                    {
                        solarDateWithOutColon = solarDateWithOutColon.Substring(0, 1) + "/" + solarDateWithOutColon.Substring(1, 2) + "/" + solarDateWithOutColon.Substring(3, 2);
                    }
                    else if (solarDateWithOutColon.Length == 4)
                    {
                        solarDateWithOutColon = "1400/" + solarDateWithOutColon.Substring(0, 2) + "/" + solarDateWithOutColon.Substring(2, 2);
                    }
                }
                string[] solarDateArr = solarDateWithOutColon.Split('/');
                if (solarDateArr.Length != 3)
                {
                    throw new Exception("Solardate should have 3 parts year/month/day");
                }
                
                PersianCalendar pc = new PersianCalendar();
                int year, month, day;
                if (solarDateArr[0].Length != 2)
                    year = int.Parse("1400".Substring(0, 4 - solarDateArr[0].Length) + solarDateArr[0]);
                else/*for eaxmple we have 98 we cannot findout it is 1398 or 1498
                 so we check current year and subtract first two numbers*/
                    year = int.Parse((pc.GetYear(DateTime.Now) / 100).ToString() + solarDateArr[0]);
                month = int.Parse(solarDateArr[1]);
                day = int.Parse(solarDateArr[2]);

                DateTime time = DateTime.Parse(timeStr);
                try
                {
                    dateTime = pc.ToDateTime(year, month, day, time.TimeOfDay.Hours, time.TimeOfDay.Minutes, time.TimeOfDay.Seconds, time.TimeOfDay.Milliseconds);
                }
                catch(Exception ex)
                {
                    return null;
                }

            }
            return dateTime;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="tableId"></param>
        /// <param name="skipRowCount">0= means no header; 1= means first row is header; 2=means first two rows are header</param>
        /// <returns></returns>
        public static void sb_fillDatatableWithHtmlTableId(string html, string tableId, int skipRowCountAtBegining, int skipRowCountAtEnd
            , int skipColCountAtBegining, int skipColCountAtEnd, DataTable dt)
        {
            if (string.IsNullOrEmpty(html)) throw new Exception("html is null or empty");
            if (string.IsNullOrEmpty(tableId)) throw new Exception("tableId is null or empty");
            if (skipRowCountAtBegining < 0) throw new Exception("skipRowCountAtBegining is a negative number");
            if (skipRowCountAtEnd < 0) throw new Exception("skipRowCountAtEnd is a negative number");
            if (skipColCountAtBegining < 0) throw new Exception("skipColCountAtBegining is a negative number");
            if (skipColCountAtEnd < 0) throw new Exception("skipColCountAtEnd is a negative number");


            if (dt == null) throw new Exception("dt is null");
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(html);
            var table = document.GetElementbyId(tableId);

            if (table == null) return;
            HtmlAgilityPack.HtmlNodeCollection rows = table.SelectNodes("tr");
            if (rows == null)
            {
                HtmlAgilityPack.HtmlNodeCollection tbody = table.SelectNodes("tbody");
                if (tbody != null)
                {
                    rows = tbody[0].SelectNodes("tr");
                }
            }
            int i, j;
            DataRow dr;
            string cellContent;
            for (i = skipRowCountAtBegining; i <= rows.Count - 1 - skipRowCountAtEnd; i++)
            {
                var cells = rows[i].SelectNodes("td");
                dr = dt.NewRow();
                for (j = skipColCountAtBegining; j <= cells.Count - 1 - skipColCountAtEnd; j++)
                {
                    cellContent = cells[j].InnerText.Replace("&nbsp;", "").TrimEnd().TrimStart();
                    if (j - skipColCountAtBegining < dt.Columns.Count)
                        dr[j - skipColCountAtBegining] = string.IsNullOrEmpty(cellContent) ? Convert.DBNull : cellContent;
                }
                dt.Rows.Add(dr);
            }

        }
        public static void sb_fillDatatableWithHtmlTableId(string html, string tableId, Models.htmlTable dt)
        {
            if (string.IsNullOrEmpty(html)) throw new Exception("html is null or empty");
            if (string.IsNullOrEmpty(tableId)) throw new Exception("tableId is null or empty");


            if (dt == null) throw new Exception("dt is null");
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(html);
            var table = document.GetElementbyId(tableId);

            HtmlAgilityPack.HtmlNodeCollection rows;
            if (table == null) return;
            HtmlAgilityPack.HtmlNode tbody = table.SelectSingleNode("tbody");
            if (tbody != null)
            {
                rows = tbody.SelectNodes("tr");
            }
            else
            {
                rows = table.SelectNodes("tr");
            }
            int i, j;
            DataRow dr;
            string cellContent;
            int skipRowCountAtBegining = dt.prp_skipRowTop;
            int skipRowCountAtEnd = dt.prp_skipRowBottom;
            Models.htmlColumn column;
            for (i = skipRowCountAtBegining; i <= rows.Count - 1 - skipRowCountAtEnd; i++)
            {
                if (dt.prp_skipRowIndecies != null && dt.prp_skipRowIndecies.Any(o => o == i))
                {
                    continue;
                }
                var cells = rows[i].SelectNodes("td");
                dr = dt.NewRow();
                for (j = 0; j <= dt.Columns.Count - 1; j++)
                {
                    column = (Models.htmlColumn)dt.Columns[j];
                    if (!column.prp_tableColIndex.HasValue)
                        continue;
                    if (column.prp_tableColIndex.Value >= cells.Count)
                    {
                        continue;
                    }
                    cellContent = cells[j].InnerText.Replace("&nbsp;", "").TrimEnd().TrimStart();
                    dr[column.ColumnName] = string.IsNullOrEmpty(cellContent) ? Convert.DBNull : cellContent;
                }

                dt.Rows.Add(dr);
            }
        }

        public static void sb_fillDataRowWithHtmlControls(string html, Models.htmlTable dt)
        {
            sb_fillDataRowWithHtmlControls(html, dt, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="dt"></param>
        /// <param name="dr">if null the row will be created and added</param>
        public static void sb_fillDataRowWithHtmlControls(string html, Models.htmlTable dt, DataRow dr)
        {
            if (string.IsNullOrEmpty(html)) throw new Exception("html is null or empty");
            if (dt == null) throw new Exception("dt");
            bool addRow = false;
            if (dr == null) { dr = dt.NewRow(); addRow = true; }


            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(html);
            int i;
            string controlChecked;
            Models.htmlColumn htmlCol;
            for (i = 0; i <= dt.Columns.Count - 1; i++)
            {
                if (!(dt.Columns[i] is Models.htmlColumn)) continue;
                htmlCol = (Models.htmlColumn)dt.Columns[i];

                if (!string.IsNullOrEmpty(htmlCol.prp_controlId))
                {
                    if (htmlCol.prp_controlType == Models.htmlColumn.enum_controlType.inputCheckBox)
                    {
                        controlChecked = fnc_getElementValue(html, htmlCol.prp_controlId, htmlCol.prp_controlType);
                        if (string.IsNullOrEmpty(controlChecked) || controlChecked.ToLower() != "checked")
                            dr[i] = false;
                        else dr[i] = true;
                    }
                    else
                    {
                        var convertor = System.ComponentModel.TypeDescriptor.GetConverter(dt.Columns[i].DataType);
                        var val = fnc_getElementValue(html, htmlCol.prp_controlId, htmlCol.prp_controlType);
                        dr[i] = Functions.IsNull(val) ? DBNull.Value : convertor.ConvertFromString(null, CultureInfo.InvariantCulture, val);
                    }

                }
            }
            if (addRow) dt.Rows.Add(dr);
        }

        public static System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> fnc_getSeleniumTableRows(IWebDriver webBrowser, string tableId)
        {
            if (webBrowser == null) throw new Exception("webBrowser is null");
            if (string.IsNullOrEmpty(tableId)) throw new Exception("tableId is null");

            var table = webBrowser.FindElement(By.Id(tableId));
            if (table != null)
            {
                System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> rows;
                try
                {
                    var tbody = table.FindElement(By.TagName("tbody"));
                    if (tbody != null)
                        rows = tbody.FindElements(By.TagName("tr"));
                    else
                        rows = table.FindElements(By.TagName("tr"));
                }
                catch (Exception ex)
                {
                    rows = table.FindElements(By.TagName("tr"));
                }
                return rows;
            }
            return null;
        }
        public static HtmlAgilityPack.HtmlNodeCollection fnc_getPaginationHrefCollection(string html, string tableId)
        {
            if (string.IsNullOrEmpty(html)) throw new Exception("html is null or empty");
            if (string.IsNullOrEmpty(tableId)) throw new Exception("tableId is null or empty");

            var table = Functions.fnc_getElementById(html, tableId);
            if (table != null)
            {
                HtmlAgilityPack.HtmlNodeCollection rows = table.SelectNodes("tr");
                if (rows == null)
                {
                    HtmlAgilityPack.HtmlNodeCollection tbody = table.SelectNodes("tbody");
                    if (tbody != null)
                    {
                        rows = tbody[0].SelectNodes("tr");
                    }
                }

                if (rows != null && rows.Count > 0)
                {
                    var lastRow = rows[rows.Count - 1];
                    var td = lastRow.SelectSingleNode("td");
                    var hrefCollection = td.SelectNodes("a");
                    return hrefCollection;
                }
            }
            return null;
        }

        public static HtmlAgilityPack.HtmlNode fnc_getElementById(string html, string Id)
        {
            if (string.IsNullOrEmpty(html)) throw new Exception("html is null or empty");
            if (string.IsNullOrEmpty(Id)) throw new Exception("Id is null or empty");

            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(html);
            var element = document.GetElementbyId(Id);

            return element;
        }

        public static string fnc_getElementValue(string html, string Id)
        {
            return fnc_getElementValue(html, Id, Models.htmlColumn.enum_controlType.input);
            //if (string.IsNullOrEmpty(html)) throw new Exception("html is null or empty");
            //if (string.IsNullOrEmpty(Id)) throw new Exception("Id is null or empty");

            //var element = fnc_getElementById(html, Id);
            //var att = element.Attributes["value"];
            //if (att == null) return null;
            //return att.Value;
        }

        public static string fnc_getElementValue(string html, string Id, Models.htmlColumn.enum_controlType controlType)
        {
            if (string.IsNullOrEmpty(html)) throw new Exception("html is null or empty");
            if (string.IsNullOrEmpty(Id)) throw new Exception("Id is null or empty");

            if (controlType == Models.htmlColumn.enum_controlType.input)
            {
                var element = fnc_getElementById(html, Id);
                var att = element.Attributes["value"];
                if (att != null)
                    return att.Value;
            }
            else if (controlType == Models.htmlColumn.enum_controlType.inputCheckBox)
            {
                var element = fnc_getElementById(html, Id);
                var att = element.Attributes["checked"];
                if (att != null)
                    return att.Value;
            }
            else if (controlType == Models.htmlColumn.enum_controlType.textArea)
            {
                var element = fnc_getElementById(html, Id);
                var value = element.InnerText.Replace("&nbsp;", "").TrimEnd().TrimStart();
                return string.IsNullOrEmpty(value) ? null : value;
            }
            return null;
        }
        public static string miladiToSolar(DateTime dt)
        {
            PersianCalendar pc = new PersianCalendar();
            string year = pc.GetYear(dt).ToString();
            string month = pc.GetMonth(dt).ToString();
            string day = pc.GetDayOfMonth(dt).ToString();
            if (year.Length < 3) year = "13" + year;
            if (month.Length < 2) month = "0" + month;
            if (day.Length < 2) day = "0" + day;

            return year + "/" + month + "/" + day;

        }

        public static DateTime solarToMiladi(string date)
        {
            if (date.Split('/').Length != 3)
            {
                throw new Exception("فرمت تاريخ صحيح  نمي باشد");
            }
            int year = int.Parse(date.Split('/')[0]);
            int month = int.Parse(date.Split('/')[1]);
            int day = int.Parse(date.Split('/')[2]);
            PersianCalendar pc = new PersianCalendar();
            return pc.ToDateTime(year, month, day, 0, 0, 0, 0);

        }

        public static void sb_control_gotFocused(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox txt = (TextBox)sender;
                txt.SelectAll();
            }
            else if (sender is MaskedTextBox)
            {

                MaskedTextBox mskTxt = (MaskedTextBox)sender;
                //mskTxt.SelectAll();
                if (mskTxt.Text != "")
                {
                    mskTxt.Select(0, mskTxt.Text.Length);
                    mskTxt.SelectionStart = 0;
                    mskTxt.SelectionLength = mskTxt.Text.Length;
                    mskTxt.SelectedText = mskTxt.Text;
                }

            }
            else if (sender is ComboBox)
            {
                ComboBox cmbx = (ComboBox)sender;
                cmbx.SelectAll();
            }
            else if (sender is NumericUpDown)
            {
                NumericUpDown numeric = (NumericUpDown)sender;
                numeric.Select(0, numeric.Value.ToString().Length);
            }
            else if (sender is DateTimePicker)
            {
                DateTimePicker date = (DateTimePicker)sender;
                date.Select();
            }

            else
            {
                Control ctrl = (Control)sender;
                ctrl.Select();
            }
        }

        public static bool fnc_isNumeric(object value)
        {
            if (value.GetType() == typeof(byte)
                || value.GetType() == typeof(sbyte)
                || value.GetType() == typeof(UInt16)
                || value.GetType() == typeof(UInt32)
                || value.GetType() == typeof(UInt64)
                || value.GetType() == typeof(Int16)
                || value.GetType() == typeof(Int32)
                || value.GetType() == typeof(Int64)
                || value.GetType() == typeof(Single)
                || value.GetType() == typeof(Decimal)
                || value.GetType() == typeof(decimal)
                || value.GetType() == typeof(double)
                || value.GetType() == typeof(float)
                || value.GetType() == typeof(int)
                || value.GetType() == typeof(long))
                return true;
            return false;
        }

        public static void sb_fillDatatable(string cnnstr, string command, DataTable dt)
        {
            SqlConnection cnn = new SqlConnection(cnnstr);
            Functions.sb_fillDatatable(cnn, command, dt);
            
        }

        public static void sb_fillDatatable(SqlConnection cnn, string command, DataTable dt)
        {
            SqlHelper.FillDataTable(cnn, CommandType.Text, command, dt);
        }

        public static object fnc_executeScalar(string cnnstr, string str)
        {
            try
            {
                return SqlHelper.ExecuteScalar(cnnstr, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                //sb_errorLoger(ex);
                return null;
            }
        }

        public static bool fnc_executeNonQuery(string cnnStr, string str)
        {
            SqlConnection cnn = new SqlConnection(cnnStr);
            SqlTransaction tran;
            cnn.Open();
            tran = cnn.BeginTransaction();
            bool retVal = false;
            try
            {
                SqlHelper.ExecuteNonQuery(tran, CommandType.Text, str);
                tran.Commit();
                retVal = true;

            }
            catch (Exception ex1)
            {
                tran.Rollback();
                retVal = false;

            }
            tran.Dispose();
            cnn.Close();
            return retVal;
        }

        public static System.Data.SqlClient.SqlDataReader fnc_executeReader(string cnnstr, string str)
        {
            try
            {
                return SqlHelper.ExecuteReader(cnnstr, CommandType.Text, str);
            }
            catch (Exception ex)
            {

                return null;
            }
        }

    }
}
