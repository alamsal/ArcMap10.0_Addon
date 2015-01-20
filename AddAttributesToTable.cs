using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using ESRI.ArcGIS.Geodatabase;

namespace ArcMapClassLibrary2
{
    public class AddAttributesToTable
    {
       
        public void StoreAttributesOnTableRow(ITable table, List<string> fields,List<string> values  ) 
        {
            IRow row = table.CreateRow();

            for (int i = 0; i < fields.Count; i++)
            {
                int index = table.FindField(fields[i]);
                row.set_Value(index, values[i]);
            }

            row.Store();
        }
    }
}
