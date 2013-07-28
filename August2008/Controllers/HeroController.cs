using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using August2008.Common.Interfaces;
using August2008.Filters;
using August2008.Model;
using August2008.Models;
using August2008.Helpers;
using August2008.Properties;
using AutoMapper;

namespace August2008.Controllers
{
    /// <summary>
    /// Orchestrates hero API for editing, saving, deleting and updating data.
    /// </summary>
    public class HeroController : BaseController
    {
        private readonly IHeroRepository _heroRepository;
        private readonly IMetadataRepository _metadataRepository; 

        /// <summary>
        /// Initializes controller with an instance of repository interface.
        /// </summary>
        public HeroController(IHeroRepository heroRepository, IMetadataRepository metadataRepository, ICacheProvider cacheProvider)
            : base(cacheProvider)
        {
            _heroRepository = heroRepository;
            _metadataRepository = metadataRepository;
        }
        /// <summary>
        /// Renders default page with initial information.
        /// </summary>
        [HttpGet]
        [NoCache]
        public ActionResult Index(int? page, string name)
        {
            var criteria = _heroRepository.GetHeros(new HeroSearchCriteria
                {
                    PageNo = page.GetValueOrDefault(1),
                    Name = name,
                    PageSize = 10,
                    LanguageId = Me.LanguageId
                });
            var alphabet = SiteHelper.GetAlphabet();
            var heroAlphabet = _heroRepository.GetAlphabet(Me.LanguageId);
            var model = Mapper.Map(criteria, new HeroSearchModel());
            model.Alphabet = alphabet.ConvertAll<AlphabetLetter>(x => new AlphabetLetter(x, heroAlphabet.Contains(x)));            
            return View(model);
        }
        /// <summary>
        /// Creates or updates hero record with photos and other attributes.
        /// </summary>
        [HttpPost]
        [AjaxValidate]
        [Authorize2(Roles = "Admin")]
        public JsonResult Save(HeroModel model, IEnumerable<HttpPostedFileBase> images) 
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var hero = new Hero();
                    var photos = new List<IPostedFile>();
                    Mapper.Map(model, hero);

                    if (!images.IsNullOrEmpty())
                    {
                        foreach (var image in images)
                        {
                            if (image.ContentLength > 0)
                            {
                                var file = new PostedFile(image);
                                if (image.FileName.Equals(model.Thumbnail, StringComparison.OrdinalIgnoreCase))
                                {
                                    file.Attributes.Add("IsThumbnail", "1");
                                }
                                file.Attributes.Add("FileName", string.Concat(Guid.NewGuid(), Path.GetExtension(image.FileName)));
                                photos.Add(file);
                            }
                        }
                    }
                    hero.UpdatedBy = Me.UserId;
                    if (model.IsNew)
                    {
                        hero.LanguageId = Me.LanguageId;
                        hero.HeroId = _heroRepository.CreateHero(hero, photos);
                    }
                    else
                    {
                        _heroRepository.UpdateHero(hero, photos);
                    }
                    return Json(new { Ok = true, HeroId = hero.HeroId });
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return Json(new {Ok = false});
        }
        /// <summary>
        /// Gets the edit form for new or existing records where users can enter information.
        /// </summary>
        [HttpGet]
        [NoCache]
        //[Authorize2(Roles = "Admin")]
        public PartialViewResult Edit(int? id)
        {
            var model = new HeroModel();
            if (id.HasValue)
            {
                var hero = _heroRepository.GetHero(id.Value, Me.LanguageId);
                Mapper.Map(hero, model);
            }
            var groups = _metadataRepository.GetMilitaryGroups(Me.LanguageId);
            var ranks = _metadataRepository.GetMilitaryRanks(Me.LanguageId);

            model.MilitaryGroups = new SelectList(groups, "MilitaryGroupId", "GroupName", model.MilitaryGroupId);
            model.MilitaryRanks = new SelectList(ranks, "MilitaryRankId", "RankName", model.MilitaryRankId);

            return PartialView("EditPartial",model);
        }
        /// <summary>
        /// Gets a single photo by name and requested size.
        /// </summary>
        [HttpGet]
        public FileResult GetPhoto(string name, PhotoSize? size)
        {
            return SiteHelper.GetHeroPhoto(name, size.GetValueOrDefault(PhotoSize.Small));
        }
        /// <summary>
        /// Deletes a single photo.
        /// </summary>
        [HttpPost]
        [Authorize2(Roles = "Admin")]
        public JsonResult DeletePhoto(int id)
        {
            var photo = _heroRepository.DeletePhoto(id);
            return Json(new { Ok = true });
        }
    }
}
