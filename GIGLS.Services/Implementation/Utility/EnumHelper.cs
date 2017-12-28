using System;
using System.ComponentModel;
using System.Reflection;

namespace GIGLS.Services.Implementation.Utility
{
    public static class EnumHelper
    {
        /// <summary>
        /// Retrieve the description on the enum, e.g.
        /// [Description("ARRIVAL SCAN AT SERVICE CENTER(TERMINAL PICK UP)")]
        /// ASP,
        /// Then when you pass in the enum, it will retrieve the description
        /// </summary>
        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }
    }
}
