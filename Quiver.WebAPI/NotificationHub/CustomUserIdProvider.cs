using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quiver.WebAPI
{
    public class CustomUserIdProvider : Controller, IUserIdProvider
    {
        public CustomUserIdProvider()
        {

        }

        public string GetUserId(IRequest request)
        {
            // your logic to fetch a user identifier goes here.

            // for example:

            //Data.Repository.UnitOfWork _uow = new Data.Repository.UnitOfWork();
            //var user = _uow.UsuarioRepository.Get(u => u.Nome == request.User.Identity.Name).FirstOrDefault();
            //return user.Id;



            return Session["UsuarioId"].ToString();
        }
    }
}