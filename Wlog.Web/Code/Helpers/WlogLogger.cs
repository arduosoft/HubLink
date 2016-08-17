﻿//******************************************************************************
// <copyright file="license.md" company="Wlog project  (https://github.com/arduosoft/wlog)">
// Copyright (c) 2016 Wlog project  (https://github.com/arduosoft/wlog)
// Wlog project is released under LGPL terms, see license.md file.
// </copyright>
// <author>Daniele Fontani, Emanuele Bucaelli</author>
// <autogenerated>true</autogenerated>
//******************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;

namespace Wlog.Web.Code.Helpers
{
    public static class WlogLogger
    {
        private static Logger logger;
        public static Logger Current
        {
            get
            {
                if (logger == null)
                {
                    logger = LogManager.GetCurrentClassLogger();
                }
                return logger;
            }
        }
    }
}