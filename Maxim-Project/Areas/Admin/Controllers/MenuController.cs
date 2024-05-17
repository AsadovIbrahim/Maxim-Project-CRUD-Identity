using Database.Entities.Concretes;
using Database.Repositories.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace Maxim_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuController : Controller
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public MenuController(IServiceRepository serviceRepository,IWebHostEnvironment webHostEnvironment)
        {
            _serviceRepository = serviceRepository;
            _webHostEnvironment = webHostEnvironment;

        }
        public async Task< IActionResult> Index()
        {
            var data = await _serviceRepository.GetAllAsync();
            return View(data);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>Create(Service service)
        {
            if (!ModelState.IsValid)
            {
                return View(service);
            }

            string path = _webHostEnvironment.WebRootPath + @"\Upload\Manage\";
            string fileName = Guid.NewGuid() + service.ImageFile.FileName;
            string fullPath=Path.Combine(path, fileName);

            using(FileStream stream=new FileStream(fullPath, FileMode.Create))
            {
                service.ImageFile.CopyTo(stream);
            }
            service.ImageUrl = fileName;

            await _serviceRepository.AddAsync(service);
            return RedirectToAction("Index");

        }
        public async Task<IActionResult>Remove(int id)
        {
            await _serviceRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet]

        public async Task< IActionResult> Update(int id)
        {
            var existingItem=await _serviceRepository.GetByIdAsync(id);
            return View(existingItem);
        }

        [HttpPost]
        public IActionResult Update(Service service)
        {
            if (!ModelState.IsValid)
            {
                return View(service);
            }
            if(service.ImageUrl == null)
            {
                string path = _webHostEnvironment.WebRootPath + @"\Upload\Manage\";
                string fileName = Guid.NewGuid() + service.ImageFile.FileName;
                string fullPath = Path.Combine(path, fileName); 

                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    service.ImageFile.CopyTo(stream);
                }
                service.ImageUrl = fileName;
            }
            _serviceRepository.UpdateAsync(service);
            return RedirectToAction("Index");
        }
    }
}
