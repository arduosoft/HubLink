using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using NLog;
using Wlog.BLL.Entities;
using Wlog.Library.BLL.Reporitories;

namespace Wlog.Web.Code.API
{
    [AllowAnonymous]
    public class DictionaryController : ApiController
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

       

        public KeyPairEntity Get(Guid publicKey, string dictionaryName, string key)
        {
            try
            {
                var application = RepositoryContext.Current.Applications.GetByApplicationKey(publicKey.ToString());
                if (application == null) throw new Exception("Unable to find appliction");
                var dictionary = RepositoryContext.Current.KeyPairRepository.GetDictionaries(application.Id, dictionaryName, 0, 1).FirstOrDefault();
                if (dictionary == null) throw new Exception("Unable to find dictionary");
                return new KeyPairEntity()
                {
                    ItemValue = RepositoryContext.Current.KeyPairRepository.GetByKey(dictionary.Id, key)
                };
                

            }
            catch (Exception err)
            {
                _logger.Error(err);
                throw err;
            }

        }

    }
}