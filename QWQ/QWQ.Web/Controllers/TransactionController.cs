using QWQ.Web.BLL;
using QWQ.Web.ViewModels.Transaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace QWQ.Web.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly ITransactionBLL transactionBLL;
        private readonly IWalletBLL walletBLL;
        public TransactionController(ITransactionBLL transactionBLL, IWalletBLL walletBLL)
        {
            this.transactionBLL = transactionBLL;
            this.walletBLL = walletBLL;
        }

        /// <summary>
        /// 顯示目前使用者的收支紀錄
        /// </summary>
        public IActionResult Overview()
        {
            string? userIdValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdValue))
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = Convert.ToInt32(userIdValue);
            List<TransactionListItemViewModel> transactionList = transactionBLL.GetTransactionsByUserId(userId);
            return View(transactionList);
        }

        /// <summary>
        /// 顯示新增交易頁面
        /// </summary>
        public IActionResult Create()
        {
            string? userIdValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdValue))
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = Convert.ToInt32(userIdValue);
            TransactionEditorViewModel transactionVM = new TransactionEditorViewModel();
            SetWalletOptions(transactionVM, userId);

            if (transactionVM.WalletOptions.Count == 0)
            {
                TempData["ErrorMessage"] = "請先建立啟用中的資產";
                return RedirectToAction("Overview", "Wallet");
            }
            return View("Editor", transactionVM);
        }

        /// <summary>
        /// 處理新增交易資料
        /// </summary>
        /// <param name="transactionVM">新增交易表單資料</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TransactionEditorViewModel transactionVM)
        {
            string? userIdValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdValue))
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = Convert.ToInt32(userIdValue);
            if (!ModelState.IsValid)
            {
                SetWalletOptions(transactionVM, userId);
                return View("Editor", transactionVM);
            }

            string result = transactionBLL.CreateTransaction(transactionVM, userId);
            if (!string.IsNullOrEmpty(result))
            {
                ModelState.AddModelError("", result);
                SetWalletOptions(transactionVM, userId);
                return View("Editor", transactionVM);
            }

            TempData["SuccessMessage"] = "交易紀錄已新增";
            return RedirectToAction("Overview");
        }

        /// <summary>
        /// 顯示編輯交易頁面
        /// </summary>
        /// <param name="id">交易 ID</param>
        public IActionResult Edit(int id)
        {
            string? userIdValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdValue))
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = Convert.ToInt32(userIdValue);
            TransactionEditorViewModel? transactionVM = transactionBLL.GetTransactionById(id, userId);
            if (transactionVM == null)
            {
                return RedirectToAction("Overview");
            }

            SetWalletOptions(transactionVM, userId);
            return View("Editor", transactionVM);
        }

        /// <summary>
        /// 處理交易編輯資料
        /// </summary>
        /// <param name="transactionVM">交易編輯資料</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TransactionEditorViewModel transactionVM)
        {
            string? userIdValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdValue))
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = Convert.ToInt32(userIdValue);
            if (!ModelState.IsValid)
            {
                SetWalletOptions(transactionVM, userId);
                return View("Editor", transactionVM);
            }

            string result = transactionBLL.UpdateTransaction(transactionVM, userId);
            if (!string.IsNullOrEmpty(result))
            {
                ModelState.AddModelError("", result);
                SetWalletOptions(transactionVM, userId);
                return View("Editor", transactionVM);
            }

            TempData["SuccessMessage"] = "交易紀錄已更新";
            return RedirectToAction("Overview");
        }

        /// <summary>
        /// 刪除交易紀錄
        /// </summary>
        /// <param name="id">交易 ID</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            string? userIdValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdValue))
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = Convert.ToInt32(userIdValue);
            string result = transactionBLL.DeleteTransaction(id, userId);
            if (!string.IsNullOrEmpty(result))
            {
                TempData["ErrorMessage"] = result;
            }
            else
            {
                TempData["SuccessMessage"] = "交易紀錄已刪除";
            }
            return RedirectToAction("Overview");
        }

        /// <summary>
        /// 設定可選擇的啟用中資產
        /// </summary>
        /// <param name="transactionVM">交易表單資料</param>
        /// <param name="userId">使用者 ID</param>
        private void SetWalletOptions(TransactionEditorViewModel transactionVM, int userId)
        {
            transactionVM.WalletOptions = walletBLL.GetWalletsByUserId(userId).Select(wallet => new SelectListItem
            {
                Text = wallet.WalletName,
                Value = wallet.WalletId.ToString(),
                Selected = wallet.WalletId == transactionVM.WalletId
            }).ToList();
        }
    }
}