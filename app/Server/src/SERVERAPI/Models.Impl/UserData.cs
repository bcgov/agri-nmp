using Agri.Interfaces;
using Agri.Models.Calculate;
using Agri.Models.Configuration;
using Agri.Models.Farm;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using SERVERAPI.Controllers;
using SERVERAPI.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using Agri.Models;
using Microsoft.Extensions.Logging;

namespace SERVERAPI.Models.Impl
{
    public class UserData
    {
        private readonly ILogger<UserData> _logger;
        private readonly IHttpContextAccessor _ctx;
        public IAgriConfigurationRepository _sd;
        private ISoilTestConverter _soilTestConversions;
        private IMapper _mapper;

        public UserData(ILogger<UserData> logger,
            IHttpContextAccessor ctx, 
            IAgriConfigurationRepository sd,
            ISoilTestConverter soilTestConversions,
            IMapper mapper)
        {
            _logger = logger;
            _ctx = ctx;
            _sd = sd;
            _soilTestConversions = soilTestConversions;
            _mapper = mapper;
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
                _logger.LogError(ex, "FarmData Exception");
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
            userData.farmDetails.farmSubRegion = fd.farmSubRegion;
            userData.farmDetails.soilTests = fd.soilTests;
            userData.farmDetails.testingMethod = fd.testingMethod;
            userData.farmDetails.manure = fd.manure;
            userData.farmDetails.year = fd.year;
            userData.farmDetails.HasAnimals = fd.HasAnimals;
            userData.farmDetails.ImportsManureCompost = fd.ImportsManureCompost;
            userData.farmDetails.UsesFertilizer = fd.UsesFertilizer;

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

        public void UpdateSTPSTK(List<Field> fields)
        {
            if (fields.Count > 0)
            {
                foreach (Field field in fields)
                {
                    if (field.soilTest != null)
                    {
                        field.soilTest.ConvertedKelownaP = _soilTestConversions.GetConvertedSTP(FarmDetails()?.testingMethod, field.soilTest);
                        field.soilTest.ConvertedKelownaK = _soilTestConversions.GetConvertedSTK(FarmDetails()?.testingMethod, field.soilTest);
                        UpdateFieldSoilTest(field);
                    }
                }
            }

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
            userData.LastAppliedFarmManureId = newMan.manureId;
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

            userData.LastAppliedFarmManureId = updtMan.manureId;

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
            if (fm != null && !fm.customized)
            {
                Manure man = _sd.GetManure(fm.manureId.ToString());
                fm.ammonia = man.Ammonia;
                fm.dmid = man.DryMatterId;
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

        public FarmManure GetFarmManureByImportedManure(ImportedManure importedManure)
        {
            FarmManure fm = new FarmManure();
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");

            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);

            if (yd.farmManures == null)
            {
                yd.farmManures = new List<FarmManure>();
            }

            fm = yd.farmManures.FirstOrDefault(c => c.sourceOfMaterialImportedManureId == importedManure.Id);
            if (!fm.customized)
            {
                Manure man = _sd.GetManure(fm.manureId.ToString());
                fm.ammonia = man.Ammonia;
                fm.dmid = man.DryMatterId;
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
                    Manure man = _sd.GetManure(fm.manureId.ToString());
                    fm.ammonia = man.Ammonia;
                    fm.dmid = man.DryMatterId;
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
            frm.sourceOfMaterialId = updtMan.sourceOfMaterialId;
            frm.manure_class = updtMan.manure_class;
            frm.moisture = updtMan.moisture;
            frm.name = updtMan.name;
            frm.sourceOfMaterialName = updtMan.sourceOfMaterialName;
            frm.nitrate = updtMan.nitrate;
            frm.nitrogen = updtMan.nitrogen;
            frm.nminerizationid = updtMan.nminerizationid;
            frm.phosphorous = updtMan.phosphorous;
            frm.potassium = updtMan.potassium;
            frm.solid_liquid = updtMan.solid_liquid;
            frm.stored_imported = updtMan.stored_imported;
            frm.IsAssignedToStorage = updtMan.IsAssignedToStorage;


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
                var name = manures.Count(m => m.manureId == r.manureId) == 1
                    ? r.name
                    : $"{r.sourceOfMaterialName}: {r.name}";
                if (r.stored_imported == NutrientAnalysisTypes.Imported)
                {
                    name = $"{r.sourceOfMaterialName}";
                }

                SelectListItem li = new SelectListItem() { Id = r.id, Value = name };
                manOptions.Add(li);
            }

            return manOptions;
        }

        public void ReCalculateManure(int farmManureId)
        {
            var calculateNutrients = new CalculateNutrients(this, _sd);
            var nOrganicMineralizations = new NOrganicMineralizations();

            List<Field> flds = GetFields();

            foreach (var fld in flds)
            {
                List<NutrientManure> mans = GetFieldNutrientsManures(fld.fieldName);

                foreach (var nm in mans)
                {
                    if (farmManureId.ToString() == nm.manureId)
                    {
                        int regionid = FarmDetails().farmRegion.Value;
                        Region region = _sd.GetRegion(regionid);
                        nOrganicMineralizations = calculateNutrients.GetNMineralization(Convert.ToInt16(nm.manureId), region.LocationId);

                        string avail = (nOrganicMineralizations.OrganicN_FirstYear * 100).ToString("###");

                        string nh4 = (calculateNutrients.GetAmmoniaRetention(Convert.ToInt16(nm.manureId), Convert.ToInt16(nm.applicationId)) * 100).ToString("###");

                        NutrientInputs nutrientInputs = new NutrientInputs();

                        calculateNutrients.manure = nm.manureId;
                        calculateNutrients.applicationSeason = nm.applicationId;
                        calculateNutrients.applicationRate = Convert.ToDecimal(nm.rate);
                        calculateNutrients.applicationRateUnits = nm.unitId;
                        calculateNutrients.ammoniaNRetentionPct = Convert.ToDecimal(nh4);
                        calculateNutrients.firstYearOrganicNAvailablityPct = Convert.ToDecimal(avail);

                        calculateNutrients.GetNutrientInputs(nutrientInputs);

                        nm.yrN = nutrientInputs.N_FirstYear;
                        nm.yrP2o5 = nutrientInputs.P2O5_FirstYear;
                        nm.yrK2o = nutrientInputs.K2O_FirstYear;
                        nm.ltN = nutrientInputs.N_LongTerm;
                        nm.ltP2o5 = nutrientInputs.P2O5_LongTerm;
                        nm.ltK2o = nutrientInputs.K2O_LongTerm;

                        UpdateFieldNutrientsManure(fld.fieldName, nm);
                    }
                }
            }
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
            return GetGeneratedManures().FirstOrDefault(gm => gm.Id == generatedManureId);
        }

        public void AddGeneratedManure(GeneratedManure generatedManure)
        {
            var userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            var yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);

            if (yd.GeneratedManures == null || yd.GeneratedManures.Count == 0)
            {
                yd.GeneratedManures = new List<GeneratedManure>();
                generatedManure.Id = 1;
            }
            else
            {
                generatedManure.Id = yd.GeneratedManures.Select(m => m.Id).Max() + 1;
            }

            yd.GeneratedManures.Add(generatedManure);
            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);

            UpdateFarmHasAnimalStatus();
        }


