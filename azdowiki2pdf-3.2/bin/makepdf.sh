#!/bin/bash
src=`pwd`
mkdir /var/tmp/azdowiki2pdf
cp -a -r . /var/tmp/azdowiki2pdf/
cd /var/tmp/azdowiki2pdf/
mkdir -p ReView/images
cp images/* $1/images
AzureDevOpsWikiToPdf . $1
cd $1

for file in `\find . -name '*.md'`; do
    echo "md2review "$file" > "${file%%.md}".re"
    md2review $file > ${file%%.md}".re"
done

rake pdf

cp *.pdf $src