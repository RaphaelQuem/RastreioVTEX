using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RastreioECO.Infra
{
    public static class ExtensionMethods
    {
        public static List<T> ToModelList<T>(this DataTable dt)
        {
            List<T> objlist = new List<T>();

            foreach (DataRow row in dt.Rows)
            {

                T obj = (T)Activator.CreateInstance(typeof(T));
                foreach (PropertyInfo info in obj.GetType().GetProperties())
                {
                    try
                    {
                        info.SetValue(obj, Convert.ChangeType(row[info.Name], info.PropertyType));
                    }
                    catch (Exception ex)
                    {
                    }
                }
                objlist.Add(obj);

            }
            return objlist;
        }

    }
}
