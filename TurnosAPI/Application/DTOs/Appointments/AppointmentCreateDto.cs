using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Appointments
{
    public class AppointmentCreateDto
    {
        public int ProfessionalId { get; set; }
        public int ClientId { get; set; }

        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }

        public string? Notes { get; set; }
    }
}
