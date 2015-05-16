using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SampleApplication.Domain;
using SampleApplication.Data.EntityFramework;
using SampleApplication.Domain.Entities;
using SampleApplication.Service.Common;
using SampleApplication.Service.SearchCriterias;
using SampleApplication.Service.Services;
using SampleApplication.Web.Models;

namespace SampleApplication.Web.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //
        // GET: Client
        public ActionResult Index()
        {
            return View();
        }
    }



}