configuration IISWebsiteWithWebDeploy
{
    node ("localhost")
    {
        WindowsFeature WebServer
        {
           Ensure = "Present"
           Name   = "Web-Server"
           IncludeAllSubFeature = $true
        }

        File FriendlyName
        {
            Ensure          = "Present"
            Contents        = "Hi from DSC!"
            DestinationPath = "c:\inetpub\wwwroot\default.html"
            DependsOn       = "[WindowsFeature]WebServer"
        }       

        Script DownloadWebDeploy
        {
            TestScript = {
                Test-Path "C:\WindowsAzure\WebDeploy_amd64_en-US.msi"
            }
            SetScript ={
                $source = "http://download.microsoft.com/download/0/1/D/01DC28EA-638C-4A22-A57B-4CEF97755C6C/WebDeploy_amd64_en-US.msi"
                $dest = "C:\WindowsAzure\WebDeploy_amd64_en-US.msi"
                Invoke-WebRequest $source -OutFile $dest
            }
            GetScript = { @{Result = "WebDeployDownload"} }
            DependsOn = "[WindowsFeature]WebServer"
        }

        Package InstallWebDeploy
        {
            Ensure = "Present"  
            Path  = "C:\WindowsAzure\WebDeploy_amd64_en-US.msi"
            Name = "Microsoft Web Deploy 3.6"
            ProductId = "{6773A61D-755B-4F74-95CC-97920E45E696}"
            Arguments = "/quiet ADDLOCAL=ALL"
            DependsOn = "[Script]DownloadWebDeploy"
        }

        Service StartWebDeploy
        {
            Name = "WMSVC"
            StartupType = "Automatic"
            State = "Running"
            DependsOn = "[Package]InstallWebDeploy"
        }
    }
}