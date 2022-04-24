using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace UREC_api
{
    public class QRToken
    {
        public string Token { get; set; }
        public DateTime ExpireTime { get; set; }
        public Student Student { get; set; }

        public QRToken() { }

        public QRToken(Student student)
        {
            Student = student;
            ExpireTime = DateTime.UtcNow.AddMinutes(60);
            Token = Convert.ToHexString(SHA1.Create().ComputeHash(Guid.NewGuid().ToByteArray()));
        }
    }

    public struct QRTokenData
    {
        [Required]
        public string Token { get; set; }
    }
}
