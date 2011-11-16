Usage:

ProjectManipulator.exe switch filepath

Switch:
/p - Replace all project references (<ProjectReference>) with assembly/weak (<Reference>) references
/cl - Set CopyLocal (<private>) to false for all references
/hp - Update reference hint paths (<HintPath>) to match output structure

Filepath:
The path of the csproj file to update.

Eg.
ProjectManipulator.exe /cl myProject.csproj