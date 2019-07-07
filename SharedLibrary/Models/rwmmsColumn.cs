using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models
{
    public class htmlColumn : DataColumn
    {
        public enum enum_controlType
        {
            input = 0,
            inputCheckBox = 1,
            textArea = 2,
            gridCell = 3
        }

        public enum_controlType prp_controlType { get; set; }
        public int? prp_tableColIndex { get; set; }
        public string prp_controlId { get; set; }
        public string prp_controlAtt { get; set; }
        public string prp_alternateColName { get; set; }

        public htmlColumn(string columnName, Type dataType, int? tableColIndex, string controlId) : this(columnName, dataType, tableColIndex, controlId, enum_controlType.input)
        {

        }

        public htmlColumn(string columnName, Type dataType, int? tableColIndex, string controlId, enum_controlType controlType) : this(columnName, dataType, tableColIndex, controlId, null, null, enum_controlType.input)
        {

        }

        public htmlColumn(string columnName, Type dataType, int? tableColIndex, string controlId, string controlAtt
            , string alternateColName, enum_controlType controlType) : base(columnName, dataType)
        {
            this.prp_tableColIndex = tableColIndex;
            this.prp_controlId = controlId;
            this.prp_controlType = controlType;
            this.prp_controlAtt = controlAtt;
            this.prp_alternateColName = alternateColName;
        }

        public object fnc_getColumnValueFromControl(IWebElement item)
        {
            object value;
            if (this.prp_controlType == enum_controlType.gridCell
                && this.prp_tableColIndex.HasValue)
            {
                var cells = item.FindElements(By.TagName("td"));
                if (cells.Count >= 0 && this.prp_tableColIndex.Value < cells.Count)
                {
                    if (this.prp_controlAtt == "text" || string.IsNullOrEmpty(this.prp_controlAtt))
                        value = Convert.ChangeType(cells[this.prp_tableColIndex.Value].Text, this.DataType);
                    else value = Convert.ChangeType(cells[this.prp_tableColIndex.Value].GetAttribute(this.prp_controlAtt), this.DataType);
                    return value;
                }
                throw new Exception("tableColIndex(" + this.prp_tableColIndex.Value + ") is not in range(0," + cells.Count.ToString() + ")");
            }
            else
            {
                if (this.prp_controlAtt == "text" || string.IsNullOrEmpty(this.prp_controlAtt))
                    value = Convert.ChangeType(item.Text, this.DataType);
                else value = Convert.ChangeType(item.GetAttribute(this.prp_controlAtt), this.DataType);
                return value;
            }

            return null;
        }
    }

    public class dateTimeComputedColumn : DataColumn
    {
        public enum enumDateTimeComputeType
        {
            solarDateToGregorianDate,
            timeIntToTime,
            solarDateTimeIntToGregorianDateTime,
            solarDateTimeToGregorianDateTime,
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="dataType"></param>
        /// <param name="solarDateTimeSourceColumns"><para>if (datetimeComputeType==solarDateTimeToGregorianDateTime and solarDate and time are seperated to different columns)
        /// first item in [solarDateTimeSourceColumns] depicts solarDate and second item depicts time</para>if (datetimeComputeType==solarDateTimeToGregorianDateTime and solarDate and time are in one column)
        /// solardate and time should be seprated with whitespace </param>
        /// <param name="datetimeComputeType"></param>
        public dateTimeComputedColumn(string columnName, Type dataType, htmlColumn[] solarDateTimeSourceColumns, enumDateTimeComputeType datetimeComputeType) : base(columnName, dataType)
        {
            this.prp_solarDateTimeSourceColumns = solarDateTimeSourceColumns;
            this.prp_dateTimeComputeType = datetimeComputeType;
        }
        public htmlColumn[] prp_solarDateTimeSourceColumns { get; set; }
        public enumDateTimeComputeType prp_dateTimeComputeType { get; set; }

        public object fnc_getColumnValueFromControl(DataRow row)
        {
            if (this.prp_solarDateTimeSourceColumns == null || this.prp_solarDateTimeSourceColumns.Length == 0)
                return null;
            if (Functions.IsNull(row[this.prp_solarDateTimeSourceColumns[0]])) return null;
            object value = null;
            switch (this.prp_dateTimeComputeType)
            {
                case enumDateTimeComputeType.solarDateToGregorianDate:
                    value = Functions.fnc_convertSolarDateAndTimeToDateTime(row[this.prp_solarDateTimeSourceColumns[0]].ToString(), null);
                    break;
                case enumDateTimeComputeType.timeIntToTime:
                    value = Functions.fnc_convertTimeLongToTimespan(long.Parse(row[this.prp_solarDateTimeSourceColumns[0]].ToString()));
                    break;
                case enumDateTimeComputeType.solarDateTimeIntToGregorianDateTime:
                    value = Functions.fnc_convertSolarDateAndTimeToDateTime(row[this.prp_solarDateTimeSourceColumns[0]].ToString(), long.Parse(row[this.prp_solarDateTimeSourceColumns[0]].ToString()));
                    break;
                case enumDateTimeComputeType.solarDateTimeToGregorianDateTime:
                    {
                        if (this.prp_solarDateTimeSourceColumns.Length == 1)
                        {
                            string solarDateAndTime = row[this.prp_solarDateTimeSourceColumns[0]].ToString();
                            string[] solarDateAndTimeArr = solarDateAndTime.Split(' ');
                            value = Functions.fnc_convertSolarDateAndTimeToDateTime(row[solarDateAndTimeArr[0]].ToString(), long.Parse(solarDateAndTimeArr[solarDateAndTimeArr.Length - 1]));
                        }
                        else if (this.prp_solarDateTimeSourceColumns.Length > 1)
                        {
                            string solarDate = row[this.prp_solarDateTimeSourceColumns[0]].ToString();
                            long? timeLong;
                            if (Functions.IsNull(row[this.prp_solarDateTimeSourceColumns[1]]))
                            {
                                timeLong = null;
                            }
                            else
                            {
                                timeLong = long.Parse(row[this.prp_solarDateTimeSourceColumns[1]].ToString());
                            }

                            value = Functions.fnc_convertSolarDateAndTimeToDateTime(solarDate, timeLong);
                        }
                        break;
                    }
                default:
                    break;
            }

            return null;
        }
    }

    /// <summary>
    /// fill columns with rwmmsColumn
    /// </summary>
    public class htmlTable : DataTable
    {
        bool v_handleRowChange = true;
        int v_skipRowTop;
        /// <summary>
        /// how many rows should skip from top of html table
        /// </summary>
        public int prp_skipRowTop
        {
            get
            {
                return v_skipRowTop;
            }
            set
            {
                if (value < 0)
                {
                    throw new Exception("skipRowTop cannot be negative value");
                }
                this.v_skipRowTop = value;
            }
        }

        int v_skipRowBottom;
        /// <summary>
        /// how many rows should skip from bottom of html table</param>
        /// </summary>
        public int prp_skipRowBottom
        {
            get
            {
                return v_skipRowBottom;
            }
            set
            {
                if (value < 0)
                {
                    throw new Exception("skipRowBottom cannot be negative value");
                }
                this.v_skipRowBottom = value;
            }
        }

        /// <summary>
        /// how many rows should skip from top of html table
        /// </summary>
        public int[] prp_skipRowIndecies { get; set; }

        public htmlTableReference[] prp_tableReferences { get; set; }
        public string[] prp_identifierColumnNames { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="skipRowTop">how many rows should skip from top of html table</param>
        /// <param name="skipRowBottom">how many rows should skip from bottom of html table</param>
        /// <param name="skipRowIndecies">which rows should be skipped in html table</param>
        public htmlTable(int skipRowTop, int skipRowBottom, int[] skipRowIndecies) : this(null, skipRowTop, skipRowBottom, skipRowIndecies)
        {

        }

        public htmlTable(string tableName, int skipRowTop, int skipRowBottom, int[] skipRowIndecies) : this(tableName, skipRowTop, skipRowBottom, skipRowIndecies, null)
        {

        }

        public htmlTable(string tableName, int skipRowTop, int skipRowBottom, int[] skipRowIndecies, string[] identifierColumnNames) : this(tableName, skipRowTop, skipRowBottom, skipRowIndecies, identifierColumnNames, null)
        {

        }

        public htmlTable(string tableName, int skipRowTop, int skipRowBottom, int[] skipRowIndecies, string[] identifierColumnNames, htmlTableReference[] tableReferences)
        {
            this.prp_skipRowBottom = skipRowBottom;
            this.prp_skipRowTop = skipRowTop;
            this.prp_skipRowIndecies = skipRowIndecies;
            this.TableName = tableName;
            this.prp_identifierColumnNames = identifierColumnNames;
            this.prp_tableReferences = tableReferences;
            this.v_handleRowChange = true;
        }

        protected override void OnRowChanged(DataRowChangeEventArgs e)
        {
            if (this.v_handleRowChange)
            {
                int i;
                dateTimeComputedColumn col;
                for (i = 0; i <= this.Columns.Count - 1; i++)
                {
                    if (this.Columns[i] is dateTimeComputedColumn)
                    {
                        col = (dateTimeComputedColumn)this.Columns[i];
                        if (col.prp_solarDateTimeSourceColumns != null
                            && col.prp_solarDateTimeSourceColumns.Length > 0)
                        {
                            this.v_handleRowChange = false;
                            e.Row[col] = col.fnc_getColumnValueFromControl(e.Row);
                            this.v_handleRowChange = true;
                        }
                    }
                }
            }
            base.OnRowChanged(e);
        }
        public bool fnc_save(System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> items)
        {
            int i;
            string whereCondition = "";
            Dictionary<string, object> dic;
            bool retVal = true;
            for (i = prp_skipRowTop; i <= items.Count - 1 - prp_skipRowBottom; i++)
            {
                if (this.prp_skipRowIndecies != null && this.prp_skipRowIndecies.Contains(i))
                    continue;

                whereCondition = this.fnc_getWhereCondition(items[i]);
                dic = this.fnc_getColumnsValue(items[i]);

                using (var entity = new Models.logisticEntities())
                {
                    var exists = entity.Database.SqlQuery<int?>("select top 1 1 from " + this.TableName + " where " + whereCondition).FirstOrDefault();
                    if (exists.HasValue)
                    {
                        if (!this.fnc_update(dic, whereCondition))
                        {
                            retVal = false;
                        }
                    }
                    else
                    {
                        if (!this.fnc_insert(dic))
                        {
                            retVal = false;
                        }
                    }
                }

            }
            return retVal;
        }

        public bool fnc_update(Dictionary<string, object> dic, string whereCondition)
        {
            string cmd = "";
            int i;
            object value;
            for (i = 0; i <= dic.Count - 1; i++)
            {
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
            cmd = " update " + this.TableName + " set " + cmd + " where " + whereCondition;
            using (var entity = new Models.logisticEntities())
            {
                try
                {
                    entity.Database.ExecuteSqlCommand(cmd);
                    return true;
                }
                catch (Exception ex)
                {
                    SharedVariables.logs.Error("Error in fnc_update", ex);
                    return false;
                }
            }
        }

        public bool fnc_insert(Dictionary<string, object> dic)
        {
            string columnNames = "";
            string columnValues = "";
            string cmd;
            int i;
            object value;
            for (i = 0; i <= dic.Count - 1; i++)
            {
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
            cmd = " insert into " + this.TableName + "(" + columnNames + ")values(" + columnValues + ")";
            using (var entity = new Models.logisticEntities())
            {
                try
                {
                    entity.Database.ExecuteSqlCommand(cmd);
                    return true;
                }
                catch (Exception ex)
                {
                    SharedVariables.logs.Error("Error in fnc_insert", ex);
                    return false;
                }
            }
        }
        public Dictionary<string, object> fnc_getColumnsValue(IWebElement item)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            int i, j;
            htmlColumn htmlCol;
            object value;
            string whereCondition;
            string selectStr;
            htmlTableReference tableReference;
            for (i = 0; i <= this.Columns.Count - 1; i++)
            {
                if (!(this.Columns[i] is Models.htmlColumn)) continue;
                htmlCol = (Models.htmlColumn)this.Columns[i];
                value = htmlCol.fnc_getColumnValueFromControl(item);
                dic.Add(htmlCol.ColumnName, value);
            }

            if (this.prp_tableReferences != null)
            {
                for (i = 0; i <= this.prp_tableReferences.Length - 1; i++)
                {
                    if (!string.IsNullOrEmpty(this.prp_tableReferences[i].prp_parentTableName)
                        && this.prp_tableReferences[i].prp_parentRefColumns != null
                        && this.prp_tableReferences[i].prp_parentIdColumns != null

                        && !string.IsNullOrEmpty(this.prp_tableReferences[i].prp_childTableName)
                        && this.prp_tableReferences[i].prp_childRefColumns != null
                        && this.prp_tableReferences[i].prp_childIdColumns != null
                        )
                    {

                        tableReference = this.prp_tableReferences[i];
                        whereCondition = "";
                        selectStr = "";
                        #region prepare where string from parentTable
                        for (j = 0; j <= tableReference.prp_childRefColumns.Length - 1; j++)
                        {
                            if (!dic.ContainsKey(tableReference.prp_childRefColumns[j]))
                            {
                                throw new Exception("TableSoucre" + this.TableName + " does not have column " + tableReference.prp_childRefColumns[j] + " which is specified in tableRefernce");
                            }
                            value = dic[tableReference.prp_childRefColumns[j]];
                            if (Functions.IsNull(value)) //throw new Exception("identifierColumn Value is null (colName = " + htmlCol.ColumnName + ")");
                            {
                                whereCondition = whereCondition + tableReference.prp_parentRefColumns[j] + " is null " + " and ";
                            }
                            else if (Functions.fnc_isNumeric(value))
                            {
                                whereCondition = whereCondition + tableReference.prp_childRefColumns[j] + "=" + value + " and ";
                            }
                            else
                            {
                                whereCondition = whereCondition + " " + tableReference.prp_parentRefColumns[j] + " = '" + value + "' and ";
                            }
                        }
                        if (!string.IsNullOrEmpty(whereCondition))
                        {
                            whereCondition = whereCondition.Remove(whereCondition.Length - 4, 4);
                        }
                        #endregion

                        #region prepare select string from parentTable
                        for (j = 0; j <= tableReference.prp_parentIdColumns.Length - 1; j++)
                        {
                            selectStr += tableReference.prp_parentIdColumns[j] + ", ";
                        }
                        if (!string.IsNullOrEmpty(selectStr))
                        {
                            selectStr = selectStr.Remove(whereCondition.Length - 2, 2);
                        }
                        #endregion

                        using (var entity = new Models.logisticEntities())
                        {
                            DataTable dt = new DataTable();
                            Functions.sb_fillDatatable((SqlConnection)entity.Database.Connection
                                , "select top 1 " + selectStr + " from" + tableReference.prp_parentTableName + " where " + whereCondition
                                , dt);

                            if (dt.Rows.Count > 0)
                            {
                                for (j = 0; j <= tableReference.prp_parentIdColumns.Length - 1; j++)
                                {
                                    dic[tableReference.prp_childIdColumns[j]] = dt.Rows[0][tableReference.prp_parentIdColumns[j]];
                                }
                            }
                        }
                    }
                }
            }
            return dic;
        }
        private string fnc_getWhereCondition(IWebElement item)
        {
            if (this.prp_identifierColumnNames == null || this.prp_identifierColumnNames.Length == 0)
            {
                return null;
            }
            int i;
            object value;
            string whereCondition = "";
            htmlColumn htmlCol;
            for (i = 0; i <= this.Columns.Count - 1; i++)
            {
                if (!(this.Columns[i] is Models.htmlColumn)) continue;
                htmlCol = (Models.htmlColumn)this.Columns[i];
                if (this.prp_identifierColumnNames.Contains(htmlCol.ColumnName, StringComparer.InvariantCultureIgnoreCase))
                {
                    //if (Functions.IsNull(Columns[j])) throw new Exception("identifierColumnName value is null (identifierColumnName = " + this.prp_identifierColumnNames[j] + ")");


                    value = htmlCol.fnc_getColumnValueFromControl(item);
                    if (Functions.IsNull(value)) //throw new Exception("identifierColumn Value is null (colName = " + htmlCol.ColumnName + ")");
                    {
                        whereCondition = whereCondition + htmlCol.ColumnName + " is null " + " and ";
                    }
                    else if (Functions.fnc_isNumeric(value))
                    {
                        whereCondition = whereCondition + htmlCol.ColumnName + "=" + value + " and ";
                    }
                    else
                    {
                        whereCondition = whereCondition + "(Replace(Replace(" + htmlCol.ColumnName + ",'ي','ی'),'ك','ک')=Replace(Replace(N'" + value + "','ي','ی'),'ك','ک')"
                            + (string.IsNullOrEmpty(htmlCol.prp_alternateColName) ? "" : " or charindex(Replace(Replace(N'" + value + "'+';','ي','ی'),'ك','ک'),Replace(Replace(" + htmlCol.prp_alternateColName + ",'ي','ی'),'ك','ک')"
                            + ")>0") + ") and ";
                    }
                    //whereCondition = this.prp_identifierColumnNames[j] + "'" +
                }


            }
            if (!string.IsNullOrEmpty(whereCondition)) whereCondition = whereCondition.Remove(whereCondition.Length - 4, 4);
            return whereCondition;
        }



    }

    public class htmlTableReference
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentTableName"></param>
        /// <param name="parentRefColumns">Columns which should be comapred with childRefcolumns</param>
        /// <param name="childTableName"></param>
        /// <param name="childRefColumns">Columns which should be comapred with parentRefcolumns</param>
        public htmlTableReference(string parentTableName, string[] parentRefColumns, string childTableName, string[] childRefColumns)
            : this(parentTableName, parentRefColumns, null, childTableName, childRefColumns, null)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentTableName"></param>
        /// <param name="parentRefColumns">Columns which should be comapred with childRefcolumns</param>
        /// <param name="parentIdColumns">Columns which are ids in parentTable</param>
        /// <param name="childTableName"></param>
        /// <param name="childRefColumns">Columns which should be comapred with parentRefcolumns</param>
        /// <param name="childIdColumns">Columns which will be filled by parentIdColumns in childTable</param>
        public htmlTableReference(string parentTableName, string[] parentRefColumns, string[] parentIdColumns
            , string childTableName, string[] childRefColumns, string[] childIdColumns)
        {
            if (string.IsNullOrEmpty(parentTableName))
                throw new Exception("parentTableName is not specified");

            if (parentRefColumns == null || parentRefColumns.Length == 0)
            {
                throw new Exception("parentRefColumns is not specified");
            }
            if (parentRefColumns.Any(o => string.IsNullOrEmpty(o)))
            {
                throw new Exception("One of the items of parentRefColumns is null or empty");
            }

            if (string.IsNullOrEmpty(childTableName))
                throw new Exception("childTableName is not specified");

            if (childRefColumns == null || childRefColumns.Length == 0)
            {
                throw new Exception("childRefColumns is not specified");
            }
            if (childRefColumns.Any(o => string.IsNullOrEmpty(o)))
            {
                throw new Exception("One of the items of childRefColumns is null or empty");
            }

            if (parentIdColumns.Length != childIdColumns.Length)
            {
                throw new Exception("Length of parentIdColumns is not equal with childIdColumns");
            }

            if (parentIdColumns != null && childIdColumns != null)
            {
                if (parentIdColumns.Any(o => string.IsNullOrEmpty(o)))
                {
                    throw new Exception("One of the items of parentIdColumns is null or empty");
                }
                if (childIdColumns.Any(o => string.IsNullOrEmpty(o)))
                {
                    throw new Exception("One of the items of childIdColumns is null or empty");
                }
                if (parentIdColumns.Length != childIdColumns.Length)
                {
                    throw new Exception("Length of parentIdColumns is not equal with childIdColumns");
                }
            }

            this.prp_childIdColumns = childIdColumns;
            this.prp_childRefColumns = childRefColumns;
            this.prp_childTableName = childTableName;
            this.prp_parentIdColumns = parentIdColumns;
            this.prp_parentRefColumns = parentRefColumns;
            this.prp_parentTableName = parentTableName;

        }

        public string prp_parentTableName { get; }
        /// <summary>
        /// Columns which should be comapred with childRefcolumns
        /// </summary>
        public string[] prp_parentRefColumns { get; }

        /// <summary>
        /// Columns which are ids in parentTable and can be used to fill the childIdColumns in childTable
        /// </summary>
        public string[] prp_parentIdColumns { get; }

        public string prp_childTableName { get; }
        /// <summary>
        /// Columns which should be comapred with parentRefcolumns
        /// </summary>
        public string[] prp_childRefColumns { get; }
        /// <summary>
        /// Columns which will be filled by parentIdColumns in childTable
        /// </summary>
        public string[] prp_childIdColumns { get; }


    }


}
