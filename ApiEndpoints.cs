using ContactApp.Contacts;
using ContactApp.Pages.Contacts;
using ContactApp.Utils;
using Microsoft.AspNetCore.Components.Endpoints;
using Microsoft.AspNetCore.Mvc;
using MiniValidation;

namespace ContactApp;

public static class ApiEndpoints
{
    public static WebApplication MapContactsApiEndpoints(this WebApplication app)
    {

        app.MapDelete("/Contacts/{id:int}", (HttpContext httpContext,
            ContactRepository contactRepository, int id) =>
        {
            contactRepository.Delete(id);

            if (httpContext.Request.Headers.TryGetValue("hx-trigger", out var trigger) && trigger == "delete-btn")
            {
                httpContext.Response.Headers.Append("Location", "/Contacts");
                return Results.StatusCode(303);
                
            }

            return Results.Empty;
        });

        app.MapDelete("/Contacts", (HttpContext httpContext,
            ContactRepository contactRepository, [FromForm] int[] selectedContactIds) =>
        {
            contactRepository.BulkDelete(selectedContactIds);
            var contacts = contactRepository.All();

            httpContext.Response.Headers.Append("Location", "/Contacts");
            return Results.StatusCode(303);
        });

        app.MapGet("/Contacts/CheckEmail", (ContactRepository contactRepository, string email) =>
        {
            return contactRepository.EmailIsUnique(email)
                ? string.Empty
                : "Email Must Be Unique";
        });

        app.MapGet("/Contacts/Count", (ContactRepository contactRepository) =>
        {
            var count = contactRepository.Count();
            return $"({count} total Contacts)";
        });

        return app;
    }


    public static WebApplication MapArchiveApiEndpoints(this WebApplication app)
    {
        app.MapGet("/Contacts/Archive", (Archiver archiver) =>
        {
            return new RazorComponentResult<ArchiveUI>(new { Archiver = archiver });
        });

        app.MapPost("/Contacts/Archive", (Archiver archiver) =>
        {
            archiver.Run();
            return new RazorComponentResult<ArchiveUI>(new { Archiver = archiver });

        });

        app.MapDelete("/Contacts/Archive", (Archiver archiver) =>
        {
            archiver.Reset();
            return new RazorComponentResult<ArchiveUI>(new { Archiver = archiver });
        });

        app.MapGet("Contacts/Archive/File", (IWebHostEnvironment env, Archiver archiver) =>
        {
            var filePath = Path.Combine(env.WebRootPath, archiver.ArchiveFile());
            return Results.File(filePath, "application/json", "archive.json");
        });

        return app;
    }


    public static WebApplication MapContactsDataApiEndpoints(this WebApplication app)
    {
        app.MapGet("/api/v1/contacts", (ContactRepository contactRepository) =>
        {
            var contactsSet = contactRepository.All();
            return Results.Ok(new { Contacts = contactsSet });
        });
        
        app.MapPost("api/v1/contacts", (ContactRepository contactRepository, Contact contact) =>
        {
            var isValid = MiniValidator.TryValidate(contact, out var errors);
            if (contact.Email != null && !contactRepository.EmailIsUnique(contact.Email))
            {
                errors["Email"] = new string[] { "Email Must Be Unique" };
                isValid = false;
            }
            if (!isValid)
                return Results.ValidationProblem(errors);

            var c = new Contact
            {
                First = contact.First,
                Last = contact.Last,
                Phone = contact.Phone,
                Email = contact.Email!
            };

            contactRepository.Add(c);
            return Results.Ok(c);

        });

        app.MapGet("api/v1/contacts/{id}", (ContactRepository contactRepository, int id) =>
        {
            var contact = contactRepository.GetById(id);

            return contact is null
                ? Results.NotFound()
                : Results.Ok(contact);
        });

        app.MapPut("api/v1/contacts/{id}", (ContactRepository contactRepository, int id, Contact contact) =>
        {
            var isValid = MiniValidator.TryValidate(contact, out var errors);
            if (contact.Email != null && !contactRepository.EmailIsUnique(contact.Email))
            {
                errors["Email"] = new string[] { "Email Must Be Unique" };
                isValid = false;
            }
            if (!isValid)
                return Results.ValidationProblem(errors);

            var c = contactRepository.GetById(id);
            if (c is null)
                return Results.NotFound();

            c.First = contact.First;
            c.Last = contact.Last;
            c.Phone = contact.Phone;
            c.Email = contact.Email!;
            contactRepository.Update(c);

            return Results.Ok(c);
        });

        app.MapDelete("api/v1/contacts/{id}", (ContactRepository contactRepository, int id) =>
        {
            contactRepository.Delete(id);

            return Results.Ok(new { Success = true });
        });

        return app;
    }
}
