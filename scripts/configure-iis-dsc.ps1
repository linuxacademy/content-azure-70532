configuration IISWebsite
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
            DestinationPath = "c:\inetpub\wwwroot\default2.html"
            DependsOn       = "[WindowsFeature]WebServer"
        }       
    }
}