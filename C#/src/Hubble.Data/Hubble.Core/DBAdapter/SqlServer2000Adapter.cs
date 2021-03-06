﻿/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Hubble.Framework.Data;
using Hubble.Core.Data;
using Hubble.Core.SFQL.Parse;

namespace Hubble.Core.DBAdapter
{
    public class SqlServer2000Adapter : IDBAdapter, INamedExternalReference
    {
        private string GetFieldLine(Data.Field field)
        {
            if (!field.Store)
            {
                return null;
            }

            string sqlType = "";
            string defaultValue = null;

            if (field.DefaultValue != null)
            {
                switch (field.DataType)
                {
                    case DataType.TinyInt:
                    case DataType.SmallInt:
                    case Hubble.Core.Data.DataType.Int:
                    case Hubble.Core.Data.DataType.BigInt:
                    case Hubble.Core.Data.DataType.Float:
                        defaultValue = field.DefaultValue;
                        break;
                    case Hubble.Core.Data.DataType.Date:
                    case Hubble.Core.Data.DataType.SmallDateTime:
                    case Hubble.Core.Data.DataType.DateTime:
                    case Hubble.Core.Data.DataType.Varchar:
                    case Hubble.Core.Data.DataType.NVarchar:
                    case Hubble.Core.Data.DataType.Char:
                    case Hubble.Core.Data.DataType.NChar:
                        defaultValue = string.Format("'{0}'", field.DefaultValue.Replace("'", "''"));
                        break;
                    default:
                        throw new ArgumentException(field.DataType.ToString());
                }
            }
            else
            {
                switch (field.DataType)
                {
                    case Hubble.Core.Data.DataType.Varchar:
                    case Hubble.Core.Data.DataType.NVarchar:
                    case Hubble.Core.Data.DataType.Char:
                    case Hubble.Core.Data.DataType.NChar:
                        if (field.DataLength <= 0)
                        {
                            defaultValue = "''";
                        }
                        break;
                }
            }


            switch (field.DataType)
            {
                case DataType.TinyInt:
                    sqlType = "TinyInt";
                    break;
                case DataType.SmallInt:
                    sqlType = "SmallInt";
                    break;
                case Hubble.Core.Data.DataType.Int:
                    sqlType = "Int";
                    break;
                case Hubble.Core.Data.DataType.Date:
                case Hubble.Core.Data.DataType.SmallDateTime:
                case Hubble.Core.Data.DataType.DateTime:
                    sqlType = "DateTime";
                    break;
                case Hubble.Core.Data.DataType.Float:
                    sqlType = "Float";
                    break;
                case Hubble.Core.Data.DataType.BigInt:
                    sqlType = "BigInt";
                    break;
                case Hubble.Core.Data.DataType.Varchar:
                    if (field.DataLength <= 0)
                    {
                        sqlType = "text";
                    }
                    else
                    {
                        sqlType = "varchar ({1})";
                    }

                    break;
                case Hubble.Core.Data.DataType.NVarchar:
                    if (field.DataLength <= 0)
                    {
                        sqlType = "ntext";
                    }
                    else
                    {
                        sqlType = "nvarchar ({1})";
                    }
                    break;
                case Hubble.Core.Data.DataType.Char:
                    if (field.DataLength <= 0)
                    {
                        sqlType = "text";
                    }
                    else
                    {
                        sqlType = "char ({1})";
                    }

                    break;
                case Hubble.Core.Data.DataType.NChar:
                    if (field.DataLength <= 0)
                    {
                        sqlType = "ntext";
                    }
                    else
                    {
                        sqlType = "nchar ({1})";
                    }

                    break;
                default:
                    throw new ArgumentException(field.DataType.ToString());
            }

            string sql = string.Format("[{0}] " + sqlType + " " , field.Name,
                field.DataLength <= 0 ? "max" : field.DataLength.ToString());

            if (!field.CanNull)
            {
                sql += "NOT NULL ";
            }

            if (defaultValue != null)
            {
                sql += "DEFAULT " + defaultValue + " ";
            }

            return sql;
        }

