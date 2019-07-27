# Azure DevOps Wiki to PDF

[Sample Wiki](https://nuits.visualstudio.com/Azure-DevOps-Wiki-to-PDF-Sample-Site/_wiki/wikis)

# Memo

dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true

docker build -t nuitsjp/azdowiki2pdf:local .

docker run -i -t --rm -v %cd%:/work nuitsjp/azdowiki2pdf:local /bin/bash

docker run --rm -v %cd%:/work nuitsjp/azdowiki2pdf:local "cd /work && makepdf.sh ReView"


docker run -i -t --rm -v ${pwd}:/work nuitsjp/azdowiki2pdf:local /bin/bash

cd work
src=`pwd`
mkdir /var/tmp/azdowiki2pdf
cp -a -r . /var/tmp/azdowiki2pdf/
cd /var/tmp/azdowiki2pdf/
mkdir ReView/images
cp images/* ReView/images
AzureDevOpsWikiToPdf . ReView
cd ReView

for file in `\find . -name '*.md'`; do
    echo "md2review "$file" > "${file%%.md}".re"
    md2review $file > ${file%%.md}".re"
done

rake pdf

cp *.pdf $src

exit