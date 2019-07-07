using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailSiteDataGrabber.rwmms.model
{
    public class columnComputed : DataColumn
    {
        public columnComputed(string columnName, Type dataType) : base(columnName, dataType)
        {

        }
    }

    public class columnComputedDateTime : columnComputed
    {
        public enum enumDateTimeComputeType
        {
            /// <summary>
            /// 1398/12/20
            /// </summary>
            solarDateToGregorianDate,
            /// <summary>
            /// 10/12/1398
            /// </summary>
            solarDateToGregorianDateReverse,
            /// <summary>
            /// 1224
            /// </summary>
            timeIntToTime,
            /// <summary>
            /// 1398/12/20 1224
            /// </summary>
            solarDateTimeIntToGregorianDateTime,
            /// <summary>
            /// 1398/12/20 12:20
            /// </summary>
            solarDateTimeToGregorianDateTime,
            CurrentGregorianDateTime,
            CurrentSolarDateTime,
        }
        public columnComputedDateTime(string columnName, Type dataType, string solarDateTimeSourceColumnName
           , enumDateTimeComputeType datetimeComputeType) : this(columnName, dataType, new string[] { solarDateTimeSourceColumnName }
           , datetimeComputeType)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="dataType"></param>
        /// <param name="solarDateTimeSourceColumnNames"><para>if (datetimeComputeType==solarDateTimeToGregorianDateTime and solarDate and time are seperated to different columns)
        /// first item in [solarDateTimeSourceColumns] depicts solarDate and second item depicts time</para>if (datetimeComputeType==solarDateTimeToGregorianDateTime and solarDate and time are in one column)
        /// solardate and time should be seprated with whitespace </param>
        /// <param name="datetimeComputeType"></param>
        public columnComputedDateTime(string columnName, Type dataType, string[] solarDateTimeSourceColumnNames
            , enumDateTimeComputeType datetimeComputeType) : base(columnName, dataType)
        {
            this.prp_solarDateTimeSourceColumnNames = solarDateTimeSourceColumnNames;
            this.prp_dateTimeComputeType = datetimeComputeType;
        }
        public string[] prp_solarDateTimeSourceColumnNames { get; set; }
        public enumDateTimeComputeType prp_dateTimeComputeType { get; set; }

        public object fnc_getColumnValue(DataRow row)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
            int i;
            for (i = 0; i <= row.Table.Columns.Count - 1; i++)
            {
                dic.Add(row.Table.Columns[i].ColumnName, row[i]);
            }
            return this.fnc_getColumnValue(dic);
        }

        public object fnc_getColumnValue(Dictionary<string, object> dicValues)
        {
            switch (this.prp_dateTimeComputeType)
            {
                case enumDateTimeComputeType.CurrentGregorianDateTime:
                    return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"); ;
                case enumDateTimeComputeType.CurrentSolarDateTime:
                    return Functions.miladiToSolar(DateTime.Now);
            }
            if (this.prp_solarDateTimeSourceColumnNames == null || this.prp_solarDateTimeSourceColumnNames.Length == 0)
                return null;


            if (Functions.IsNull(dicValues[this.prp_solarDateTimeSourceColumnNames[0]])) return null;
            object value = null;
            string solarDate;
            switch (this.prp_dateTimeComputeType)
            {
                case enumDateTimeComputeType.solarDateToGregorianDate:
                    value = Functions.fnc_convertSolarDateAndTimeToDateTime(dicValues[this.prp_solarDateTimeSourceColumnNames[0]].ToString(), null);
                    break;
                case enumDateTimeComputeType.solarDateToGregorianDateReverse:
                    string[] parts = dicValues[this.prp_solarDateTimeSourceColumnNames[0]].ToString().Split('/');
                    Array.Reverse(parts, 0, parts.Length);
                    solarDate = string.Join("/", parts);
                    value = Functions.fnc_convertSolarDateAndTimeToDateTime(solarDate, null);
                    break;
                case enumDateTimeComputeType.timeIntToTime:
                    value = Functions.fnc_convertTimeLongToTimespan(long.Parse(dicValues[this.prp_solarDateTimeSourceColumnNames[0]].ToString()));
                    break;
                case enumDateTimeComputeType.solarDateTimeIntToGregorianDateTime:
                    value = Functions.fnc_convertSolarDateAndTimeToDateTime(dicValues[this.prp_solarDateTimeSourceColumnNames[0]].ToString(), long.Parse(dicValues[this.prp_solarDateTimeSourceColumnNames[0]].ToString()));
                    break;
                case enumDateTimeComputeType.solarDateTimeToGregorianDateTime:
                    {
                        if (this.prp_solarDateTimeSourceColumnNames.Length == 1)
                        {
                            string solarDateAndTime = dicValues[this.prp_solarDateTimeSourceColumnNames[0]].ToString();
                            string[] solarDateAndTimeArr = solarDateAndTime.Split(' ');
                            value = Functions.fnc_convertSolarDateAndTimeToDateTime(dicValues[solarDateAndTimeArr[0]].ToString(), long.Parse(solarDateAndTimeArr[solarDateAndTimeArr.Length - 1]));
                        }
                        else if (this.prp_solarDateTimeSourceColumnNames.Length > 1)
                        {
                            solarDate = dicValues[this.prp_solarDateTimeSourceColumnNames[0]].ToString();
                            long? timeLong;
                            if (Functions.IsNull(dicValues[this.prp_solarDateTimeSourceColumnNames[1]]))
                            {
                                timeLong = null;
                            }
                            else
                            {
                                timeLong = long.Parse(dicValues[this.prp_solarDateTimeSourceColumnNames[1]].ToString());
                            }

                            value = Functions.fnc_convertSolarDateAndTimeToDateTime(solarDate, timeLong);
                        }
                        break;
                    }
                
                default:
                    break;
            }
            if (value is TimeSpan?)
            {
                if (((TimeSpan?)value).HasValue)
                    return ((TimeSpan)value).ToString("c");
                return null;
            }
            else if (value is DateTime?)
            {
                if (((DateTime?)value).HasValue)
                    return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff");
                return null;
            }
            return value;
        }
    }

    public class columnComputedReference : columnComputed
    {
        public columnComputedReference(string columnName, Type dataType, string parentTableName, string parentTableColumnName
          , string parentIdColumnName, string childTableColumnName) : this(columnName, dataType, parentTableName
              , new string[] { parentTableColumnName }, parentIdColumnName, new string[] { childTableColumnName })
        {

        }

        public columnComputedReference(string columnName, Type dataType, string parentTableName, string[] parentTableColumnNames
            , string parentIdColumnName, string[] childTableColumnNames)
            : base(columnName, dataType)
        {
            if (string.IsNullOrEmpty(parentTableName))
                throw new Exception("parentTableName is not specified");

            if (parentTableColumnNames == null || parentTableColumnNames.Length == 0)
            {
                throw new Exception("parentTableColumnNames is not specified");
            }

            if (string.IsNullOrEmpty(parentIdColumnName))
            {
                throw new Exception("parentIdColumnName is not specified");
            }
            if (childTableColumnNames == null || childTableColumnNames.Length == 0)
            {
                throw new Exception("childTableColumnNames is not specified");
            }
            if (parentTableColumnNames.Length != childTableColumnNames.Length)
            {
                throw new Exception("Length of parentTableColumnNames is not equal with childTableColumnNames");
            }

            string[,] parentTableColumnNamesWithAlternate = new string[parentTableColumnNames.Length, 2];
            int i;
            for (i = 0; i <= parentTableColumnNames.Length - 1; i++)
            {
                parentTableColumnNamesWithAlternate[i, 0] = parentTableColumnNames[i];
                parentTableColumnNamesWithAlternate[i, 1] = null;
            }


            this.prp_childTableColumnNames = childTableColumnNames;
            this.prp_parentIdColumnName = parentIdColumnName;
            this.prp_parentTableColumnNamesWithAlternate = parentTableColumnNamesWithAlternate;
            this.prp_parentTableName = parentTableName;

        }

        public columnComputedReference(string columnName, Type dataType, string parentTableName
            , string parentTableColumnName, string parentTableColumnAlternateName
            , string parentIdColumnName, string childTableColumnName) : this(columnName, dataType, parentTableName
                , new string[,] { { parentTableColumnName, parentTableColumnAlternateName } }
                , parentIdColumnName, new string[] { childTableColumnName })
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="dataType"></param>
        /// <param name="parentTableName"></param>
        /// <param name="parentTableColumnNamesWithAlternate">e.g. parenttable is companies then companyName is columnName and alternames is the alternate column
        /// [0,0]='columnName' [0,1]='alsternateNames'</param>
        /// <param name="parentIdColumnName"></param>
        /// <param name="childTableColumnNames"></param>
        public columnComputedReference(string columnName, Type dataType, string parentTableName, string[,] parentTableColumnNamesWithAlternate
            , string parentIdColumnName, string[] childTableColumnNames) : base(columnName, dataType)
        {
            if (string.IsNullOrEmpty(parentTableName))
                throw new Exception("parentTableName is not specified");

            if (parentTableColumnNamesWithAlternate == null || parentTableColumnNamesWithAlternate.Length == 0)
            {
                throw new Exception("parentTableColumnNames is not specified");
            }

            if (string.IsNullOrEmpty(parentIdColumnName))
            {
                throw new Exception("parentIdColumnName is not specified");
            }
            if (childTableColumnNames == null || childTableColumnNames.Length == 0)
            {
                throw new Exception("childTableColumnNames is not specified");
            }
            if (parentTableColumnNamesWithAlternate.Length / 2 != childTableColumnNames.Length)
            {
                throw new Exception("Length of parentTableColumnNames is not equal with childTableColumnNames");
            }


            this.prp_childTableColumnNames = childTableColumnNames;
            this.prp_parentIdColumnName = parentIdColumnName;
            this.prp_parentTableColumnNamesWithAlternate = parentTableColumnNamesWithAlternate;
            this.prp_parentTableName = parentTableName;
        }

        private void sb_initialize(string parentTableName, string[,] parentTableColumnNames
          , string parentIdColumnName, string[] childTableColumnNames)
        {

        }
        public string prp_parentTableName { get; }
        /// <summary>
        /// Columns which should be comapred with childRefcolumns
        /// first index is columnname and second index is alternateColumn if exists
        /// ['columnName','columnNameWithAlternate']
        /// </summary>
        public string[,] prp_parentTableColumnNamesWithAlternate { get; }

        /// <summary>
        /// Columns which are ids in parentTable and can be used to fill the childIdColumns in childTable
        /// </summary>
        public string[] prp_childTableColumnNames { get; }

        public string prp_parentIdColumnName { get; }
        /// <summary>
        /// Columns which should be comapred with parentRefcolumns
        /// </summary>

        public object fnc_getColumnValue(Dictionary<string, object> dicValues)
        {

            string whereCondition = this.fnc_getWhereCondition(dicValues);

            using (var entity = new Model.logisticEntities())
            {
                var value = entity.Database.SqlQuery<int?>("select top 1 " + this.prp_parentIdColumnName + " from " + this.prp_parentTableName + " where " + whereCondition).FirstOrDefault();
                return value;
            }
        }

        private string fnc_getWhereCondition(Dictionary<string, object> dicValues)
        {
            int i, j;
            string whereCondition = "";
            object columnValue;
            string innerCondition;
            string replaceKaYaColumn = "";
            string replaceKaYaValue = "";
            string replaceKaYaAlternate = "";

            for (i = 0; i <= this.prp_childTableColumnNames.Length - 1; i++)
            {
                for (j = 0; j <= dicValues.Count - 1; j++)
                {
                    if (dicValues.ContainsKey(this.prp_childTableColumnNames[i].ToLower()))
                        break;
                }
                if (j < dicValues.Count)
                {
                    columnValue = dicValues[this.prp_childTableColumnNames[i]];
                    if (Functions.IsNull(columnValue)) //throw new Exception("identifierColumn Value is null (colName = " + htmlCol.ColumnName + ")");
                    {
                        innerCondition = this.prp_parentTableColumnNamesWithAlternate[i, 0] + " is null ";
                        whereCondition = whereCondition + innerCondition + " and ";

                    }
                    else if (Functions.fnc_isNumeric(columnValue))
                    {
                        replaceKaYaValue = "Replace(Replace(N'" + columnValue + "', N'ي', N'ی'), N'ك', N'ک')";
                        if (!string.IsNullOrEmpty(this.prp_parentTableColumnNamesWithAlternate[i, 1]))
                            replaceKaYaAlternate = "Replace(Replace(" + this.prp_parentTableColumnNamesWithAlternate[i, 1] + ", N'ي', N'ی'), N'ك', N'ک')";
                        else replaceKaYaAlternate = "";

                        whereCondition = whereCondition + "(" + this.prp_parentTableColumnNamesWithAlternate[i, 0] + "=" + columnValue
                             + (string.IsNullOrEmpty(this.prp_parentTableColumnNamesWithAlternate[i, 1])
                             ? ""
                             : " or " + replaceKaYaAlternate + " like cast(" + replaceKaYaValue + "as nvarchar(50))+';%'" + " or " + replaceKaYaAlternate + " like '%;'+ cast(" + replaceKaYaValue + " as nvarchar(50)) + ';%'")
                             + ") and ";
                    }
                    else
                    {
                        replaceKaYaColumn = "Replace(Replace(" + this.prp_parentTableColumnNamesWithAlternate[i, 0] + ", N'ي', N'ی'), N'ك', N'ک')";
                        replaceKaYaValue = "Replace(Replace(N'" + columnValue + "', N'ي', N'ی'), N'ك', N'ک')";
                        if (!string.IsNullOrEmpty(this.prp_parentTableColumnNamesWithAlternate[i, 1]))
                            replaceKaYaAlternate = "Replace(Replace(" + this.prp_parentTableColumnNamesWithAlternate[i, 1] + ", N'ي', N'ی'), N'ك', N'ک')";
                        else replaceKaYaAlternate = "";

                        whereCondition = whereCondition + "(" + replaceKaYaColumn + "=" + replaceKaYaValue
                            + (string.IsNullOrEmpty(this.prp_parentTableColumnNamesWithAlternate[i, 1])
                            ? ""
                            : " or " + replaceKaYaAlternate + " like " + replaceKaYaValue + "+';%'" + " or " + replaceKaYaAlternate + " like '%;'+" + replaceKaYaValue + "+';%'")
                            + ") and ";

                        //                        whereCondition = whereCondition + "(Replace(Replace(" + this.prp_parentTableColumnNamesWithAlternate[i] + ",'ي','ی'),'ك','ک')=Replace(Replace(N'" + columnValue + "','ي','ی'),'ك','ک')" + ") and ";
                    }
                }
            }

            if (!string.IsNullOrEmpty(whereCondition)) whereCondition = whereCondition.Remove(whereCondition.Length - 4, 4);
            return whereCondition;
        }
    }

    public class columnComputedTrueFalse : columnComputed
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="dataType"></param>
        ///<param name="sourceColumnName"></param>
        public columnComputedTrueFalse(string columnName, Type dataType, string sourceColumnName)
            : this(columnName, dataType, sourceColumnName, new List<string>() { "true", "checked", "yes", "1", "دارد", "صحیح", "بلی", "بله", "1" }
            , new List<string>() { "true", "checked", "yes", "0", "ندارد", "غلط", "خیر", "0" })
        {

        }

        public columnComputedTrueFalse(string columnName, Type dataType, string sourceColumnName
            , List<string> valuesTrue, List<string> valuesFalse)
          : base(columnName, dataType)
        {
            this.prp_sourceColumnName = sourceColumnName;
            this.prp_valuesFalse = valuesFalse;
            this.prp_valuesTrue = valuesTrue;
        }
        public List<string> prp_valuesTrue
        {
            get; set;
        }
        public List<string> prp_valuesFalse
        {
            get; set;
        }
        public string prp_sourceColumnName { get; set; }

        public object fnc_getColumnValue(DataRow row)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
            int i;
            for (i = 0; i <= row.Table.Columns.Count - 1; i++)
            {
                dic.Add(row.Table.Columns[i].ColumnName, row[i]);
            }
            return this.fnc_getColumnValue(dic);
        }

        public int? fnc_getColumnValue(Dictionary<string, object> dicValues)
        {
            if (this.prp_sourceColumnName == null)
                return null;

            if (this.prp_valuesFalse == null || this.prp_valuesFalse.Count == 0 || this.prp_valuesTrue == null || this.prp_valuesTrue.Count == 0)
                return null;
            if (Functions.IsNull(dicValues[this.prp_sourceColumnName])) return null;

            object value = dicValues[this.prp_sourceColumnName];

            if (Functions.IsNull(value)) return null;
            string valueStr = value.ToString().Replace(" ", "").Replace("&nbsp;", "").TrimEnd().TrimStart();
            valueStr = valueStr.Replace("ي", "ی").Replace("ك", "ک");
            this.prp_valuesFalse = this.prp_valuesFalse.Select(o => { o = o.Replace("ي", "ی").Replace("ك", "ک"); return o; }).ToList();
            this.prp_valuesTrue = this.prp_valuesTrue.Select(o => { o = o.Replace("ي", "ی").Replace("ك", "ک"); return o; }).ToList();

            if (this.prp_valuesTrue.Any(o => o == valueStr)) return 1;
            if (this.prp_valuesFalse.Any(o => o == valueStr)) return 0;
            return null;
        }
    }
}
