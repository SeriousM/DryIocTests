using System.Reflection;
using ImTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DryIoc.Tests
{
  [TestClass]
  public class AllPropertiesWithImportAttributeInjection
  {
    [TestMethod]
    public void Test()
    {
      var container = new Container(Rules.Default.With(AllPropertiesWithImportAttribute()));

      container.Register<A>();
      container.Register<B>();
      container.Register<C>();

      var a = container.Resolve<A>();

      Assert.IsNotNull(a.PublicDep);
      Assert.IsNotNull(a.GetType().GetProperty("PrivateDep", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(a));

      var b = container.Resolve<B>();

      Assert.IsNull(b.NotMarkedAsImportDep);
    }

    public static PropertiesAndFieldsSelector AllPropertiesWithImportAttribute()
    {
      PropertyOrFieldServiceInfo info(MemberInfo m, Request r) => PropertyOrFieldServiceInfo.Of(m).WithDetails(ServiceDetails.Of(ifUnresolved: IfUnresolved.Throw));

      return req =>
      {
        var properties = req.ImplementationType.GetMembers(_ => _.DeclaredProperties, includeBase: true)
                            .Match(
                               p => CustomAttributeExtensions.GetCustomAttribute<ImportAttribute>((MemberInfo)p) != null
                                 && PropertiesAndFields.IsInjectable((PropertyInfo)p, withNonPublic: true, withPrimitive: false),
                               p => info(p, req));

        return properties;
      };
    }

    class A
    {
      [Import]
      public C PublicDep { get; set; }

      [Import]
      private C PrivateDep { get; set; }
    }

    class B 
    {
      public C NotMarkedAsImportDep { get; set; }
    }

    class C {}
  }
}