using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HardinStrongRazor.Pages
{
    public class ApplyModel : PageModel
    {
        public void OnGet()
        {

        }

        [BindProperty]
        public ApplyFormModel Apply { get; set; }

        public class ApplyFormModel
        {
            [Required]
            public string FirstName { get; set; }
            [Required]
            public string LastName { get; set; }
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            [EmailAddress]
            [Compare("Email", ErrorMessage = "Emails do not match")]
            public string EmailConfirm { get; set; }

            [Required]
            public string Address { get; set; }
            
            public string Address2 { get; set; }
            [Required]
            public string City { get; set; }
            [Required]
            public string State { get; set; }
            [Required]
            public string Zip { get; set; }
            [Required]
            public string Phone { get; set; }
            
            public string Phone2 { get; set; }
            
            public string Message { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var mailbody = $@"
                This is a new contact request from your website:

                Email: {Apply.Email}
                FirstName: {Apply.FirstName}
                LastName: {Apply.LastName}
                
                Address: {Apply.Address}
                Address2: {Apply.Address2}
                City: {Apply.City}
                State: {Apply.State}
                Zipcode: {Apply.Zip}

                Phone: {Apply.Phone}
                Phone2: {Apply.Phone2}
                Message: ""{Apply.Message}""


                via Apply form";

            SendMail(mailbody);

            return RedirectToPage("Index");
        }
        private void SendMail(string mailbody)
        {
            using (var message = new MailMessage(Apply.Email, "cnitsuj@gmail.com"))
            {
                message.To.Add(new MailAddress("cnitsuj@gmail.com"));
                message.From = new MailAddress(Apply.Email);
                message.Subject = "New E-Mail from my website";
                message.Body = mailbody;

                using (var smtpClient = new SmtpClient("smtp.gmail.com"))
                {

                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;

                    smtpClient.Credentials = new System.Net.NetworkCredential("testsender12345321@gmail.com", "testSenderWhatever123");
                    smtpClient.Send(message);
                }
            }
        }
    }
}