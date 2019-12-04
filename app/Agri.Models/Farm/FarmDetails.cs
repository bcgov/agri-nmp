namespace Agri.Models.Farm
{
    public class FarmDetails
    {
        public FarmDetails()
        {
            HasAnimals = true;
            ImportsManureCompost = true;
        }

        public string Year { get; set; }
        public string FarmName { get; set; }
        public int? FarmRegion { get; set; }
        public int? FarmSubRegion { get; set; }
        public bool? SoilTests { get; set; }
        public string TestingMethod { get; set; }
        public bool? Manure { get; set; }
        public bool HasSelectedFarmType { get; set; }
        public bool HasAnimals { get; set; }
        public bool ImportsManureCompost { get; set; }
        public bool UsesFertilizer { get; set; }

        //TODO: Remove HasAnimals1 when Menu has been revised
        public bool HasAnimals1 { get; set; }

        public bool HasDairyCows { get; set; }
        public bool HasBeefCows { get; set; }
        public bool HasPoultry { get; set; }
        public bool HasMixedLiveStock { get; set; }

        public UserJourney UserJourney
        {
            get
            {
                var userJourney = UserJourney.Initial;
                if (HasSelectedFarmType)
                {
                    userJourney = UserJourney.Crops;
                    if (HasAnimals1)
                    {
                        if (HasMixedLiveStock)
                        {
                            return UserJourney.Mixed;
                        }
                        var typeCount = 0;
                        if (HasDairyCows)
                        {
                            typeCount += 1;
                            userJourney = UserJourney.Dairy;
                        }
                        if (HasBeefCows)
                        {
                            typeCount += 1;
                            userJourney = UserJourney.Ranch;
                        }
                        if (HasPoultry)
                        {
                            typeCount += 1;
                            userJourney = UserJourney.Poultry;
                        }
                        if (typeCount > 1)
                        {
                            userJourney = UserJourney.Mixed;
                        }
                    }
                }
                return userJourney;
            }
        }
    }
}