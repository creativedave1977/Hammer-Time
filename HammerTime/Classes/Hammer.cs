using HammerTime.Classes.Data;
using System;
using System.Data;

namespace HammerTime.Classes
{
    public class Hammer
    {
        #region "assets"
        //fields
        private HammerData data;

        #endregion

        #region "public methods"
        //constructor
        public Hammer()
        {
            data = new HammerData();
        }

        public int Add(string HammerName, string HammerDesc, int Qty)
        {
            return data.AddHammer(HammerName, HammerDesc, Qty);
        }

        public bool AdjustHammerQty(int HammerID, int adjQty,  App.AdjustmentType adjType)
        {
            //set adjustment amount according to type of adjustment
            adjQty = adjType == App.AdjustmentType.Increment ? adjQty : 0 - adjQty;

            //return result flag from adjustment
            return data.UpdateHammerQty(HammerID, adjQty);
        }

        public bool DeleteHammer(int HammerID)
        {
            return data.DeleteHammer(HammerID);
        }

        public static DataTable GetAllHammers()
        {
            HammerData thisData = new HammerData();
            return thisData.GetHammers();
        }

        public bool Update(int HammerID, string HammerName, string HammerDesc, int Qty)
        {
            return data.UpdateHammer(HammerID, HammerName, HammerDesc, Qty);
        }
        #endregion
    }
}