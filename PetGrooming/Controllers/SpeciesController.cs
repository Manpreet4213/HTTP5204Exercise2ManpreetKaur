using System;
using System.Collections.Generic;
using System.Data;
//required for SqlParameter class
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PetGrooming.Data;
using PetGrooming.Models;
using System.Diagnostics;

namespace PetGrooming.Controllers
{
    public class SpeciesController : Controller
    {

        private PetGroomingContext db = new PetGroomingContext();
        // GET: Species
        public ActionResult List()
        {
            //to modifying into the search bar
            List<Species> species = db.Species.SqlQuery("Select * from Species").ToList();
            return View(species);
        }
        // GET: Species/Details/4
        //to see the species with species id say id
        public ActionResult Show(int? id)
        {
            //if we don't provide id of the species that we want to get on the show
            //page then the page will be displaying the following error of the bad request.
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Species species = db.Species.Find(id); //EF 6 technique
            Species species = db.Species.SqlQuery("select * from species where speciesid=@SpeciesID", new SqlParameter("@SpeciesID", id)).FirstOrDefault();
            //if the species of the id that we are looking for, is not found.Then, the following error
            //of  HttpNotFound will be displayed.
            if (species == null)
            {
                return HttpNotFound();
            }
            return View(species);
        }

        //the following is the URL for the add species page.
        //URL: /Species/Add
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(string SpeciesName)
        {
            //test to pull the data correctly from the database.
            //Debug.WriteLine("Want to create a species with name " + SpeciesName + " ;

            // FORMAT QUERY- the query will look something like "insert into () values ()"...
            string query = "insert into species (Name) values (@SpeciesName)";
            SqlParameter[] sqlparams = new SqlParameter[1]; //1 piece of information to add
            //information will be like the key value pairs.
            sqlparams[0] = new SqlParameter("@SpeciesName",SpeciesName);
            

            //db.Database.ExecuteSqlCommand for executing add query
            db.Database.ExecuteSqlCommand(query,sqlparams);


            //to see the new species that we crated, running the list method.
            return RedirectToAction("List");
        }


        //URL: /Species/Update/5  -> update species with id 5
        public ActionResult Update(int id)
        {
            //how to get species data
            //run  select query to select the species whose data we want to update.
            Species selectedspecies = db.Species.SqlQuery("select * from species where speciesid = @SpeciesID", new SqlParameter("@SpeciesID", id)).FirstOrDefault();
            return View(selectedspecies);
        }
        [HttpPost]
        public ActionResult Update(string SpeciesName, int id)
        {
            //Step:1 writing debug line to pull the data.
            Debug.WriteLine("I'm pulling data of " + SpeciesName);
            //Step:2 for updating our species details, applying update query to change data for that species.
            string query = "Update species set Name = @SpeciesName where speciesid =" + id;
            SqlParameter[] sqlparams = new SqlParameter[1]; //1 piece of information to update.
            sqlparams[0] = new SqlParameter("@SpeciesName", SpeciesName);


            //to run update query, we need following
            db.Database.ExecuteSqlCommand(query,sqlparams);
            //going back to list page to see the updated details for our species.
            return RedirectToAction("List");
        }

        //[HttpPost]
        //for deleting the species with speciesid say id
        public ActionResult Delete(int id)
        {
            //writing query to delete species from the database.
            string query = "delete from species where speciesid = " + id;
            //writing the following to run delete statement.
            db.Database.ExecuteSqlCommand(query);
            //returning back to the list page to see that the deleted species is not there anymore.
            return RedirectToAction("List");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
