using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel;
using System.Reflection;
using System.Collections;

namespace DataManager
{
    public class SortableSearchableList<T> : BindingList<T>, IBindingList
    {
        private const string PROJ_FILE_NAME = "CollectionHelper";
        bool isSortedValue;
        ListSortDirection sortDirectionValue;
        PropertyDescriptor sortPropertyValue;
        ArrayList sortedList = null;
        ArrayList unsortedItems = null;

        protected override bool SupportsSearchingCore
        {
            get
            {
                return true;
            }
        }

        public SortableSearchableList(IList<T> list)
            : base(list)
        {

        }

        bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        protected override int FindCore(PropertyDescriptor prop, object key)
        {
            // Get the property info for the specified property.
            PropertyInfo propInfo = typeof(T).GetProperty(prop.Name);
            T item;

            if (key != null)
            {
                // Loop through the items to see if the key
                // value matches the property value.
                for (int i = 0; i < Count; ++i)
                {
                    item = (T)Items[i];
                    if (propInfo.GetValue(item, null).Equals(key))
                        return i;
                }
            }
            return -1;
        }

        public int Find(string property, object key)
        {
            // Check the properties for a property with the specified name.
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            PropertyDescriptor prop = properties.Find(property, true);

            // If there is not a match, return -1 otherwise pass search to
            // FindCore method.
            if (prop == null)
                return -1;
            else
                return FindCore(prop, key);
        }

        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        protected override bool IsSortedCore
        {
            get { return isSortedValue; }
        }

        protected override void ApplySortCore(PropertyDescriptor prop,
            ListSortDirection direction)
        {
            sortedList = new ArrayList();

            // Check to see if the property type we are sorting by implements
            // the IComparable interface.
            Type interfaceType = prop.PropertyType.GetInterface("IComparable");

            if (interfaceType != null)
            {
                // If so, set the SortPropertyValue and SortDirectionValue.
                sortPropertyValue = prop;
                sortDirectionValue = direction;

                unsortedItems = new ArrayList(this.Count);

                // Loop through each item, adding it the the sortedItems ArrayList.
                foreach (Object item in this.Items)
                {
                    sortedList.Add(prop.GetValue(item));
                    unsortedItems.Add(item);
                }
                // Call Sort on the ArrayList.
                sortedList.Sort();
                T temp;

                // Check the sort direction and then copy the sorted items
                // back into the list.
                if (direction == ListSortDirection.Descending)
                    sortedList.Reverse();

                for (int i = 0; i < this.Count; i++)
                {
                    int position = Find(prop.Name, sortedList[i]);
                    if (position != i)
                    {
                        temp = this[i];
                        this[i] = this[position];
                        this[position] = temp;
                    }
                }

                isSortedValue = true;

                // Raise the ListChanged event so bound controls refresh their
                // values.
                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
            else
                // If the property type does not implement IComparable, let the user
                // know.
                throw new NotSupportedException("Cannot sort by " + prop.Name + 
                    ". This" + prop.PropertyType.ToString() + " does not implement IComparable" + Environment.NewLine +
                    "Error CNF-339 in " + PROJ_FILE_NAME + ".ApplySortCore()");
        }

        protected override void RemoveSortCore()
        {
            int position;
            object temp;
            // Ensure the list has been sorted.
            if (unsortedItems != null)
            {
                // Loop through the unsorted items and reorder the
                // list per the unsorted list.
                for (int i = 0; i < unsortedItems.Count; )
                {
                    position = this.Find(sortPropertyValue.Name,
                        unsortedItems[i].GetType().
                        GetProperty(sortPropertyValue.Name).GetValue(unsortedItems[i], null));
                    if (position > 0 && position != i)
                    {
                        temp = this[i];
                        this[i] = this[position];
                        this[position] = (T)temp;
                        i++;
                    }
                    else if (position == i)
                        i++;
                    else
                        // If an item in the unsorted list no longer exists,
                        // delete it.
                        unsortedItems.RemoveAt(i);
                }
                isSortedValue = false;
                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
        }

        public void RemoveSort()
        {
            RemoveSortCore();
        }

        protected override PropertyDescriptor SortPropertyCore
        {
            get { return sortPropertyValue; }
        }

        protected override ListSortDirection SortDirectionCore
        {
            get { return sortDirectionValue; }
        }

        public override void EndNew(int itemIndex)
        {
            // Check to see if the item is added to the end of the list,
            // and if so, re-sort the list.
            if (sortPropertyValue != null && itemIndex == this.Count - 1)
                ApplySortCore(this.sortPropertyValue, this.sortDirectionValue);

            base.EndNew(itemIndex);
        }
    }

    public class CollectionHelper
    {
        private const string PROJ_FILE_NAME = "CollectionHelper";
        private CollectionHelper()
        {
        }

        public static List<T> ConvertToListOf<T>(IList<T> iList)
        {
            List<T> result = new List<T>();
            foreach (T value in iList)
                result.Add(value);

            return result;
        }  

        public static DataTable ConvertTo<T>(IList<T> list)
        {
            DataTable table = CreateTable<T>();
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (T item in list)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item);
                }

                table.Rows.Add(row);
            }
            return table;
        }

