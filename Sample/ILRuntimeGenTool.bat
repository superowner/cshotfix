echo //        created by lichunlin
echo //        qq:576067421
echo //        git:https://github.com/lichunlincn/cshotfix


rd /s /Q "%cd%\ILRuntimeGen\src"
md "%cd%\ILRuntimeGen\src"
ILRuntimeGenTool\bin\Debug\ILRuntimeGenTool.exe "%cd%\HotFixDll\bin\Debug\HotFixDll.dll" "%cd%\ILRuntimeGen\src"
pause