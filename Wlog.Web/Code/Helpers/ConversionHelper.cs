using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wlog.Web.Code.Classes;
using Wlog.Web.Models;

namespace Wlog.Web.Code.Helpers
{
    public class ConversionHelper
    {
        #region Entity To Model
        ///// <summary>
        ///// Convert LogEntity to LogModel
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //public static LogModel ConvertEntityToLogModel(LogEntity entity)
        //{
        //    LogModel result = new LogModel();
        //    result.ApplictionId = entity.ApplictionId;
        //    result.Level = entity.Level;
        //    result.Message = entity.Message;
        //    result.SourceDate = entity.SourceDate;
        //    return result;
        //}

        ///// <summary>
        ///// Convert List LogEntity to list LogModel
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //public static List<LogModel> ConvertListEntryToListLogModel(List<LogEntity> entity)
        //{
        //    List<LogModel> result = new List<LogModel>();
        //    foreach (LogEntity item in entity)
        //    {
        //        result.Add(ConvertEntityToLogModel(item));
        //    }
        //    return result;
        //}

        /// <summary>
        /// Convert ApplicationEntity to ApplicationHome
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static ApplicationHomeModel ConvertEntityToApplicationHome(ApplicationEntity entity)
        {
            ApplicationHomeModel result = new ApplicationHomeModel();
            result.Id = entity.IdApplication;
            result.ApplicationName = entity.ApplicationName;
            result.StartDate = entity.StartDate;
            result.IsActive = entity.IsActive;
            return result;
        }



        /// <summary>
        /// Convert List<ApplicationEntity> to List<ApplicationHome>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static List<ApplicationHomeModel> ConvertListEntityToListApplicationHome(List<ApplicationEntity> entity)
        {
            List<ApplicationHomeModel> result = new List<ApplicationHomeModel>();
            foreach (ApplicationEntity app in entity)
            {
                result.Add(ConvertEntityToApplicationHome(app));
            }
            return result;
        }
        #endregion

        internal static LogEntity ConvertLog(UnitOfWork uow,LogMessage log)
        {
            LogEntity result = new LogEntity();
            result.Uid = Guid.NewGuid();
            result.Level = log.Level;
            result.Message = log.Message;
            result.SourceDate = log.SourceDate;

            result.UpdateDate = DateTime.Now;

            try
            {
                Guid searchGuid = new Guid(log.ApplicationKey);
                result.ApplictionId = uow.Query<ApplicationEntity>().Where(p => p.PublicKey == searchGuid).First().IdApplication;
            }
            catch
            { }

            return result;

        }

        

        public static List<LogMessage> ConvertLogEntityToMessage(UnitOfWork uow, List<LogEntity> list)
        {
            List<LogMessage> result = new List<LogMessage>();
            foreach (LogEntity le in list)
            {
                //ApplicationEntity app = uow.Query<ApplicationEntity>().Where(p => p.IdApplication == le.ApplictionId).FirstOrDefault();//TODO: Map inside entity as referenced field
                LogMessage lm = new LogMessage();
                //if (app != null)
                //{
                //    lm.ApplicationKey = app.PublicKey.ToString();                    
                //}

                lm.Level = le.Level;
                lm.Message = le.Message;
                lm.SourceDate = le.SourceDate;
                
                result.Add(lm);
            }
            return result;
        }
    }
}