        #region IDBAdapter Members

        Data.Table _Table;

        public Hubble.Core.Data.Table Table
        {
            get
            {
                return _Table;
            }
            set
            {
                _Table = value;
            }
        }

        string _ConnectionString = null;

        public string ConnectionString
        {
            get
            {
                return _ConnectionString;
            }
            set
            {
                _ConnectionString = value;
            }
        }

        public int MaxDocId
        {
            get 
            {
                using (SQLDataProvider sqlData = new SQLDataProvider())
                {
                    sqlData.Connect(Table.ConnectionString);
                    System.Data.DataSet ds;

                    if (DocIdReplaceField == null)
                    {
                        ds = sqlData.QuerySql("select max(DocId) as MaxDocId from " + Table.DBTableName);
                    }
                    else
                    {
                        ds = sqlData.QuerySql(string.Format("select max({0}) as MaxDocId from " + Table.DBTableName,
                            DocIdReplaceField));
                    }

                    if (ds.Tables[0].Rows.Count <= 0)
                    {
                        return -1;
                    }

                    if (ds.Tables[0].Rows[0][0] == System.DBNull.Value)
                    {
                        return -1;
                    }
                    else
                    {
                        return int.Parse(ds.Tables[0].Rows[0][0].ToString()); 
                    }

                } 
            }
        }


        public void Drop()
        {
            Debug.Assert(Table != null);

            string sql = string.Format("if exists (select * from sysobjects where id = object_id('{0}') and type = 'u')	drop table {0}",
                Table.DBTableName);

            using (SQLDataProvider sqlData = new SQLDataProvider())
            {
                sqlData.Connect(Table.ConnectionString);
                sqlData.ExcuteSql(sql);
            }
        }


        public void Truncate()
        {
            Debug.Assert(Table != null);

            string sql = string.Format("truncate table {0}",
                Table.DBTableName);

            using (SQLDataProvider sqlData = new SQLDataProvider())
            {
                sqlData.Connect(Table.ConnectionString);
                sqlData.ExcuteSql(sql);
            }
        }

        public void Truncate(string tableName)
        {
            Debug.Assert(Table != null);

            string sql = string.Format("truncate table {0}",
                tableName);

            using (SQLDataProvider sqlData = new SQLDataProvider())
            {
                sqlData.Connect(Table.ConnectionString);
                sqlData.ExcuteSql(sql);
            }
        }


        public void Create()
        {
            Debug.Assert(Table != null);

            List<string> primaryKeys = new List<string>();

            StringBuilder sql = new StringBuilder();

            sql.AppendFormat("create table {0} (", Table.DBTableName);

            sql.Append("DocId Int NOT NULL,");

            int i = 0;

            for (i = 0; i < Table.Fields.Count; i++)
            {
                Data.Field field = Table.Fields[i];
                string fieldSql = GetFieldLine(field);

                if (fieldSql != null)
                {
                    sql.Append(fieldSql);

                    if (i < Table.Fields.Count - 1)
                    {
                        sql.Append(",");
                    }
                }

                if (field.PrimaryKey)
                {
                    primaryKeys.Add(field.Name);
                }
            }


            if (primaryKeys.Count > 0)
            {
                i = 0;
                sql.Append(" primary key NONCLUSTERED(");

                foreach (string pkName in primaryKeys)
                {
                    if (i == 0)
                    {
                        sql.Append(pkName);
                    }
                    else
                    {
                        sql.Append("," + pkName);
                    }

                    i++;
                }

                sql.Append(")");
            }

            sql.Append(")");

            using (SQLDataProvider sqlData = new SQLDataProvider())
            {
                sqlData.Connect(Table.ConnectionString);
                sqlData.ExcuteSql(sql.ToString());

                sqlData.ExcuteSql(string.Format("create UNIQUE CLUSTERED Index I_{0}_DocId on {0}(DocId)",
                    Table.DBTableName));

                if (!string.IsNullOrEmpty(Table.SQLForCreate))
                {
                    sqlData.ExcuteSql(Table.SQLForCreate);
                }

            }

        }

