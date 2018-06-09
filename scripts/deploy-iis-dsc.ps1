$resourceGroup = "winserver"
$vmName = "winserver"
$storageName = "la70532dsc"

Publish-AzureRmVMDscConfiguration -ConfigurationPath .\configure-iis-dsc.ps1 `
    -ResourceGroupName $resourceGroup -StorageAccountName $storageName -force

Set-AzureRmVmDscExtension -Version 2.21 `
    -ResourceGroupName $resourceGroup `
    -VMName $vmName `
    -ArchiveStorageAccountName $storageName `
    -ArchiveBlobName configure-iis-dsc.ps1.zip `
    -AutoUpdate:$true `
    -ConfigurationName IISWebsite