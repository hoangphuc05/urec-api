using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;

namespace UREC_api
{
    public static class FirebaseAdminApp
    {
        static FirebaseApp DefaultApp;
        public static void Create()
        {
            DefaultApp = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("urec-admin.json"),
                }
            );
        }

        public static async Task<FirebaseToken?> VerifyToken (string token)
        {
            FirebaseToken? decodedToken = null;
            if (DefaultApp == null)
                Create();
            try { 
                decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
            } catch (FirebaseAuthException e)
            {
                // log here maybe
                return null;
            }
            return decodedToken;
        }


    }
}
