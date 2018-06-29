using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using System;
using System.ComponentModel;
using System.Numerics;

namespace EventsTestingServerContract
{
    public class EventsTestsContract : SmartContract
    {
        public static byte[] Main(string op, string key, string value)
        {
            if (op == ParametersAndEventTestContractConstants.SET_METHOD)
            {
                if (SaveEntry(key, value))
                {
                    Runtime.Notify(value);
                    return new byte[] { };
                }
            }
            if (op == ParametersAndEventTestContractConstants.SINGLE_PARAM_ACTION)
            {
                Runtime.Notify("single-param", value);
            }
            if (op == ParametersAndEventTestContractConstants.MULTIPLE_PARAM_ACTION)
            {
                Runtime.Notify("multiple params", "param1", "param2", "param3", "param444444444");
            }
            if (op == ParametersAndEventTestContractConstants.GET_METHOD)
            {
                return GetResultByte(key);
            }
            return null;
        }

        private static bool SaveEntry(string key, string newResult)
        {
            try
            {
                Storage.Put(Storage.CurrentContext, key, newResult);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private static byte[] GetResultByte(string key)
        {
            return Storage.Get(Storage.CurrentContext, key);
        }
    }
}
