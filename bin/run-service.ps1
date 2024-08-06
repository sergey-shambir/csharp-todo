$contentRootDir = Join-Path (Split-Path -parent "$PSScriptRoot") frontend

$Env:ASPNETCORE_ENVIRONMENT="Development"
$Env:APP_CONTENT_ROOT_PATH="$contentRootDir"

dotnet run --project src/Todo.Api/ --launch-profile https
