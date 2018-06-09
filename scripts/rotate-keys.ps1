Login-AzureRmAccount

$resourceGroupName = "security"
$storageAccountName = "la70532kvdiag"
$vaultName = "la70532kv"
$secretName = "storageAccountKey1"

New-AzureRmStorageAccountKey -ResourceGroupName $resourceGroupName `
  -Name $storageAccountName `
  -KeyName key1 

$newKey = (Get-AzureRmStorageAccountKey -Name $storageAccountName -ResourceGroupName $resourceGroupName).Value[0]

$secretValue = ConvertTo-SecureString $newKey -AsPlainText -Force

Set-AzureKeyVaultSecret -VaultName $vaultName -Name $secretName -SecretValue $secretValue

