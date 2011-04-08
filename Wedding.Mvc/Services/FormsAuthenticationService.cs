using System;
using System.Web;
using System.Web.Security;

namespace Wedding.Mvc.Services
{

    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie, string role);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string userName, bool createPersistentCookie, string userData)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");

            var ticket = new FormsAuthenticationTicket(1,
                                                                                        userName,
                                                                                        DateTime.Now,
                                                                                        DateTime.Now.AddMinutes(60),
                                                                                        createPersistentCookie,
                                                                                        userData
                );

            var cookie = new HttpCookie(".ASPXAUTH");
            cookie.Value = FormsAuthentication.Encrypt(ticket);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }

}