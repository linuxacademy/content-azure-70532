powershell -Command "Add-WindowsFeature Web-Server"
powershell -Command "Set-Content c:\inetpub\wwwroot\default.html 'Hi from CSE!'"