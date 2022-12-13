# .NET-MAUI-Fantasy-Football-App

Extensive .NET MAUI fantasy football application made with love and dedication. 
It started with intensive research into the beautiful, robust, and modern .NET MAUI framework. 
It continued with many hours of debugging and figuring out compatibility issues, that arise as a result of MAUI's native cross-platform compatibility, with
other .NET frameworks such as Entity Framework. I decided I wanted to be able to distribute this application to any user around the world and thus needed a way
to facilitate users accessing centralized data. Hence why I decided to create an API; I also needed a way to authenticate users and grant database access to
information based on user-provided credentials, and so took a passionate interest in OAuth and authentication. While I did not end up implementing OAuth in the 
sense of permitting users to login using Microsoft, Facebook, Google, etc. my custom authentication protocol works in the same way oauth does. Might add traditional
OAuth in the colloquial sense in the not too distant future. 

Technologies features used/implemented: 
-.NET MAUI w/Animations and countdown timer
-Entity Framework
-SQL Express/SQL Server
-.NET Web API custom authentication/Rresource controller
-Fully functional password generator
-Complex network of interdependent buttons, functions, and API requests pieced together to form cohesively functioning application
-MailHog
-Postman
-Forgot password capability with password reset
-User registration
-User login
-User create, update, delete leagues, create, update, delete teams, add drop players
-All redeemable actions sent to API controller/database with data returned in real time and updated in the client view in real time

More to come... :)

