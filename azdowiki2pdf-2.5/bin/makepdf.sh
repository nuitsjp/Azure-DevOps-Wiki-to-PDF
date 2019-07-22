#!/bin/bash
src=`pwd`
mkdir /var/tmp/azdowiki2pdf
cp -a -r . /var/tmp/azdowiki2pdf/
cd /var/tmp/azdowiki2pdf/
cp images/* $1/images
AzureDevOpsWikiToPdf . $1
cd $1
md2reviews.sh
rake pdf

cp *.pdf $src
