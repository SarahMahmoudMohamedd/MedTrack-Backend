using MedTrack.Shared.Dtos.AuthDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.ServicesAbstraction.Security
{
    public interface IJwtTokenService
    {
        string GenerateToken(string userId, string identifier, string role);
    }
}
