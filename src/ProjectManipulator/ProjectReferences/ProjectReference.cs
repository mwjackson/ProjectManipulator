using System.Text;

namespace ProjectManipulator.ProjectReferences
{
    public class ProjectReference
    {
        public string Name { get; private set; }
        public string IncludePath { get; private set; }

        public ProjectReference(string name, string includePath)
        {
            Name = name;
            IncludePath = includePath;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0} : {1}", Name, IncludePath);
            return sb.ToString();
        }
    }
}