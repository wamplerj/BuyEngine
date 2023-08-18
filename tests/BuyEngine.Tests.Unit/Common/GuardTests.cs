using BuyEngine.Common;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace BuyEngine.Tests.Unit.Common;

public class GuardTests
{
    [Test]
    public async Task NullString_Throws() => Assert.Throws<ArgumentException>(() => Guard.Empty(null, "value"));

    [Test]
    public async Task EmptyStrings_Throws() => Assert.Throws<ArgumentException>(() => Guard.Empty("", "value"));

    [Test]
    public async Task AnyString_Succeeds() => Guard.Empty("test", "value");
}
