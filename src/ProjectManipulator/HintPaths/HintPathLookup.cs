using System.IO;

namespace ProjectManipulator.HintPaths
{
    public interface IHintPathLookup
    {
        string For(string oldPath, string projectPath);
    }

    public class HintPathLookup : IHintPathLookup
    {
        private readonly string _buildFolder;

        public HintPathLookup()
        {
            _buildFolder = @"buildsolutions";
        }

        public string For(string oldPath, string projectPath)
        {
            string baseBuildPath;
            if (projectPath.Contains(@"components")) baseBuildPath = string.Format(@"..\..\..\..\{0}", _buildFolder);
            else baseBuildPath = string.Format(@"..\..\..\{0}", _buildFolder);
            
            var fileName = new FileInfo(oldPath).Name;

            // services
            if (oldPath.Contains("Wonga.Common")) return string.Format(@"{1}\Wonga.Common\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.Api")) return string.Format(@"{1}\Wonga.Common\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.Integration")) return string.Format(@"{1}\Wonga.Integration\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Messages")) return string.Format(@"{1}\messages\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Commands")) return string.Format(@"{1}\messages\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.Ops")) return string.Format(@"{1}\Wonga.Ops\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.Comms")) return string.Format(@"{1}\Wonga.Comms\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.Payments")) return string.Format(@"{1}\Wonga.Payments\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.Risk")) return string.Format(@"{1}\Wonga.Risk\{0}", fileName, baseBuildPath);
            
            // components
            if (oldPath.Contains("Wonga.Address")) return string.Format(@"{1}\components\Wonga.Address\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.BankGateway")) return string.Format(@"{1}\components\Wonga.BankGateway\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.BankValidate")) return string.Format(@"{1}\components\Wonga.BankValidate\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.BlackList")) return string.Format(@"{1}\components\Wonga.BlackList\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.CallReport")) return string.Format(@"{1}\components\Wonga.CallReport\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.CallValidate")) return string.Format(@"{1}\components\Wonga.CallValidate\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.CardPayment")) return string.Format(@"{1}\components\Wonga.CardPayment\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.ColdStorage")) return string.Format(@"{1}\components\Wonga.ColdStorage\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.Email")) return string.Format(@"{1}\components\Wonga.Email\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.Equifax")) return string.Format(@"{1}\components\Wonga.Equifax\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.Experian")) return string.Format(@"{1}\components\Wonga.Experian\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.ExperianBulk")) return string.Format(@"{1}\components\Wonga.ExperianBulk\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.FileStorage")) return string.Format(@"{1}\components\Wonga.FileStorage\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.HPI")) return string.Format(@"{1}\components\Wonga.HPI\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.Iovation")) return string.Format(@"{1}\components\Wonga.Iovation\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.Salesforce")) return string.Format(@"{1}\components\Wonga.Salesforce\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.Scheduler")) return string.Format(@"{1}\components\Wonga.Scheduler\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.Sms")) return string.Format(@"{1}\components\Wonga.Sms\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.TimeZone")) return string.Format(@"{1}\components\Wonga.TimeZone\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.Transunion")) return string.Format(@"{1}\components\Wonga.Transunion\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.URU")) return string.Format(@"{1}\components\Wonga.URU\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.WongaPay")) return string.Format(@"{1}\components\Wonga.WongaPay\{0}", fileName, baseBuildPath);
            
            return "";
        }
    }
}