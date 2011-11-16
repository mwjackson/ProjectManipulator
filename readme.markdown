Can manipulate project files to in an effort to bulk update an entire directory structure. For example, turn all project references from this:

	<ProjectReference Include="..\somepath\someproject.csproj">
		<Project>{2457953E-5E0E-4B2A-830F-D1FA5F57EE9B}</Project>
		<Name>someproject.name</Name>
	</ProjectReference>

to assembly (weak) reference:

    <Reference Include="assembly.name">
      <HintPath>..\path\to\assembly.dll</HintPath>
    </Reference>

Usage
----------------------

	ProjectManipulator.exe switch filepath

Switch:
/p - Replace all project references (<ProjectReference>) with assembly/weak (<Reference>) references
/cl - Set CopyLocal (<private>) to false for all references
/hp - Update reference hint paths (<HintPath>) to match some desired structure

Filepath:
The path of the csproj file to update.

Eg.
	ProjectManipulator.exe /cl myProject.csproj

Bulk Usage
----------------------

To replace all csproj files in a directory tree:

	find src/ -name *.csproj -exec ProjectManipulator.exe /cl {} \;
	
Disclaimer!
----------------------

This is certainly not production ready! Use at your own risk!