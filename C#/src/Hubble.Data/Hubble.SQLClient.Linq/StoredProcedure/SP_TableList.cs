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
using System.Linq;
using System.Text;

using Hubble.SQLClient.Linq.Entities;
namespace Hubble.SQLClient.Linq.StoredProcedure
{
    public partial class SPDataContext : DataContext
    {
        /// <summary>
        /// List all the tables in all the database
        /// </summary>
        /// <returns></returns>
        public HashSet<TableInfo> SP_TableList()
        {
            SPExecutor<TableInfo> spExecutor = new SPExecutor<TableInfo>(this);

            return spExecutor.ReturnAsHashSet("exec SP_TableList", null);
        }


        public HashSet<TableInfo> SP_TableList(string databaseName)
        {
            SPExecutor<TableInfo> spExecutor = new SPExecutor<TableInfo>(this);

            HashSet<TableInfo> result = spExecutor.ReturnAsHashSet("exec SP_TableList {0}", databaseName);

            foreach (TableInfo tableInfo in result)
            {
                tableInfo.TableName = tableInfo.TableName.Substring(databaseName.Trim().Length + 1,
                    tableInfo.TableName.Length - (databaseName.Trim().Length + 1));
            }

            return result;
        }

    }
}
