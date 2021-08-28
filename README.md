
 # Sticky
 She is a clean, high performance and completely open source retargeting platform which was created over 5 years ago and was actively working for almost 2 years. She is now retired and wants to share her experience with other ad-networks.

## Dictionary
**Retargeting**:Retargeting is to show dynamic Ads to user based on their activity in sites and online shops.\
 **Host**: A verified address which can inject Sticky script(track.js) to record their user's data into sticky.\
 **Activity**: All the things a user can do in a website. For example : visiting a page, visiting a product, adding a product to his/her cart, removing it, like or dislike an item and purchase.\
**Segment**: a group of users who has done some specific activity.\
**Product Template**: a way of showing to a user more interesting dynamic ads to increase the chance of him/her clicking  on the ad.\
**Host Admin**: a user who owns the host and can create and modify its segment and ad templates.\
**Membership**: All the segments from one or multiple hosts which a **single** user is a member of.\
**Sticky Id**:  a **Cookie** for all the hosts in sticky.\
**Partner**:  the network who is using sticky to view dynamic ad to a user.\
**Cookie Matching**:  a way for a network to match their user with Sticky Id. 
 ## Requirements
 '*' means **not interchangeable** to other provider or framework.
  - .*[NET Core 5](https://dotnet.microsoft.com/download/dotnet/5.0). 
  - A non-relational database for saving cookie matching data. (Currently [MongoDb](https://www.mongodb.com/) has been implemented).
  - *[Druid](https://druid.apache.org/) for recording all the user activities in a host.
  - A Messaging provider (Currently [Kafka](https://kafka.apache.org/) has been implemented).
  - *[Aerospike](https://aerospike.com/) (For Key-Value Persistence)
  - A relational-Database that works with Entity Framework to save all host admins customizations ,segments and product templates. (MSSQL has been implemented).
  - A Cache provider ([Redis](https://redis.com/) has been Implemented)\
### Note!
Sticky is a retargeting focused solution and it is not  **not** a complete advertising platform. it relies on Advertising networks and RTB (Real Time Biding  System) to work.

## Why use this?

- This project has passed its test for more than 40M records per day with a medium linux server and it can easily be tuned for more than this.
- It is clean and the code base is pretty SOLID (ihh ihh  :D ).
- It is free.
- It is based on .NET Core so you can host it in either windows server or linux-based server.
- It can be a good template for ad network projects because it can be modified with ease.

## Usage
### 1- Sticky.Application.Dashboard
this project is responsible for saving segments, hosts, partners and text-templates and customizing the way host admins can show to users the dynamic ad.\
It does not have much of a dependency, Installing MSSQL ( or any other Entity Framework supported relational database) is enough. Change the connection string (and other configuration if needed) inside the appsetting.json then run the migration and the dashboard is ready.
### 2- Sticky.Application.Script
this project is responsible for recording all user activities in verified hosts.\
For this project to work the minimum dependency is [Kafka](https://kafka.apache.org/) and [Redis](https://redis.com/).

### 3- Sticky.Application.Advertising
this project is responsible for grouping all the membership from different databases and returning the result with products and interesting ad templates in one object.\
The dependency for this project is only [Redis](https://redis.com/),  Redis is used for its cache functionality and some memory repository for high performance query result.
### 4- Sticky.Application.CookieMatching
if you are not using other partners for your advertisement, you can ignore deploying this project. If you are, This project should be provided with a Not-Relational Database and Also [Redis](https://redis.com/) for providing cache.\
This project has no controller and just a middleware to load some iframes into each other.

### 5- Sticky.Application.CacheUpdater
a scheduler for updating the data from the relational database into our cache system. It depends on a relational database that entity framework supports and a cache provider  It can be run as a service inside Linux-based systems and can be modified to a service for windows based systems. 
### 6- Sticky.Application.ResponseUpdater
a scheduler for updating the handling the ad requests from partners and creating a response based on the data received by different databases or our data cube. It should be connected to Druid, [Aeorspike](https://aerospike.com/), [Redis](https://redis.com/) and [MongoDb](https://www.mongodb.com/).  It can be run as a service inside Linux-based systems and can be modified to a service for windows based systems. \
you can even run multiple services of this project for faster response generation and you can even configure it to be run just for a specific segment.
### 7- Sticky.Application.Consumer
a scheduler for getting the messages from Messaging provider and save it to a key-value persistence. It depends on [Aerospike](https://aerospike.com/) client (as a key-value persistence) and Kafka consumer. It can be run as a service inside Linux-based systems and can be modified to a service for windows based systems. 

## Notes: 
Any questions that can bootstrap you into deployment, feel free to contact and ask at : ghaagh@gmail.com.\
Happy coding ;)
