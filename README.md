# Azure DevOps Wiki to PDF

[Sample Wiki](https://nuits.visualstudio.com/Azure-DevOps-Wiki-to-PDF-Sample-Site/_wiki/wikis)

# Memo

dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true

docker build -t nuitsjp/azdowiki2pdf:local .

docker run -i -t --rm -v %cd%:/work nuitsjp/azdowiki2pdf:local /bin/bash

docker run --rm -v %cd%:/work nuitsjp/azdowiki2pdf:local "cd /work && makepdf.sh ReView"