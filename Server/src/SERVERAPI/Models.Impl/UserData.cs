using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERVERAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERVERAPI.Models.Impl
{
    public class UserData
    {
        private readonly IHttpContextAccessor _ctx;

        public UserData(IHttpContextAccessor ctx)
        {
            _ctx = ctx;
        }

        public void NewFarm()
        {
            string newYear = DateTime.Now.ToString("yyyy");
            FarmData userData = new FarmData();
            userData.farmDetails = new FarmDetails();
            userData.farmDetails.year = newYear;
            userData.years = new List<YearData>();
            userData.years.Add(new YearData() { year = newYear });
            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }

        public FarmDetails FarmDetails()
        {
            FarmData farmData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            return farmData.farmDetails;
        }

        public void UpdateFarmDetails(FarmDetails fd)
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.farmDetails.farmName = fd.farmName;
            userData.farmDetails.farmRegion = fd.farmRegion;
            userData.farmDetails.soilTests = fd.soilTests;
            userData.farmDetails.manure = fd.manure;
            userData.farmDetails.year = fd.year;
            YearData yd = userData.years.FirstOrDefault(y => y.year == fd.year);
            if(yd == null)
            {
                YearData ny = new YearData();
                ny.year = fd.year;
                userData.years.Add(ny);
            }
            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }

        public void AddField(Field newFld)
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);

            if(yd.fields == null)
            {
                yd.fields = new List<Field>();
            }

            yd.fields.Add(newFld);
            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }

        public void UpdateField(Field updtFld)
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            Field fld = yd.fields.FirstOrDefault(f => f.fieldName == updtFld.fieldName);

            fld.area = updtFld.area;
            fld.comment = updtFld.comment;
           
            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }

        public void DeleteField(string name)
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            Field fld = yd.fields.FirstOrDefault(f => f.fieldName == name);
            yd.fields.Remove(fld);

            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }

        public Field GetFieldDetails(string fieldName)
        {
            Field fld = new Field();
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");

            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);

            if (yd.fields == null)
            {
                yd.fields = new List<Field>();
            }

            fld = yd.fields.FirstOrDefault(y => y.fieldName == fieldName);

            return fld;
        }

        public List<Field> GetFields()
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");

            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);

            if (yd.fields == null)
            {
                yd.fields = new List<Field>();
            }

            return yd.fields;
        }

        public void AddFieldNutrientsManure(string fldName, NutrientManure newMan)
        {
            int nextId = 1;

            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            Field fld = yd.fields.FirstOrDefault(f => f.fieldName == fldName);

            if (fld.nutrients == null)
            {
                fld.nutrients = new Nutrients();
                fld.nutrients.nutrientManures = new List<NutrientManure>();
            }
            else
            {
                if(fld.nutrients.nutrientManures == null)
                {
                    fld.nutrients.nutrientManures = new List<NutrientManure>();
                }
            }

            foreach (var f in fld.nutrients.nutrientManures)
            {
                nextId = nextId <= f.id ? f.id + 1 : nextId;
            }
            newMan.id = nextId;

            fld.nutrients.nutrientManures.Add(newMan);
            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }
    }
}