$gitrepo="https://github.com/CentriqTraining/QueueCaseStudy1.git"
$usr="QueueAdmin"
$pwd="Pa55w.rd!"
$random = Get-Random -Minimum 1000 -Maximum 1200
$app="mywebapp" + $random
$rg="CentriqAzureDemo"
$sb="CentriqSBQueueDemo"
$queue="timeentry"
$svr="QueueCaseStudy"
$vault="Queue" + $random
$db="QueueCaseStudy.V1"

# Promp for user login
az login 

# Create a resource group called CentriqAzureDemo
#   We'll use this only for our demo so we can easily clean up
az group create -n $rg -l eastus

#  Create the Service Buss Namespace
az servicebus namespace create -n $sb -g $rg -l eastus

#  Create the Queue
Az servicebus queue create -n $queue --namespace-name $sb -g $rg

#  Create a Access Policy for the Listener Service
az servicebus queue authorization-rule create --queue-name $queue --resource-group $rg --namespace-name $sb --name ListenerAccessKey --rights Listen
 
#  Create a Access Policy for the Sender Service
az servicebus queue authorization-rule create --queue-name $queue --resource-group $rg --namespace-name $sb --name SenderAccessKey --rights Send


# Now create the database that we will be using for this demo
az sql server create -g $rg -n $svr -u $usr -p $pwd
az sql db create -n $db -s $svr -g $rg

# Scale the new copy down to Basic edition 5 DTU's to reduce cost
#  Only if needed 
$state = (az sql db list --server QueueCaseStudy --resource-group CentriqAzureDemo --query "[1].sku.tier")
$basic = ($state -notmatch "Basic")
if ($basic)
{
    az sql db update -g $rg -s $svr -n $db --edition Basic -c 5 --max-size 500MB
}

#  Now create the website - deploy from github
# Create an App Service plan in `FREE` tier.
az appservice plan create --name $app --resource-group $rg --sku FREE

# Create a web app.
az webapp create --name $app --resource-group $rg --plan $app

# Deploy code from a public GitHub repository. 
az webapp deployment source config --name $app --resource-group $rg --repo-url $gitrepo --branch master --manual-integration

# Copy the result of the following command into a browser to see the web app.
echo http://$app.azurewebsites.net

# Now set firewall rules so that the user can access it from his machine and the web server
#  Can also add it.
$MyIP = (Invoke-WebRequest -uri "http://ifconfig.me/ip").Content
az sql server firewall-rule create --server $svr --resource-group $rg --name ClientIP --start-ip-address $MyIP --end-ip-address $MyIP

# Query the Keys for this new Service Bus
$ListenerKey = (az servicebus queue authorization-rule keys list --namespace-name $sb -n ListenerAccessKey --queue-name $queue -g $rg --query "primaryConnectionString")
$SenderKey = (az servicebus queue authorization-rule keys list --namespace-name $sb -n SenderAccessKey --queue-name $queue -g $rg --query "primaryConnectionString")

# Get Connection String information
$ConnectionString = (az sql db show-connection-string -c ado.net -n QueueCaseStudy.V1 --server QueueCaseStudy --auth-type SqlPassword)
$ConnectionString = $ConnectionString.Replace("<username>", $usr).Replace("<password>", $pwd)

# Create the key vault
az keyvault create -n $vault -g $rg

#  Set the appropriate Secrets
az keyvault secret set --vault-name $vault -n DBConnectionString --value $ConnectionString
az keyvault secret set --vault-name $vault -n ListenerKey --value $ListenerKey
az keyvault secret set --vault-name $vault -n SenderKey --value $SenderKey
az keyvault secret set --vault-name $vault -n QueueName --value $queue

# Grab the pertinent configuration requirements
$cred = (az ad sp create-for-rbac -n QueueCaseStudyApp --skip-assignment | ConvertFrom-Json)
$vaultClient = $cred.appId
$vaultTenant = $cred.tenant
$vaultSecret = $cred.password
$VaultUri = (az keyvault show -n $vault --query "properties.vaultUri")
az keyvault set-policy --name $vault --spn $vaultClient --key-permissions backup delete get list create encrypt decrypt update
az keyvault set-policy --name $vault --spn $vaultClient --secret-permissions backup delete get list purge recover restore set

New-Item Settings.txt
Set-Content Settings.txt "************************************************************"
Add-Content Settings.txt "*  Whichever project you are working with                  "
Add-Content Settings.txt "*  (QueueCaseStudy1, QueueCaseStudy2, QueueCaseStudy3)     "
Add-Content Settings.txt "*  "
Add-Content Settings.txt "*  Open either the App.Config file or the web.config file  "
Add-Content Settings.txt "*  in the project and make sure to set the following       "
Add-Content Settings.txt "*  settings to the values shown below:                     "
Add-Content Settings.txt "*  "
Add-Content Settings.txt "*     clientId: $vaultClient"
Add-Content Settings.txt "*     clientSecret: $vaultSecret"
Add-Content Settings.txt "*     tenant: $vaultTenant"
Add-Content Settings.txt "*     baseUrl: $VaultUri"
Add-Content Settings.txt "*  "
Add-Content Settings.txt "************************************************************"
Add-Content Settings.txt "*  Without these settings, the application will not run    "
Add-Content Settings.txt "*  in any environment since it cannot even access the      "
Add-Content Settings.txt "*  database without it                                     "
Add-Content Settings.txt "************************************************************"
Add-Content Settings.txt "*  Cleanup.ps1 will delete this file and all resources     "
Add-Content Settings.txt "************************************************************"

Invoke-Item Settings.txt

Write-Host "==============================================================="
Write-Host "Ready to use your Azure Service Bus Account..."