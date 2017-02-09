using Babymart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Babymart.DAL.Tags
{
    public class TagsRepository : ITagsRepository
    {
        private babymart_vnEntities _context;
        public TagsRepository(babymart_vnEntities binhsuachobeContext)
        {
            this._context = binhsuachobeContext;
        }
        public List<sys_tags_Summary> ListTagsSummary(int type)
        {
            return _context.sys_tags_Summary.Where(o => o.RefType == type).ToList();
        }
        public sys_tags_Summary TagsSummarybysTringTag(string tag, int type)
        {
            return _context.sys_tags_Summary.Where(o => o.Tags.Equals(tag) && o.RefType == type).FirstOrDefault();
        }
        public sys_tags_Ref TagsRefbyRef(int refId, int refTag)
        {
            return _context.sys_tags_Ref.Where(o => o.RefTag == refTag && o.RefId == refId).FirstOrDefault();
        }
        public int AddTagsSummary(sys_tags_Summary tags)
        {
            _context.sys_tags_Summary.Add(tags);
            //  _context.SaveChanges();
            return tags.Id;
        }
        public int AddTagsRef(sys_tags_Ref tags, bool isSumit = true)
        {
            _context.sys_tags_Ref.Add(tags);
            if (isSumit)
                _context.SaveChanges();
            return tags.Id;
        }
        public void AddListagsRef(List<sys_tags_Ref> tags)
        {
            foreach (var item in tags)
            {
                _context.sys_tags_Ref.Add(item);
            }
            _context.SaveChanges();
        }
        public sys_tags_Summary GetbyIdtags_Summary(int id)
        {
            return _context.sys_tags_Summary.Find(id);
        }
        public List<sys_tags_Ref> ListTags_ref(int type, int refIds)
        {
            return _context.sys_tags_Ref.Where(o => o.sys_tags_Summary.RefType == type && o.RefId == refIds).ToList();
        }
        public List<sys_tags_Ref> ListTags_ref_RroductAmazing(int typecollection, int refIds)
        {
            return _context.sys_tags_Ref.Where(o => o.TypeCollection == typecollection && o.RefId == refIds).ToList();
        }
        public List<sys_tags_Ref> ListTagsRefbyRefTag(int Reftags)
        {
            return _context.sys_tags_Ref.Where(o => o.RefTag == Reftags).ToList();
        }
        public List<sys_tags_Ref> ListTagsRefArticle(int RefId, int Type, int TypeCollection)
        {
            return _context.sys_tags_Ref.Where(o => o.RefId == RefId && o.Type == Type && o.TypeCollection == TypeCollection).ToList();
        }
        public void RemoveTagsRef(int id)
        {
            var item = _context.sys_tags_Ref.Find(id);
            _context.sys_tags_Ref.Remove(item);
            _context.SaveChanges();
        }
        public void RemoveTagsSummary(int id)
        {
            var item = _context.sys_tags_Summary.Find(id);
            _context.sys_tags_Summary.Remove(item);
            _context.SaveChanges();
        }
        ////----------------sys_tags_Summary-------------------------
        //#region sys_tags_Summary
        //public sys_tags_Summary GetbyId(int id)
        //{
        //    return _context.sys_tags_Summary.Find(id);
        //}
        //public List<sys_tags_Summary> ListTagsSummary(int type)
        //{
        //    return _context.sys_tags_Summary.Where(o => o.RefType == type).ToList();
        //}
        //public List<sys_tags_Summary> ListTagsSummary(int type, int refid)
        //{
        //    return _context.sys_tags_Summary.Where(o => o.RefType == type && o.RefId == refid).ToList();
        //}
        //public sys_tags_Summary ListTagsSummary(int type, string Tags)
        //{
        //    return _context.sys_tags_Summary.Where(o => o.RefType == type && o.Tags.Equals(Tags)).FirstOrDefault();
        //}
        //public sys_tags_Summary ListTagsSummary(int type, int refid, int RefTag)
        //{
        //    return _context.sys_tags_Summary.Where(o => o.RefType == type && o.Id == RefTag).FirstOrDefault();
        //}
        //public int AddTagsSummary(sys_tags_Summary tags)
        //{
        //    _context.sys_tags_Summary.Add(tags);
        //    _context.SaveChanges();
        //    return tags.Id;
        //}
        //#endregion
        ////-----------------sys_tags_Ref-----------------------
        //#region sys_tags_Ref
        //public List<sys_tags_Ref> ListTags(int refIds)
        //{
        //    return _context.sys_tags_Ref.Where(o => o.RefId == refIds).ToList();
        //}
        //public List<sys_tags_Ref> ListTags(int type, int refIds)
        //{
        //    return _context.sys_tags_Ref.Where(o => o.sys_tags_Summary.RefType == type && o.RefId == refIds).ToList();
        //}
        //public List<sys_tags_Ref> ListTags(int type, string tag)
        //{
        //    return _context.sys_tags_Ref.Where(o => o.sys_tags_Summary.RefType == type && o.sys_tags_Summary.Tags.Equals(tag)).ToList();
        //}
        //public List<sys_tags_Ref> ListTagsbyRefTags(int type, int refTag)
        //{
        //    return _context.sys_tags_Ref.Where(o => o.sys_tags_Summary.RefType == type && o.RefTag == refTag).ToList();
        //}
        //public List<sys_tags_Ref> ListTagsbyRefTags(int refTags)
        //{
        //    return _context.sys_tags_Ref.Where(o => o.RefTag.Value == refTags).ToList();
        //}

        //public int AddTags(sys_tags_Ref tags)
        //{
        //    _context.sys_tags_Ref.Add(tags);
        //    _context.SaveChanges();
        //    return tags.Id;
        //}
        //public void RemoveTags(int id)
        //{
        //    var item = _context.sys_tags_Ref.Find(id);
        //    _context.sys_tags_Ref.Remove(item);
        //    _context.SaveChanges();
        //}
        //#endregion

        //#region sys_tags_Collection

        //public List<sys_tags_Collection> ListTagsbyCollection(int Type, int RefId)
        //{
        //    return _context.sys_tags_Collection.Where(o => o.Type == Type && o.RefId == RefId).ToList();
        //}

        //#endregion
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}