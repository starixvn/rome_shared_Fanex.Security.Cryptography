using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fanex.Cryptography.Aes.Site
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var secretKey = "1234";
            var plainText = "1234aaAA朝鮮凌晨發射";
            var encrypted = AesExtension.Encrypt(plainText, secretKey);
            var decrypted = AesExtension.Decrypt(encrypted, secretKey);

            if (decrypted == plainText)
            {
                Response.Write("<script>alert('AES works fine!!!');</script>");
            }            
        }
    }
}