# Azure DevOps Wiki to PDF

[Sample Wiki](https://nuits.visualstudio.com/Azure-DevOps-Wiki-to-PDF-Sample-Site/_wiki/wikis)

# Commands

dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true

docker build -t nuitsjp/ado-wiki2pdf:local .

docker run -i -t --rm -v %cd%:/work nuitsjp/ado-wiki2pdf:local /bin/bash