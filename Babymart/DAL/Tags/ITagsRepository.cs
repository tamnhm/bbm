using Babymart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Babymart.DAL.Tags
{
    public interface ITagsRepository : IDisposable
    {
        List<sys_tags_Summary> ListTagsSummary(int type);
        sys_tags_Summary TagsSummarybysTringTag(string tag, int type);
        sys_tags_Ref TagsRefbyRef(int refId, int refTag);
        int AddTagsSummary(sys_tags_Summary tags);
        int AddTagsRef(sys_tags_Ref tags, bool isSumit = true);
        sys_tags_Summary GetbyIdtags_Summary(int id);
        List<sys_tags_Ref> ListTags_ref(int type, int refIds);
        List<sys_tags_Ref> ListTagsRefbyRefTag(int Reftags);
        List<sys_tags_Ref> ListTagsRefArticle(int RefId, int Type, int TypeCollection);
        void RemoveTagsRef(int id);
        void RemoveTagsSummary(int id);
        void AddListagsRef(List<sys_tags_Ref> tags);
        List<sys_tags_Ref> ListTags_ref_RroductAmazing(int typecollection, int refIds);
    }
}