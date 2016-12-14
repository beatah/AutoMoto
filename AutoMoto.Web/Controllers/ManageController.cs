using AutoMoto.Contracts;
using AutoMoto.Contracts.Interfaces;
using AutoMoto.Contracts.ViewModels;
using AutoMoto.Models;
using AutoMoto.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PagedList;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AutoMoto.Web.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private IMessageService _messageService;
        private IAddressService _addressService;
        private IAdvertisementService _advertisementService;
        private IUserNotificationService _userNotificationService;
        private IUnitOfWorkAsync _unitOfWorkAsync;


        public ManageController(IMessageService messageService, IUserNotificationService userNotificationService, IUnitOfWorkAsync unitOfWorkAsync, IAddressService addressService, IAdvertisementService advertisementService)
        {

            _messageService = messageService;
            _userNotificationService = userNotificationService;
            _unitOfWorkAsync = unitOfWorkAsync;
            _addressService = addressService;
            _advertisementService = advertisementService;
        }


        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public async Task<ActionResult> Messages(int? page)
        {
            int pageSize = 20;
            int pageNumber = (page ?? 1);

            var userId = User.Identity.GetUserId();
            var messages =
                await
                    _messageService.Query(x => x.FromUserId == userId || x.ToUserId == userId)
                        .Include(x => x.FromUser)
                        .Include(x => x.ToUser)
                        .Include(x => x.Advertisement)
                        .SelectAsync();
            var messageEntities = messages.ToList();


            messageEntities = messageEntities.GroupBy(x => new
            {
                x.AdvertisementId
            },
 (
                    key, group) => group.OrderBy(x => x.Id).First()).ToList();

            //  var threads = messageEntities.Select(x => x.Thread).Distinct().ToList();
            // var threadMessages = threads.Select(thread => messageEntities.Where(x => x.Thread == thread).OrderByDescending(x => x.Id).First()).ToList().OrderByDescending(x => x.Id);



            var model = messageEntities.ToPagedList(pageNumber, pageSize);

            return View(model);
        }


        public async Task<ActionResult> Message(int advertisementId)
        {
            var userIdCurrent = User.Identity.GetUserId();

            var messages =
                await
                    _messageService.Queryable()
                        .Where(x => x.AdvertisementId == advertisementId && (x.FromUserId == userIdCurrent || x.ToUserId == userIdCurrent))
                        .Include(x => x.Advertisement)
                        .Include(x => x.Advertisement.Photos)
                        .Include(x => x.Advertisement.Car)
                        .Include(x => x.Advertisement.User.Address)
                        .Include(x => x.FromUser)
                        .Include(x => x.ToUser)
                        .OrderByDescending(x => x.Id)
                        .ToListAsync();



            if (!messages.Any())
                return RedirectToAction("Messages");

            var viewModel = new MessageDetailsViewModel();
            viewModel.Messages = messages;
            if (messages.First().Advertisement.Photos.Any())
            {
                var picturesModel = messages.First().Advertisement.Photos.Select(x =>
                    new PictureModel()
                    {
                        ID = x.Id,
                        Url = string.Format("data:{0};base64,{1}", x.Extension, Convert.ToBase64String(x.Content)),
                        AdvertisementId = messages.First().Advertisement.Id

                    }).ToList();

                viewModel.Url = picturesModel.FirstOrDefault().Url;
            }


            var messageNotifications = _userNotificationService.GetNewNotificationsFor(userIdCurrent).Select(x => x.Notification).OfType<MessageNotification>().Include(x => x.Message).ToList();

            foreach (var messageNotification in messageNotifications)
            {
                foreach (var message in messages)
                {
                    if (message.Id == messageNotification.MessageId)
                    {
                        var markAsRead = await _userNotificationService.Query(x => x.NotificationId == messageNotification.Id).SelectAsync();
                        foreach (var userNotification in markAsRead)
                        {
                            userNotification.IsRead = true;
                            userNotification.ObjectState = ObjectState.Modified;

                        }
                    }
                }
            }


            await _unitOfWorkAsync.SaveChangesAsync();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Message(int messageId, string messageText)
        {
            var userIdCurrent = User.Identity.GetUserId();

            var lastMessage = _messageService.Find(messageId);
            string messageTo;
            if (userIdCurrent == lastMessage.FromUserId)
            {
                messageTo = lastMessage.ToUserId;
            }
            else
            {
                messageTo = lastMessage.FromUserId;
            }


            var message = new Message()
            {
                FromUserId = userIdCurrent,
                Content = messageText,
                ObjectState = ObjectState.Added,
                AdvertisementId = lastMessage.AdvertisementId,
                ToUserId = messageTo
            };

            _messageService.Insert(message);

            var messageNotification = new MessageNotification() { Message = message, DateTime = DateTime.Now, ObjectState = ObjectState.Added };

            var userNotification = new UserNotification()
            {
                IsRead = false,
                Notification = messageNotification,
                UserId = message.ToUserId,
                ObjectState = ObjectState.Added
            };
            _userNotificationService.Insert(userNotification);

            await _unitOfWorkAsync.SaveChangesAsync();

            TempData[TempKeys.UserMessage] = "Wiadomość została wysłana";

            return RedirectToAction("Message", new { advertisementId = lastMessage.AdvertisementId });
        }

        ////
        //// GET: /Manage/Index
        //public async Task<ActionResult> Index(ManageMessageId? message)
        //{
        //    ViewBag.StatusMessage =
        //        message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
        //        : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
        //        : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
        //        : message == ManageMessageId.Error ? "An error has occurred."
        //        : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
        //        : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
        //        : "";

        //    var userId = User.Identity.GetUserId();
        //    var model = new IndexViewModel
        //    {
        //        HasPassword = HasPassword(),
        //        PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
        //        TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
        //        Logins = await UserManager.GetLoginsAsync(userId),
        //        BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
        //    };
        //    return View(model);
        //}

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}