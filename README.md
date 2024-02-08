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

NOTE: I am no longer paying to maintain the Azure channel on which my SQL database was hosted and thus can no longer utilize app features such as create-account or login, which are required to access the fantasy-football portion of the app.

Screenshots of the application (video demonstration in LinkedIn):
![2024-02-07 19_31_52-](https://github.com/danwiener/.NET-MAUI-Fantasy-Football-App/assets/111164839/05264d79-97ba-4f55-a358-84fdbdb66dc7)
![2024-02-07 19_33_54-](https://github.com/danwiener/.NET-MAUI-Fantasy-Football-App/assets/111164839/10d6702e-4863-496d-8488-b96362cad706)
![2024-02-07 19_32_58-](https://github.com/danwiener/.NET-MAUI-Fantasy-Football-App/assets/111164839/978d8866-52c2-4f4b-bd19-69a6eb92d330)
![2024-02-07 19_33_42-](https://github.com/danwiener/.NET-MAUI-Fantasy-Football-App/assets/111164839/33d374e3-3fcb-48e4-98d0-7d2550808084)
![2024-02-07 19_33_31-](https://github.com/danwiener/.NET-MAUI-Fantasy-Football-App/assets/111164839/643ccfa8-67cf-4dc1-9684-25df0748c45f)
![2024-02-07 19_34_01-](https://github.com/danwiener/.NET-MAUI-Fantasy-Football-App/assets/111164839/a080a7a2-79a3-4349-8db5-7c27b89699a2)




