<img src="https://github.com/arduosoft/HubLink/blob/release/v1.0/Wlog.Web/Images/HubLinkLogo.png" width="200"> 


[![Build status](https://ci.appveyor.com/api/projects/status/c4v8ill28a9wbjaj?svg=true)](https://ci.appveyor.com/project/zeppaman/wlog)
[![Build status](https://www.codefactor.io/Content/badges/A.svg)](https://www.codefactor.io/repository/github/arduosoft/hublink/overview/release/v1.0)
[![Downloads](http://github-analytics.apphb.com/badges/RepositoryDownloads/51643460.svg)](http//github-analytics.apphb.com/Stats)
      
### Project Description
Hublink born as an easy-to-install, intuitive and lightweight remote log serivice made in asp.net. Later more feature was added so became an Hub of services that can be easily plugged into applications.

It's purpose it to reduce the work about standard features (full text search, log collecting, key pair store, etc..) moving from inside application to an external system that exposes services.

There are many product that cover single functionality, but in most cases to integrate all it is complex, and you'll have a lot of external system to manage. Using HubLink you'll manage only one external system  and you'll have all feature ready to be activated on demand. 

Moreover, HubLink manage multitenancy at application level, so you can attach many consumers to the same application.

Another benefit about Hublink is that it doesn't require any external system, just installa a simple .net application on your server.

HubLink doesn't introduce any technological limitation on the client side. Rest service can be consumed by any language. Log collectin is integrated with standard tools like nxlog.



### How to install server

Because wlog is a simple web site installation is easy, database independent. Please see documentation to discover how to install
How to integrate in your application

Nowadays it's easy to inegrate a new output for your log. I hope most of application will yest employ logging framework as log4net or Nlog, so in these cases you just need to implement a new "output" ( they are calledapppenders for log4net and target for Nlog..) and without edit any line of code all your log will be routed to our platform. 

Do you think it is to much complicate? just download our library that integrate with Nlog, so you'll need only to edit few settings in configuration. See Integrate wlog in your application using Nlog

You do not use a log framework? Easy, start to use it; even if you won't to employ wlog tecnology.

Your project are not written in .net? No problems, our client library integrate throug a simple api call, so you just need to make a REST call to store log or extend your current log library. See Integrate wlog in your application with custom access

#
#
