﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using _036_MoviesMvcWissen.Models.Demos.Templates;

namespace _036_MoviesMvcWissen.Controllers
{
    #region Razor Demos
    public static class NameUtil
    {
        public static string GetName()
        {
            return "Name: Çağıl Alsaç";
        }
    }
    #endregion

    public class DemosController : Controller
    {
        #region Razor Demos
        public ActionResult Razor1() // kodlar için view'a gidilmelidir.
        {
            return View();
        }

        public ActionResult Razor2() // kodlar için view'a gidilmelidir.
        {
            return View();
        }
        #endregion

        #region Route Values
        public string FromRoute(int id)
        {
            return id.ToString();
        }
        #endregion

        #region Query String
        //public string FromQueryString(string name, string surname)
        public string FromQueryString()
        {
            var name = Request.QueryString["name"];
            var surname = Request.QueryString["surname"];
            return name + " " + surname;
        }
        #endregion


        #region Templates
        public ActionResult GetPeople()
        {
            List<PersonModel> people;
            if (Session["people"] == null)
            {
                people = new List<PersonModel>()
            {
                new PersonModel()
                {
                    Id = 1,
                    FullName = "Çağıl Alsaç",
                    IdentityNo = "123456",
                    GraduatedFromUniversity = true,
                    BirthDate = DateTime.Parse("19.06.1980")
                },
                new PersonModel()
                {
                    Id = 2,
                    FullName = "Leo Alsaç",
                    IdentityNo = "654321",
                    GraduatedFromUniversity = false,
                    BirthDate = DateTime.Parse("25.05.2015")
                }
            };
                Session["people"] = people;
            }
            else
            {
                people = Session["people"] as List<PersonModel>;
            }

            return View(people);

        }

        public ActionResult GetPersonDetails(int id)
        {
            List<PersonModel> people = Session["people"] as List<PersonModel>;
            PersonModel person = people.SingleOrDefault(e => e.Id == id);
            return View(person);
        }
        #endregion

        public ActionResult AddPerson()
        {
            return View();
        }
        [HttpPost]
        [ActionName("AddPerson")]
        public ActionResult AddPerson2(PersonModel personModel)
        {
            List<PersonModel> people = Session["people"] as List<PersonModel>;
            if (people.Count > 0)
                personModel.Id = 1;
            else
                personModel.Id = people.Max(e => e.Id) + 1;
            people.Add(personModel);
            Session["people"] = people;

            return RedirectToAction("GetPeople");

            return View(personModel);
        }
    }
}