        public void UpdateGeneratedManures(GeneratedManure updatedGeneratedManure)
        {
            FarmData userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            YearData yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            GeneratedManure farmDataGeneratedManure = yd.GeneratedManures.FirstOrDefault(f => f.Id == updatedGeneratedManure.Id);

            farmDataGeneratedManure.animalSubTypeId = updatedGeneratedManure.animalSubTypeId;
            farmDataGeneratedManure.averageAnimalNumber = updatedGeneratedManure.averageAnimalNumber;
            farmDataGeneratedManure.Id = updatedGeneratedManure.Id;
            farmDataGeneratedManure.ManureType = updatedGeneratedManure.ManureType;
            farmDataGeneratedManure.animalId = updatedGeneratedManure.animalId;
            farmDataGeneratedManure.animalId = updatedGeneratedManure.animalId;
            farmDataGeneratedManure.ManureType = updatedGeneratedManure.ManureType;
            farmDataGeneratedManure.manureTypeName = updatedGeneratedManure.manureTypeName;
            farmDataGeneratedManure.milkProduction = updatedGeneratedManure.milkProduction;
            farmDataGeneratedManure.animalSubTypeName = updatedGeneratedManure.animalSubTypeName;
            farmDataGeneratedManure.washWater = updatedGeneratedManure.washWater;
            farmDataGeneratedManure.washWaterUnits = updatedGeneratedManure.washWaterUnits;
            farmDataGeneratedManure.annualAmount = updatedGeneratedManure.annualAmount;
            farmDataGeneratedManure.AssignedToStoredSystem = updatedGeneratedManure.AssignedToStoredSystem;
            farmDataGeneratedManure.solidPerGalPerAnimalPerDay = updatedGeneratedManure.solidPerGalPerAnimalPerDay;
            farmDataGeneratedManure.BreedId = updatedGeneratedManure.BreedId;
            farmDataGeneratedManure.BreedName = updatedGeneratedManure.BreedName;
            farmDataGeneratedManure.showBreedAndGrazingDaysPerYear =
                updatedGeneratedManure.showBreedAndGrazingDaysPerYear;
            farmDataGeneratedManure.grazingDaysPerYear = updatedGeneratedManure.grazingDaysPerYear;

            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);

