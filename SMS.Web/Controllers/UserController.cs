using SMS.Models.DAO;
using SMS.Models.EF;
using SMS.Web.Models;
using System.Linq;
using System.Web.Mvc;

namespace SMS.Web.Controllers
{
    public class UserController : BaseController
    {
        private SMSDbContext _context;

        public UserController()
        {
            _context = new SMSDbContext();
        }

        // GET: User
        public ActionResult GetUserByCode(string empCode)
        {
            var dao = new UserDAO();
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


        [HttpPost]
        public ActionResult UpdateRole(User u)
        {
            var user = _context.Users.Where(r => r.EmpCode == u.EmpCode);

            //user.Admin = u.Admin;

            //user.SubAdmin = u.SubAdmin;

            //user.PIC = u.PIC;

            //user.ITT_TM = u.ITT_TM;

            //user.SMT_TM = u.SMT_TM;

            //user.FST_TM = u.FST_TM;

            //user.PIC_TM = u.PIC_TM;

            //user.Group_Leader = u.Group_Leader;

            //user.Guard = u.Guard;

            //user.Read_Only = u.Read_Only;

            _context.SaveChanges();

            return Content("Success");
        }
    }
}