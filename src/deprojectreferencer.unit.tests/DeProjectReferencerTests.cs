using System.IO;
using NUnit.Framework;

namespace deprojectreferencer.unit.tests
{
    [TestFixture, Category("AcceptanceTest")]
    public class DeProjectReferencerTests
    {
        [Test]
        public void Deprojectreferencing_should_match_expected_output()
        {
            string expected = File.ReadAllText(@"samples\expected.csproj");

            // to not interact with the other tests
            File.Copy(@"samples\project.csproj", @"samples\acceptance.csproj", true);

            new DeProjectReferencer(@"samples\acceptance.csproj").Dereference();

            string dereferenced = File.ReadAllText(@"samples\acceptance.csproj");

            Assert.That(dereferenced, Is.EqualTo(expected));
        }
    }
}