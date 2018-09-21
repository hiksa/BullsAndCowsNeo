using Neo;
using Neo.VM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;

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
        
        public static T CreateObject<T>(this IEnumerable<StackItem> stackItems)
        {
            var instance = Activator.CreateInstance<T>();
            var properties = instance.GetType().GetProperties()
                .Where(x => !x.GetCustomAttributes(typeof(NotMappedAttribute), true).Any())
                .ToArray();

            if (stackItems.Count() == properties.Count())
            {
                for (int i = 0; i < stackItems.Count(); i++)
                {
                    var property = properties[i];
                    var rawValue = stackItems.ElementAt(i).GetByteArray();
                    if (property.PropertyType == typeof(BigInteger))
                    {
                        var valueAsString = rawValue.ToHexString().HexStringToString();
                        if (BigInteger.TryParse(valueAsString, out BigInteger defaultInt))
                        {
                            SetPropertyValue(property.Name, instance, BigInteger.Parse(valueAsString));
                        }
                    }
                    else if (property.PropertyType == typeof(byte[]))
                    {
                        SetPropertyValue(property.Name, instance, rawValue);
                    }
                    else if (property.PropertyType == typeof(string))
                    {
                        var valueAsString = rawValue.ToHexString().HexStringToString();
                        SetPropertyValue(property.Name, instance, valueAsString);
                    }
                }
            }

            return instance;
        }
        
        public static IEnumerable<string> ToStringList(this IEnumerable<StackItem> stackItems) =>
            stackItems.Skip(1).Select(si => si.GetByteArray().ToHexString().HexStringToString());

        private static void SetPropertyValue(string propertyName, object instance, object value) =>
            instance.GetType().GetProperty(propertyName).SetValue(instance, value);
    }
}
