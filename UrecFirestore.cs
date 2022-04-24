using Google.Cloud.Firestore;
using Newtonsoft.Json;

namespace UREC_api
{
    public static class UrecFirestore
    {
        public static FirestoreDb db = FirestoreDb.Create("urec-whitworth");

        public static async Task AddToken(QRToken token)
        {
            DocumentReference docRef = db.Collection("qrtoken").Document(token.Token);

            // flatten the QRToken object
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                ["token"] = token.Token,
                ["expire"] = token.ExpireTime,
                ["uid"] = token.Student.Uid,
                ["name"] = token.Student.Name,
                ["email"] = token.Student.Email,

            };
            await docRef.SetAsync(data);
        }

        /// <summary>
        /// Verify if the token exist and valid, if no token sastified, return null
        /// </summary>
        /// <param name="userToken">Token to be verified</param>
        /// <returns>null if invalid, the QRToken object if its valid</returns>
        public static async Task<QRToken?> VerifyToken(string userToken)
        {
            DocumentReference docRef = db.Collection("qrtoken").Document($"{ userToken }");
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                Console.WriteLine("Document data for {0} document:", snapshot.Id);
                Dictionary<string, object> qrtoken = snapshot.ToDictionary();

                // reconstruct the Student object
                Student student = new Student
                {
                    Uid = qrtoken["uid"] as String,
                    Name = qrtoken["name"] as String,
                    Email = qrtoken["email"] as String,
                };

                // reconstruct the qrcode object
                QRToken token = new QRToken
                {
                    Token = qrtoken["token"] as String,
                    ExpireTime = ((Timestamp)qrtoken["expire"]).ToDateTime(),
                    Student = student,
                };

                // Expiretime before utcnow
                if (DateTime.Compare(token.ExpireTime, DateTime.UtcNow) < 0 )
                {
                    return null;
                }
                return token;

            }
            else
            {
                return null;
            }
        }
    }
}
