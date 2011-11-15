using System.IO;

namespace deprojectreferencer.HintPaths
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
            if (oldPath.Contains("Wonga.Common")) return string.Format(@"{1}\common\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.Api")) return string.Format(@"{1}\common\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.Integration")) return string.Format(@"{1}\integration\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Messages")) return string.Format(@"{1}\messages\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Commands")) return string.Format(@"{1}\messages\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.Ops")) return string.Format(@"{1}\ops\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.Comms")) return string.Format(@"{1}\comms\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.Payments")) return string.Format(@"{1}\payments\{0}", fileName, baseBuildPath);
            if (oldPath.Contains("Wonga.Risk")) return string.Format(@"{1}\risk\{0}", fileName, baseBuildPath);
            
            if (oldPath.Contains("Wonga.")) return string.Format(@"{1}\components\{0}", fileName, baseBuildPath);
            
            return "";
        }
    }
}