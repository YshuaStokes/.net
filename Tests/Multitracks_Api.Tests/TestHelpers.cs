using System;
using System.Data;
using Moq;

namespace Multitracks_Api.Tests
{
    public static class TestHelpers
    {
        // This helper class provides utilities for testing classes that use static methods
    }

    public class MockDataRow
    {
        private readonly object _value;

        public MockDataRow(object value)
        {
            _value = value;
        }

        public T GetValue<T>()
        {
            return (T)Convert.ChangeType(_value, typeof(T));
        }
    }
}