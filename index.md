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



### Documentation
You can look at [wiki](https://github.com/arduosoft/HubLink/wiki) to find all official docs.

### Server side:

 * [[Install and configure your wlog server (windows)|install-and-configure-wlog-server]]
 * [[Install and configure your wlog server (Linux)|install-and-configure-wlog-linux]]

### Client Side:

#### Agent based solution
Installing an agent that monitor some log file you do not need to change anything into your application. Here some options:

   * [[Agent integration using nxlog|wlog-nxlog-integration-using-agent]]

#### .Net

 * [[Integrate wlog in your application using Nlog|integrate-wlog-using-nlog]]
 * [[Integrate wlog in your application using log4net|integrate-wlog-using-log4net]]
 * [[Integrate wlog in your application with custom access|integrate-wlog-with-custom-access]]

#### PHP
There isn't any logger for PHP, yet. We are planning to implement following appenders:

  * Monolog appender
  * [Wordpress plugin to log into wlog](https://github.com/arduosoft/wlog.wordpress) 
  * Drupal log integration

It is easy to implement a Monolog logger,just follow this [guide](https://github.com/Seldaek/monolog/blob/master/doc/04-extending.md). If you need to implement it please add an issues, we will support you to develop the appender.


#### Java
Logging in java is quite easy using log4j framework. We are planing to implement an appender for it, but rightnow there isn't something ready. You can write your own appender simply by following this [tutorial](http://www.wideskills.com/log4j-tutorial/10-custom-appender-and-layout-in-log4j)


## Error reporting
Please open an issue to report us error. This channel can be used also for asking for documentation.


## Contributions
Any forms of contributions are welcome. Also error reporting or new feature request are an help for us. Of course we need to develop and mantain, so dev contributions are needed, but I put in your shoes so I suggest following way to give su help without spending lot of time. 

  * Create documentation
  * Telling us about your esperince (good or not, we are open to critiques!)
  * Tell to a friend about this project, or better write a post!
  * Report any issues yuou'll find
  * Ask for new feature if missing
  * Implement client libraries 

## License
This program is free software: you can redistribute it and/or modify   it under the terms of the GNU Lesser General Public License as   
published by the Free Software Foundation, version 3.  This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU  Lesser General Lesser Public License for more details. You should have received a copy of the GNU Lesser General Public License along with this program. If not, see <http://www.gnu.org/licenses/>.
 

## Off Topic: where you found the "download count badge"?
The download badge [![Downloads](http://github-analytics.apphb.com/badges/RepositoryDownloads/51643460.svg)](http//github-analytics.apphb.com/Stats) is part of "GitHub Analytics" one small application that give statistic about your github profile.
[Please try this also and let me know what you think about! ]( http//github-analytics.apphb.com/Stats)
