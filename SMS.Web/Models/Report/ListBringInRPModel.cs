using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.Web.Models.Report
{
    public class ListBringInRPModel
    {
        public string Type { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Team { get; set; }
        public string Pos { get; set; }
        public string Reason { get; set; }

        public string CatID { get; set; }
        public string Item { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public string AssetType { get; set; }
        public string Return { get; set; }
        public string MN { get; set; }
        public string ITT { get; set; }
        public string GAT { get; set; }
        public string GuardIn { get; set; }
        public string GuardOut { get; set; }
        public string GuardReturn { get; set; }
        public string ReturnDate { get; set; }
        public string ReturnTime { get; set; }
        //public List<ListBringInDetailModel> BringInDetails { get; set; }
    }
    //public class ListBringInDetailModel
    //{
    //    public string CatID { get; set; }
    //    public string Item { get; set; }
    //    public int Quantity { get; set; }
    //    public string Unit { get; set; }
    //    public string AssetType { get; set; }
    //    public string Return { get; set; }
    //    public string MN { get; set; }
    //    public string ITT { get; set; }
    //    public string GAT { get; set; }
    //    public string GuardIn { get; set; }
    //    public string GuardOut { get; set; }
    //    public string GuardReturn { get; set; }
    //    public string ReturnDate { get; set; }
    //    public string ReturnTime { get; set; }
    //}
}