using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class CommonTools
{
    public static List<Type> GetSubClassNames(Type parentType)
    {
        var subTypeList = new List<Type>();
        var assembly = parentType.Assembly;
        var assemblyAllTypes = assembly.GetTypes();
        foreach (var itemType in assemblyAllTypes)
        {
            var baseType = itemType.BaseType;
            if (baseType != null)
            {
                if (baseType.Name == parentType.Name)
                {
                    subTypeList.Add(itemType);
                }
            }
        }
        return subTypeList;
    }

    public static object CreateInstance(this Type instrument)
    {
        return Activator.CreateInstance(instrument);
    }

    public static InstrumentBase CreateInstrumentInstance(this Type instrument)
    {
        return Activator.CreateInstance(instrument) as InstrumentBase;
    }
}
