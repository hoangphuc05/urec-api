
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using System.ComponentModel.DataAnnotations;

namespace UREC_api
{
    public class UserFirebaseToken
    {
        [Required]
        public string Token { get; set; } = "";

        public async Task<bool> CheckToken()
        {
            // check if the token is valid
            if (await FirebaseAdminApp.VerifyToken(Token) != null)
                return true;
            //Console.WriteLine(await FirebaseAdminApp.VerifyToken(Token));
            return false;
        }

        public async Task<FirebaseToken?> DecodeToken()
        {
            return await FirebaseAdminApp.VerifyToken(Token);
        }
    }
}
