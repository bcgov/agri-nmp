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
            userData.unsaved = true;
            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }

        public FarmData FarmData()
        {
            FarmData farmData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            return farmData;
        }

        public void SaveFarmData(FarmData userData)
        {
            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
            return;
        }

        public FarmDetails FarmDetails()
        {
            FarmData farmData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            return farmData.farmDetails;
        }

        public void UpdateFarmDetails(FarmDetails fd)
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            userData.farmDetails.farmName = fd.farmName;
            userData.farmDetails.farmRegion = fd.farmRegion;
            userData.farmDetails.soilTests = fd.soilTests;
            userData.farmDetails.testingMethod = fd.testingMethod;
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
            int nextId = 1;
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);

            if(yd.fields == null)
            {
                yd.fields = new List<Field>();
            }
            foreach (var f in yd.fields)
            {
                nextId = nextId <= f.id ? f.id + 1 : nextId;
            }
            newFld.id = nextId;
            yd.fields.Add(newFld);
            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }

        public void UpdateField(Field updtFld)
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            Field fld = yd.fields.FirstOrDefault(f => f.id == updtFld.id);

            fld.fieldName = updtFld.fieldName;
            fld.area = updtFld.area;
            fld.comment = updtFld.comment;

            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }

        public void UpdateFieldSoilTest(Field updtFld)
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            Field fld = yd.fields.FirstOrDefault(f => f.fieldName == updtFld.fieldName);

            if (fld.soilTest == null)
                fld.soilTest = new SoilTest();

            if (updtFld.soilTest == null)
            {
                fld.soilTest = null;
            }
            else
            {
                fld.soilTest.sampleDate = updtFld.soilTest.sampleDate;
                fld.soilTest.ValP = updtFld.soilTest.ValP;
                fld.soilTest.valK = updtFld.soilTest.valK;
                fld.soilTest.valNO3H = updtFld.soilTest.valNO3H;
                fld.soilTest.valPH = updtFld.soilTest.valPH;
            }

            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }

        public void DeleteField(string name)
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
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

        public List<NutrientManure> GetFieldNutrientsManures(string fldName)
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            Field fld = yd.fields.FirstOrDefault(f => f.fieldName == fldName);

            if(fld == null)
            {
                fld = new Field();
            }

            if (fld.nutrients == null)
            {
                fld.nutrients = new Nutrients();
            }
            List<NutrientManure> fldManures = fld.nutrients.nutrientManures;
            if (fldManures == null)
            {
                fldManures = new List<NutrientManure>();
            }
            return fldManures;
        }
        public List<NutrientOther> GetFieldNutrientsOthers(string fldName)
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            Field fld = yd.fields.FirstOrDefault(f => f.fieldName == fldName);

            if (fld == null)
            {
                fld = new Field();
            }

            if (fld.nutrients == null)
            {
                fld.nutrients = new Nutrients();
            }
            List<NutrientOther> fldManures = fld.nutrients.nutrientOthers;
            if (fldManures == null)
            {
                fldManures = new List<NutrientOther>();
            }
            return fldManures;
        }

        public NutrientManure GetFieldNutrientsManure(string fldName, int manId)
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            Field fld = yd.fields.FirstOrDefault(f => f.fieldName == fldName);
            NutrientManure nm = fld.nutrients.nutrientManures.FirstOrDefault(m => m.id == manId);

            return nm;
        }

        public NutrientOther GetFieldNutrientsOther(string fldName, int otherId)
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            Field fld = yd.fields.FirstOrDefault(f => f.fieldName == fldName);
            NutrientOther no = fld.nutrients.nutrientOthers.FirstOrDefault(m => m.id == otherId);

            return no;
        }

        public void AddFieldNutrientsManure(string fldName, NutrientManure newMan)
        {
            int nextId = 1;

            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
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
        public void AddFieldNutrientsOther(string fldName, NutrientOther newOther)
        {
            int nextId = 1;

            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            Field fld = yd.fields.FirstOrDefault(f => f.fieldName == fldName);

            if (fld.nutrients == null)
            {
                fld.nutrients = new Nutrients();
                fld.nutrients.nutrientOthers = new List<NutrientOther>();
            }
            else
            {
                if (fld.nutrients.nutrientOthers == null)
                {
                    fld.nutrients.nutrientOthers = new List<NutrientOther>();
                }
            }

            foreach (var f in fld.nutrients.nutrientOthers)
            {
                nextId = nextId <= f.id ? f.id + 1 : nextId;
            }
            newOther.id = nextId;

            fld.nutrients.nutrientOthers.Add(newOther);
            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }

        public void UpdateFieldNutrientsManure(string fldName, NutrientManure updtMan)
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            Field fld = yd.fields.FirstOrDefault(f => f.fieldName == fldName);
            NutrientManure nm = fld.nutrients.nutrientManures.FirstOrDefault(m => m.id == updtMan.id);

            nm.applicationId = updtMan.applicationId;
            nm.ltK2o = updtMan.ltK2o;
            nm.ltN = updtMan.ltN;
            nm.ltP2o5 = updtMan.ltP2o5;
            nm.manureId = updtMan.manureId;
            nm.nAvail = updtMan.nAvail;
            nm.nh4Retention = updtMan.nh4Retention;
            nm.rate = updtMan.rate;
            nm.unitId = updtMan.unitId;
            nm.yrK2o = updtMan.yrK2o;
            nm.yrN = updtMan.yrN;
            nm.yrP2o5 = updtMan.yrP2o5;

            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }
        public void UpdateFieldNutrientsOther(string fldName, NutrientOther updtOther)
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            Field fld = yd.fields.FirstOrDefault(f => f.fieldName == fldName);
            NutrientOther no = fld.nutrients.nutrientOthers.FirstOrDefault(m => m.id == updtOther.id);

            no.description = updtOther.description;
            no.nitrogen = updtOther.nitrogen;
            no.phospherous = updtOther.phospherous;
            no.potassium = updtOther.potassium;

            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }

        public void DeleteFieldNutrientsManure(string fldName, int id)
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            Field fld = yd.fields.FirstOrDefault(f => f.fieldName == fldName);
            NutrientManure nm = fld.nutrients.nutrientManures.FirstOrDefault(m => m.id == id);

            fld.nutrients.nutrientManures.Remove(nm);

            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }
        public void DeleteFieldNutrientsOther(string fldName, int id)
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            Field fld = yd.fields.FirstOrDefault(f => f.fieldName == fldName);
            NutrientOther no = fld.nutrients.nutrientOthers.FirstOrDefault(m => m.id == id);

            fld.nutrients.nutrientOthers.Remove(no);

            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }

        public List<FieldCrop> GetFieldCrops(string fldName)
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            Field fld = yd.fields.FirstOrDefault(f => f.fieldName == fldName);

            if (fld == null)
            {
                fld = new Field();
            }

            if (fld == null)
            {
                fld.nutrients = new Nutrients();
            }
            List<FieldCrop> fldCrops = fld.crops;
            if (fldCrops == null)
            {
                fldCrops = new List<FieldCrop>();
            }
            return fldCrops;
        }

        public FieldCrop GetFieldCrop(string fldName, int cropId)
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            Field fld = yd.fields.FirstOrDefault(f => f.fieldName == fldName);
            FieldCrop crp = fld.crops.FirstOrDefault(m => m.id == cropId);

            return crp;
        }

        public void AddFieldCrop(string fldName, FieldCrop newCrop)
        {
            int nextId = 1;

            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            Field fld = yd.fields.FirstOrDefault(f => f.fieldName == fldName);

            if (fld.crops == null)
            {
                fld.crops = new List<FieldCrop>();
            }

            foreach (var f in fld.crops)
            {
                nextId = nextId <= f.id ? f.id + 1 : nextId;
            }
            newCrop.id = nextId;

            fld.crops.Add(newCrop);
            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }

        public void UpdateFieldCrop(string fldName, FieldCrop updtCrop)
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            Field fld = yd.fields.FirstOrDefault(f => f.fieldName == fldName);
            FieldCrop crp = fld.crops.FirstOrDefault(m => m.id == updtCrop.id);

            crp.cropId = updtCrop.cropId;
            crp.yield = updtCrop.yield;
            crp.reqK2o = updtCrop.reqK2o;
            crp.reqN = updtCrop.reqN;
            crp.reqP2o5 = updtCrop.reqP2o5;
            crp.remK2o = updtCrop.remK2o;
            crp.remN = updtCrop.remN;
            crp.remP2o5 = updtCrop.remP2o5;
            crp.crudeProtien = updtCrop.crudeProtien;
            crp.prevCropId = updtCrop.prevCropId;
            crp.cropOther = updtCrop.cropOther;
            crp.coverCropHarvested = updtCrop.coverCropHarvested;

            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }

        public void DeleteFieldCrop(string fldName, int id)
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            Field fld = yd.fields.FirstOrDefault(f => f.fieldName == fldName);
            FieldCrop crp = fld.crops.FirstOrDefault(m => m.id == id);

            fld.crops.Remove(crp);

            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }
    }
}