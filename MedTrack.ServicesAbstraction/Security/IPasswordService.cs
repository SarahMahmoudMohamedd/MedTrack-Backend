using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.ServicesAbstraction.Security
{
    public interface IPasswordService
    {
        string HashPassword(string password);

        // ميثود بتقارن الباسورد اللي المستخدم كاتبه باللي متسيف في الداتابيز عشان اللوج إن
        bool VerifyPassword(string password, string passwordHash);
    }
}
