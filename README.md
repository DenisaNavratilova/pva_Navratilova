Water Level Monitoring System (WLMS)

Overview
Welcome to the Water Level Monitoring System (WLMS) project! This application is designed to help manage and monitor water levels at various river stations. It allows users to visualize water level data, receive warnings when certain thresholds are exceeded, and configure new river stations. This project serves as a pilot application, aimed at demonstrating core functionalities and quality to potential customers.

Settings before using this app:
1. appsettings.json file in "pva" project
- "DefaultConnection": "Server=your_database"
- "Email": Configure e-mail notifications
    - "SmtpHost": "SMTP_SERVER_ADDRESS",
    - "SmtpPort": "SMTP_PORT",
    - "SmtpUser": "YOUR_SMTP_USERNAME",
    - "SmtpPassword": "YOUR_SMTP_PASSWORD",
    - "SmtpEnableSsl": "true_or_false",
    - "FromAddress": "no-reply@yourdomain.com",
    - "ToAddress": "recipient@domain.com"
2. Program.cs file in "pva" project
- "builder.WebHost.UseUrls("http://your_ip:port")"
3. Properties folder - launchSettings.json file
- "profiles": "http": "applicationUrl": "http://your_ip:port"
4. appsettings.json file in project "vosplzen.sem2.2023.apiClient"
- "BaseUri": "http://your_ip:port/api/values"

Features

Core Features
1.	Data Display:
-	View water level data from multiple river stations.
-	Monitor the latest updates on water levels and station status.
2.	Data Input and Validation:
-	Accept water level data in the appropriate format.
-	Validate incoming data to ensure it meets the required standards.
3.	Station Configuration:
-	Interface to configure and add new river stations.
-	Define key parameters such as flood level and drought level.


Additional Features
1.	Timeout Configuration:
-	Set and manage the timeout duration for station data. If a station fails to report within the timeout period, an alert is triggered.
2.	Email Notifications:
-	Automatically send email notifications if water levels exceed predefined thresholds for flood or drought conditions.
-	Configurable recipient email addresses for warning notifications.
3.	Historical Data Filtering:
-	Filter and view historical water level records by station.
4.	User Authentication and API Security:
-	Implement authentication for users accessing the application.
-	Secure API endpoints using tokens or other authentication mechanisms.
5.	Data Visualization:
-	Display water level data in graphical formats (e.g., charts) for better understanding and analysis.


"# pva_Navratilova" 
