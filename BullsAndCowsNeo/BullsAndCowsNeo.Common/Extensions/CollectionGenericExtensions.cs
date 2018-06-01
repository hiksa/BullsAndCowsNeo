using Neo;
using Neo.VM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BullsAndCowsNeo.Common
{
    public static class CollectionGenericExtensions
    {
        /// <summary>
        /// This method will create a object instance with the right properties and values if the properties list
        /// has all the corresponding values in the same type siquence written for the object/class as well
        /// will be used mainly for smart contract custom events and their m
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propListValues"></param>
        /// <returns></returns>
        public static T CreateObject<T>(this IEnumerable<object> propListValues)
        {
            var newT = Activator.CreateInstance<T>();
            if (propListValues.Count() == newT.GetType().GetProperties().Length)
            {
                for (int i = 0; i < propListValues.Count(); i++)
                {
                    var value = propListValues.ElementAt(i);
                    var property = newT.GetType().GetProperties()[i];

                    if (property.PropertyType == typeof(int) && value.GetType() == typeof(string))
                    {
                        int defaultInt = 0;
                        if (int.TryParse((string)value, out defaultInt))
                        {
                            newT.GetType().GetProperty(property.Name).SetValue(newT, int.Parse((string)value));
                        }
                    }
                    else
                    {
                        if (value.GetType() == property.PropertyType)
                        {
                            newT.GetType().GetProperty(property.Name).SetValue(newT, value);
                        }
                    }
                }
                return newT;
            }
            return default(T);
        }

        public static IEnumerable<string> ToStringList(this IEnumerable<StackItem> stackItems)
        {
            var result = stackItems.Skip(1).Select(si => si.GetByteArray().ToHexString().HexStringToString());
            return result;
        }
    }
}
