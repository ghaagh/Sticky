# Sticky


Sticky is an all-in-one open-source retargeting solution for ad-networks and RTBs. It includes:

  - Cookie-Matching API
  - Advertising API
  - Script API for gathering information form websites and webstores.
  - Dashboard API for integrating easily with your DSP panel.

# Features!

  - Tested for High traffic approximately 40M request per day
  - Behavior retargeting, based on user's differenct activities like Like, Dislike, Add To Cart and Page and Product visit.

# Requirements
  - .Net core 2.2.
  - MongoDB (For cookie syncing).
  - Druid (for retargeting users base on product categories).
  - Kafka (for sending message from script API).
  - Aerospike (for recording user's product visits).
  - MSSQL (for saving user's setting and created Segments And Hosts).
  - Redis (for using as a cache).


# Note!
- You may also need IIS or nginX to publish all API .
- for services part, it is recommended to use SupervisorCTL to manage your service. you can also use a tmux window for services. but it is not recommended.

# Important!
- Be patient . I am updating the docs of this project
