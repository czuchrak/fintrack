using System.Reflection;

namespace Fintrack.App;

public static class AssemblyClass
{
    public static Assembly Assembly => typeof(AssemblyClass).GetTypeInfo().Assembly;
}