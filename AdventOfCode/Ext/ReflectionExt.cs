using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

public static class ReflectionExt
{
    public static IEnumerable<T> GetTypeSubclasses<T>(params object[] args)
        where T : class
	{
		var assemblyForType = Assembly.GetAssembly(typeof(T));
        if (assemblyForType != null)
        {
            var types = assemblyForType.GetTypes();

            foreach (Type type in types.Where(x => !x.IsAbstract && x.IsSubclassOf(typeof(T))))
            {
                var obj = Activator.CreateInstance(type, args);
                if (obj is T typedObj)
                {
                    yield return typedObj;
                }
            }
        }
    }
}
