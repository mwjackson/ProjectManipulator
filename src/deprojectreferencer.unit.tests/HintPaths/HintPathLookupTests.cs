using NUnit.Framework;
using deprojectreferencer.HintPaths;

namespace deprojectreferencer.unit.tests.HintPaths
{
    public class HintPathLookupTests
    {
        [Test]
        public void Should_construct_the_path_correctly_for_common_dlls()
        {
            const string commonPath = @"..\..\..\build\common\Wonga.Common.Data.dll";
            
            var newPath = new HintPathLookup().For(@"..\..\..\build\Wonga.Common.Data\Wonga.Common.Data.dll");
            
            Assert.That(newPath, Is.EqualTo(commonPath));
        }

        [Test]
        public void Should_construct_the_path_correctly_for_api_dlls()
        {
            const string componentsDll = @"..\..\..\build\common\Wonga.Api.dll";

            var newPath = new HintPathLookup().For(@"..\..\..\build\Wonga.Api\Wonga.Api.dll");

            Assert.That(newPath, Is.EqualTo(componentsDll));
        }

        [Test]
        public void Should_construct_the_path_correctly_for_integration_dlls()
        {
            const string integrationPath = @"..\..\..\build\integration\Wonga.Integration.Payments.dll";

            var newPath = new HintPathLookup().For(@"..\..\..\build\Wonga.Integration.Payments\Wonga.Integration.Payments.dll");

            Assert.That(newPath, Is.EqualTo(integrationPath));
        }

        [Test]
        public void Should_construct_the_path_correctly_for_ops_dlls()
        {
            const string opsPath = @"..\..\..\build\ops\Wonga.Ops.Commands.dll";

            var newPath = new HintPathLookup().For(@"..\..\..\build\Wonga.Ops.Commands\Wonga.Ops.Commands.dll");

            Assert.That(newPath, Is.EqualTo(opsPath));
        }

        [Test]
        public void Should_construct_the_path_correctly_for_payments_dlls()
        {
            const string paymentsPath = @"..\..\..\build\payments\Wonga.Payments.Commands.dll";

            var newPath = new HintPathLookup().For(@"..\..\..\build\Wonga.Payments.Commands\Wonga.Payments.Commands.dll");

            Assert.That(newPath, Is.EqualTo(paymentsPath));
        }

        [Test]
        public void Should_construct_the_path_correctly_for_risk_dlls()
        {
            const string riskPath = @"..\..\..\build\risk\Wonga.Risk.PublicMessages.dll";

            var newPath = new HintPathLookup().For(@"..\..\..\build\Wonga.Risk.PublicMessages\Wonga.Risk.PublicMessages.dll");

            Assert.That(newPath, Is.EqualTo(riskPath));
        }

        [Test]
        public void Should_construct_the_path_correctly_for_components_dlls()
        {
            const string componentsDll = @"..\..\..\build\components\Wonga.BankGateway.PublicMessages.dll";

            var newPath = new HintPathLookup().For(@"..\..\..\build\Wonga.BankGateway.PublicMessages\Wonga.BankGateway.PublicMessages.dll");

            Assert.That(newPath, Is.EqualTo(componentsDll));
        }
    }
}