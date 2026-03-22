using Microsoft.AspNetCore.Mvc;
using TrainingTracker.Application.Interfaces;
using TrainingTracker.Application.Models.Assignments;

namespace TrainingTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseAssignmentsController : ControllerBase
{
    private readonly ICourseAssignmentService _service;

    public CourseAssignmentsController(ICourseAssignmentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseAssignmentModel>>> GetAll(CancellationToken cancellationToken)
    {
        var items = await _service.GetAllAsync(cancellationToken);
        return Ok(items);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CourseAssignmentModel>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var item = await _service.GetByIdAsync(id, cancellationToken);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<CourseAssignmentModel>> Create([FromBody] CreateAssignmentModel model, CancellationToken cancellationToken)
    {
        var created = await _service.CreateAsync(model, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateAssignmentStatusModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id) return BadRequest();
        await _service.UpdateStatusAsync(model, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
