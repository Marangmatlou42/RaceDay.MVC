using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Services;
using RaceDay.MVC.Models; 

namespace RaceDay.MVC.Controllers;

[Authorize(Roles = "Participant")]
public class ParticipantController : Controller
{
    private readonly ApiClient _api;
    public ParticipantController(ApiClient api) => _api = api;

    public async Task<IActionResult> MyEnrolments()
    {
        // Change List<dynamic> to List<EnrolmentViewModel>
        var enrolments = await _api.GetAsync<List<EnrolmentViewModel>>("api/enrolments/my-enrolments");
        return View(enrolments);
    }

    public async Task<IActionResult> MyResults()
    {
        var results = await _api.GetAsync<List<dynamic>>("api/results/my-results");
        return View(results);
    }
}
