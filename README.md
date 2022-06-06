# Author
Daniel Ferreira Lacerda (https://www.linkedin.com/in/daniel-lacerda-0753bb151/) .NET Fullstack Sênior Developer

# Description
This Rest API project was developed for Strider test using MVC arquitecture and non-relational database. 

# Personal Experience Report
It was my first practical experience with MongoDB, i've already studied the tech and the best situations to use her before but never practiced, i choosed her because when it cames to social media we're talking about Softwares that receives hundreds of millions of requests daily and non-relational databases are the best for this kind of project, so i gave it a shot. 

It was a really great experience, i really doesn't have much free time to study what i want because of work and college so i choosed to do the test and study something i wanted at the same time, as she is not my main stack of database(My main is Microsoft SQL Server) i took a little more time than i expected to complete the test, something about 9 hours.

I did 

# Stack
* .NET Core 6.0
* C#
* MongoDB
* Docker

# Applications for localhost test
### 1 - VS Code: https://code.visualstudio.com/download
* VS Code Extensions
Name: MongoDB for VS Code
Id: mongodb.mongodb-vscode
Description: Connect to MongoDB and Atlas directly from your VS Code environment, navigate your databases and collections, inspect your schema and use playgrounds to prototype queries and aggregations.
Version: 0.9.3
Publisher: MongoDB
VS Marketplace Link: https://marketplace.visualstudio.com/items?itemName=mongodb.mongodb-vscode

Name: C#
Id: ms-dotnettools.csharp
Description: C# for Visual Studio Code (powered by OmniSharp).
Version: 1.25.0
Publisher: Microsoft
VS Marketplace Link: https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp

### 2 - Docker: https://docs.docker.com/engine/install/
### 3 - Postman: https://www.postman.com/downloads/


# Running
### 1 - Add MongoDB package to Visual Studio Code through console:
`dotnet add package MongoDB.Driver`

### 2 - Run Docker Desktop Engine 

### 3 - Command to create Docker image on VS Code  onsole:
`docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db mongo`

### 4 - Connect the database
1. Go to the MongoDB icon on the screen left side;
2. You will see a disconnected base "localhost:27017";
3. Click on it to connect;
4. In case it fail, remove the connection;
5. Create it again by clicking on "Add Connection" after removing her;
6. Click on "Connect" on the screen that will show up and check if the leaf left sided on the localhost:27017 becames green to ensure it is connected;

### 5 - Run start base script
1. Search for the file "StartBase.mongodb";
2. Acces it;
3. Then look for the white play icon on top right corner and click on it;

### 6 - Start application
Press F5 to run application

# Testing
* In the src you'll find a Postman collection so you can try it out this application(Posterr.postman_collection.json)
* Or you can do it by Swagger, after running the project acces this link: https://localhost:7099/swagger/index.html

For the test your session user will be "TheJoshua" and this is yours initial data:
{
  "_id": 1,
  "Username": "TheJoshua",
  "CreatedOn": "2013-03-01T08:00:00",
  "Followers": [
    "Biden",
    "Obama",
    "Trump"
  ],
  "Following": [
    "Biden",
    "Obama",
    "JhonnyUchiha",
    "LucasRmax"
  ],
  "PostsCount": 8
}

# Phases
## Phase 1 - Coding
- [x] Build out a RESTful API 
- [x] Create some automated tests
- [x] Extra feature: **Search**

## Planning
### Doubts
* There will be a limit of characters for these replies as the normal post limit? If yes, the limit will count the "@ mentioning"?
* It will count as a post for the user?
* It will count as a post for the pagination of the feed?
* It will count as a post at all?
* It will be possible to reply a reply?
* It will be possible to edit the reply?
* It will be possible to delete the reply? If yes, when a reply have otherreply, it will delete all his child-replies or somehow we need to preserve the replies of the deleted reply?
* The feed would show posts from user that had replies or posts that the user replyied or both?
* Wich load would be better for replys? 3 by 3 or 5 by 5 as posts are recovered in userpage?
* It will be shown automatically or the user must click on something to show the replys for the post?

### Technical Planning
#### Front-end:
1. Assuming that the front-end team has a normal structured social media page i would add a tab control so the user can select wich feed he wants to see;
2. Would implement a call to the new endpoint that will be created for loading this new feed;
3. Would implement a loadout screen while data is beeing recovered;
4. Load the data in page following the pattern on the other 2 feeds;
5. Increment the design to allow the creation of replies and for showing them below the main post;

#### Back-end:
`Assuming the reply count as a post in every situation:`
1. Would adjust the Post entity to have an long attribute that indicates the repliedToPostId that he is replying to;
2. Would adjust the methods that already use the old Post structure so i won't break when running with the new model;
3. Would adjust the endpoint that creates the posts to create the new structure of posts so it can support the creation of replies;
4. Would create the endpoint that recovers the list of posts that have the attribute mentioned early filled, assuming that the sessionUser must be vinculated to this post somehow, beeing him the one who posted or the one who replied;
5. Increase the user posts counter;
6. Increase the posts counter;
7. If PM confirms that reply will be editable, would need to implement a PUT endpoint to modify the reply after validate that it is a reply;
8. If PM confirms that reply will be deletable, would need to implement a DELETE endpoint to delete the reply and all of his child-replies assuming that the PM said to delete every reply under the deleted reply;
9. Adjust starter data script for the new model;

## Critique
### Improvements
* Would work more on organizing the logic in the database manipulation code;
* Would work on the logic that recovers the feeds posts to recover the next page of posts based on the last post recovered createdOn date;
* Would implement a version control to the api;
* Review the repositories methods performance for upgrades;
* Documentate better the API code;

### Scaling

1. Fails:
* The first thing that would fail is the methods that loads the feeds, wich doesn't have any control of last post recovered so it would search the next page, considering new posts added between the load of first data page and next ones, in other words you could end recovering the same 10 posts you loaded before;
* The same for the extra search method;
* Work more on charge tests, i did some in Postman but wasn't a real charge test;
* Implement a great log for each method, now the project just have the class for this but don't use it, so if i had more time would use it to register evetual errors that would occur in production so i could know the error just by consulting the database or log file;

2. Steps to scale:
`Assuming that the scale doesn't change the business rules added to the application`
* Work on a session control for user, so we can remove the fixed user and reduce the consumption of database that is used today unecessarily, such as verifying user posts counter everytime he posts, checking the user quantity of followers;
* Review the repositories methods as i said before, looking for performance upgrades;
* Implement some authentication, using token;
* Implement some firewall rules in the server that would host the API to block DDOS Attacks and others kinds of attacks;
* Would move from git to Azure so we could see graphics and data about the development tasks;
* Would look for a great server to host this API and other for the DB;