{
    "$schema": "https://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#",
    "contentVersion": "1.0.0.0",
    "parameters": {
        "vm_name": {
            "defaultValue": "winserver1",
            "type": "string",
            "metadata": {
                "description": "Azure name of the server"
            }
        },
        "admin_username": {
            "defaultValue": "la70532",
            "type": "string",
            "metadata": {
                "description": "Administrator user name for the VM"
            }
        },
        "admin_password": {
            "defaultValue": "La!70532!2018",
            "type": "string",
            "metadata": {
                "description": "Administrator password for the VM"
            }
        },
        "vm_size": {
            "defaultValue": "Standard_D2s_v3",
            "type": "string",
            "metadata": {
                "description": "Size of the server"
            }
        },
        "vnet_name": {
            "defaultValue": "",
            "type": "string",
            "metadata": {
                "description": "Name of the vnet for the VM placement"
            }
        },
        "vnet_CIDR": {
            "defaultValue": "10.0.0.0/16",
            "type": "string",
            "metadata": {
                "description": "CIDR block for the vnet"
            }
        },
        "subnet_name": {
            "defaultValue": "default",
            "type": "string",
            "metadata": {
                "description": "Name of the subnet for the VM placement"
            }
        },
        "subnet_CIDR": {
            "defaultValue": "10.0.1.0/24",
            "type": "string",
            "metadata": {
                "description": "CIDR block of the subnet"
            }
        }
    },
    "variables": {
        "vmName": "[parameters('vm_name')]",
        "nicName": "[concat('nic-', variables('vmName'))]",
        "pipName": "[concat('pip-', variables('vmName'))]",
        "nsgName": "[concat('nsg-', variables('vmName'))]",
        "vmSize": "[parameters('vm_size')]",
        "vmDiskName": "[concat('pip-', variables('vmName'))]",
        "adminUsername": "[parameters('admin_username')]",
        "adminPassword": "[parameters('admin_password')]",
        "vnetName": "[if(empty(parameters('vnet_name')), concat('vnet-', variables('vmName')), parameters('vnet_name'))]",
        "vnetCIDR": "[parameters('vnet_CIDR')]",
        "subnetName": "[parameters('subnet_name')]",
        "subnetCIDR": "[parameters('subnet_CIDR')]",
        "dnsLabel": "[concat('la70532-', variables('vmName'))]"
    },
    "resources": [
        {
            "type": "Microsoft.Network/virtualNetworks",
            "name": "[variables('vnetName')]",
            "apiVersion": "2018-01-01",
            "location": "[resourceGroup().location]",
            "properties": {
                "addressSpace": {
                    "addressPrefixes": [
                        "[variables('vnetCIDR')]" 
                    ]
                },
                "subnets": [
                    {
                        "name": "[variables('subnetName')]",
                        "properties":{
                            "addressPrefix": "[variables('subnetCIDR')]"
                        }
                    }
                ]
            },
            "dependsOn": []
        },
        {
            "type": "Microsoft.Network/publicIPAddresses",
            "name": "[variables('pipName')]",
            "apiVersion": "2018-01-01",
            "location": "[resourceGroup().location]",
            "properties": {
                "publicIPAddressVersion": "IPv4",
                "publicIPAllocationMethod": "Dynamic",
                "idleTimeoutInMinutes": 4,
                "dnsSettings": {
                    "domainNameLabel": "[variables('dnsLabel')]"
                }            
            },
            "dependsOn": []
        },
        {
            "type": "Microsoft.Network/virtualNetworks/subnets",
            "name": "[concat(variables('vnetName'), '/', variables('subnetName'))]",
            "apiVersion": "2018-01-01",
            "properties": {
                "addressPrefix": "[variables('subnetCIDR')]"
            },
            "dependsOn": [
                "[resourceId('Microsoft.Network/virtualNetworks', variables('vnetName'))]"
            ]
        },
        {
            "type": "Microsoft.Network/networkInterfaces",
            "name": "[variables('nicName')]",
            "apiVersion": "2018-01-01",
            "location": "[resourceGroup().location]",
            "properties": {
                "ipConfigurations": [
                    {
                        "name": "ipconfig1",
                        "properties": {
                            "privateIPAllocationMethod": "Dynamic",
                            "publicIPAddress": {
                                "id": "[resourceId('Microsoft.Network/publicIPAddresses', variables('pipName'))]"
                            },
                            "subnet": { "id": "[resourceId('Microsoft.Network/virtualNetworks/subnets', variables('vnetName'), variables('subnetName'))]" }
                        }
                    }
                ],
                "networkSecurityGroup": {
                    "id": "[resourceId('Microsoft.Network/networkSecurityGroups', variables('nsgName'))]"
                }
            },
            "dependsOn": [
                "[resourceId('Microsoft.Network/publicIPAddresses', variables('pipName'))]",
                "[resourceId('Microsoft.Network/virtualNetworks/subnets', variables('vnetName'), variables('subnetName'))]",
                "[resourceId('Microsoft.Network/networkSecurityGroups', variables('nsgName'))]"a
            ]
        },                                                                                                                                                                                     
        {
            "type": "Microsoft.Network/networkSecurityGroups",
            "name": "[variables('nsgName')]",
            "apiVersion": "2018-01-01",
            "location": "[resourceGroup().location]",
            "properties": {
                "securityRules": [
                    {
                        "name": "allow-rdp",
                        "properties": {
                            "protocol": "TCP",
                            "sourcePortRange": "*",
                            "destinationPortRange": "3389",
                            "sourceAddressPrefix": "*",
                            "destinationAddressPrefix": "*",
                            "access": "Allow",
                            "priority": 1000,
                            "direction": "Inbound",
                            "sourcePortRanges": [],
                            "destinationPortRanges": [],
                            "sourceAddressPrefixes": [],
                            "destinationAddressPrefixes": []
                        }
                    }
                ],
                "defaultSecurityRules": [
                    {
                        "name": "AllowVnetInBound",
                        "properties": {
                            "description": "Allow inbound traffic from all VMs in VNET",
                            "protocol": "*",
                            "sourcePortRange": "*",
                            "destinationPortRange": "*",
                            "sourceAddressPrefix": "VirtualNetwork",
                            "destinationAddressPrefix": "VirtualNetwork",
                            "access": "Allow",
                            "priority": 65000,
                            "direction": "Inbound",
                            "sourcePortRanges": [],
                            "destinationPortRanges": [],
                            "sourceAddressPrefixes": [],
                            "destinationAddressPrefixes": []
                        }
                    },
                    {
                        "name": "AllowAzureLoadBalancerInBound",
                        "properties": {
                            "description": "Allow inbound traffic from azure load balancer",
                            "protocol": "*",
                            "sourcePortRange": "*",
                            "destinationPortRange": "*",
                            "sourceAddressPrefix": "AzureLoadBalancer",
                            "destinationAddressPrefix": "*",
                            "access": "Allow",
                            "priority": 65001,
                            "direction": "Inbound",
                            "sourcePortRanges": [],
                            "destinationPortRanges": [],
                            "sourceAddressPrefixes": [],
                            "destinationAddressPrefixes": []
                        }
                    },
                    {
                        "name": "DenyAllInBound",
                        "properties": {
                            "description": "Deny all inbound traffic",
                            "protocol": "*",
                            "sourcePortRange": "*",
                            "destinationPortRange": "*",
                            "sourceAddressPrefix": "*",
                            "destinationAddressPrefix": "*",
                            "access": "Deny",
                            "priority": 65500,
                            "direction": "Inbound",
                            "sourcePortRanges": [],
                            "destinationPortRanges": [],
                            "sourceAddressPrefixes": [],
                            "destinationAddressPrefixes": []
                        }
                    },
                    {
                        "name": "AllowVnetOutBound",
                        "properties": {
                            "description": "Allow outbound traffic from all VMs to all VMs in VNET",
                            "protocol": "*",
                            "sourcePortRange": "*",
                            "destinationPortRange": "*",
                            "sourceAddressPrefix": "VirtualNetwork",
                            "destinationAddressPrefix": "VirtualNetwork",
                            "access": "Allow",
                            "priority": 65000,
                            "direction": "Outbound",
                            "sourcePortRanges": [],
                            "destinationPortRanges": [],
                            "sourceAddressPrefixes": [],
                            "destinationAddressPrefixes": []
                        }
                    },
                    {
                        "name": "AllowInternetOutBound",
                        "properties": {
                            "description": "Allow outbound traffic from all VMs to Internet",
                            "protocol": "*",
                            "sourcePortRange": "*",
                            "destinationPortRange": "*",
                            "sourceAddressPrefix": "*",
                            "destinationAddressPrefix": "Internet",
                            "access": "Allow",
                            "priority": 65001,
                            "direction": "Outbound",
                            "sourcePortRanges": [],
                            "destinationPortRanges": [],
                            "sourceAddressPrefixes": [],
                            "destinationAddressPrefixes": []
                        }
                    },
                    {
                        "name": "DenyAllOutBound",
                        "properties": {
                            "description": "Deny all outbound traffic",
                            "protocol": "*",
                            "sourcePortRange": "*",
                            "destinationPortRange": "*",
                            "sourceAddressPrefix": "*",
                            "destinationAddressPrefix": "*",
                            "access": "Deny",
                            "priority": 65500,
                            "direction": "Outbound",
                            "sourcePortRanges": [],
                            "destinationPortRanges": [],
                            "sourceAddressPrefixes": [],
                            "destinationAddressPrefixes": []
                        }
                    }
                ]
            },
            "dependsOn": []
        },
        {
            "type": "Microsoft.Compute/virtualMachines",
            "name": "[variables('vmName')]",
            "apiVersion": "2017-03-30",
            "location": "[resourceGroup().location]",
            "properties": {
                "hardwareProfile": {
                    "vmSize": "[variables('vmSize')]"
                },
                "storageProfile": {
                    "imageReference": {
                        "publisher": "MicrosoftWindowsServer",
                        "offer": "WindowsServer",
                        "sku": "2016-Datacenter",
                        "version": "latest"
                    },
                    "osDisk": {
                        "osType": "Windows",
                        "name": "[variables('vmDiskName')]",
                        "createOption": "FromImage",
                        "caching": "ReadWrite",
                        "diskSizeGB": 127
                    },
                    "dataDisks": []
                },
                "osProfile": {
                    "computerName": "[variables('vmName')]",
                    "adminUsername": "[variables('adminUsername')]",
                    "adminPassword": "[variables('adminPassword')]",
                    "windowsConfiguration": {
                        "provisionVMAgent": true,
                        "enableAutomaticUpdates": true
                    }
                },
                "networkProfile": {
                    "networkInterfaces": [
                        {
                            "id": "[resourceId('Microsoft.Network/networkInterfaces', variables('nicName'))]"
                        }
                    ]
                }
            },
            "dependsOn": [
                "[resourceId('Microsoft.Network/networkInterfaces', variables('nicName'))]"
            ]
        },    
    ],
    "outputs": {
        "subnetCIDR":{
            "type": "string",
            "value": "[variables('subnetCIDR')]"
        },
        "vnetName":{
            "type": "string",
            "value": "[variables('vnetName')]"
        }
    }
}