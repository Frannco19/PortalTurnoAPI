using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Appointments
{
    public class AppointmentResponseDto
    {
        public int AppointmentId { get; set; }

        public int ProfessionalId { get; set; }
        public string? ProfessionalName { get; set; }

        public int ClientId { get; set; }
        public string? ClientName { get; set; }

        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }

        public AppointmentStatus Status { get; set; }
        public string? Notes { get; set; }
    }
}
