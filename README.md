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


PRIVACY POLICY
https://app.termly.io/document/privacy-policy/1072374e-2247-45d2-9899-7f8a4cca91e5
