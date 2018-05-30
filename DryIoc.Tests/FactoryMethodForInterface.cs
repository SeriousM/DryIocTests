using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DryIoc.Tests
{
  [TestClass]
  public class FactoryMethodForInterface
  {
    [TestMethod]
    public void Test()
    {
      var container = new Container();
      container.Register<ILogger>(Reuse.Transient, Made.Of(() => new Logger(Arg.Index<string>(0)), r => r.Parent.ImplementationType.Name));
      container.Register<IB, B>();
      container.Register<B>();

      using (var scope = container.OpenScope())
      {
        var b1 = scope.Resolve<IB>();
        var b2 = scope.Resolve<B>();

        Assert.IsNotNull(b1);
        Assert.IsNotNull(b1.LoggerInstance);
        Assert.AreNotEqual(b1.LoggerInstance, b2.LoggerInstance);
        Assert.AreNotEqual(b1.LoggerInstance.RandomId, b2.LoggerInstance.RandomId);
        Assert.AreEqual("B", b1.LoggerInstance.NameOfLogger);
        Assert.AreEqual("B", b2.LoggerInstance.NameOfLogger);
      }
    }

    public interface ILogger
    {
      string RandomId { get; }
      string NameOfLogger { get; }
    }

    public class Logger : ILogger
    {
      public string NameOfLogger { get; }
      public string RandomId { get; } = Guid.NewGuid().ToString();

      public Logger(string nameOfLogger)
      {
        NameOfLogger = nameOfLogger;
      }
    }

    public interface IB
    {
      ILogger LoggerInstance { get; }
    }

    public class B : IB
    {
      public ILogger LoggerInstance { get; }

      public B(ILogger logger)
      {
        LoggerInstance = logger;
      }
    }
  }
}