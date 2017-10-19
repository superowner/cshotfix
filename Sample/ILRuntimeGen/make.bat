echo "警告，你可能需要确认是否合理设置编译器，路径形如C:\Program Files (x86)\MSBuild\14.0\Bin"
cd %~dp0
"%MsBuildDir%/MsBuild" ILRuntimeGen.sln /t:rebuild /p:configuration=Debug
pause