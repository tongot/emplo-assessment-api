

using System;
using System.ComponentModel.DataAnnotations;

namespace oldMutual.Models
{
    public class Reward
    {
        public int RewardId { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string description { get; set; }
        public DateTime dateAdded{ get; set; }
        public string addedBy { get; set; }
        [Required]
        public DateTime dateEnd { get; set; }

        /// <summary>
        /// duration of the time
        /// </summary>
        public string durationOfReward { get {
                return dateEnd.Subtract(dateAdded).ToString();
            } }
        /// <summary>
        /// the mark that must be reached to get the price
        /// </summary>
        public float pointsToEarn { get; set; }
    }
}