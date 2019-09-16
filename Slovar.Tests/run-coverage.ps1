dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:Exclude="[*]Slovar.Migrations.*%2c[*]Slovar.Seeders.*"
Remove-Item "./Report/*"
dotnet reportgenerator -reports:coverage.cobertura.xml -sourcedirs:"../Slovar" -targetdir:"./Report"