# .NET-MAUI-Fantasy-Football-App

Extensive .NET MAUI fantasy football application made with love and dedication. 

It started with intensive research into the beautiful, robust, and modern .NET MAUI framework. 

It continued with many hours of debugging and figuring out compatibility issues, that arise as a result of MAUI's native cross-platform compatibility, with
other .NET frameworks such as Entity Framework. 

I decided I wanted to be able to distribute this application to any user around the world and thus needed a way
to facilitate users accessing centralized data. Hence why I decided to create an API; I also needed a way to authenticate users and grant database access to
information based on user-provided credentials, and so took a passionate interest in OAuth and authentication. 

While I did not end up implementing OAuth in the sense of permitting users to login with Microsoft, Facebook, Google, etc. my custom authentication protocol works in the same way OAuth does. Might add traditional OAuth in the colloquial sense in the not too distant future. 

Technologies features used/implemented: 

-.NET MAUI w/Animations and countdown timer

-Entity Framework

-SQL Express/SQL Server with many one-to-one, one-to-many, and many-to-many relationships.

-.NET Web API custom authentication/Resource controller

-Modifiable password generator

-Complex network of interdependent buttons, functions, and API requests which cohesively form seamlessly functioning application

-MailHog

-Postman

-Forgot password capability with password reset email using Gmail SMTP

-User registration

-User login

-User create, update, delete leagues, create, update, delete teams, add drop players

-All redeemable actions sent to API controller/database with data returned in real time and updated in the client view in real time

More to come... :)

12/14 added players to my app (not app users, but football players). I didn't reference any player or team namesakes of actual players/teams since the NFL has strict licensing rights and when I reached out to them, they informed me that each player has to individually approve of their namesake being used. 

To circumvent this, I randomly generate a list of 1248 names from a list of 1500+ first names and 120+ last names, which I uniquely assign to 238 QBs, 291 RBs, 436 WRs, 184 TEs, 99 Ks, plus 32 team defenses. 

Since we don't want my API controller to generate a NEW list of players each time a new league is created, my API checks the db for existing players. If none exist, my algorithm deploys to generate 1280 total players, and otherwise, my API copies the list of 1280 existing players (per league), removes the league ID # formerly associated with each of those players, and assigns to each copied player the league ID # of the newly created league. This way users of my app can select from a consistent, non-varying pool of players regardless of which fantasy league they play from. Fun logic to implement. :)

12/17 added SEND EMAIL capability to send password reset emails using a reset token when a user selects "Forgot My Password". Also added "Join League" functionality to my front end which already existed as an endpoint capability in my API, but is now integrated with the front end.


12/21 Got backend API and database fully up and running in Azure cloud! :) Submitted front end client to Microsoft App store and thereafter added logout capability as well.

12/27 After a marathon of online research including posting/devouring Stackoverflow posts, hours of YouTube videos, and reaching out to everybody I know, I erroneously submitted a .msix installer for my app to the app store- but soon discovered that the msix file only correctly ran/installed my app when launched and stored in the same directory as all of my app's 900+ dependency files. Upon bundling these into a .msixupload package, I was denied because the app store does not permit more than 16 files per upload. An alternative to this solution was to package my app into a single .exe, which I was successfully able to do. However, I encountered the same issue where the app could only be launched among/from the same directory as all of its dependencies. I then determined upon further research that I would need to embed my dependency files into the singular .exe for a larger overall file, but one that would not need be present among hundreds of other files to execute. I achieved success by downloading and tinkering with a wonderful app called BoxedApp Packer. This enabled me to submit to the app store a second time. However since I submitted a directly downloadable .exe file the second time around rather than an .msix package, I had to link to a CDN and provide a list of exe return codes which are listed below :).


STANDARD INSTALL SCENARIOS AND RETURN VALUES FOR INSTALLING MY APP FROM THE APP STORE:
-Installation cancelled by user: The install operation was cancelled by the user. Return Code 0
-Application already exists: The applicaton already exists on the device. Return Code 1
-Installation already in progress: Another installation is already in progress. User needs to complete the installation before proceeding with this install. Return Code 2
-Disk space is full: The disk space is full. Return Code 3
-Reboot required: A restart is required to complete the install. Return Code 4
-Network failure: Provide custom return code values for various network related failures. Return Code 5
-Package rejected during installation: Package rejected during installation due to a security policy enabled on the device. Return Code 6
-Installation successful: Installation has been successful. Return Code 7

PRIVACY POLICY
https://app.termly.io/document/privacy-policy/1072374e-2247-45d2-9899-7f8a4cca91e5