        public void CreateMirrorTable()
        {
            throw new NotImplementedException();
        }


        private string BuildWriteTextLine(string tableName, string fieldName, string text, int docid, DataType datatype)
        {
            switch (datatype)
            {
                case DataType.NChar:
                case DataType.NVarchar:
                    return string.Format("SELECT @ptrval = TEXTPTR({0}) FROM {1} WHERE docid = {2} writeTEXT {1}.{0} @ptrval  N'{3}'",
                        fieldName, tableName, docid, text.Replace("'", "''"));
                default:
                    return string.Format("SELECT @ptrval = TEXTPTR({0}) FROM {1} WHERE docid = {2} writeTEXT {1}.{0} @ptrval  '{3}'",
                        fieldName, tableName, docid, text.Replace("'", "''"));
            }

        }

        public void Insert(IList<Hubble.Core.Data.Document> docs)
        {
            StringBuilder insertString = new StringBuilder();

            insertString.AppendLine("DECLARE @ptrval binary(16)");

            foreach (Hubble.Core.Data.Document doc in docs)
            {
                StringBuilder writeTextString = new StringBuilder();

                insertString.AppendFormat("Insert {0} ([DocId]", _Table.DBTableName);

                foreach (Data.FieldValue fv in doc.FieldValues)
                {
                    if (fv.Value == null)
                    {
                        continue;
                    }

                    switch (fv.Type)
                    {
                        case Hubble.Core.Data.DataType.Varchar:
                        case Hubble.Core.Data.DataType.NVarchar:
                        case Hubble.Core.Data.DataType.Char:
                        case Hubble.Core.Data.DataType.NChar:
                            if (fv.DataLength <= 0)
                            {
                                continue;
                            }
                            break;
                    }

                    insertString.AppendFormat(", [{0}]", fv.FieldName);
                }

                insertString.AppendFormat(") Values({0}", doc.DocId);
                
                foreach (Data.FieldValue fv in doc.FieldValues)
                {
                    if (fv.Value == null)
                    {
                        continue;
                    }

                    switch (fv.Type)
                    {
                        case Hubble.Core.Data.DataType.Varchar:
                        case Hubble.Core.Data.DataType.NVarchar:
                        case Hubble.Core.Data.DataType.Char:
                        case Hubble.Core.Data.DataType.NChar:
                            if (fv.DataLength <= 0)
                            {
                                writeTextString.AppendLine(BuildWriteTextLine(_Table.DBTableName,
                                    fv.FieldName, fv.Value, doc.DocId, fv.Type)); 
                                continue;
                            }

                            insertString.AppendFormat(",'{0}'", fv.Value.Replace("'", "''"));
                            break;

                        case Hubble.Core.Data.DataType.DateTime:
                        case Hubble.Core.Data.DataType.Date:
                        case Hubble.Core.Data.DataType.SmallDateTime:
                        case Hubble.Core.Data.DataType.Data:
                            insertString.AppendFormat(",'{0}'", fv.Value.Replace("'", "''"));
                            break;
                        default:
                            insertString.AppendFormat(",{0}", fv.Value);
                            break;
                    }
                }

                insertString.Append(")\r\n");
                insertString.AppendLine(writeTextString.ToString());
            }

            using (SQLDataProvider sqlData = new SQLDataProvider())
            {
                sqlData.Connect(Table.ConnectionString);
                sqlData.ExcuteSql(insertString.ToString());
            }

        }

        public void MirrorInsert(IList<Document> docs)
        {
            throw new NotImplementedException();
        }

        public void Delete(IList<int> docIds)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("delete ");

            sql.AppendFormat(" {0} where docId in (", Table.DBTableName);

