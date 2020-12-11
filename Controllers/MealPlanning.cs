using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GC_PlanMyMeal.Controllers
{
    public class MealPlanning : Controller
    {
        
        // GET: MealPlanning
        public ActionResult Index()
        {
            return View();
        }

        // GET: MealPlanning/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MealPlanning/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MealPlanning/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MealPlanning/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MealPlanning/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MealPlanning/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MealPlanning/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
