using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentDetails.Data;
using StudentDetails.Models;
using StudentDetails.Models.Entity;
using StudentDetails.ViewModel;
using System.Runtime.CompilerServices;

namespace StudentDetails.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public StudentsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }



        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpGet]
        public IActionResult logout()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddStudentViewModel viewModel)
        {

            var student = new Student
            {
                Name = viewModel.Name,
                Email = viewModel.Email,
                Phone = viewModel.Phone,
                Subscribed = viewModel.Subscribed

            };
            await dbContext.Students.AddAsync(student);
            dbContext.SaveChanges();
            return View();

        }


        [HttpGet]
        public async Task<IActionResult> Login(UserVM viewModel)
        {

            return View();

        }

        public async Task<IActionResult> List()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var student = await dbContext.Students.FindAsync(id);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Student viewModel)
        {
             var student = await dbContext.Students.FindAsync(viewModel.Id);
            if(student != null)
            {
                student.Name=viewModel.Name;
                student.Email=viewModel.Email;
                student.Phone=viewModel.Phone;
                student.Subscribed=viewModel.Subscribed;
                 
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "Students");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Student viewModel)
        {
            var student = await dbContext.Students.AsNoTracking().FirstOrDefaultAsync(
                x =>x.Id == viewModel.Id);
            if(student != null)
            {
                dbContext.Students.Remove(viewModel);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "Students");
        }
    }
}
