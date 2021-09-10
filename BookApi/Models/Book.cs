using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Models
{
    public class Book
    {
        [Required]
        public string Id { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }

        [Required]
        public string Venue { get; set; }
        [Required]

        public decimal Amount { get; set; }

        public string NumberofTickets { get; set; }
        [Required]

        public string Currency { get; set; }
    }
}
