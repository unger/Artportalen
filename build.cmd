@echo Off

set solutionName=Artportalen.sln

set config=%1
if "%config%" == "" (
	set config=Release
)

set buildDir=Build\%config%


nuget restore
msbuild %solutionName% /t:Rebuild /p:Configuration=%config%;OutDir="%cd%\%buildDir%";UseWPP_CopyWebApplication=True;PipelineDependsOnBuild=False

