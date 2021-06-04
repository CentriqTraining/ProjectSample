$rg="CentriqAzureDemo"

az login
az group delete -g $rg --yes

if (Test-Path Settings.txt)
{
	Remove-Item Settings.txt
}
