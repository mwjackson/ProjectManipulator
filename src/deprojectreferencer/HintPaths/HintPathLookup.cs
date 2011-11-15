using System.IO;

namespace deprojectreferencer.HintPaths
{
    public class HintPathLookup
    {
        public string For(string oldPath)
        {
            var fileName = new FileInfo(oldPath).Name;
            if (oldPath.Contains("Wonga.Common")) return string.Format(@"..\..\..\build\common\{0}", fileName);
            if (oldPath.Contains("Wonga.Api")) return string.Format(@"..\..\..\build\common\{0}", fileName);
            if (oldPath.Contains("Wonga.Integration")) return string.Format(@"..\..\..\build\integration\{0}", fileName);
            if (oldPath.Contains("Wonga.Ops")) return string.Format(@"..\..\..\build\ops\{0}", fileName);
            if (oldPath.Contains("Wonga.Payments")) return string.Format(@"..\..\..\build\payments\{0}", fileName);
            if (oldPath.Contains("Wonga.Risk")) return string.Format(@"..\..\..\build\risk\{0}", fileName);
            if (oldPath.Contains("Wonga.")) return string.Format(@"..\..\..\build\components\{0}", fileName);
            return "";
        }
    }
}