            //Update the Materials saved in the Storage Systems
            var storageSystem = GetStorageSystems()
                                            .SingleOrDefault(s => s.MaterialsIncludedInSystem.Any(m => m.ManureId == updatedGeneratedManure.ManureId));
            if (storageSystem != null)
            {
                var oldMaterial =
                    storageSystem.MaterialsIncludedInSystem.Single(m => m.ManureId == updatedGeneratedManure.ManureId);
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
            var generatedManure = yd.GeneratedManures.FirstOrDefault(gm => gm.Id == id);
            
            yd.GeneratedManures.Remove(generatedManure);

            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);

            //Update the Materails saved in the Storage Systems
            var storageSystem = GetStorageSystems()
                .SingleOrDefault(s => s.MaterialsIncludedInSystem.Any(m => m.ManureId == generatedManure.ManureId));
            if (storageSystem != null)
            {
                var droppedGeneratedMaterial =
                    storageSystem.GeneratedManuresIncludedInSystem.Single(m => m.ManureId == generatedManure.ManureId);
                storageSystem.GeneratedManuresIncludedInSystem.Remove(droppedGeneratedMaterial);
                UpdateManureStorageSystem(storageSystem);
            }

            UpdateFarmHasAnimalStatus();
        }

        public void UpdateManagedManuresAllocationToStorage()
        {
            var currentManures = GetAllManagedManures();
            var currentStorages = GetStorageSystems();

            foreach (var manure in currentManures)
            {
                manure.AssignedToStoredSystem = currentStorages.Any(s =>
                    s.MaterialsIncludedInSystem.Any(mis => mis.ManureId == manure.ManureId));

                if (manure is GeneratedManure)
                {
                    UpdateGeneratedManures(manure as GeneratedManure);
                }
                else if(manure is ImportedManure)
                {
                    UpdateImportedManure(manure as ImportedManure);
                }
                else
                {
                    UpdateSeparatedSolidManure(manure as SeparatedSolidManure);
                }
            }
        }

        public void UpdateManagedImportedManuresAllocationToNutrientAnalysis()
        {
            var currentManures = GetAllManagedManures();
            var currentFarmManures = GetFarmManures();

            foreach (var manure in currentManures)
            {
                manure.AssignedWithNutrientAnalysis = currentFarmManures.Any(fm =>
                    (fm.sourceOfMaterialId.Split(',')[0] + fm.sourceOfMaterialId.Split(',')[1]) == manure.ManureId);

                if (manure is ImportedManure)
                {
                    UpdateImportedManure(manure as ImportedManure);
                }
            }
        }

