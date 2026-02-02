using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CruscottoIncidenti.Utils
{
    public static class SelectListMapper
    {
        public static List<SelectListItem> GetSelectListFromEnum<T>() where T : Enum, IConvertible
        {
            var selectList = new List<SelectListItem>();

            var enumItems = Enum.GetValues(typeof(T))
                .Cast<T>()
                .ToDictionary(k => k.ToInt32(null), v => v.ToString());

            foreach (var item in enumItems)
            {
                selectList.Add(new SelectListItem() { Value = item.Key.ToString(), Text = item.Value });
            }

            return selectList;
        }

        public static List<SelectListItem> GetSelectListFromDictionary(Dictionary<string, string> dictionary)
        {
            var selectList = new List<SelectListItem>();

            foreach (var keyValuePair in dictionary)
            {
                selectList.Add(new SelectListItem() { Value = keyValuePair.Key, Text = keyValuePair.Value });
            }

            return selectList;
        }
    }
}