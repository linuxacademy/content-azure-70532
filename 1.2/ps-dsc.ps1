$resourceGroupName = "rg-winvm-extensions"
$location = "westus"
$vmName = "winvm2"
$storageAccountName = "rgwinvmextensionsdiag428"

Login-AzureRmAccount

Publish-AzureRmVMDscConfiguration -ConfigurationPath .\WindowsWebServer.ps1 `
    -ResourceGroupName $resourceGroupName `
    -StorageAccountName $storageAccountName

Set-AzureRmVMDscExtension -Version 2.21 `
    -ResourceGroupName $resourceGroupName `
    -VMName $vmName `
    -ArchiveStorageAccountName $storageAccountName `
    -ArchiveBlobName WindowsWebServer.ps1.zip `
    -AutoUpdate:$true `
    -ConfigurationName IIS

Publish-AzureRmVMDscConfiguration -ConfigurationPath .\WindowsWebServer.ps1 -OutputArchivePath ".\WindowsWebServer.ps1.zip"
