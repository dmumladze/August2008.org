using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using August2008.Common.Interfaces;
using August2008.Filters;
using August2008.Model;
using August2008.Models;
using August2008.Properties;
using AutoMapper;

namespace August2008.Controllers
{
    public class HeroController : Controller
    {
        private User _principal;
        private readonly IHeroRepository _heroRepository;

        public HeroController(IHeroRepository heroRepository)
        {
            _heroRepository = heroRepository;
        }
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            _principal = requestContext.HttpContext.User as User;

            base.Initialize(requestContext);
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AjaxValidate]
        public JsonResult Save(HeroModel model, IEnumerable<HttpPostedFileBase> images) 
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var hero = new Hero();
                    var photos = new List<IPostedFile>();

                    Mapper.Map(model, hero);
                    
                    foreach (var image in images)
                    {                        
                        if (image.ContentLength > 0)
                        {
                            var file = new PostedFile(image);
                            if (image.FileName.Equals(model.Thumbnail, StringComparison.OrdinalIgnoreCase))
                            {
                                file.Attributes.Add("IsThumbnail", "1");
                            }
                            file.Attributes.Add("FileName", Path.Combine(Server.MapPath(Settings.Default.HeroPhotoDirectory), 
                                string.Format("{0}-{1}-{2}", 
                                    hero.FirstName, hero.LastName, Guid.NewGuid())));
                            photos.Add(file);
                        }
                    }
                    hero.UpdatedBy = _principal.UserId;
                    hero.LanguageId = _principal.Profile.Lang.LanguageId;
                    hero.HeroId = _heroRepository.CreateHero(hero, photos);
                    return Json(new {Ok = true, HeroId = 1});
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return Json(new {Ok = false});
        }
        public PartialViewResult Edit(int? id)
        {
            var model = new HeroModel();
            if (id.HasValue)
            {
                //model.Hero = _heroRepository.GetHero(id.Value, 1);
            }
            else
            {
                model = new HeroModel();
            }
            var groups = _heroRepository.GetMilitaryGroups(1);
            var ranks = _heroRepository.GetMilitaryRanks(1);

            model.MilitaryGroups = new SelectList(groups, "MilitaryGroupId", "GroupName", model.MilitaryGroupId);
            model.MilitaryRanks = new SelectList(ranks, "MilitaryRankId", "RankName", model.MilitaryRankId);

            return PartialView(model);
        }
    }
}
