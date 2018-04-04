using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zxw.Framework.NetCore.Attributes;
using Zxw.Framework.NetCore.Models;
using Zxw.Framework.NetCore.UnitOfWork;
using Zxw.Framework.Website.IRepositories;
using Zxw.Framework.Website.Models;

namespace Zxw.Framework.Website.Controllers
{
    public class SysMenuController : Controller
    {
        private IUnitOfWork _unitOfWork;
        
        public SysMenuController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public IActionResult Index()
        {
            return View();
        }

        [AjaxRequestOnly, HttpGet]
        public Task<IActionResult> GetMenus()
        {
            return Task.Factory.StartNew<IActionResult>(() =>
            {
                using (var repository = _unitOfWork.GetRepository<ISysMenuRepository>())
                {
                    #region ����ģ�飬ע�͵���

                    //repository.AddRange(new List<SysMenu>
                    //{
                    //    new SysMenu
                    //    {
                    //        Identity = "SysMenu/Add",
                    //        MenuName = "����",
                    //        ParentId = 1,
                    //        RouteUrl = "~/SysMenu/Add",
                    //        Visiable = false
                    //    },
                    //    new SysMenu
                    //    {
                    //        Identity = "SysMenu/Edit",
                    //        MenuName = "�༭",
                    //        ParentId = 1,
                    //        RouteUrl = "~/SysMenu/Edit",
                    //        Visiable = false
                    //    },
                    //    new SysMenu
                    //    {
                    //        Identity = "SysMenu/Delete",
                    //        MenuName = "ɾ��",
                    //        ParentId = 1,
                    //        RouteUrl = "~/SysMenu/Delete",
                    //        Visiable = false
                    //    },
                    //    new SysMenu
                    //    {
                    //        Identity = "SysMenu/Active",
                    //        MenuName = "��ͣ��",
                    //        ParentId = 1,
                    //        RouteUrl = "~/SysMenu/Active",
                    //        Visiable = false
                    //    },
                    //    new SysMenu
                    //    {
                    //        Identity = "SysMenu/Visualize",
                    //        MenuName = "��ʾ/����",
                    //        ParentId = 1,
                    //        RouteUrl = "~/SysMenu/Visualize",
                    //        Visiable = false
                    //    }
                    //});

                    #endregion
                    var rows = repository.GetMenusByTreeView().Where(m=>m.Activable && m.Visiable).ToList();
                    return Json(ExcutedResult.SuccessResult(rows));
                }
            });
        }

        [AjaxRequestOnly]
        public Task<IActionResult> GetMenusByPaged(int pageSize, int pageIndex)
        {
            return Task.Factory.StartNew<IActionResult>(() =>
            {
                using (var repository = _unitOfWork.GetRepository<ISysMenuRepository>())
                {
                    var total = repository.Count(m => true);
                    var rows = repository.GetByPagination(m => true, pageSize, pageIndex, true,
                        m => m.Id).ToList();
                    return Json(PaginationResult.PagedResult(rows, total, pageSize, pageIndex));
                }
            });
        }
        /// <summary>
        /// �½�
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        [AjaxRequestOnly,HttpPost]
        public Task<IActionResult> Add(SysMenu menu)
        {
            return Task.Factory.StartNew<IActionResult>(() =>
            {
                using (var repository = _unitOfWork.GetRepository<ISysMenuRepository>())
                {
                    repository.AddAsync(menu);
                    return Json(ExcutedResult.SuccessResult());
                }
            });
        }
        /// <summary>
        /// �༭
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        [AjaxRequestOnly, HttpPost]
        public Task<IActionResult> Edit(SysMenu menu)
        {
            return Task.Factory.StartNew<IActionResult>(() =>
            {
                using (var repository = _unitOfWork.GetRepository<ISysMenuRepository>())
                {
                    repository.Edit(menu);
                    return Json(ExcutedResult.SuccessResult());
                }
            });
        }
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxRequestOnly]
        public Task<IActionResult> Delete(int id)
        {
            return Task.Factory.StartNew<IActionResult>(() =>
            {
                using (var repository = _unitOfWork.GetRepository<ISysMenuRepository>())
                {
                    repository.Delete(id);
                    return Json(ExcutedResult.SuccessResult());
                }
            });
        }

        /// <summary>
        /// ��ͣ��
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxRequestOnly]
        public Task<IActionResult> Active(int id)
        {
            return Task.Factory.StartNew<IActionResult>(() =>
            {
                using (var repository = _unitOfWork.GetRepository<ISysMenuRepository>())
                {
                    var entity = repository.GetSingle(id);
                    entity.Activable = !entity.Activable;
                    repository.Update(entity, "Activable");
                    return Json(ExcutedResult.SuccessResult());
                }
            });
        }
        /// <summary>
        /// �Ƿ������˵�����ʾ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AjaxRequestOnly]
        public Task<IActionResult> Visualize(int id)
        {
            return Task.Factory.StartNew<IActionResult>(() =>
            {
                using (var repository = _unitOfWork.GetRepository<ISysMenuRepository>())
                {
                    var entity = repository.GetSingle(id);
                    entity.Visiable = !entity.Visiable;
                    repository.Update(entity, "Visiable");
                    return Json(ExcutedResult.SuccessResult());
                }
            });
        }

		protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
	}
}