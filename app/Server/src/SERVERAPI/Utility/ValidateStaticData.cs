using Agri.Interfaces;
using System.Collections.Generic;

namespace SERVERAPI.Utility
{
    public class ValidateStaticData
    {        
        private IAgriConfigurationRepository _sd;

        public ValidateStaticData(IAgriConfigurationRepository sd)
        {            
            _sd = sd;
        }

        public List<Agri.Models.Configuration.StaticDataValidationMessages> PerformValidation()
        {
            Agri.Models.Configuration.StaticDataValidationMessages sdvm = new Agri.Models.Configuration.StaticDataValidationMessages();
            List<Agri.Models.Configuration.StaticDataValidationMessages> retMessages = new List<Agri.Models.Configuration.StaticDataValidationMessages>();
            List<Agri.Models.Configuration.StaticDataValidationMessages> messages;
            // each region should have a location            
            messages = _sd.ValidateRelationship("['agri']['nmp']['regions']['region']", "locationid", "['agri']['nmp']['locations']['location']", "id");
            retMessages.AddRange(messages);
            messages = _sd.ValidateRelationship("['agri']['nmp']['manures']['manure']", "dmid", "['agri']['nmp']['dms']['dm']", "ID");
            retMessages.AddRange(messages);
            messages = _sd.ValidateRelationship("['agri']['nmp']['regions']['region']", "soil_test_phospherous_region_cd", "['agri']['nmp']['crop_stp_regioncds']['crop_stp_regioncd']", "soil_test_phosphorous_region_cd");
            retMessages.AddRange(messages);
            messages = _sd.ValidateRelationship("['agri']['nmp']['regions']['region']", "soil_test_potassium_region_cd", "['agri']['nmp']['crop_stk_regioncds']['crop_stk_regioncd']", "soil_test_potassium_region_cd");
            retMessages.AddRange(messages);
            messages = _sd.ValidateRelationship("['agri']['nmp']['manures']['manure']", "nminerizationid", "['agri']['nmp']['nmineralizations']['nmineralization']", "id");
            retMessages.AddRange(messages);
            messages = _sd.ValidateRelationship("['agri']['nmp']['crops']['crop']", "croptypeid", "['agri']['nmp']['croptypes']['croptype']", "id");
            retMessages.AddRange(messages);
            messages = _sd.ValidateRelationship("['agri']['nmp']['crops']['crop']", "prevcropcd", "['agri']['nmp']['prevcroptypes']['prevcroptype']", "id");
            retMessages.AddRange(messages);
            messages = _sd.ValidateRelationship("['agri']['nmp']['crops']['crop']", "n_recommcd", "['agri']['nmp']['n_recommcds']['n_recommcd']", "id");
            retMessages.AddRange(messages);
            messages = _sd.ValidateRelationship("['agri']['nmp']['crops']['crop']", "yieldcd", "['agri']['nmp']['yields']['yield']", "id");
            retMessages.AddRange(messages);
            messages = _sd.ValidateRelationship("['agri']['nmp']['fertilizers']['fertilizer']", "dry_liquid", "['agri']['nmp']['fertilizertypes']['fertilizertype']", "dry_liquid");
            retMessages.AddRange(messages);
            messages = _sd.ValidateRelationship("['agri']['nmp']['crop_stp_regioncds']['crop_stp_regioncd']", "cropid", "['agri']['nmp']['crops']['crop']", "id");
            retMessages.AddRange(messages);
            messages = _sd.ValidateRelationship("['agri']['nmp']['crop_stp_regioncds']['crop_stp_regioncd']", "soil_test_phosphorous_region_cd", "['agri']['nmp']['stp_recommends']['stp_recommend']", "soil_test_phosphorous_region_cd");
            retMessages.AddRange(messages);
            messages = _sd.ValidateRelationship("['agri']['nmp']['crop_stp_regioncds']['crop_stp_regioncd']", "phosphorous_crop_group_region_cd", "['agri']['nmp']['stp_recommends']['stp_recommend']", "phosphorous_crop_group_region_cd");
            retMessages.AddRange(messages);
            messages = _sd.ValidateRelationship("['agri']['nmp']['ammoniaretentions']['ammoniaretention']", "seasonapplicatonid", "['agri']['nmp']['season-applications']['season-application']", "id");
            retMessages.AddRange(messages);
            messages = _sd.ValidateRelationship("['agri']['nmp']['ammoniaretentions']['ammoniaretention']", "dm", "['agri']['nmp']['dms']['dm']", "ID");
            retMessages.AddRange(messages);
            messages = _sd.ValidateRelationship("['agri']['nmp']['nmineralizations']['nmineralization']", "locationid", "['agri']['nmp']['locations']['location']", "id");
            retMessages.AddRange(messages);
            return retMessages;
        }

        

    }
}
