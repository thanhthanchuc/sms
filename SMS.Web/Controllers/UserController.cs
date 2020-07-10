using SMS.Models.DAO;
using SMS.Models.EF;
using SMS.Web.Models;
using System.Web.Mvc;

namespace SMS.Web.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult GetUserByCode(string empCode)
        {
            var dao = new LeaveEarlyDAO();
            User user = new User();
            UserViewModel userViewModel = new UserViewModel();
            user = dao.GetByCode(empCode);
            userViewModel.UpdateUser(user);
            if (userViewModel.EmpCode != null)
            {
                return Json(new
                {
                    data = userViewModel,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    status = false
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}