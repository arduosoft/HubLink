using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using Wlog.BLL.Entities;
using Wlog.Library.BLL.Classes;
using Wlog.Library.BLL.Reporitories.Interfaces;

namespace Wlog.Library.BLL.Reporitories
{
    public class DBKeyPairRepository : EntityRepository<KeyPairEntity>, IKeyPairRepository
    {
        public DictionaryEntity CreateDictionary(DictionaryEntity d)
        {
            using (var op = this.BeginUnitOfWork())
            {
                op.SaveOrUpdate(d);
                op.Commit();
                return d;
            }
        }

        public string GetByKey(Guid dictionaryId, string key)
        {
            using (var op = this.BeginUnitOfWork())
            {
                var item = op.Query<KeyPairEntity>().Where(x => x.DictionaryId.CompareTo(dictionaryId) == 0 && x.ItemKey.Equals(key)).FirstOrDefault();
                if (item == null) return null;
                return item.ItemValue;
            }
        }

        public IPagedList<DictionaryEntity> GetDictionaries(Guid applicationId, string dictionaryName, int start, int count)
        {
            using (var op = this.BeginUnitOfWork())
            {
                var query = op.Query<DictionaryEntity>();
                if (applicationId != null)
                {
                    query = query.Where(x => x.ApplicationId == applicationId);
                }

                if (dictionaryName != null)
                {
                    query = query.Where(x => x.Name.Contains(dictionaryName));
                }

                if (start > 0)
                {
                    query = query.Skip(start);
                }


                int total = query.Count();

                if (count > 0)
                {
                    query = query.Take(count);
                }

                var items = query.ToList();
                var page = 1;
                if (start > 0 && count > 0)
                {
                    page = (start / count)+1;
                }
                if (count == 0) count = int.MaxValue;
                return new StaticPagedList<DictionaryEntity>(items, page, count, total);
            }
        }

        public KeyPairEntity Save(Guid dictionaryId, string key, string value)
        {
          


            using (var op = this.BeginUnitOfWork())
            {
                op.BeginTransaction();

                var kpe = op.Query<KeyPairEntity>().Where(x => x.ItemKey == key && x.DictionaryId == dictionaryId).FirstOrDefault();
                if (kpe == null)
                {
                    kpe = new KeyPairEntity()
                    {
                        DictionaryId = dictionaryId,
                       
                    };
                }

                kpe.ItemKey = key;
                kpe.ItemValue = value;

                op.SaveOrUpdate(kpe);
                op.Commit();
                return kpe;
            }
            
        }

        public IPagedList<KeyPairEntity> Search(Guid dictionaryId, string key, int start, int count)
        {
            using (var op = this.BeginUnitOfWork())
            {
                var query = op.Query<KeyPairEntity>();
                if (dictionaryId != null)
                {
                    query = query.Where(x => x.DictionaryId == dictionaryId);
                }

                if (key != null)
                {
                    query = query.Where(x => x.ItemKey.Contains(key));
                }

                int total = query.Count();


                if (start > 0)
                {
                    query = query.Skip(start);
                }
              

                if (count > 0)
                {
                    query = query.Take(count);
                }

                var items = query.ToList();
                var page = 1;
                if (start > 0 && count > 0)
                {
                    page = (start / count) + 1;
                }
                if (count == 0) count = int.MaxValue;
                return new StaticPagedList<KeyPairEntity>(items, page, count, total);
            }
        }
    }
}
