using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Services;

namespace RaceDay.MVC.Controllers;

[Authorize(Roles = "Organiser")]
public class OrganiserController : Controller
{
    private readonly ApiClient _api;
    public OrganiserController(ApiClient api) => _api = api;

    public async Task<IActionResult> Dashboard()
    {
        var events = await _api.GetAsync<List<dynamic>>("api/events");
        return View(events);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEvent(string eventName, string description, DateTime eventDate, string location, string distance, Guid eventTypeId, IFormFile banner)
    {
        string bannerUrl = null;

        
        if (banner != null && banner.Length > 0)
        {
            var uploadResponse = await _api.UploadFileAsync("api/upload/event-banner", banner);
            if (uploadResponse.IsSuccessStatusCode)
            {
                var result = await uploadResponse.Content.ReadFromJsonAsync<dynamic>();
                bannerUrl = result.url;
            }
        }

        
        var eventDto = new { eventName, description, eventDate, location, distance, eventTypeId, bannerImageUrl = bannerUrl, isPublished = true };
        await _api.PostAsync("api/events", eventDto);

        return RedirectToAction("Dashboard");
    }
}