Set-Location .\azdowiki2pdf-2.5
docker build -t nuitsjp/azdowiki2pdf:2.5 .
Set-Location ..
Set-Location .\azdowiki2pdf-3.2
docker build -t nuitsjp/azdowiki2pdf:3.2 .
docker build -t nuitsjp/azdowiki2pdf:latest .
Set-Location ..