using NUnit.Framework;
using deprojectreferencer.HintPaths;

namespace deprojectreferencer.unit.tests.HintPaths
{
    public class HintPathLookupTests
    {
        private const string _base = @"..\..\..\buildsolutions\";
        private const string _projectPath = @"src\project\project.csproj";

        [Test]
        public void Should_construct_the_path_correctly_for_common_dlls()
        {
            string commonPath = string.Format(@"{0}common\Wonga.Common.Data.dll", _base);
            
            var newPath = new HintPathLookup().For(@"..\..\..\build\Wonga.Common.Data\Wonga.Common.Data.dll", _projectPath);
            
            Assert.That(newPath, Is.EqualTo(commonPath));
        }

        [Test]
        public void Should_construct_the_path_correctly_for_api_dlls()
        {
            string apiPath = string.Format(@"{0}common\Wonga.Api.dll", _base);

            var newPath = new HintPathLookup().For(@"..\..\..\build\Wonga.Api\Wonga.Api.dll", _projectPath);
            

            Assert.That(newPath, Is.EqualTo(apiPath));
        }

        [Test]
        public void Should_construct_the_path_correctly_for_integration_dlls()
        {
            string integrationPath = string.Format(@"{0}integration\Wonga.Integration.Payments.dll", _base);

            var newPath = new HintPathLookup().For(@"..\..\..\build\Wonga.Integration.Payments\Wonga.Integration.Payments.dll", _projectPath);

            Assert.That(newPath, Is.EqualTo(integrationPath));
        }

        [Test]
        public void Should_construct_the_path_correctly_for_ops_dlls()
        {
            string opsPath = string.Format(@"{0}ops\Wonga.Ops.Data.dll", _base);

            var newPath = new HintPathLookup().For(@"..\..\..\build\Wonga.Ops.Data\Wonga.Ops.Data.dll", _projectPath);

            Assert.That(newPath, Is.EqualTo(opsPath));
        }

        [Test]
        public void Should_construct_the_path_correctly_for_comms_dlls()
        {
            string commsPath = string.Format(@"{0}comms\Wonga.Comms.dll", _base);

            var newPath = new HintPathLookup().For(@"..\..\..\build\Wonga.Comms\Wonga.Comms.dll", _projectPath);

            Assert.That(newPath, Is.EqualTo(commsPath));
        }

        [Test]
        public void Should_construct_the_path_correctly_for_payments_dlls()
        {
            string paymentsPath = string.Format(@"{0}payments\Wonga.Payments.Data.dll", _base);

            var newPath = new HintPathLookup().For(@"..\..\..\build\Wonga.Payments.Data\Wonga.Payments.Data.dll", _projectPath);

            Assert.That(newPath, Is.EqualTo(paymentsPath));
        }

        [Test]
        public void Should_construct_the_path_correctly_for_risk_dlls()
        {
            string riskPath = string.Format(@"{0}risk\Wonga.Risk.Data.dll", _base);

            var newPath = new HintPathLookup().For(@"..\..\..\build\Wonga.Risk.Data\Wonga.Risk.Data.dll", _projectPath);

            Assert.That(newPath, Is.EqualTo(riskPath));
        }

        [Test]
        public void Should_construct_the_path_correctly_for_components_dlls()
        {
            string componentsPath = string.Format(@"{0}components\Wonga.BankGateway.Common.dll", _base);

            var newPath = new HintPathLookup().For(@"..\..\..\build\Wonga.BankGateway.Common\Wonga.BankGateway.Common.dll", _projectPath);

            Assert.That(newPath, Is.EqualTo(componentsPath));
        }

        [Test]
        public void Should_use_different_base_directory_for_component_project_files() {
            const string componentsProject = @"src\components\somecomponent\someprojectfolder\someproject.csproj";
            
            var newPath = new HintPathLookup().For(@"..\..\..\..\build\Wonga.Integration.Payments\Wonga.Integration.Payments.dll", componentsProject);

            Assert.That(newPath, Is.StringContaining(@"..\..\..\..\buildsolutions\"));
        }
    }
}