        public void UpdateStorageSystemsAllocationToNutrientAnalysis()
        {
            var currentManureStorageSystems = GetStorageSystems();
            var currentFarmManures = GetFarmManures();

            foreach (var manureStorageSystem in currentManureStorageSystems)
            {
                manureStorageSystem.AssignedWithNutrientAnalysis = currentFarmManures.Any(fm => fm.sourceOfMaterialId.Split(',')[1] == manureStorageSystem.Id.ToString());

                if (manureStorageSystem is ManureStorageSystem)
                {
                    UpdateManureStorageSystem(manureStorageSystem as ManureStorageSystem);
                }
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

            if (yd.ManureStorageSystems == null || yd.ManureStorageSystems?.Count == 0)
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

            ProcessSeparatedManureForStorageSystem(storageSystem, false);
        }

        public void UpdateManureStorageSystem(ManureStorageSystem updatedSystem)
        {
            var userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            var yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);

            var savedSystem = yd.ManureStorageSystems.Single(ss => ss.Id == updatedSystem.Id);
            _mapper.Map(updatedSystem, savedSystem);

            savedSystem.ManureStorageStructures.RemoveAll(s => !updatedSystem.ManureStorageStructures.Any(u => u.Id == s.Id));
            foreach (var updateStorageStructure in updatedSystem.ManureStorageStructures)
            {
                savedSystem.AddUpdateManureStorageStructure(updateStorageStructure);
            }

            // Remove the NutrientAnalsis if the StorageSystem has no more materials.
            var deleteStorageForSeparatedManure = false;
            if (!updatedSystem.MaterialsIncludedInSystem.Any())
            {
                RemoveFarmManuresRelatedToManureStorageSystem(yd, updatedSystem);
                deleteStorageForSeparatedManure = true;
            }
            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
            ProcessSeparatedManureForStorageSystem(updatedSystem, deleteStorageForSeparatedManure);
        }

        public void DeleteManureStorageSystem(int id)
        {
            var userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            var yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            var storageSystem = yd.ManureStorageSystems.FirstOrDefault(mss => mss.Id == id);
            yd.ManureStorageSystems.Remove(storageSystem);

            RemoveFarmManuresRelatedToManureStorageSystem(yd, storageSystem);

            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);