        public static DataTable ConvertTo<T>(IList<T> list, string filter)
        {
            DataTable table = CreateTable<T>();
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (T item in list)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item);
                }

                table.Rows.Add(row);
            }

            return CreateFilteredTable(table, filter);
        }

        private static DataTable CreateFilteredTable(DataTable dt, string filter)
        {
            DataTable dt1 = dt.Clone();
            try
            {
                foreach (DataRow dr in dt.Select(filter))
                {
                    dt1.ImportRow(dr);
                }
                return dt1;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating a filtered table using a filter value of: " + filter + "." + Environment.NewLine +
                    "Error CNF-339 in " + PROJ_FILE_NAME + ".CreateFilteredTable(): " + ex.Message);
            }
        }

        public static void AddObjectsToTable<T>(IList<T> list, ref DataTable table, bool ignoreExceptions)
        {
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (T item in list)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    if ((row.Table.Columns.Contains(prop.Name)))
                    {
                        row[prop.Name] = prop.GetValue(item);
                    }
                }
                try
                {
                    table.Rows.Add(row);
                }
                catch (Exception ex)
                {
                    if (ignoreExceptions == false)
                        throw new Exception("An error occurred while creating and populating internal data storage." + Environment.NewLine +
                            "Error CNF-340 in " + PROJ_FILE_NAME + ".AddObjectsToTable(): " + ex.Message);
                }
            }
        }

        public static IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            IList<T> list = null;

            if (rows != null)
            {
                list = new List<T>();

                foreach (DataRow row in rows)
                {
                    T item = CreateObjectFromDataRow<T>(row);
                    list.Add(item);
                }
            }

            return list;
        }

        public static IList<T> ConvertTo<T>(DataTable table)
        {
            if (table == null)
            {
                return null;
            }

            List<DataRow> rows = new List<DataRow>();

            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }

            return ConvertTo<T>(rows);
        }

        public static T CreateObjectFromDataRow<T>(DataRow row)
        {
            T obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in row.Table.Columns)
                {
                    PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName);
                    try
                    {
                        object value = row[column.ColumnName];
                        if(! (value is System.DBNull))
                        {
                            prop.SetValue(obj, value, null);
                        }
                    }
                    catch(Exception ex)
                    {
                        throw new Exception("An error occurred while creating and populating internal data storage." + Environment.NewLine +
                            "Error CNF-341 in " + PROJ_FILE_NAME + ".CreateObjectFromDataRow(): " + ex.Message);
                    }
                }
            }
            return obj;
        }

        public static T Cast<T>(object o)
        {
            return (T)o;
        }

        public static T CreateObjectFromDataRow<T>(DataRow row, bool formatColumnName)
        {
            T obj = default(T);
            string columnName = "";
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in row.Table.Columns)
                {
                    columnName = column.ColumnName;
                    if (formatColumnName)
                    {
                        columnName = convertPropColumnName(columnName);
                    }
                    PropertyInfo prop = obj.GetType().GetProperty(columnName);
                    if (prop != null)
                    {
                        try
                        {
                            object value = row[column.ColumnName];
                            TypeCode valueTypeCode = Type.GetTypeCode(value.GetType());
                            TypeCode typeCode = Type.GetTypeCode(prop.PropertyType);

                            if (!(value is System.DBNull))
                            {
                                prop.SetValue(obj, (Convert.ChangeType(value, typeCode)), null);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("An error occurred while creating and populating internal data storage." + Environment.NewLine +
                                "Error CNF-342 in " + PROJ_FILE_NAME + ".CreateObjectFromDataRow(): " + ex.Message);
                        }
                    }
                    else
                    {
                        throw new Exception("An error occurred while creating and populating internal data storage." + Environment.NewLine +
                            "The problem occurred while finding the property value for: " + columnName + "." + Environment.NewLine +
                            "Error CNF-343 in " + PROJ_FILE_NAME + ".CreateObjectFromDataRow()");
                    }
                }
            }
            return obj;
        }

        private static string convertPropColumnName(string columnName)
        {
            // A COLUMN NAME FROM THE DATABASE IS XXX_XXX_XXX
            // the property values for the coresponding data object row must be converted to XxxXxxXxx
            // this is a hack and a quick fix to get off of gigaspaces
            columnName = columnName.ToLower();
            columnName = columnName.Replace("_", " ");

            columnName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(columnName);
            columnName = columnName.Replace(" ", "");
            return columnName;
        }

        public static DataTable CreateTable<T>()
        {
            Type entityType = typeof(T);
            DataTable table = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            return table;
        }

        public static DataRow CreateDataRowFromObject<T1>(object obj, DataRow dataRow)
        {
            Type entityType = typeof(T1);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);
            foreach (PropertyDescriptor prop in properties)
            {
                if ((dataRow.Table.Columns.Contains(prop.Name)))
                {
                    dataRow[prop.Name] = prop.GetValue(obj);
                }
            }
            return dataRow;
        }

        public static void UpdateDataRowFromObject<T1>(object obj, ref DataRow dataRow)
        {
            Type entityType = typeof(T1);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);
            foreach (PropertyDescriptor prop in properties)
            {
                dataRow[prop.Name] = prop.GetValue(obj);
            }
        }

        private static int getColumnIndex(IDataReader dr, string objFieldName)
        {
            int i = 0;
            for (i = 0; i < dr.FieldCount - 1; i++)
            {
                string colName = dr.GetName(i).Replace("_", "");
                if (colName.Equals(objFieldName))
                    return i;
            }
            return -1;
        }
    }
}

