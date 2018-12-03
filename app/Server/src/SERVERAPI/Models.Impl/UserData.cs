using Microsoft.AspNetCore.Http;
using SERVERAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using Agri.Interfaces;
using Agri.Models.Farm;
using SERVERAPI.ViewModels;
using Agri.LegacyData.Models.Impl;
using Agri.Models.Configuration;

namespace SERVERAPI.Models.Impl
{
    public class UserData
    {
        private readonly IHttpContextAccessor _ctx;
        public IAgriConfigurationRepository _sd { get; set; }

        public UserData(IHttpContextAccessor ctx, IAgriConfigurationRepository sd)
        {
            _ctx = ctx;
            _sd = sd;
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
            FarmData farmData = null;
            try
            {
                farmData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            }
            catch (Exception ex)
            {

            }
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

            //change the year associated with the array
            YearData yd = userData.years.FirstOrDefault();
            if (yd == null)
            {
                YearData ny = new YearData();
                ny.year = fd.year;
                userData.years.Add(ny);
            }
            else
            {
                yd.year = fd.year;
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

            fld.prevYearManureApplicationFrequency = updtFld.prevYearManureApplicationFrequency;
            fld.prevYearManureApplicationNitrogenCredit = updtFld.prevYearManureApplicationNitrogenCredit;
            fld.SoilTestNitrateOverrideNitrogenCredit = updtFld.SoilTestNitrateOverrideNitrogenCredit;

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
                fld.soilTest.ConvertedKelownaK = updtFld.soilTest.ConvertedKelownaK;
                fld.soilTest.ConvertedKelownaP = updtFld.soilTest.ConvertedKelownaP;
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
        public List<NutrientFertilizer> GetFieldNutrientsFertilizers(string fldName)
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
            List<NutrientFertilizer> fldFertilizers = fld.nutrients.nutrientFertilizers;
            if (fldFertilizers == null)
            {
                fldFertilizers = new List<NutrientFertilizer>();
            }
            return fldFertilizers;
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

        public NutrientFertilizer GetFieldNutrientsFertilizer(string fldName, int fertId)
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            Field fld = yd.fields.FirstOrDefault(f => f.fieldName == fldName);
            NutrientFertilizer nf = fld.nutrients.nutrientFertilizers.FirstOrDefault(m => m.id == fertId);

            return nf;
        }

        public int AddFieldNutrientsManure(string fldName, NutrientManure newMan)
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

            return newMan.id;
        }

        public int AddFieldNutrientsFertilizer(string fldName, NutrientFertilizer newFert)
        {
            int nextId = 1;

            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            Field fld = yd.fields.FirstOrDefault(f => f.fieldName == fldName);

            if (fld.nutrients == null)
            {
                fld.nutrients = new Nutrients();
                fld.nutrients.nutrientFertilizers = new List<NutrientFertilizer>();
            }
            else
            {
                if (fld.nutrients.nutrientFertilizers == null)
                {
                    fld.nutrients.nutrientFertilizers = new List<NutrientFertilizer>();
                }
            }

            foreach (var f in fld.nutrients.nutrientFertilizers)
            {
                nextId = nextId <= f.id ? f.id + 1 : nextId;
            }
            newFert.id = nextId;

            fld.nutrients.nutrientFertilizers.Add(newFert);
            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);

            return newFert.id;
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
            no.yrN = updtOther.yrN;
            no.yrP2o5 = updtOther.yrP2o5;
            no.yrK = updtOther.yrK;
            no.ltN = updtOther.ltN;
            no.ltP2o5 = updtOther.ltP2o5;
            no.ltK = updtOther.ltK;

            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }
        public void UpdateFieldNutrientsFertilizer(string fldName, NutrientFertilizer updtFert)
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            Field fld = yd.fields.FirstOrDefault(f => f.fieldName == fldName);
            NutrientFertilizer nf = fld.nutrients.nutrientFertilizers.FirstOrDefault(m => m.id == updtFert.id);