            ProcessSeparatedManureForStorageSystem(storageSystem, true);
        }

        public void UpdateSeparatedSolidManure(SeparatedSolidManure separatedSolidManure)
        {
            var userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            var yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);

            var savedSeparatedSolidManure =
                yd.SeparatedSolidManures.SingleOrDefault(s => s.Id == separatedSolidManure.Id);

            _mapper.Map(separatedSolidManure, savedSeparatedSolidManure);

            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }

        private void ProcessSeparatedManureForStorageSystem(ManureStorageSystem sourceManureStorageSystem,
            bool sourceStorageDeleted)
        {
            var userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            var yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            var separatedSolidManureToDrop = yd.SeparatedSolidManures?.SingleOrDefault(s =>
                s.SeparationSourceStorageSystemId == sourceManureStorageSystem.Id);

            if (sourceManureStorageSystem.IsThereSolidLiquidSeparation && !sourceStorageDeleted)
            {
                //Check if the Separated Manure was added to the Year Data
                if (separatedSolidManureToDrop == null)
                {
                    //Create the Separated Manure and attach the Source System
                    var newId = yd.SeparatedSolidManures.Any() ? yd.SeparatedSolidManures.Max(ssm => ssm.Id) + 1 : 1;
                    var nameIndex = newId == 1 ? string.Empty : $" {newId}";
                    var separatedSolidManure = new SeparatedSolidManure
                    {
                        Id = newId,
                        AnnualAmountTonsWeight = sourceManureStorageSystem.SeparatedSolidsTons,
                        SeparationSourceStorageSystemId = sourceManureStorageSystem.Id,
                        Name = $"Separated solids{nameIndex}"
                    };

                    yd.SeparatedSolidManures.Add(separatedSolidManure);
                    _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
                }
            }
            else
            {
                //Check if a Separted Solid was created previous for System in case IsThereSolidLiquidSeparation was
                //changed to false and thus needs to be dropped
                if (separatedSolidManureToDrop != null)
                {
                    //If Stored remove it from Storage
                    var currentStorageOfSeparatedSolid = yd.ManureStorageSystems
                        .SingleOrDefault(mss =>
                            mss.SeparatedSolidManuresIncludedInSystem.Any(ssm =>
                                ssm.ManureId == separatedSolidManureToDrop.ManureId));

                    yd.SeparatedSolidManures.Remove(separatedSolidManureToDrop);
                    _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);

                    if (currentStorageOfSeparatedSolid != null)
                    {
                        var storedSeparatedSolid =
                            currentStorageOfSeparatedSolid.SeparatedSolidManuresIncludedInSystem.Single(m =>
                                m.ManureId == separatedSolidManureToDrop.ManureId);
                        currentStorageOfSeparatedSolid.SeparatedSolidManuresIncludedInSystem.Remove(storedSeparatedSolid);
                        UpdateManureStorageSystem(currentStorageOfSeparatedSolid);
                    }
                }
            }
        }

        private void RemoveFarmManuresRelatedToManureStorageSystem(YearData yd, ManureStorageSystem storageSystem)
        {
            var farmManuresToDrop = yd.farmManures?.Where(im =>
                im.sourceOfMaterialStoredSystemId.HasValue &&
                im.sourceOfMaterialStoredSystemId == storageSystem.Id).ToList();

            if (farmManuresToDrop != null && farmManuresToDrop.Any())
            {
                var nutrientManures =
                    yd.GetNutrientManuresFromFields(farmManuresToDrop.Select(fm => fm.id).ToList());

                foreach (var nutrientManure in nutrientManures)
                {
                    yd.fields
                        .Single(f => f.nutrients.nutrientManures.Any(nm =>
                            nm.id == nutrientManure.id && nm.manureId == nutrientManure.manureId))
                        .nutrients.nutrientManures.Remove(nutrientManure);
                }

                foreach (var farmManureId in farmManuresToDrop.Select(fm => fm.id).ToList())
                {
                    //Drop the farm Manure
                    var farmManure = yd.farmManures.Single(fm => fm.id == farmManureId);
                    yd.farmManures.Remove(farmManure);
                }
            }
        }

        public List<SeparatedSolidManure> GetSeparatedManures()
        {
            var userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            var yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            return yd.SeparatedSolidManures ?? new List<SeparatedSolidManure>();
        }

        public SeparatedSolidManure GetSeparatedManure(int id)
        {
            return GetSeparatedManures().SingleOrDefault(sm => sm.Id == id);
        }

        public List<ImportedManure> GetImportedManures()
        {
            var userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            var yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            return yd.ImportedManures?.ToList() ?? new List<ImportedManure>();
        }

        public ImportedManure GetImportedManure(int id)
        {
            return GetImportedManures().SingleOrDefault(im => im.Id == id);
        }
        public ImportedManure GetImportedManureByManureId(string manureId)
        {
            return GetImportedManures().SingleOrDefault(im => im.ManureId == manureId);
        }

        public void AddImportedManure(ImportedManure newManure)
        {
            var userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            var yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            if (yd.ImportedManures == null || yd.ImportedManures.Count == 0)
            {
                yd.ImportedManures = new List<ImportedManure>();
                newManure.Id = 1;
            }
            else
            {
                newManure.Id = yd.ImportedManures.Max(im => im.Id) + 1;
            }
            yd.ImportedManures.Add(newManure);

            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);

            UpdateFarmImportsManureStatus();
        }

        public void UpdateImportedManure(ImportedManure updatedManure)
        {
            var userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            var yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            var savedManure = yd.ImportedManures.Single(im => im.Id == updatedManure.Id);
            _mapper.Map(updatedManure, savedManure);

            //Update the Materials saved in the Storage Systems
            var storageSystem = GetStorageSystems()
                .SingleOrDefault(s => s.ImportedManuresIncludedInSystem.Any(m => m.ManureId == updatedManure.ManureId));
            if (storageSystem != null)
            {
                var oldMaterial =
                    storageSystem.ImportedManuresIncludedInSystem.Single(m => m.ManureId == updatedManure.ManureId);
                storageSystem.ImportedManuresIncludedInSystem.Remove(oldMaterial);
                storageSystem.ImportedManuresIncludedInSystem.Add(updatedManure);
                UpdateManureStorageSystem(storageSystem);
            }

            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }

        public void DeleteImportedManure(int importedManureId)
        {
            var userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            var yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            var importedManure = yd.ImportedManures.Single(im => im.Id == importedManureId);

            yd.ImportedManures.Remove(importedManure);


            //Update the Materials saved in the Storage Systems
            if (importedManure.IsMaterialStored)
            {
                _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
                var storageSystem = GetStorageSystems()
                    .SingleOrDefault(s => s.MaterialsIncludedInSystem.Any(m => m.ManureId == importedManure.ManureId));
                if (storageSystem != null)
                {
                    var oldMaterial =
                        storageSystem.ImportedManuresIncludedInSystem.Single(m =>
                            m.ManureId == importedManure.ManureId);
                    storageSystem.ImportedManuresIncludedInSystem.Remove(oldMaterial);
                    UpdateManureStorageSystem(storageSystem);
                }
            }
            else
            {
                var farmManuresToDrop = yd.farmManures?.Where(im =>
                    im.sourceOfMaterialImportedManureId.HasValue &&
                    im.sourceOfMaterialImportedManureId == importedManureId).ToList();

                if (farmManuresToDrop != null && farmManuresToDrop.Any())
                {
                    var nutrientManures =
                        yd.GetNutrientManuresFromFields(farmManuresToDrop.Select(fm => fm.id).ToList());

                    foreach (var nutrientManure in nutrientManures)
                    {
                        yd.fields
                            .Single(f => f.nutrients.nutrientManures.Any(nm =>
                                nm.id == nutrientManure.id && nm.manureId == nutrientManure.manureId))
                            .nutrients.nutrientManures.Remove(nutrientManure);
                    }

                    foreach (var farmManureId in farmManuresToDrop.Select(fm => fm.id).ToList())
                    {
                        //Drop the farm Manure
                        var farmManure = yd.farmManures.Single(fm => fm.id == farmManureId);
                        yd.farmManures.Remove(farmManure);
                    }
                }
                _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
            }

            UpdateFarmImportsManureStatus();
        }

        private void UpdateFarmHasAnimalStatus()
        {
            var hasAnimals = GetGeneratedManures().Any();
            var importsManure = GetImportedManures().Any();

            var userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            userData.farmDetails.HasAnimals = hasAnimals;

            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }

        private void UpdateFarmImportsManureStatus()
        {
            var importsManure = GetImportedManures().Any();

            var userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            userData.farmDetails.ImportsManureCompost = importsManure;

            _ctx.HttpContext.Session.SetObjectAsJson("FarmData", userData);
        }

        public List<ManagedManure> GetAllManagedManures()
        {
            var userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            userData.unsaved = true;
            var yd = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            var generated = yd.GeneratedManures?.ToList<ManagedManure>() ?? new List<ManagedManure>();
            var imported = yd.ImportedManures?.ToList<ManagedManure>() ?? new List<ManagedManure>();
            var separatedSolids = yd.SeparatedSolidManures?.ToList<ManagedManure>() ?? new List<ManagedManure>();
            
            var manures = new List<ManagedManure>();
            manures.AddRange(generated);
            manures.AddRange(imported);
            manures.AddRange(separatedSolids);
            return manures;
        }

        public YearData GetYearData()
        {
            var userData = _ctx.HttpContext.Session.GetObjectFromJson<FarmData>("FarmData");
            var yearData = userData.years.FirstOrDefault(y => y.year == userData.farmDetails.year);
            return yearData;
        }

        public ManagedManure GetManagedManure(string managedManureId)
        {
            var result = GetManagedManures(new List<string> {managedManureId}).SingleOrDefault();

            return result;
        }

        public List<ManagedManure> GetManagedManures(List<string> managedManureIds)
        {
            var managedManures = GetAllManagedManures().Where(gm => managedManureIds.Any(id => id == gm.ManureId)).ToList();

            return managedManures;
        }

    }
}
