using FluentValidation;
using GerenciadorContatos.Application.DTOs.Contacts;
using GerenciadorContatos.Application.Services.Contacts;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorContatos.Api.Controllers;

[ApiController]
[Route("api/contacts")]
public class ContactsController(IContactService contactService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ContactResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<ContactResponse> Create([FromBody] CreateContactRequest request)
    {
        try
        {
            var response = contactService.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (Exception exception)
        {
            return HandleException(exception);
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ContactResponse>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<ContactResponse>> GetAll()
    {
        try
        {
            var response = contactService.GetAll();
            return Ok(response);
        }
        catch (Exception exception)
        {
            return HandleException(exception);
        }
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ContactResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<ContactResponse> GetById(Guid id)
    {
        try
        {
            var response = contactService.GetById(id);
            return Ok(response);
        }
        catch (Exception exception)
        {
            return HandleException(exception);
        }
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ContactResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<ContactResponse> Update(Guid id, [FromBody] UpdateContactRequest request)
    {
        try
        {
            var response = contactService.Update(id, request);
            return Ok(response);
        }
        catch (Exception exception)
        {
            return HandleException(exception);
        }
    }

    [HttpPatch("{id:guid}/activate")]
    [ProducesResponseType(typeof(ContactResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<ContactResponse> Activate(Guid id)
    {
        try
        {
            var response = contactService.Activate(id);
            return Ok(response);
        }
        catch (Exception exception)
        {
            return HandleException(exception);
        }
    }

    [HttpPatch("{id:guid}/deactivate")]
    [ProducesResponseType(typeof(ContactResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<ContactResponse> Deactivate(Guid id)
    {
        try
        {
            var response = contactService.Deactivate(id);
            return Ok(response);
        }
        catch (Exception exception)
        {
            return HandleException(exception);
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        try
        {
            contactService.Delete(id);
            return NoContent();
        }
        catch (Exception exception)
        {
            return HandleException(exception);
        }
    }

    private ActionResult HandleException(Exception exception)
    {
        return exception switch
        {
            ValidationException validationException => BadRequest(new
            {
                errors = validationException.Errors.Select(error => error.ErrorMessage).ToList()
            }),
            ArgumentException argumentException => BadRequest(new
            {
                errors = new[] { argumentException.Message }
            }),
            KeyNotFoundException keyNotFoundException => NotFound(new
            {
                errors = new[] { keyNotFoundException.Message }
            }),
            InvalidOperationException invalidOperationException => Conflict(new
            {
                errors = new[] { invalidOperationException.Message }
            }),
            _ => StatusCode(StatusCodes.Status500InternalServerError, new
            {
                errors = new[] { "An unexpected error occurred." }
            })
        };
    }
}
