using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RailSiteDataGrabber.rwmms
{
    class webpageGridColumn
    {
        public webpageGridColumn(string colName, int colIndexInGrid, Type datatype)
        {
            this.prp_colIndexInGrid = colIndexInGrid;
            this.prp_colName = colName;
            this.prp_datatype = datatype;
        }
        public int prp_colIndexInGrid { get; set; }
        public string prp_colName { get; set; }

        public Type prp_datatype { get; set; }
    }

    class webpageGridColumnDetail : webpageGridColumn
    {
        public webpageGridColumnDetail(string colName, int colIndexInGrid, Type datatype, Type webpgType, By controlLoadDataBy)
            : this(colName, colIndexInGrid, datatype, webpgType, false, false, 0, controlLoadDataBy)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colIndexInGrid"></param>
        /// <param name="webpg">webpage that goes when click on this column/cell</param>
        public webpageGridColumnDetail(string colName, int colIndexInGrid, Type datatype, Type webpgType
            , bool getDetail, bool download, int startPageIndex, By controlLoadDataBy) : base(colName, colIndexInGrid, datatype)
        {
            this.prp_webpgType = webpgType;
            this.prp_controlLoadDataBy = controlLoadDataBy;
            
        }

        public Type prp_webpgType { get; set; }

        public bool prp_getDetail { get; set; }
        public bool prp_download { get; set; }

        public int prp_startPageIndex { get; set; }

        public By prp_controlLoadDataBy { get; set; }
        
    }

    class webpageGridColumnDownload : webpageGridColumn
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="colIndexInGrid"></param>
        /// <param name="datatype"></param>
        /// <param name="downloadDirectory"></param>
        /// <param name="fileDirPath"></param>
        /// <param name="fileName"> can be like this {1}_{2} it means replace {1} with cells[1] value and concat it with _cells[2] value</param>
        public webpageGridColumnDownload(string colName, int colIndexInGrid, Type datatype, string downloadDirectory
            , string fileDirPath, string fileName) : base(colName, colIndexInGrid, datatype)
        {
            this.prp_downloadDirectory = downloadDirectory;
            this.prp_fileDirPath = fileDirPath;
            this.prp_fileName = fileName;
        }
        public string prp_downloadDirectory { get; set; }
        public string prp_fileDirPath { get; set; }

        /// <summary>
        /// can be like this {1}_{2} it means replace {1} with cells[1] value and concat it with _cells[2] value
        /// </summary>
        public string prp_fileName { get; set; }

        public string fnc_getFileName(System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> htmlCells)
        {
            if (string.IsNullOrEmpty(this.prp_fileName) || !this.prp_fileName.Contains("{")) return this.prp_fileName;
            string fileName = this.prp_fileName;
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
}
