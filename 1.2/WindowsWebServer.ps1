configuration IIS
{
    node ("localhost")
    {
        WindowsFeature WebServer
        {
           Ensure = "Present"
           Name   = "Web-Server"
           IncludeAllSubFeature = $true
        }      
    }
}