$resourceGroup = "winserver"
$vmName = "winserver"
$location = "westus"

$Settings = @{"commandToExecute" = "powershell -ExecutionPolicy Unrestricted Set-Content c:\helloworld.txt 'HI!'"};

Set-AzureRmVMExtension -ExtensionName "CustomScriptExtension" `
  -ResourceGroupName $resourceGroup -VMName $vmName -Location $location `
  -Publisher "Microsoft.Compute" -ExtensionType "CustomScriptExtension" `
  -TypeHandlerVersion 1.4 `
  -Settings $Settings
  