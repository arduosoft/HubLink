# wlog
[![Build status](https://ci.appveyor.com/api/projects/status/c4v8ill28a9wbjaj?svg=true)](https://ci.appveyor.com/project/zeppaman/wlog)
[![Code Quality](https://scan.coverity.com/projects/9697/badge.svg)](https://scan.coverity.com/projects/arduosoft-wlog)
      
### Project Description
Wlog is an easy-to-install, intuitive and lightweight remote log serivice made in asp.net.

Wlog is a simple platform tha allow to log remotely from all kind of application, without any technological limitation.

You just need to configure your application (desktop or web) to log into wlog server, Nlog integration is provided. Since that all log will be sent to wlog server and displayed in a web application. 


##How it work


###How to install server

Because wlog is a simple web site installation is easy, database independent. Please see documentation to discover how to install
How to integrate in your application

Nowadays it's easy to inegrate a new output for your log. I hope most of application will yest employ logging framework as log4net or Nlog, so in these cases you just need to implement a new "output" ( they are calledapppenders for log4net and target for Nlog..) and without edit any line of code all your log will be routed to our platform. 

Do you think it is to much complicate? just download our library that integrate with Nlog, so you'll need only to edit few settings in configuration. See Integrate wlog in your application using Nlog

You do not use a log framework? Easy, start to use it; even if you won't to employ wlog tecnology.

Your project are not written in .net? No problems, our client library integrate throug a simple api call, so you just need to make a REST call to store log or extend your current log library. See Integrate wlog in your application with custom access


###License
Copyright (C) 2016, 

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    **This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.**

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
