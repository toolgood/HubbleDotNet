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
using Hubble.Core.Service;

namespace Hubble.Core.StoredProcedure
{
    class SP_SchemaList : StoredProcedure, IStoredProc, IHelper
    {
        #region IStoredProc Members

        override public string Name
        {
            get
            {
                return "SP_SchemaList";
            }
        }

        public void Run()
        {
            AddColumn("SchemaId");
            AddColumn("Name");
            AddColumn("Description");
            AddColumn("State");

            foreach (Task task in ScheduleTaskMgr.ScheduleMgr.GetTaskList())
            {
                NewRow();

                OutputValue("SchemaId", task.SchemaId);
                OutputValue("Name", task.Schema.Name);
                OutputValue("Description", task.Schema.Description);
                OutputValue("State", task.Schema.State.ToString());
            }
        }

        #endregion

        #region IHelper Members

        public string Help
        {
            get
            {
                return "List all task schemas!";
            }
        }

        #endregion
    }
}