            nf.applDate = updtFert.applDate;
            nf.applMethodId = updtFert.applMethodId;
            nf.applRate = updtFert.applRate;
            nf.applUnitId = updtFert.applUnitId;
            nf.fertilizerId = updtFert.fertilizerId;
            nf.fertilizerTypeId = updtFert.fertilizerTypeId;
            nf.fertK2o = updtFert.fertK2o;
            nf.fertN = updtFert.fertN;
            nf.fertP2o5 = updtFert.fertP2o5;
            nf.customN = updtFert.customN;
            nf.customP2o5 = updtFert.customP2o5;
            nf.customK2o = updtFert.customK2o;
            nf.liquidDensity = updtFert.liquidDensity;
            nf.liquidDensityUnitId = updtFert.liquidDensityUnitId;

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
        public void DeleteFieldNutrientsFertilizer(string fldName, int id)
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            Field fld = yd.fields.FirstOrDefault(f => f.fieldName == fldName);
            NutrientFertilizer nf = fld.nutrients.nutrientFertilizers.FirstOrDefault(m => m.id == id);

            fld.nutrients.nutrientFertilizers.Remove(nf);

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
            crp.stdN = updtCrop.stdN;
            crp.reqP2o5 = updtCrop.reqP2o5;
            crp.remK2o = updtCrop.remK2o;
            crp.remN = updtCrop.remN;
            crp.remP2o5 = updtCrop.remP2o5;
            crp.crudeProtien = updtCrop.crudeProtien;
            crp.prevCropId = updtCrop.prevCropId;
            crp.cropOther = updtCrop.cropOther;
            crp.coverCropHarvested = updtCrop.coverCropHarvested;
            // cannot be modified in the UI
            crp.prevYearManureAppl_volCatCd = _sd.GetCropPrevYearManureApplVolCatCd(Convert.ToInt32(crp.cropId));
            crp.yieldHarvestUnit = updtCrop.yieldHarvestUnit;
            crp.yieldByHarvestUnit = updtCrop.yieldByHarvestUnit;

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
            if(fld.crops.Count() == 0)
            {
                fld.crops = null;
            }

            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }
        public FarmManure GetFarmManure(int id)
        {
            FarmManure fm = new FarmManure();
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");

            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);

            if (yd.farmManures == null)
            {
                yd.farmManures = new List<FarmManure>();
            }

            fm = yd.farmManures.FirstOrDefault(c => c.id == id);
            if (!fm.customized)
            {
                Manure man = _sd.GetManure(fm.manureId.ToString());
                fm.ammonia = man.Ammonia;
                fm.dmid = man.DMId;
                fm.manure_class = man.ManureClass;
                fm.moisture = man.Moisture;
                fm.name = man.Name;
                fm.nitrate = man.Nitrate;
                fm.nitrogen = man.Nitrogen;
                fm.nminerizationid = man.NMineralizationId;
                fm.phosphorous = man.Phosphorous;
                fm.potassium = man.Potassium;
                fm.solid_liquid = man.SolidLiquid;
            }

            return fm;
        }

        public List<FarmManure> GetFarmManures()
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");

            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);

            if (yd.farmManures == null)
            {
                yd.farmManures = new List<FarmManure>();
            }
            foreach (var fm in yd.farmManures)
            {
                if (!fm.customized)
                {
                    Agri.Models.Configuration.Manure man = _sd.GetManure(fm.manureId.ToString());
                    fm.ammonia = man.Ammonia;
                    fm.dmid = man.DMId;
                    fm.manure_class = man.ManureClass;
                    fm.moisture = man.Moisture;
                    fm.name = man.Name;
                    fm.nitrate = man.Nitrate;
                    fm.nitrogen = man.Nitrogen;
                    fm.nminerizationid = man.NMineralizationId;
                    fm.phosphorous = man.Phosphorous;
                    fm.potassium = man.Potassium;
                }
            }

            return yd.farmManures;
        }

        public void AddFarmManure(FarmManure newManure)
        {
            int nextId = 1;

            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);

            if (yd.farmManures == null)
            {
                yd.farmManures = new List<FarmManure>();
            }

            foreach (var f in yd.farmManures)
            {
                nextId = nextId <= f.id ? f.id + 1 : nextId;
            }
            newManure.id = nextId;

            yd.farmManures.Add(newManure);
            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }

        public void UpdateFarmManure(FarmManure updtMan)
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            FarmManure frm = yd.farmManures.FirstOrDefault(f => f.id == updtMan.id);

            frm.ammonia = updtMan.ammonia;
            frm.customized = updtMan.customized;
            frm.dmid = updtMan.dmid;
            frm.manureId = updtMan.manureId;
            frm.manure_class = updtMan.manure_class;
            frm.moisture = updtMan.moisture;
            frm.name = updtMan.name;
            frm.nitrate = updtMan.nitrate;
            frm.nitrogen = updtMan.nitrogen;
            frm.nminerizationid = updtMan.nminerizationid;
            frm.phosphorous = updtMan.phosphorous;
            frm.potassium = updtMan.potassium;
            frm.solid_liquid = updtMan.solid_liquid;

            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }
        public void DeleteFarmManure(int id)
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            FarmManure fm = yd.farmManures.FirstOrDefault(f => f.id == id);

            yd.farmManures.Remove(fm);

            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }

        public List<SelectListItem> GetFarmManuresDll()
        {
            List<FarmManure> manures = GetFarmManures();

            manures.Sort(delegate (FarmManure f1, FarmManure f2) { return f1.customized.CompareTo(f2.customized); });
            manures.Reverse();

            List<SelectListItem> manOptions = new List<SelectListItem>();

            foreach (var r in manures)
            {
                SelectListItem li = new SelectListItem() { Id = r.id, Value = r.name };
                manOptions.Add(li);
            }

            return manOptions;
        }

        public List<GeneratedManure> GetGeneratedManures()
        {
            var userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");

            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);

            if (yd.GeneratedManures == null)
            {
                yd.GeneratedManures = new List<GeneratedManure>();
            }

            return yd?.GeneratedManures ??new List<GeneratedManure>();
        }

        public GeneratedManure GetGeneratedManure(int? generatedManureId)
        {
            return GetGeneratedManures().FirstOrDefault(gm => gm.id == generatedManureId);
        }

        public void AddGeneratedManure(GeneratedManure generatedManure)
        {
            var userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            var yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);

            if (yd.GeneratedManures == null || yd.GeneratedManures.Count == 0)
            {
                yd.GeneratedManures = new List<GeneratedManure>();
                generatedManure.id = 1;
            }
            else
            {
                generatedManure.id = yd.GeneratedManures.Select(m => m.id).Max() + 1;
            }

            yd.GeneratedManures.Add(generatedManure);
            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }


        public void UpdateGeneratedManures(GeneratedManure updatedGeneratedManure)
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            GeneratedManure farmDataGeneratedManure = yd.GeneratedManures.FirstOrDefault(f => f.id == updatedGeneratedManure.id);

            farmDataGeneratedManure.animalSubTypeId = updatedGeneratedManure.animalSubTypeId;
            farmDataGeneratedManure.averageAnimalNumber = updatedGeneratedManure.averageAnimalNumber;
            farmDataGeneratedManure.id = updatedGeneratedManure.id;
            farmDataGeneratedManure.manureType = updatedGeneratedManure.manureType;
            farmDataGeneratedManure.animalId = updatedGeneratedManure.animalId;
            farmDataGeneratedManure.animalId = updatedGeneratedManure.animalId;
            farmDataGeneratedManure.manureType = updatedGeneratedManure.manureType;
            farmDataGeneratedManure.manureTypeName = updatedGeneratedManure.manureTypeName;
            farmDataGeneratedManure.milkProduction = updatedGeneratedManure.milkProduction;
            farmDataGeneratedManure.animalSubTypeName = updatedGeneratedManure.animalSubTypeName;
            farmDataGeneratedManure.washWater = updatedGeneratedManure.washWater;
            farmDataGeneratedManure.washWaterGallons = updatedGeneratedManure.washWaterGallons;
            farmDataGeneratedManure.annualAmount = updatedGeneratedManure.annualAmount;
            farmDataGeneratedManure.AssignedToStoredSystem = updatedGeneratedManure.AssignedToStoredSystem;

            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);

            //Update the Materails saved in the Storage Systems
            var storageSystem = GetStorageSystems()
                                            .SingleOrDefault(s => s.MaterialsIncludedInSystem.Any(m => m.id == updatedGeneratedManure.id));
            if (storageSystem != null)
            {
                var oldMaterial =
                    storageSystem.MaterialsIncludedInSystem.Single(m => m.id == updatedGeneratedManure.id);
                storageSystem.MaterialsIncludedInSystem.Remove(oldMaterial);
                storageSystem.MaterialsIncludedInSystem.Add(updatedGeneratedManure);
                UpdateManureStorageSystem(storageSystem);
            }
            
        }

        public void DeleteGeneratedManure(int id)
        {
            var userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            var yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            var generatedManure = yd.GeneratedManures.FirstOrDefault(gm => gm.id == id);

            yd.GeneratedManures.Remove(generatedManure);

            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);

            //Update the Materails saved in the Storage Systems
            var storageSystem = GetStorageSystems()
                .SingleOrDefault(s => s.MaterialsIncludedInSystem.Any(m => m.id == generatedManure.id));
            if (storageSystem != null)
            {
                var oldMaterial =
                    storageSystem.MaterialsIncludedInSystem.Single(m => m.id == generatedManure.id);
                storageSystem.MaterialsIncludedInSystem.Remove(oldMaterial);
                UpdateManureStorageSystem(storageSystem);
            }
        }

        public void UpdateGenerateManuresAllocationToStorage()
        {
            var currentGeneratedManures = GetGeneratedManures();
            var currentStorages = GetStorageSystems();
            foreach (var generatedManure in currentGeneratedManures)
            {
                generatedManure.AssignedToStoredSystem = currentStorages.Any(s =>
                    s.MaterialsIncludedInSystem.Any(mis => mis.id == generatedManure.id));

                UpdateGeneratedManures(generatedManure);
            }
        }

        public List<ManureStorageSystem> GetStorageSystems()
        {
            var userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            var yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            return yd?.ManureStorageSystems ?? new List<ManureStorageSystem>();
        }

        public ManureStorageSystem GetStorageSystem(int id)
        {
            return GetStorageSystems().SingleOrDefault(s => s.Id == id);
        }

        public void AddManureStorageSystem(ManureStorageSystem storageSystem)
        {
            var userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            var yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);

            if (yd.ManureStorageSystems == null)
            {
                yd.ManureStorageSystems = new List<ManureStorageSystem>();
                storageSystem.Id = 1;
            }
            else
            {
                storageSystem.Id = yd.ManureStorageSystems.Select(m => m.Id).Max() + 1;
            }

            storageSystem.ManureStorageStructures.First().Id = 1;

            yd.ManureStorageSystems.Add(storageSystem);
            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }

        public void UpdateManureStorageSystem(ManureStorageSystem updatedSystem)
        {
            var userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            var yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);

            var savedSystem = yd.ManureStorageSystems.Single(ss => ss.Id == updatedSystem.Id);
            savedSystem.ManureMaterialType = updatedSystem.ManureMaterialType;
            savedSystem.MaterialsIncludedInSystem = updatedSystem.MaterialsIncludedInSystem;
            savedSystem.Name = updatedSystem.Name;
            savedSystem.GetsRunoffFromRoofsOrYards = updatedSystem.GetsRunoffFromRoofsOrYards;
            savedSystem.RunoffAreaSquareFeet = updatedSystem.RunoffAreaSquareFeet;

            savedSystem.ManureStorageStructures.RemoveAll(s => !updatedSystem.ManureStorageStructures.Any(u => u.Id == s.Id));
            foreach (var updateStorageStructure in updatedSystem.ManureStorageStructures)
            {
                savedSystem.AddUpdateManureStorageStructure(updateStorageStructure);
            }

            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }

        public void DeleteManureStorageSystem(int id)
        {
            var userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            var yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            var storageSystem = yd.ManureStorageSystems.FirstOrDefault(mss => mss.Id == id);

            yd.ManureStorageSystems.Remove(storageSystem);

            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }


        public List<ImportedManure> GetImportedManures()
        {
            var userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            var yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            return yd.ImportedManures?.ToList() ?? new List<ImportedManure>();
        }

        public object GetImportedManure(int id)
        {
            return GetImportedManures().SingleOrDefault(im => im.id == id);
        }
    }
}