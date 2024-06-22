using DotK_TechShop.Models;
using DotK_TechShop.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace DotK_TechShop.Controllers;

public class IdentityController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly EmailSender _emailSender;

    public IdentityController(UserManager<IdentityUser> userManager, 
        SignInManager<IdentityUser> signInManager, EmailSender emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
    }

    public IActionResult Login()
    {
        return View();
    }
    public IActionResult Register()
    {
        return View();
    }
    public IActionResult ForgotPassword()
    {
        return View();
    }
    [HttpGet]
    public IActionResult ResetPassword(string? email = null, string? code = null)
    {
        ResetPasswordViewModel model = new ResetPasswordViewModel { Code = code , Email = email};
        return View(model);
    }
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }


    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
    {
        returnUrl ??= Url.Content("~/");

        if (model.Email == null || model.Password == null)
            return View(model);

        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email , model.Password, true, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = true });
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (model.Password == null)
            return View(model);
        if (ModelState.IsValid)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Product");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(model);
    }


    
    [HttpPost]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        ViewData["Email"] = email;
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ViewData["Error"] = "Email not found or invalid";
                return View("");
            }

            // For more information on how to enable account confirmation and password reset please
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Identity",
                new { email = user.Email, code = code }, protocol: HttpContext.Request.Scheme);
            //Send the email with the link
             await _emailSender.SendEmailAsync(email, "Reset Password",
                 $"Please reset your password by clicking here: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>link</a>");
            return View("ForgotPasswordConfirmation");
        }

        // If we got this far, something failed, redisplay form
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (model.Email == null || model.Password == null || model.Code == null)
        {
            return View(model);
        }
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            // Don't reveal that the user does not exist
            return RedirectToAction("ResetPasswordConfirmation");
        }
        var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
        if (result.Succeeded)
        {
            return View("ResetPasswordConfirmation");
        }
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
        return View(model);
    }

}
