using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace MVC_EF_Start.Models
{

    public class DRegion
    {
        [Key]
        public int RegionID { get; set; }
        public string RegionName { get; set; }

        public virtual ICollection<DCounty> Counties { get; set; }
    }

    public class DCounty
    {
        [Key]
        public int CountyID { get; set; }
        public string CountyName { get; set; }

        public int RegionID { get; set; } // Foreign key
        public virtual DRegion Region { get; set; }

        public virtual ICollection<DVehicle> Vehicles { get; set; }
        public virtual ICollection<DMake> Makes { get; set; }
        public virtual ICollection<DModel> Models { get; set; }
    }

    public class DMake
    {
        [Key]
        public int MakeID { get; set; }
        public string MakeName { get; set; }

        public int CountyID { get; set; } // Foreign key
        public virtual DCounty County { get; set; }

        public virtual ICollection<DModel> Models { get; set; }
    }

    public class DModel
    {
        [Key]
        public int ModelID { get; set; }
        public string ModelName { get; set; }

        public int CountyID { get; set; } // Foreign key
        public virtual DCounty County { get; set; }

        public virtual ICollection<DVehicle> Vehicles { get; set; }
    }

    public class DVehicle
    {
        [Key]
        public string VIN { get; set; }
        public string Make { get; set; }
        public int MakeID { get; set; }
        public string Model { get; set; }
        public int ModelID { get; set; }
        public int Range { get; set; }

        public int CountyID { get; set; } // Foreign key
        public virtual DCounty County { get; set; }
    }


    public class ExcelDataViewModel
    {
        public string VIN { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int ElectricRange { get; set; }

        public override string ToString()
        {
            return $"VIN: {VIN}, County: {County}, State: {State}, Make: {Make}, Model: {Model}, ElectricRange: {ElectricRange}";
        }
    }

    public class ChartModel
    {
        public string ChartType { get; set; } = "horizontalBar"; // Default chart type

        public List<string> Labels { get; set; }
        public List<int> Data { get; set; }

        public List<string> Colors { get; set; }

        public string Title { get; set; }
    }

    // model for regional aggregation
    public class RegionViewModel
    {
        public string RegionName { get; set; }
        public int TotalCars { get; set; }
    }

    public class RegionData
    {
        public string RegionName { get; set; }
        public int TotalCars { get; set; }
    }


}
