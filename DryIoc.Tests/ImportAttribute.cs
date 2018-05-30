using System;

namespace DryIoc.Tests
{
  [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
  public class ImportAttribute : Attribute
  {
  }
}