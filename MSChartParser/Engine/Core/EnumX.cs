// Copyright (c) 2016-2020 Alexander Ong
// See LICENSE in project root for license information.

using System;
using System.Collections;
using System.Linq.Expressions;

namespace MoonscraperEngine
{
    public static class EnumX<EnumType> where EnumType : Enum
    {
        public static readonly EnumType[] Values = (EnumType[])Enum.GetValues(typeof(EnumType));
        public static int Count => Values.Length;
        static readonly string[] StringValues = new string[Count];

        static EnumX()
        {
            for (int i = 0; i < Values.Length; ++i)
            {
                StringValues[i] = Values[i].ToString();
            }
        }

        public static bool GenericTryParse(string str, out EnumType enumType)
        {
            for (int i = 0; i < Count; ++i)
            {
                if (string.Equals(str, StringValues[i]))
                {
                    enumType = Values[i];
                    return true;
                }
            }

            enumType = Values[0];

            return false;
        }
    }
}
