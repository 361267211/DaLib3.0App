using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.EntityFramework.Core.Entitys
{
    public class Region : IPrivateEntity
    {
        public int ID { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public int PId { get; set; }
        [StringLength(100)]
        public string SName { get; set; }
        public int Level { get; set; }
        [StringLength(100)]
        public string CityCode { get; set; }
        [StringLength(100)]
        public string YzCode { get; set; }
        [StringLength(100)]
        public string MerName { get; set; }
        public float Lng { get; set; }
        public float Lat { get; set; }
        [StringLength(100)]
        public string PinYin { get; set; }
    }
}
