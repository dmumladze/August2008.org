using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using August2008.Common.Interfaces;
using August2008.Filters;
using August2008.Model;
using August2008.Models;

namespace August2008.Controllers
{
    public class HeroController : Controller
    {
        private readonly IHeroRepository _heroRepository;

        public HeroController(IHeroRepository heroRepository)
        {
            _heroRepository = heroRepository;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AjaxValidate]
        public JsonResult Save(Hero hero)
        {
            if (ModelState.IsValid)
            {
                var heroId = _heroRepository.CreateHero(hero);

                return Json(new {Ok = true, HeroId = heroId});
            }
            return Json(new {Ok = false});
        }
        public PartialViewResult Edit(int? id)
        {
            var model = new HeroModel();
            if (id.HasValue)
            {
                model.Hero = _heroRepository.GetHero(id.Value, 1);
            }
            else
            {
                model.Hero = new Hero();
            }
            var groups = _heroRepository.GetMilitaryGroups(1);
            var ranks = _heroRepository.GetMilitaryRanks(1);

            model.MilitaryGroups = new SelectList(groups, "MilitaryGroupId", "GroupName", model.Hero.MilitaryGroupId);
            model.MilitaryRanks = new SelectList(ranks, "MilitaryRankId", "RankName", model.Hero.MilitaryRankId);

            return PartialView(model);
        }
        [HttpPost]
        public JsonResult Upload(IEnumerable<HttpPostedFileBase> files)
        {
            foreach (var file in files)
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                    file.SaveAs(path);
                }
            }
            return Json(new {Ok = true});
        }
    }
}
