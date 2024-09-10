using InfoMed.Models;
using InfoMed.Services.Interface;
using InfoMed.Utils;
using Microsoft.AspNetCore.Mvc;

namespace InfoMed.Controllers
{
    public class LastYearMemoriesController : Controller
    {
        private readonly ILastYearMemoriesService _lastYearMemoriesService;
        private readonly Session _session;
        public LastYearMemoriesController(ILastYearMemoriesService lastYearMemories, Session session)
        {
            _lastYearMemoriesService = lastYearMemories;
            _session = session;
        }
        [HttpGet]
        public IActionResult AddLastYearMemories()
        {
            string textContent = Utils.Utils.RenderRazorViewToString(this, "../../Views/LastYearMemories/UpsertLastYearMemories");
            return Json(new { html = textContent });
        }

        [HttpPost]
        public async Task<IActionResult> AddLastYearMemories(LastYearMemoryDto lastYeatMemoryDto)
        {
            ViewBag.sessionUser = _session.GetUserDetails();
            if (ModelState.IsValid)
            {
                if (lastYeatMemoryDto.LastYearMemoryMap != null && lastYeatMemoryDto.LastYearMemoryMap.Length > 0)
                {

                    //var allowedExtensions = new[] { ".mp4", ".avi", ".mov", ".wmv" };
                   // var extension = Path.GetExtension(lastYeatMemoryDto.LastYearMemoryMap.FileName).ToLower();
                    
                        var imagePath = await SaveImageToFileSystem(lastYeatMemoryDto.LastYearMemoryMap, "LastYearMemories");

                        lastYeatMemoryDto.LastYearMemoryDetail.MediaPath = imagePath;
                        lastYeatMemoryDto.LastYearMemoryDetail.MediaType = lastYeatMemoryDto.LastYearMemoryMap.ContentType;
                        lastYeatMemoryDto.Status = true;
                                   
                     
                }
                await _lastYearMemoriesService.AddLastYearMemories(lastYeatMemoryDto);
            }
            int id = _session.GetEventId();
            int eventVersion = _session.GetEventVersion();
            var content = await _lastYearMemoriesService.GetLastYearMemoriesList(id, eventVersion);
            string feesPartialView = Utils.Utils.RenderRazorViewToString(this, "../../Views/LastYearMemories/_LastYearMemoriesList", content);
            return Json(new { html = feesPartialView });
        }
        public async Task<string> SaveImageToFileSystem(IFormFile file, string moduleName)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", moduleName);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/uploads/{moduleName}/{uniqueFileName}";
        }

        [HttpGet]
        public async Task<IActionResult> EditLastYearMemories(int idLastYearMemory)
        {
            var content = await _lastYearMemoriesService.GetLastYearMemoriesById(idLastYearMemory);
            string upsertFeesView = Utils.Utils.RenderRazorViewToString(this, "../../Views/LastYearMemories/UpsertLastYearMemories", content);
            return Json(new { html = upsertFeesView });
        }

        [HttpPost]
        public async Task<IActionResult> EditLastYearMemories(LastYearMemoryDto lastYeatMemoryDto)
        {
            ViewBag.sessionUser = _session.GetUserDetails();
            if (ModelState.IsValid)
            {
                if (lastYeatMemoryDto.LastYearMemoryMap != null && lastYeatMemoryDto.LastYearMemoryMap.Length > 0)
                {

                    //var allowedExtensions = new[] { ".mp4", ".avi", ".mov", ".wmv" };
                    // var extension = Path.GetExtension(lastYeatMemoryDto.LastYearMemoryMap.FileName).ToLower();

                    var imagePath = await SaveImageToFileSystem(lastYeatMemoryDto.LastYearMemoryMap, "LastYearMemories");

                    lastYeatMemoryDto.LastYearMemoryDetail.MediaPath = imagePath;
                    lastYeatMemoryDto.LastYearMemoryDetail.MediaType = lastYeatMemoryDto.LastYearMemoryMap.ContentType;
                    lastYeatMemoryDto.Status = true;


                }
                else
                {
                    var lastyearmeneory = await _lastYearMemoriesService.GetLastYearMemoriesById(lastYeatMemoryDto.IdLastYearMemory);
                    lastYeatMemoryDto.LastYearMemoryDetail.MediaPath = lastyearmeneory.LastYearMemoryDetail.MediaPath;
                    lastYeatMemoryDto.LastYearMemoryDetail.MediaType = lastyearmeneory.LastYearMemoryDetail.MediaType;
                    lastYeatMemoryDto.LastYearMemoryHeader = lastyearmeneory.LastYearMemoryHeader;
                    lastYeatMemoryDto .LastYearMemoryText = lastyearmeneory.LastYearMemoryText;
                    lastYeatMemoryDto.Status = true;
                }
                await _lastYearMemoriesService.UpdateLastYearMemories(lastYeatMemoryDto);
            }
            int id = _session.GetEventId();
            int eventVersion = _session.GetEventVersion();
            var content = await _lastYearMemoriesService.GetLastYearMemoriesList(id, eventVersion);
            string upsertScheduleView = Utils.Utils.RenderRazorViewToString(this, "../../Views/LastYearMemories/_LastYearMemoriesList", content);
            return Json(new { html = upsertScheduleView });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteLastYearMaster(int idLastYearMemory)
        {
            var scheduleMaster = await _lastYearMemoriesService.GetLastYearMemoriesById(idLastYearMemory);
            scheduleMaster.Status = false;
            await _lastYearMemoriesService.UpdateLastYearMemories(scheduleMaster);
            int id = _session.GetEventId();
            int eventVersion = _session.GetEventVersion() ;
            ViewBag.sessionUser = _session.GetUserDetails();
            var content = await _lastYearMemoriesService.GetLastYearMemoriesList(id, eventVersion);
            string schedulePartialView = Utils.Utils.RenderRazorViewToString(this, "../../Views/LastYearMemories/_LastYearMemoriesList", content);
            return Json(new { html = schedulePartialView });
        }

        [HttpGet]
        public async Task<IActionResult> ViewMediaContent(int mediaContent)
        {

            var content = await _lastYearMemoriesService.GetLastYearMemoriesById(mediaContent);
            var mediaDetail = content.LastYearMemoryDetail;          
            if (mediaDetail.MediaType.StartsWith("image"))
            {
                string imgDataURL = content.LastYearMemoryDetail.MediaPath;
                LastYearMediaDto obj = new LastYearMediaDto();
                obj.MediaContent = imgDataURL;
                obj.IsVideo = false;
                string _textContent = Utils.Utils.RenderRazorViewToString(this, "../../Views/LastYearMemories/MediaContentParial", obj);
                return Json(new { html = _textContent });
            }
            else if (mediaDetail.MediaType.StartsWith("video"))
            {
                LastYearMediaDto obj = new LastYearMediaDto();
                obj.FilePath =mediaDetail.MediaPath;
                obj.IsVideo = true;
                string _textContent = Utils.Utils.RenderRazorViewToString(this, "../../Views/LastYearMemories/MediaContentParial", obj);
                return Json(new { html = _textContent });
            }
            return BadRequest("Unsupported media type");
        }

       
    }
}
