using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SharedLibrary.htmlModel
{
    public class htmlControl : DataColumn
    {
        public By prp_controlBy { get; set; }
        public string prp_controlAtt { get; set; }
        public string prp_alternateColName { get; set; }

        //some columns like yes/no can be ignored and save as bit via computedcolumn
        public bool prp_saveToDB { get; set; }
        public htmlControl(string columnName, Type dataType, By controlBy, string controlAtt
            , string alternateColName) : this(columnName, dataType, controlBy, controlAtt, alternateColName, true)
        {

        }

        public htmlControl(string columnName, Type dataType, By controlBy, string controlAtt
           , string alternateColName, bool saveToDB) : base(columnName, dataType)
        {
            this.prp_controlBy = controlBy;
            this.prp_controlAtt = controlAtt;
            this.prp_alternateColName = alternateColName;
            this.prp_saveToDB = saveToDB;
        }
        public IWebElement fnc_getElement()
        {
            return ((htmlTable)this.Table).prp_webBrowser.FindElement(this.prp_controlBy);
        }
        public virtual object fnc_getColumnValue()
        {
            IWebElement item = this.fnc_getElement();
            object value;

            if (this.prp_controlAtt == "text" || string.IsNullOrEmpty(this.prp_controlAtt))
                value = item.Text;
            else value = item.GetAttribute(this.prp_controlAtt);

            if (Functions.IsNull(value)) return null;
            Convert.ChangeType(value, this.DataType);
            return value;
        }
    }

    public class htmlControlRegex : htmlControl
    {
        public htmlControlRegex(string columnName, Type dataType, By controlBy, string controlAtt, string regexPattern, int matchGroupIndex
          , string alternateColName) : this(columnName, dataType, controlBy, controlAtt, regexPattern, matchGroupIndex, alternateColName, true)
        {

        }

        public htmlControlRegex(string columnName, Type dataType, By controlBy, string controlAtt, string regexPattern, int matchGroupIndex
           , string alternateColName, bool saveToDB) : base(columnName, dataType, controlBy, controlAtt, alternateColName, saveToDB)
        {
            this.prp_regexPattern = regexPattern;
            this.prp_matchGroupIndex = matchGroupIndex;
        }

        public string prp_regexPattern { get; set; }

        public int prp_matchGroupIndex { get; set; }
        public override object fnc_getColumnValue()
        {
            object value = base.fnc_getColumnValue();
            if (!string.IsNullOrEmpty(this.prp_regexPattern) && value != null && value != Convert.DBNull && !string.IsNullOrEmpty(value.ToString()))
            {
                string valueStr = value.ToString().Replace("&nbsp;", " ");
                Match mtch = Regex.Match(value.ToString(), this.prp_regexPattern);
                if (mtch.Success
                    && mtch.Groups.Count >= this.prp_matchGroupIndex)
                {
                    return mtch.Groups[this.prp_matchGroupIndex].Value;
                }
            }


            return value;
        }
    }

    public class htmlControlGridSimpleColumn : htmlControl
    {
        public int prp_gridColIndex { get; set; }
        public htmlControlGridSimpleColumn(string columnName, Type dataType, int gridColIndex
          , string alternateColName) : this(columnName, dataType, gridColIndex, null, null)
        {


        }

        public htmlControlGridSimpleColumn(string columnName, Type dataType, int gridColIndex, string controlAtt
        , string alternateColName) : this(columnName, dataType, gridColIndex, controlAtt, alternateColName, true)
        {

        }

        public htmlControlGridSimpleColumn(string columnName, Type dataType, int gridColIndex, string controlAtt
           , string alternateColName, bool saveToDB) : base(columnName, dataType, null, null, alternateColName, saveToDB)
        {
            this.prp_gridColIndex = gridColIndex;
        }

        public object fnc_getColumnValue(IWebElement row)
        {
            object value;

            var cells = row.FindElements(By.TagName("td"));
            if (cells.Count >= 0 && this.prp_gridColIndex < cells.Count)
            {
                if (this.prp_controlAtt == "text" || string.IsNullOrEmpty(this.prp_controlAtt))
                    value = Convert.ChangeType(cells[this.prp_gridColIndex].Text, this.DataType);
                else value = Convert.ChangeType(cells[this.prp_gridColIndex].GetAttribute(this.prp_controlAtt), this.DataType);
                return value;
            }
            throw new Exception("tableColIndex(" + this.prp_gridColIndex + ") is not in range(0," + cells.Count.ToString() + ")");

        }
    }

    public class htmlControlGridDetailColumn : htmlControl
    {
        public int prp_gridColIndex { get; set; }
        public Type prp_webpageType { get; set; }
        public bool prp_getDetail { get; set; }
        public bool prp_download { get; set; }

        public bool prp_startLastPageIndex { get; set; }
        public htmlControlGridDetailColumn(string columnName, Type dataType, int gridColIndex, Type pageType)
            : this(columnName, dataType, gridColIndex, pageType, false, false, false)
        {
        }

        public htmlControlGridDetailColumn(string columnName, Type dataType, int gridColIndex, Type pageType, bool getDetail
            , bool download, bool startLastPageIndex)
            : base(columnName, dataType, null, null, null, false)
        {
            this.prp_gridColIndex = gridColIndex;
            this.prp_webpageType = pageType;
            this.prp_getDetail = getDetail;
            this.prp_download = download;
            this.prp_startLastPageIndex = startLastPageIndex;
        }

        public object fnc_getColumnValue(IWebElement row)
        {
            return null;

        }
    }

    public class htmlControlGridDownloadColumn : htmlControlDownload
    {
        public int prp_gridColIndex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="datatype"></param>
        /// <param name="browserDownloadDirectory"></param>
        /// <param name="fileDestinationDirectory"></param>
        /// <param name="fileName"> can be like this {1}_{2} it means replace {1} with cells[1] value and concat it with _cells[2] value</param>
        public htmlControlGridDownloadColumn(string columnName, Type datatype, int gridColIndex, string browserDownloadDirectory
            , string fileDestinationDirectory, string fileName) : base(columnName, datatype, null, browserDownloadDirectory, fileDestinationDirectory, fileName)
        {
            this.prp_browserDownloadDirectory = browserDownloadDirectory;
            this.prp_fileDestinationDirectory = fileDestinationDirectory;
            this.prp_fileNameWithoutExtension = fileName;
            this.prp_gridColIndex = gridColIndex;
        }

        public object fnc_getColumnValue(IWebElement row)
        {
            string fileName = this.fnc_getFileName(row.FindElements(By.TagName("td")));

            var cells = row.FindElements(By.TagName("td"));
            if (cells.Count >= 0 && this.prp_gridColIndex < cells.Count)
            {

                return base.fnc_downloadFile(cells[this.prp_gridColIndex], fileName);
            }
            throw new Exception("tableColIndex(" + this.prp_gridColIndex + ") is not in range(0," + cells.Count.ToString() + ")");

        }
        private string fnc_getFileName(System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> htmlCells)
        {
            if (string.IsNullOrEmpty(this.prp_fileNameWithoutExtension) || !this.prp_fileNameWithoutExtension.Contains("{")) return this.prp_fileNameWithoutExtension;
            string fileName = this.prp_fileNameWithoutExtension;
            MatchCollection matchCollection = Regex.Matches(fileName, "{[\\d]}");
            int cellIndex;
            string cellValue;
            foreach (Match m in matchCollection)
            {
                cellIndex = int.Parse(m.Value.Replace("{", "").Replace("}", ""));
                if (cellIndex < 0 || cellIndex >= htmlCells.Count)
                    throw new Exception("cellIndex(" + cellIndex + ") in prp_fileName in not in range of cells count (" + htmlCells.Count + ")");
                cellValue = htmlCells[cellIndex].Text.Replace("&nbps;", "").TrimEnd().TrimStart();
                fileName = fileName.Replace(m.Value, cellValue);
            }
            return fileName;
        }

    }

    public class htmlControlDownload : htmlControl
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="datatype"></param>
        /// <param name="browserDownloadDirectory"></param>
        /// <param name="fileDestinationDirectory"></param>
        /// <param name="fileNameWithoutExtension"></param>
        public htmlControlDownload(string columnName, Type datatype, By controlBy, string browserDownloadDirectory
            , string fileDestinationDirectory, string fileNameWithoutExtension) : base(columnName, datatype, controlBy, null, null)
        {
            this.prp_browserDownloadDirectory = browserDownloadDirectory;
            this.prp_fileDestinationDirectory = fileDestinationDirectory;
            this.prp_fileNameWithoutExtension = fileNameWithoutExtension;
        }
        public string prp_browserDownloadDirectory { get; set; }
        public string prp_fileDestinationDirectory { get; set; }

        public string prp_fileNameWithoutExtension { get; set; }

        public override object fnc_getColumnValue()
        {
            IWebElement elementDownload = this.fnc_getElement();
            return this.fnc_getColumnValue(elementDownload);
        }

        public object fnc_getColumnValue(IWebElement elementDownload)
        {
            return this.fnc_downloadFile(elementDownload, this.prp_fileNameWithoutExtension);
        }
        protected string fnc_downloadFile(IWebElement elementDownload, string fileNameWithoutExtension)
        {
            elementDownload.Click();
            Functions.sb_waitForReady(((htmlTable)this.Table).prp_webBrowser);
            FileInfo myFile;
            do
            {
                myFile = (new DirectoryInfo(this.prp_browserDownloadDirectory)).GetFiles("*.*").OrderByDescending(o => o.LastWriteTime).FirstOrDefault();

                if (myFile == null || myFile.LastWriteTime < DateTime.Now.AddMinutes(-5))
                {
                    //there is error
                    throw new Exception("File cannot be downloaded");
                }
                else if (myFile.Extension != ".crdownload" && myFile.Extension != ".tmp")
                {
                    if (string.IsNullOrEmpty(this.prp_fileNameWithoutExtension))
                        this.prp_fileNameWithoutExtension = myFile.Name;
                    char[] chArrInvalidFileChars = Path.GetInvalidFileNameChars();
                    char[] chArrInvalidPathChars = Path.GetInvalidPathChars();
                    fileNameWithoutExtension = string.Join("_", fileNameWithoutExtension.Split(chArrInvalidFileChars));
                    fileNameWithoutExtension = string.Join("_", fileNameWithoutExtension.Split(chArrInvalidPathChars));
                    fileNameWithoutExtension = this.prp_fileDestinationDirectory + "\\" + fileNameWithoutExtension + myFile.Extension;
                    if (!Directory.Exists(this.prp_fileDestinationDirectory))
                        Directory.CreateDirectory(this.prp_fileDestinationDirectory);
                    if (File.Exists(fileNameWithoutExtension))
                        File.Delete(fileNameWithoutExtension);

                    myFile.MoveTo(fileNameWithoutExtension);

                    return fileNameWithoutExtension;


                }
            } while (myFile.Extension == ".crdownload" || myFile.Extension == ".tmp"/*wait till .crdownload or .tmp to change to .pdf*/);
            return null;
        }
    }

    public class htmlControlCombo : htmlControl
    {
        public htmlControlCombo(string columnName, Type dataType, string controlAtt
           , string alternateColName) : base(columnName, dataType, null, controlAtt, alternateColName)
        {
        }

        public object fnc_getColumnValue(IWebElement comboItem)
        {
            object value;
            if (this.prp_controlAtt == "text" || string.IsNullOrEmpty(this.prp_controlAtt))
                value = Convert.ChangeType(comboItem.Text, this.DataType);
            else value = Convert.ChangeType(comboItem.GetAttribute(this.prp_controlAtt), this.DataType);
            return value;
        }
    }

    public class htmlControlComboSelectedItem : htmlControl
    {
        public htmlControlComboSelectedItem(string columnName, Type dataType, By controlBy, string controlAtt
           , string alternateColName) : this(columnName, dataType, controlBy, controlAtt, alternateColName, true)
        {
        }

        public htmlControlComboSelectedItem(string columnName, Type dataType, By controlBy, string controlAtt
           , string alternateColName, bool saveToDB) : base(columnName, dataType, controlBy, controlAtt, alternateColName, saveToDB)
        {
        }
        public override object fnc_getColumnValue()
        {
            var element = ((htmlTable)this.Table).prp_webBrowser.FindElement(this.prp_controlBy);
            object value = null;
            if (element != null)
            {
                try
                {
                    SelectElement selectedItem = new SelectElement(element);
                    if (selectedItem != null && selectedItem.SelectedOption != null)
                    {
                        if (string.IsNullOrEmpty(this.prp_controlAtt) || this.prp_controlAtt.ToLower() == "text")
                        {
                            value = selectedItem.SelectedOption.Text.Replace("&nbsp;", " ").TrimStart().TrimEnd();
                        }
                        else
                        {
                            value = selectedItem.SelectedOption.GetAttribute(this.prp_controlAtt).Replace("&nbsp;", " ").TrimStart().TrimEnd();
                        }
                    }
                }
                catch (Exception ex)
                {
                    SharedVariables.logs.Error("htmlControlComboSelectedItem(" + this.prp_controlBy.ToString() + "):getColumnValu", ex);
                }
            }

            return value;
        }
    }
    public class htmlControlParent : htmlControl
    {
        public htmlControlParent(string columnName, Type dataType) : this(columnName, dataType, true)
        {
        }
        public htmlControlParent(string columnName, Type dataType, bool saveToDB) : base(columnName, dataType, null, null, null, saveToDB)
        {
        }
        public override object fnc_getColumnValue()
        {
            if (this.Table == null || !(this.Table is htmlTable)) return null;
            return ((htmlTable)this.Table).prp_parentId;
        }
    }

}
