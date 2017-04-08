using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using Wlog.BLL.Entities;

namespace Wlog.Library.BLL.Reporitories.Interfaces
{
    public interface IKeyPairRepository
    {
        string GetByKey(Guid dictionaryId,string key);

        KeyPairEntity GetById(Guid itemId);

        KeyPairEntity Save(Guid dictionaryId, string key, string value);


        IPagedList<KeyPairEntity> Search(Guid dictionaryId, string key, int start, int count);



        IPagedList<DictionaryEntity> GetDictionaries(Guid id, string dictionaryName, int start, int count);
        DictionaryEntity CreateDictionary(DictionaryEntity d);
        bool Delete(KeyPairEntity value);
    }
}
