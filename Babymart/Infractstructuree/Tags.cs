using Babymart.DAL.Tags;
using Babymart.Models;
using Babymart.Models.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Babymart.Models.Module.Enum;
namespace Babymart.Infractstructuree
{
    public static class TagsCommon
    {
        static babymart_vnEntities db = new babymart_vnEntities();
        static ITagsRepository _rpTags = new TagsRepository(new babymart_vnEntities());
        public static List<int> ListRefIdTagBy(int type, string tagString)
        {
            var tags = _rpTags.TagsSummarybysTringTag(tagString, type);
            var result = new List<int>();
            if (tags != null && tags.sys_tags_Ref != null)
            {
                foreach (var item in tags.sys_tags_Ref)
                    if (item != null)
                        result.Add(item.RefId.Value);
            }
            return result;
        }
        public static List<sys_tags_SummaryModel> Gettags_SummarybyTagRef(int type, int refid)
        {
            var result = new List<sys_tags_SummaryModel>();
            var TagRef = _rpTags.ListTags_ref(type, refid);// lay id summary
            if (TagRef != null)
            {
                TagRef.ForEach(x =>
                {
                    var tmp = _rpTags.GetbyIdtags_Summary(x.RefTag.Value);
                    if (tmp != null)
                    {
                        result.Add(Mapper.Map<sys_tags_SummaryModel>(tmp));
                    }
                });
            }
            return result;
        }
        public static List<sys_tags_SummaryModel> Gettags_Summary(int type)
        {
            return Mapper.Map<List<sys_tags_SummaryModel>>(_rpTags.ListTagsSummary(type));
        }
        public static void ProcessTags(string[] tags, int refId, int Reftype, int TypeCollection = 0)
        {
            var tagObj = new sys_tags_Summary();
            foreach (var item in tags)
            {
                if (!String.IsNullOrEmpty(item))
                {
                    tagObj = _rpTags.TagsSummarybysTringTag(item, Reftype);

                    if (tagObj == null)
                    {
                        var tagSummary = _rpTags.AddTagsSummary(new sys_tags_Summary
                        {
                            Tags = item,
                            RefType = Reftype,
                        });
                        _rpTags.AddTagsRef(new sys_tags_Ref
                        {
                            RefId = refId,
                            RefTag = tagSummary,
                            Type = Reftype,
                            TypeCollection = TypeCollection != 0 ? TypeCollection : 0
                        });
                    }
                    else
                    {
                        var oo = _rpTags.TagsRefbyRef(refId, tagObj.Id);
                        if (oo == null)
                        {
                            _rpTags.AddTagsRef(new sys_tags_Ref
                            {
                                RefId = refId,
                                RefTag = tagObj.Id,
                                Type = Reftype,
                                TypeCollection = TypeCollection != 0 ? TypeCollection : 0
                            });
                        }
                    }
                }
            }
        }
        public static bool RemoveTag(int IdTags)
        {
            bool isDeleteSummaryTag = false;
            var tagRef = db.sys_tags_Ref.Find(IdTags);
            if (tagRef != null && tagRef.RefTag != null)
            {
                var checkhasTag = _rpTags.ListTagsRefbyRefTag(tagRef.RefTag.Value);
                if (checkhasTag.Count <= 1)
                {
                    isDeleteSummaryTag = true;
                }
                _rpTags.RemoveTagsRef(IdTags);
                if (isDeleteSummaryTag == true
                    && !tagRef.sys_tags_Summary.Tags.Equals(">>Gói đồ sơ sinh")
                    && !tagRef.sys_tags_Summary.Tags.Equals(">>Home"))
                    _rpTags.RemoveTagsSummary(tagRef.RefTag.Value);
                return true;
            }
            return false;
        }
        public static int[] listIdArticlebyTag(int RefId, int TypeCollection, int RefType)
        {
            var listIds = new List<int>();
            var List_tags_Ref = db.sys_tags_Ref.Where(o => o.RefId == RefId && o.TypeCollection == TypeCollection).ToList();
            foreach (var item in List_tags_Ref)
            {
                var tag = new List<sys_tags_Ref>();
                if (TypeCollection == (int)TagsCollection.TagsProduct_Magazine)
                    tag = db.sys_tags_Ref.Where(o => o.RefTag == item.RefTag && o.Type == RefType && (o.TypeCollection == 0 || o.TypeCollection == null)).ToList();
                else
                    tag = db.sys_tags_Ref.Where(o => o.RefTag == item.RefTag && o.Type == RefType).ToList();
                if (tag != null && tag.Count > 0)
                {
                    foreach (var item2 in tag)
                    {
                        listIds.Add(item2.RefId.Value);
                    }

                }
            }
            return listIds.ToArray();
        }
    }
}