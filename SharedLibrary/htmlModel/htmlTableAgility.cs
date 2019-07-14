using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.htmlModel
{
    public class htmlTableAgility : DataTable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName">name of the table in db</param>
        /// <param name="page"></param>
        /// <param name="columnIdName"></param>
        /// <param name="gridBy">for those pages that have grid in them</param>
        /// <param name="comboBy">for those pages that have combo in them</param>
        public htmlTableAgility(string tableName, webpage page, string columnIdName, string gridId, string comboId) : base(tableName)
        {
            this.prp_webpage = page;

            this.v_handleRowChange = true;
            this.Columns.CollectionChanged += Columns_CollectionChanged;
            this.prp_gridId = gridId;
            this.prp_comboId = comboId;
            this.prp_columnIdName = columnIdName;
        }

        #region properties and variables
        public string prp_gridId { get; set; }
        public string prp_comboId { get; set; }
        /// <summary>
        /// if we have cimputed column we handle OnRowChange for preventing stackoverflow in OnRowChange
        /// </summary>
        bool v_handleRowChange = true;
        public webpage prp_webpage { get; }
        /// <summary>
        /// for nested tables (page which is detail of another page) this value is used to refer to parent id
        /// </summary>
        public int? prp_parentId
        {
            get
            {
                return this.prp_webpage.prp_parentId;
            }
        }

        public IWebDriver prp_webBrowser
        {
            get
            {
                return this.prp_webpage.prp_webBrowser;
            }
        }

        public By prp_controlLoadBy { get; set; }

        /// <summary>
        /// gor those which have combo or grid
        /// </summary>
        public int prp_skipRowTop
        {
            get; set;
        }

        /// <summary>
        /// for those which have combo or grid
        /// </summary>
        public int prp_skipRowBottom
        {
            get; set;
        }
        /// <summary>
        /// for those which have grid and pagination
        /// </summary>
        public By prp_rowCountBy
        {
            get; set;
        }
        /// <summary>
        /// if rowcount is specfied in specific pattern
        /// </summary>
        public string prp_rowCountLabelRegexPattern
        {
            get; set;
        }
        /// <summary>
        /// for grid it specifies number of rows in each page of grid
        /// </summary>
        public int prp_pageRowCount { get; set; }
        /// <summary>
        /// which indecies should be skipped
        /// </summary>
        public int[] prp_skipRowIndecies { get; set; }
        /// <summary>
        /// for update and insert it indicates when to update and when to insert.
        /// e.g. in table emplyers we use employername as identifier. in this table we have alternate names 
        /// which are other form of employername. when we create wherecondition to identify row. we check emplyerName column
        /// if it has alternateColumn we add those columns to our where condition
        /// </summary>
        public string[] prp_identifierColumnNames { get; set; }
        /// <summary>
        /// which column is primary key. it is used for saving information
        /// </summary>
        public string prp_columnIdName { get; set; }
        /// <summary>
        /// for example we have a grid with 1000 pages we fetched 1000 pages yesterday
        /// today site adds 10 pages to the BEGINING of the table so in normal style we should 
        /// fetch 1010 pages which is unnecessary. the alternate way is to get 10 new pages which are added to
        /// the BEGINING of the table. in this way we cannot use lastPageIndex because rows add to the begining
        /// of the table so we use this property to Stop Fetch if the record aleardy exists in db
        /// it is used for GRID controls
        /// </summary>
        public bool prp_stopFetchingWhenGotToExistedRows { get; set; }

        /// <summary>
        /// for those tables which have grid in them this property show the name of the pageIndex columnName
        /// to get the lastPageIndex to startWith
        /// </summary>
        public string prp_pageIndexColumnName { get; set; }
        /// <summary>
        /// for selecting last page index from table we should specify identifier columnnames for it
        /// e.g. we have rwmmsRepairs we have different companies in this table for example from railpardaz/seir and harkat and...
        /// each company hast its pages index and it means page 1 from railpardaz does not like page 1 from seir and harkat
        /// so for getting the lastPageIndex for railpardaz we should specify prp_identifierColumnNamesPageIndex for distinguishing between
        /// different companies
        /// </summary>
        public string[] prp_pageIndexIdentifierColumnNames { get; set; }

        /// <summary>
        /// identifies if pagesIndex is used in grid or not
        /// </summary>
        public bool prp_pageIndexUsed { get; set; } = true;
        #endregion

        #region subs and functions

        private void Columns_CollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            if (e.Action == CollectionChangeAction.Add)
            {
                int i;
                if (e.Element is htmlControlCombo)
                {

                    if (this.prp_comboId == null)
                    {
                        throw new Exception("prp_comboId is not specified");
                    }
                    for (i = 0; i <= this.Columns.Count - 2; i++)
                    {
                        if (this.Columns[i] is htmlControlGridDetailColumn
                            || this.Columns[i] is htmlControlGridDownloadColumn
                            || this.Columns[i] is htmlControlGridSimpleColumn)
                        {
                            throw new Exception("Cannot use gridcolumn and combocolumn simultaneously");
                        }
                    }

                }
                else if (e.Element is htmlControlGridDetailColumn
                           || e.Element is htmlControlGridDownloadColumn
                           || e.Element is htmlControlGridSimpleColumn)
                {
                    if (this.prp_gridId == null)
                    {
                        throw new Exception("prp_gridId is not specified");
                    }
                    for (i = 0; i <= this.Columns.Count - 2; i++)
                    {
                        if (this.Columns[i] is htmlControlCombo)
                        {
                            throw new Exception("Cannot use gridcolumn and combocolumn simultaneously");
                        }
                    }
                }

            }
        }

        protected override void OnRowChanged(DataRowChangeEventArgs e)
        {
            if (this.v_handleRowChange)
            {
                int i;
                columnComputedDateTime colDateTime;
                columnComputedTrueFalse colTrueFalse;
                for (i = 0; i <= this.Columns.Count - 1; i++)
                {
                    if (this.Columns[i] is columnComputedDateTime)
                    {
                        colDateTime = (columnComputedDateTime)this.Columns[i];
                        if (colDateTime.prp_solarDateTimeSourceColumnNames != null
                            && colDateTime.prp_solarDateTimeSourceColumnNames.Length > 0)
                        {
                            this.v_handleRowChange = false;
                            e.Row[colDateTime] = colDateTime.fnc_getColumnValue(e.Row);
                            this.v_handleRowChange = true;
                        }
                    }
                    else if (this.Columns[i] is columnComputedTrueFalse)
                    {
                        colTrueFalse = (columnComputedTrueFalse)this.Columns[i];
                        if (colTrueFalse.prp_sourceColumnName != null
                            && colTrueFalse.prp_valuesTrue != null
                            && colTrueFalse.prp_valuesFalse != null)
                        {
                            this.v_handleRowChange = false;
                            e.Row[colTrueFalse] = colTrueFalse.fnc_getColumnValue(e.Row);
                            this.v_handleRowChange = true;
                        }
                    }
                }
            }
            base.OnRowChanged(e);
        }
        public virtual bool fnc_save(iLogin login)
        {
            return this.fnc_save(false, false, 1,login);
        }
        public virtual bool fnc_save(bool getDetail, bool download, bool startFromLastPage,iLogin login)
        {
            int startPageIndex = 1;
            if (startFromLastPage && !string.IsNullOrEmpty(this.prp_pageIndexColumnName))
            {
                startPageIndex = this.fnc_getLastPageIndex();
            }
            return this.fnc_save(getDetail, download, startPageIndex, login);

        }
        public virtual bool fnc_save(bool getDetail, bool download, int startPageIndex, iLogin login)
        {
            if (this.prp_controlLoadBy != null)
            {
                this.prp_webBrowser.FindElement(this.prp_controlLoadBy).Click();
                Functions.sb_waitForReady(this.prp_webBrowser);
            }
            string errMsg;
            if (!this.fnc_checkIdentifierColumns(out errMsg))
            {
                throw new Exception(errMsg);
            }
            int i;
            int? idValue;
            bool stopFetch = false;
            if (this.prp_gridId != null)
            {
                if (!this.fnc_saveGrid(getDetail, download, startPageIndex, login))
                    return false;
            }
            else if (this.prp_comboId != null)
            {
                IWebElement combo = this.prp_webBrowser.FindElement(this.prp_comboId);
                var items = combo.FindElements(By.TagName("option"));
                for (i = this.prp_skipRowTop; i <= items.Count - this.prp_skipRowBottom - 1 && !stopFetch; i++)
                {
                    if (this.prp_skipRowIndecies != null && this.prp_skipRowIndecies.Any(o => o == i)) continue;
                    idValue = this.fnc_saveValues(items[i], download, out stopFetch);
                    if (!idValue.HasValue) return false;
                }
            }
            else
            {
                idValue = this.fnc_saveValues(null, download, out stopFetch);
                if (!idValue.HasValue) return false;
            }
            return true;
        }

        private bool fnc_checkIdentifierColumns(out string errMsg)
        {
            errMsg = "";
            if (this.prp_identifierColumnNames == null || this.prp_identifierColumnNames.Length == 0)
                return true;
            int i;
            for (i = 0; i <= this.prp_identifierColumnNames.Length - 1; i++)
            {
                if (!this.Columns.Contains(this.prp_identifierColumnNames[i]))
                {
                    errMsg = "Table " + this.TableName + " does not contain " + this.prp_identifierColumnNames[i] + " as its Columns Collection";
                    return false;
                }
            }
            return true;
        }
        public int fnc_getLastPageIndex()
        {
            int? startPageIndex = 1;
            if (string.IsNullOrEmpty(this.TableName))
                throw new Exception("TableName is not specified");
            if (string.IsNullOrEmpty(this.prp_pageIndexColumnName))
                throw new Exception("prp_columnPageIndexName is not specified");
            string whereCondition = "";
            using (var entityLogistic = new logisticEntities())
            {
                if (this.prp_pageIndexIdentifierColumnNames != null && this.prp_pageIndexIdentifierColumnNames.Length > 0)
                {
                    Dictionary<string, object> dicValues = this.fnc_getValues(null, false, this.prp_pageIndexIdentifierColumnNames);
                    whereCondition = this.fnc_getWhereCondition(dicValues, this.prp_pageIndexIdentifierColumnNames);
                }
                startPageIndex = entityLogistic.Database.SqlQuery<int?>("select max(" + this.prp_pageIndexColumnName + ") from " + this.TableName + (string.IsNullOrEmpty(whereCondition) ? "" : " where " + whereCondition)).FirstOrDefault();

                if (!startPageIndex.HasValue) return 1;
                return startPageIndex.Value;
            }
        }

        private bool fnc_saveGrid(bool getDetail, bool download, int startPageIndex, iLogin login)
        {
            int i;
            int pageIndex;
            IWebElement grid = this.prp_webBrowser.FindElement(this.prp_gridId);
            int pageCount;
            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> htmlRows;
            int? idValue;
            bool stopFetch = false;

            if (!this.prp_pageIndexUsed)
            {
                startPageIndex = 1;
                pageCount = 1;
            }
            else
            {
                pageCount = htmlGrid.fnc_getPageCount(this.prp_webBrowser, this.prp_webpage.prp_url, grid, this.fnc_getRowCountHtmlElement(), null, this.prp_pageRowCount, false, login);
            }

            for (pageIndex = startPageIndex; pageIndex <= pageCount && !stopFetch; pageIndex++)
            {
                if (pageIndex > 1)
                {
                    pageIndex = htmlGrid.fnc_gotoPage(this.prp_webBrowser, this.prp_webpage.prp_url, this.prp_gridId, this.fnc_getRowCountHtmlElement(), pageIndex, pageCount, false
                        , this.prp_rowCountLabelRegexPattern, this.prp_pageRowCount, login, true);
                    if (pageIndex == -1) return false;
                    grid = this.prp_webBrowser.FindElement(this.prp_gridId);
                }
                htmlRows = grid.FindElements(By.TagName("tr"));
                i = this.prp_skipRowTop;
                while (i <= htmlRows.Count - this.prp_skipRowBottom - 1 && !stopFetch)
                //for (i = this.prp_skipRowTop; i <= htmlRows.Count - 1 - this.prp_skipRowBottom; i++)
                {
                    if (this.prp_skipRowIndecies != null && this.prp_skipRowIndecies.Any(o => o == i))
                        continue;
                    idValue = this.fnc_saveValues(htmlRows[i], download, out stopFetch);

                    if (!idValue.HasValue) return false;
                    if (getDetail)
                    {
                        if (!this.fnc_gotoDetail(idValue.Value, i))
                            return false;
                        //after going to detail reattach/renew grid
                        grid = this.prp_webBrowser.FindElement(this.prp_gridId);
                        htmlRows = grid.FindElements(By.TagName("tr"));
                    }
                    i++;
                }
            }

            return true;
        }

        private bool fnc_gotoDetail(int parentId, int rowIndex)
        {
            int j;
            htmlControlGridDetailColumn colDetail;
            Type webpageType;
            object page;
            bool retvalLoad, retvalSave;
            for (j = 0; j <= this.Columns.Count - 1; j++)
            {
                if (this.Columns[j] is htmlControlGridDetailColumn)
                {
                    colDetail = (htmlControlGridDetailColumn)this.Columns[j];

                    IWebElement grid = this.prp_webBrowser.FindElement(this.prp_gridId);
                    var htmlRows = grid.FindElements(By.TagName("tr"));
                    var detailCell = htmlRows[rowIndex].FindElements(By.TagName("td"))[colDetail.prp_gridColIndex];
                    detailCell.Click();
                    Functions.sb_waitForReady(this.prp_webBrowser);


                    webpageType = colDetail.prp_webpageType;
                    page = Activator.CreateInstance(webpageType, parentId, (ChromeDriver)this.prp_webBrowser);

                    //page.prp_parentId = parentId;
                    webpageType.GetProperty("prp_parentId").SetValue(page, parentId);
                    retvalLoad = (bool)webpageType.GetMethod("fnc_load").Invoke(page, null);
                    if (retvalLoad)
                    {
                        MethodInfo mi = webpageType.GetMethod("fnc_save", new Type[] { typeof(bool), typeof(bool), typeof(bool) });
                        retvalSave = (bool)mi.Invoke(page, new object[] { colDetail.prp_getDetail, colDetail.prp_download, colDetail.prp_startLastPageIndex });
                        if (!retvalSave)
                            return false;
                        webpageType.GetMethod("sb_back").Invoke(page, null);
                        Functions.sb_waitForReady(this.prp_webBrowser);
                    }
                    //return false;
                    //colDetail.prp
                    //dr[this.Columns[j]] = ((htmlControlGridDownloadColumn)this.Columns[j]).fnc_getColumnValue();
                }

            }
            return true;
        }
        private int? fnc_saveValues(IWebElement element, bool download, out bool stopFetch)
        {
            stopFetch = false;
            Dictionary<string, object> dic = this.fnc_getValues(element, download);
            string whereCondition = this.fnc_getWhereCondition(dic, this.prp_identifierColumnNames);

            using (var entity = new logisticEntities())
            {
                int? idValue;
                var exists = entity.Database.SqlQuery<int?>("select top 1 1 from " + this.TableName + (string.IsNullOrEmpty(whereCondition) ? "" : " where " + whereCondition)).FirstOrDefault();
                if (exists.HasValue)
                {
                    idValue = this.fnc_update(dic, whereCondition);
                    if (this.prp_stopFetchingWhenGotToExistedRows)
                        stopFetch = true;
                    return idValue;
                }
                else
                {
                    idValue = this.fnc_insert(dic);
                    return idValue;
                }
            }

        }

        private Dictionary<string, object> fnc_getValues(IWebElement element, bool download, string[] columnNames)
        {
            if (columnNames == null || columnNames.Length == 0)
                return null;
            DataColumn[] columnsArr = new DataColumn[columnNames.Length];
            int i;
            for (i = 0; i <= columnNames.Length - 1; i++)
                columnsArr[i] = this.Columns[columnNames[i]];
            return this.fnc_getValues(element, download, columnsArr);
        }
        private Dictionary<string, object> fnc_getValues(IWebElement element, bool download)
        {
            DataColumn[] columnsArr = new DataColumn[this.Columns.Count];
            this.Columns.CopyTo(columnsArr, 0);
            return this.fnc_getValues(element, download, columnsArr);
        }

        private Dictionary<string, object> fnc_getValues(IWebElement element, bool download, DataColumn[] columnCollection)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

            int j;
            for (j = 0; j <= columnCollection.Length - 1; j++)
            {
                if (columnCollection[j] is htmlControlCombo)
                {
                    dic.Add(columnCollection[j].ColumnName, ((htmlControlCombo)columnCollection[j]).fnc_getColumnValue(element));
                }
                else if (columnCollection[j] is htmlControlComboSelectedItem)
                {
                    dic.Add(columnCollection[j].ColumnName, ((htmlControlComboSelectedItem)columnCollection[j]).fnc_getColumnValue());
                }
                else if (columnCollection[j] is htmlControlGridSimpleColumn)
                {
                    dic.Add(columnCollection[j].ColumnName, ((htmlControlGridSimpleColumn)columnCollection[j]).fnc_getColumnValue(element));
                }
                else if (columnCollection[j] is htmlControlGridDetailColumn)
                {
                    //if (getDetail)
                    //{

                    //}
                    //dr[columnCollection[j]] = ((htmlControlGridDownloadColumn)columnCollection[j]).fnc_getColumnValue();
                }
                else if (columnCollection[j] is htmlControlGridDownloadColumn)
                {
                    if (download)
                        dic.Add(columnCollection[j].ColumnName, ((htmlControlGridDownloadColumn)columnCollection[j]).fnc_getColumnValue(element));
                }
                else if (columnCollection[j] is htmlControlDownload)
                {
                    dic.Add(columnCollection[j].ColumnName, ((htmlControlDownload)columnCollection[j]).fnc_getColumnValue());
                }
                else if (columnCollection[j] is htmlControlParent)
                {
                    dic.Add(columnCollection[j].ColumnName, ((htmlControlParent)columnCollection[j]).fnc_getColumnValue());

                }
                else if (columnCollection[j] is htmlControl)
                {
                    dic.Add(columnCollection[j].ColumnName, ((htmlControl)columnCollection[j]).fnc_getColumnValue());

                }
            }
            #region computed columns like computedDate and computedReference
            for (j = 0; j <= columnCollection.Length - 1; j++)
            {
                if (columnCollection[j] is columnComputedDateTime)
                {
                    dic.Add(columnCollection[j].ColumnName, ((columnComputedDateTime)columnCollection[j]).fnc_getColumnValue(dic));
                }
                else if (columnCollection[j] is columnComputedReference)
                {
                    dic.Add(columnCollection[j].ColumnName, ((columnComputedReference)columnCollection[j]).fnc_getColumnValue(dic));
                }
                else if (columnCollection[j] is columnComputedTrueFalse)
                {
                    dic.Add(columnCollection[j].ColumnName, ((columnComputedTrueFalse)columnCollection[j]).fnc_getColumnValue(dic));
                }
            }
            #endregion
            return dic;
        }

        private int? fnc_update(Dictionary<string, object> dic, string whereCondition)
        {
            string cmd = "";
            int i;
            object value;

            for (i = 0; i <= dic.Count - 1; i++)
            {
                if (this.Columns[dic.ElementAt(i).Key] is htmlControl
                    && !string.IsNullOrEmpty(((htmlControl)this.Columns[dic.ElementAt(i).Key]).prp_alternateColName))
                {
                    //those columns which have alternate names should not be updated
                    //for example we have companyName ='microsoft' column which has alternatenames = 'micro soft'
                    //now we have 'micro soft' as value so we should not update companyName with 'micro soft'
                    continue;
                }
                if (this.Columns[dic.ElementAt(i).Key] is htmlControl
                    && !((htmlControl)this.Columns[dic.ElementAt(i).Key]).prp_saveToDB)
                {
                    //some columns like yes/no can be ignored and save as bit via computedcolumn
                    continue;
                }
                value = dic.ElementAt(i).Value;

                if (Functions.IsNull(value))
                    cmd = cmd + dic.ElementAt(i).Key + "=" + " Null " + ",";
                else if (Functions.fnc_isNumeric(value))
                    cmd = cmd + dic.ElementAt(i).Key + "=" + value.ToString() + ",";
                else
                    cmd = cmd + dic.ElementAt(i).Key + "='" + value.ToString() + "',";
            }
            if (!string.IsNullOrEmpty(cmd))
            {
                cmd = cmd.Remove(cmd.Length - 1, 1);
            }
            cmd = " update " + this.TableName + " set " + cmd
                + " output inserted." + this.prp_columnIdName + (string.IsNullOrEmpty(whereCondition) ? "" : " where " + whereCondition);
            using (var entity = new logisticEntities())
            {
                try
                {
                    int? id = entity.Database.SqlQuery<int?>(cmd).FirstOrDefault();
                    return id;
                }
                catch (Exception ex)
                {
                    SharedVariables.logs.Error("Error in fnc_update", ex);
                    //return null;
                }
                return null;
            }
        }

        private int? fnc_insert(Dictionary<string, object> dic)
        {
            string columnNames = "";
            string columnValues = "";
            string cmd;
            int i;
            object value;
            for (i = 0; i <= dic.Count - 1; i++)
            {
                if (this.Columns[dic.ElementAt(i).Key] is htmlControl
                    && !((htmlControl)this.Columns[dic.ElementAt(i).Key]).prp_saveToDB)
                {
                    //some columns like yes/no can be ignored and save as bit via computedcolumn
                    continue;
                }
                value = dic.ElementAt(i).Value;
                if (Functions.IsNull(value))
                    columnValues = columnValues + " Null " + ",";
                else if (Functions.fnc_isNumeric(value))
                    columnValues = columnValues + value + ",";
                else
                    columnValues = columnValues + "'" + value + "',";

                columnNames = columnNames + dic.ElementAt(i).Key + ",";
            }
            if (!string.IsNullOrEmpty(columnNames))
            {
                columnNames = columnNames.Remove(columnNames.Length - 1, 1);
                columnValues = columnValues.Remove(columnValues.Length - 1, 1);
            }
            cmd = " insert into " + this.TableName + "(" + columnNames + ")" + " OUTPUT inserted." + this.prp_columnIdName + " values(" + columnValues + ")";
            using (var entity = new logisticEntities())
            {
                try
                {
                    int? id = entity.Database.SqlQuery<int?>(cmd).FirstOrDefault();
                    return id;
                }
                catch (Exception ex)
                {
                    SharedVariables.logs.Error("Error in fnc_insert", ex);
                }
                return null;
            }
        }


        private string fnc_getWhereCondition(Dictionary<string, object> dicValues, string[] identifierColumnNames)
        {
            int i;
            if (identifierColumnNames == null || identifierColumnNames.Length == 0)
            {
                return null;
            }

            string whereCondition = "";
            string replaceKaYaColumn = "";
            string replaceKaYaValue = "";
            string replaceKaYaAlternate = "";
            DataColumn column;
            object columnValue;
            for (i = 0; i <= dicValues.Count - 1; i++)
            {

                column = this.Columns[dicValues.ElementAt(i).Key];

                columnValue = dicValues.ElementAt(i).Value;
                if (identifierColumnNames.Contains(column.ColumnName, StringComparer.InvariantCultureIgnoreCase))
                {
                    //if (Functions.IsNull(Columns[j])) throw new Exception("identifierColumnName value is null (identifierColumnName = " + this.prp_identifierColumnNames[j] + ")");
                    if (Functions.IsNull(columnValue)) //throw new Exception("identifierColumn Value is null (colName = " + htmlCol.ColumnName + ")");
                    {
                        whereCondition = whereCondition + column.ColumnName + " is null " + " and ";
                    }
                    else if (Functions.fnc_isNumeric(columnValue))
                    {
                        replaceKaYaValue = "Replace(Replace(N'" + columnValue + "','ي','ی'),'ك','ک')";
                        if ((column is htmlControl) && !string.IsNullOrEmpty(((htmlControl)column).prp_alternateColName))
                            replaceKaYaAlternate = "Replace(Replace(" + ((htmlControl)column).prp_alternateColName + ",'ي','ی'),'ك','ک')";
                        else replaceKaYaAlternate = "";
                        whereCondition = whereCondition + "(" + column.ColumnName + "=" + columnValue
                            + ((!(column is htmlControl) || ((column is htmlControl) && string.IsNullOrEmpty(((htmlControl)column).prp_alternateColName)))
                            ? ""
                            : " or " + replaceKaYaAlternate + " like cast(" + replaceKaYaValue + "as nvarchar(50))+';%'" + " or " + replaceKaYaAlternate + " like '%;'+ cast(" + replaceKaYaValue + " as nvarchar(50)) + ';%'")
                            + ") and ";
                    }
                    else
                    {
                        replaceKaYaColumn = "Replace(Replace(" + column.ColumnName + " , N'ي', N'ی'), N'ك', N'ک')";
                        replaceKaYaValue = "Replace(Replace(N'" + columnValue + "', N'ي', N'ی'), N'ك', N'ک')";
                        if ((column is htmlControl) && !string.IsNullOrEmpty(((htmlControl)column).prp_alternateColName))
                            replaceKaYaAlternate = "Replace(Replace(" + ((htmlControl)column).prp_alternateColName + ", N'ي', N'ی'), N'ك', N'ک')";
                        else replaceKaYaAlternate = "";

                        whereCondition = whereCondition + "(" + replaceKaYaColumn + "=" + replaceKaYaValue
                            + ((!(column is htmlControl) || ((column is htmlControl) && string.IsNullOrEmpty(((htmlControl)column).prp_alternateColName)))
                                ? ""
                                : " or " + replaceKaYaAlternate + " like " + replaceKaYaValue + "+';%'" + " or " + replaceKaYaAlternate + " like '%;'+" + replaceKaYaValue + "+';%'")
                            + ") and ";
                    }
                    //whereCondition = this.prp_identifierColumnNames[j] + "'" +
                }


            }
            if (!string.IsNullOrEmpty(whereCondition)) whereCondition = whereCondition.Remove(whereCondition.Length - 4, 4);
            return whereCondition;
        }

        public IWebElement fnc_getRowCountHtmlElement()
        {
            if (this.prp_rowCountBy != null)
            {
                return this.prp_webBrowser.FindElement(this.prp_rowCountBy);
            }
            return null;
        }
        #endregion
    }


}