            int i = 0;
            foreach (int docId in docIds)
            {
                if (i++ == 0)
                {
                    sql.AppendFormat("{0}", docId);
                }
                else
                {
                    sql.AppendFormat(",{0}", docId);
                }
            }

            sql.Append(")");

            using (SQLDataProvider sqlData = new SQLDataProvider())
            {
                sqlData.Connect(Table.ConnectionString);
                sqlData.ExcuteSql(sql.ToString());
            }

        }

        public void MirrorDelete(IList<int> docIds)
        {
            throw new NotImplementedException();
        }

        public void Update(Data.Document doc, IList<Query.DocumentResultForSort> docs)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("DECLARE @ptrval binary(16)");

            int textFieldCount = 0;

            StringBuilder writeTextString = new StringBuilder();

            foreach (Data.FieldValue fv in doc.FieldValues)
            {
                switch (fv.Type)
                {
                    case Hubble.Core.Data.DataType.Varchar:
                    case Hubble.Core.Data.DataType.NVarchar:
                    case Hubble.Core.Data.DataType.Char:
                    case Hubble.Core.Data.DataType.NChar:
                        if (fv.DataLength <= 0)
                        {
                            textFieldCount++;

                            foreach (Query.DocumentResultForSort mydoc in docs)
                            {
                                writeTextString.AppendLine(BuildWriteTextLine(Table.DBTableName,
                                    fv.FieldName, fv.Value.Replace("'", "''"), mydoc.DocId, fv.Type));
                            }
                        }
                        break;
                }
            }

            if (textFieldCount < doc.FieldValues.Count)
            {

                sql.AppendFormat("update {0} set ", Table.DBTableName);

                int i = 0;

                foreach (Data.FieldValue fv in doc.FieldValues)
                {
                    string value;

                    if (fv.Value == null)
                    {
                        value = "NULL";
                    }
                    else
                    {
                        value = fv.Type == Data.DataType.Varchar || fv.Type == Data.DataType.NVarchar ||
                            fv.Type == Data.DataType.NChar || fv.Type == Data.DataType.Char ||
                            fv.Type == Data.DataType.Date || fv.Type == Data.DataType.DateTime ||
                            fv.Type == Data.DataType.SmallDateTime || fv.Type == Data.DataType.Data ?
                            string.Format("'{0}'", fv.Value.Replace("'", "''")) : fv.Value;
                    }

                    if (i++ == 0)
                    {
                        sql.AppendFormat("[{0}]={1}", fv.FieldName, value);
                    }
                    else
                    {
                        sql.AppendFormat(",[{0}]={1}", fv.FieldName, value);
                    }
                }

                if (DocIdReplaceField == null)
                {
                    sql.Append(" where docId in (");
                }
                else
                {
                    sql.AppendFormat(" where {0} in (", DocIdReplaceField);
                }

                i = 0;

                foreach (Query.DocumentResultForSort docResult in docs)
                {
                    int docId = docResult.DocId;

                    if (DocIdReplaceField == null)
                    {
                        if (i++ == 0)
                        {
                            sql.AppendFormat("{0}", docId);
                        }
                        else
                        {
                            sql.AppendFormat(",{0}", docId);
                        }
                    }
                    else
                    {
                        long replaceFieldValue = this.DBProvider.GetDocIdReplaceFieldValue(docId);

                        if (i++ == 0)
                        {
                            sql.AppendFormat("{0}", replaceFieldValue);
                        }
                        else
                        {
                            sql.AppendFormat(",{0}", replaceFieldValue);
                        }
                    }
                }

                sql.Append(")");
            }

            sql.AppendLine(writeTextString.ToString());

