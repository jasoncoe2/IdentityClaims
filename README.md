# IdentityClaims

After researching Identity Claims, I put together this example as to how I planned to use it. At the time of this article, there     wasn't a lot of documentation on how to use Claims. Hence this example. I tried to show three important things in this example.

First, I wanted to show a real-world example or something close that implements claims. I wanted the example to use a level of granularity below roles. Thus, this example shows how to Manage (Add or Edit), Delete, or View data with seperate claims simulating more granular access

Second, I wanted to show an implementation of claims in a master table. Unlike Roles, Claims does not have a table for master data. The claim is simply linked in the UserClaims or RoleClaims table. I have created a Lookup and LookupValues tables that I have used in other applications for Drop Down Lists. In the Claims example, Attribute1 is the ClaimValue and Attribute3 is the ClaimType. I just the RoleName as the ClaimType. In the LookupValues table, I populated Attribute2 the same as Attribute1. This is in case I need a Drop Down List in the future. However, it is not used in this example. Role Claims are not used in this example.

Third, I want to implement a solution where User Claims are added or removed when the role is added or removed. In my implementation, when a role is granted, all claims from the master are either granted or removed. The Application Administrator has the option to user claims to control acccess. The User, Master Data and Admin menus are controlled by Role Authorization. The User and Master Data pages are controlled by User Claims. The Admin section is controlled by the Admin Role.
