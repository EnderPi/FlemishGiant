using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RngGenetics.Models
{
    public class SboxFourBits
    {
        public int Id { get; set; }
        [Column(TypeName ="char(16)")]
        public string Sbox {  get; set; }
        public int DifferentialUniformity { get; set; }
        public int Linearity { get; set; }

    }
}