            using (SQLDataProvider sqlData = new SQLDataProvider())
            {
                sqlData.Connect(Table.ConnectionString);
                sqlData.ExcuteSql(sql.ToString());
            }

        }

        public void MirrorUpdate(IList<FieldValue> fieldValues, IList<List<FieldValue>> docValues, IList<Hubble.Core.Query.DocumentResultForSort> docs)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable Query(IList<Hubble.Core.Data.Field> selectFields, IList<Query.DocumentResultForSort> docs)
        {
            return Query(selectFields, docs, 0, docs.Count - 1);
        }

        public System.Data.DataTable Query(IList<Hubble.Core.Data.Field> selectFields, IList<Query.DocumentResultForSort> docs, int begin, int end)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("SET TEXTSIZE 2147483647");
            sql.Append("select ");

            int i = 0;
            foreach (Hubble.Core.Data.Field field in selectFields)
            {
                if (DocIdReplaceField != null)
                {
                    if (field.Name.Equals("DocId", StringComparison.CurrentCultureIgnoreCase))
                    {
                        continue;
                    }
                }

                if (i++ == 0)
                {
                    sql.AppendFormat("[{0}]", field.Name);
                }
                else
                {
                    sql.AppendFormat(",[{0}]", field.Name);
                }
            }

            if (DocIdReplaceField != null)
            {
                sql.AppendFormat(",[{0}]", DocIdReplaceField);
            }


            if (DocIdReplaceField == null)
            {
                sql.AppendFormat(" from {0} where docId in (", Table.DBTableName);
            }
            else
            {
                sql.AppendFormat(" from {0} where {1} in (", Table.DBTableName, DocIdReplaceField);
            }

            i = 0;

            Dictionary<long, int> replaceFieldValueToDocId = null;

            if (DocIdReplaceField != null)
            {
                replaceFieldValueToDocId = new Dictionary<long, int>();
            }

            for (int j = begin; j <= end; j++)
            {
                if (j >= docs.Count)
                {
                    break;
                }

                int docId = docs[j].DocId;

                if (DocIdReplaceField == null)
                {
                    if (i++ == 0)
                    {
                        sql.AppendFormat("{0}", docId);
                    }
                    else
                    {
                        sql.AppendFormat(",{0}", docId);
                    }
                }
                else
                {
                    long replaceFieldValue = this.DBProvider.GetDocIdReplaceFieldValue(docId);

                    replaceFieldValueToDocId.Add(replaceFieldValue, docId);

                    if (i++ == 0)
                    {
                        sql.AppendFormat("{0}", replaceFieldValue);
                    }
                    else
                    {
                        sql.AppendFormat(",{0}", replaceFieldValue);
                    }
                }
            
            }

            sql.Append(")");

            using (SQLDataProvider sqlData = new SQLDataProvider())
            {
                sqlData.Connect(Table.ConnectionString);

                System.Data.DataTable table = sqlData.QuerySql(sql.ToString()).Tables[0];

                if (DocIdReplaceField != null)
                {
                    table.Columns.Add(new System.Data.DataColumn("DocId", typeof(int)));

                    for (i = 0; i < table.Rows.Count; i++)
                    {
                        table.Rows[i]["DocId"] =
                            replaceFieldValueToDocId[long.Parse(table.Rows[i][DocIdReplaceField].ToString())];
                    }
                }

                return table;
            }
        }

        unsafe private string GetInSql(Data.DBProvider dbProvider, Query.DocumentResultForSort[] result, int begin, int count)
        {
            if (begin + count > result.Length)
            {
                return null;
            }

            StringBuilder sql = new StringBuilder();

            if (dbProvider.DocIdReplaceField == null)
            {
                sql.Append("docId in (");
            }
            else
            {
                sql.AppendFormat("{0} in (", dbProvider.DocIdReplaceField);
            }

            Dictionary<long, int> replaceFieldValueToDocId = null;

            if (dbProvider.DocIdReplaceField != null)
            {
                replaceFieldValueToDocId = new Dictionary<long, int>();
            }

            int i = 0;

            for (int j = begin; j < begin + count; j++)
            {
                Query.DocumentResultForSort docResult = result[j];
                int docId = docResult.DocId;

                if (dbProvider.DocIdReplaceField == null)
                {
                    if (i++ == 0)
                    {
                        sql.AppendFormat("{0}", docId);
                    }
                    else
                    {
                        sql.AppendFormat(",{0}", docId);
                    }
                }
                else
                {
                    long replaceFieldValue = dbProvider.GetDocIdReplaceFieldValue(docId);

                    if (i++ == 0)
                    {
                        sql.AppendFormat("{0}", replaceFieldValue);
                    }
                    else
                    {
                        sql.AppendFormat(",{0}", replaceFieldValue);
                    }
                }
            }

            sql.Append(")");

            return sql.ToString();
        }

        public IList<Core.Query.DocumentResultForSort> GetDocumentResultForSortList(int end,
            Core.Query.DocumentResultForSort[] docResults, string orderby)
        {
            if (docResults.Length == 0)
            {
                return new List<Core.Query.DocumentResultForSort>();
            }

            int unitLength = Math.Max(10000, (end + 1) * 10);

            List<IList<Core.Query.DocumentResultForSort>> docResultForSortList = new List<IList<Hubble.Core.Query.DocumentResultForSort>>();

            for (int begin = 0; begin < docResults.Length; begin += unitLength)
            {
                int count = Math.Min(unitLength, docResults.Length - begin);
                docResultForSortList.Add(GetDocumentResultForSortList(end,
                    GetInSql(_DBProvder, docResults, begin, count), orderby));
            }

            if (docResultForSortList.Count == 1)
            {
                return docResultForSortList[0];
            }

            List<Core.Query.DocumentResultForSort> temp = new List<Hubble.Core.Query.DocumentResultForSort>();

            foreach (List<Core.Query.DocumentResultForSort> docSort in docResultForSortList)
            {
                temp.AddRange(docSort);
            }

            return GetDocumentResultForSortList(end, temp.ToArray(), orderby);
        }

        public IList<Core.Query.DocumentResultForSort> GetDocumentResultForSortList(int end, string where, string orderby)
        {
            string sql;

            if (end >= 0)
            {
                sql = string.Format("select top {0} ", end + 1);
            }
            else
            {
                sql = "select ";
            }

            if (string.IsNullOrEmpty(where))
            {
                if (DocIdReplaceField == null)
                {
                    sql += string.Format(" docid from {0} ", Table.DBTableName);
                }
                else
                {
                    sql += string.Format(" {0} from {1} ", DocIdReplaceField, Table.DBTableName);
                }
            }
            else
            {
                if (DocIdReplaceField == null)
                {
                    sql += string.Format(" docid from {0} where {1}", Table.DBTableName, where);
                }
                else
                {
                    sql += string.Format(" {0} from {1} where {2}", DocIdReplaceField, Table.DBTableName, where);
                }
            }

            if (!string.IsNullOrEmpty(orderby))
            {
                sql += " order by " + orderby;
            }

            List<Core.Query.DocumentResultForSort> result = new List<Core.Query.DocumentResultForSort>();

            using (SQLDataProvider sqlData = new SQLDataProvider())
            {
                sqlData.Connect(Table.ConnectionString);
                foreach (System.Data.DataRow row in sqlData.QuerySql(sql).Tables[0].Rows)
                {
                    int docId;
                    if (DocIdReplaceField == null)
                    {
                        docId = int.Parse(row[0].ToString());
                    }
                    else
                    {
                        docId = DBProvider.GetDocIdFromDocIdReplaceFieldValue(long.Parse(row[DocIdReplaceField].ToString()));

                        if (docId < 0)
                        {
                            continue;
                        }
                    }

                    result.Add(new Core.Query.DocumentResultForSort(docId));
                }
            }

            return result;

        }


        public Core.SFQL.Parse.DocumentResultWhereDictionary GetDocumentResults(int end, string where, string orderby)
        {
            string sql;

            if (end >= 0)
            {
                sql = string.Format("select top {0} ", end + 1);
            }
            else
            {
                sql = "select ";
            }

            if (string.IsNullOrEmpty(where))
            {
                if (DocIdReplaceField == null)
                {
                    sql += string.Format(" docid from {0} ", Table.DBTableName);
                }
                else
                {
                    sql += string.Format(" {0} from {1} ", DocIdReplaceField, Table.DBTableName);
                }
            }
            else
            {
                if (DocIdReplaceField == null)
                {
                    sql += string.Format(" docid from {0} where {1}", Table.DBTableName, where);
                }
                else
                {
                    sql += string.Format(" {0} from {1} where {2}", DocIdReplaceField, Table.DBTableName, where);
                }
            }

            if (!string.IsNullOrEmpty(orderby))
            {
                sql += " order by " + orderby;
            }

            Core.SFQL.Parse.DocumentResultWhereDictionary result = new Core.SFQL.Parse.DocumentResultWhereDictionary();

            using (SQLDataProvider sqlData = new SQLDataProvider())
            {
                sqlData.Connect(Table.ConnectionString);
                foreach (System.Data.DataRow row in sqlData.QuerySql(sql).Tables[0].Rows)
                {
                    int docId;
                    if (DocIdReplaceField == null)
                    {
                        docId = int.Parse(row[0].ToString());
                    }
                    else
                    {
                        docId = DBProvider.GetDocIdFromDocIdReplaceFieldValue(long.Parse(row[DocIdReplaceField].ToString()));

                        if (docId < 0)
                        {
                            continue;
                        }
                    }

                    result.Add(docId, new Hubble.Core.Query.DocumentResult(docId));
                }

                System.Data.DataSet ds;

                if (string.IsNullOrEmpty(where))
                {
                    ds = sqlData.QuerySql(string.Format("select count(*) cnt from {0}",
                        Table.DBTableName));
                }
                else
                {
                    ds = sqlData.QuerySql(string.Format("select count(*) cnt from {0} where {1}",
                        Table.DBTableName, where));
                }

                result.RelTotalCount = int.Parse(ds.Tables[0].Rows[0][0].ToString());
            }



            return result;
        }

        public System.Data.DataSet QuerySql(string sql)
        {
            using (SQLDataProvider sqlData = new SQLDataProvider())
            {
                string connectionString;
                if (Table == null)
                {
                    connectionString = this.ConnectionString;
                }
                else
                {
                    connectionString = Table.ConnectionString;
                }

                sqlData.Connect(connectionString);

                return sqlData.QuerySql(sql);
            }
        }


        public System.Data.DataSet GetSchema(string tableName)
        {
            string sql = string.Format("select * from {0}", tableName);

            using (SQLDataProvider sqlData = new SQLDataProvider())
            {
                string connectionString = this.ConnectionString;

                sqlData.Connect(connectionString);

                return sqlData.GetSchema(sql);
            }
        }

        public int ExcuteSql(string sql)
        {
            using (SQLDataProvider sqlData = new SQLDataProvider())
            {
                string connectionString;
                if (Table == null)
                {
                    connectionString = this.ConnectionString;
                }
                else
                {
                    connectionString = Table.ConnectionString;
                }

                sqlData.Connect(connectionString);

                return sqlData.ExcuteSql(sql);
            }
        }

        public void ConnectionTest()
        {
            using (SQLDataProvider sqlData = new SQLDataProvider())
            {
                string connectionString;
                if (Table == null)
                {
                    connectionString = this.ConnectionString;
                }
                else
                {
                    connectionString = Table.ConnectionString;
                }

                sqlData.Connect(connectionString);
            }
        }

        string _DocIdReplaceField = null;

        public string DocIdReplaceField
        {
            get
            {
                return _DocIdReplaceField;
            }
            set
            {
                _DocIdReplaceField = value;
            }
        }

        DBProvider _DBProvder;
        public DBProvider DBProvider
        {
            get
            {
                return _DBProvder;
            }
            set
            {
                _DBProvder = value;
            }
        }

        #endregion


        #region INamedExternalReference Members

        public string Name
        {
            get 
            {
                return "SQLSERVER2000";
            }
        }

        #endregion

    }
}
