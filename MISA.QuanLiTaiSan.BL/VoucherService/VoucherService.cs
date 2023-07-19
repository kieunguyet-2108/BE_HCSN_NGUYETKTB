using MISA.QuanLiTaiSan.BL.BaseBL;
using MISA.QuanLiTaiSan.BL.BudgetDetailService;
using MISA.QuanLiTaiSan.BL.VoucherDetailService;
using MISA.QuanLiTaiSan.Common.Entities;
using MISA.QuanLiTaiSan.Common.Enumeration;
using MISA.QuanLiTaiSan.Common.Exceptions;
using MISA.QuanLiTaiSan.Common.Resources;
using MISA.QuanLiTaiSan.Common.UnitOfWork;
using MISA.QuanLiTaiSan.Common.Utilities;
using MISA.QuanLiTaiSan.DL.BaseDL;
using MISA.QuanLiTaiSan.DL.BudgetDetailRepository;
using MISA.QuanLiTaiSan.DL.FixedAssetDL;
using MISA.QuanLiTaiSan.DL.VoucherDetailRepository;
using MISA.QuanLiTaiSan.DL.VoucherRepository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace MISA.QuanLiTaiSan.BL.VoucherService
{
    public class VoucherService : BaseService<Voucher>, IVoucherService
    {
        private IVoucherRepository _voucherRepository;
        private IVoucherDetailRepository _voucherDetailRepository;
        private IBudgetDetailRepository _budgetDetailRepository;
        private IVoucherDetailService _voucherDetailService;
        private IBudgetDetailService _budgetDetailService;

        public VoucherService(IVoucherRepository voucherRepository, IVoucherDetailRepository voucherDetailRepository, IUnitOfWork unitOfWork, IVoucherDetailService voucherDetailService, IBudgetDetailRepository budgetDetailRepository, IBudgetDetailService budgetDetailService) : base(voucherRepository, unitOfWork)
        {
            _voucherRepository = voucherRepository;
            _voucherDetailRepository = voucherDetailRepository;
            _voucherDetailService = voucherDetailService;
            _budgetDetailRepository = budgetDetailRepository;
            _budgetDetailService = budgetDetailService;
        }

        /// <summary>
        /// Thực hiện xóa nhiều chứng từ theo id được truyền vào
        /// </summary>
        /// <param name="ids">id chứng từ cần xóa</param>
        /// <returns>số bản ghi đã được xóa</returns>
        /// Created By: NguyetKTB (25/06/2023)
        public override int Delete(string[] guids)
        {
            var transaction = _unitOfWork.GetTransaction();
            try
            {
                int rowBudget = _budgetDetailRepository.DeleteByField(guids, "voucher");
                int rowVoucherDetail = _voucherDetailRepository.DeleteByField(guids, "voucher");
                int rowVoucher = _voucherRepository.Delete(guids);
                _unitOfWork.Commit();
                return rowVoucher;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }


        /// <summary>
        /// Thực hiện thêm chứng từ 
        /// </summary>
        /// <param name="voucher">thông tin chứng từ</param>
        /// <param name="FixedAssets">thông tin tài sản thuộc chứng từ</param>
        /// <param name="BudgetDetails">thông tin nguồn chi phí</param>
        /// <returns>chứng từ đã được thêm mới thành công</returns>
        /// Created By: NguyetKTB (09/07/2023)
        public Voucher InsertVoucher(Voucher voucher, List<FixedAsset>? FixedAssets, List<BudgetDetail>? BudgetDetails)
        {
            // validate thông tin chứng từ
            ValidateData(voucher);
            if (errors.Count > 0)
            {
                throw new MISAException(ResourceVN.Msg_Exception, (IDictionary?)errors);
            }
            else
            {
                List<VoucherDetail> voucherDetails = new List<VoucherDetail>();
                decimal totalPrice = 0;
                foreach (var asset in FixedAssets)
                {
                    VoucherDetail voucherDetail = new VoucherDetail();
                    voucherDetail.voucher_id = voucher.voucher_id;
                    voucherDetail.fixed_asset_id = asset.fixed_asset_id;
                    DateTime currentDate = DateTime.Now;
                    voucherDetail.created_date = currentDate;
                    voucherDetails.Add(voucherDetail);
                }
                voucher.total_orgprice = CaculateTotalPrice(FixedAssets);
                foreach (var budget in BudgetDetails)
                {
                    BudgetDetail budgetDetail = budget;
                }
                // thực hiện gọi transaction để insert
                _unitOfWork.GetTransaction();
                try
                {
                    // insert bảng voucher
                    int rowVoucher = _voucherRepository.Insert(voucher);
                    int rowVoucherDetail = _voucherDetailRepository.InsertMultiple(voucherDetails);
                    int rowBudgetDetail = _budgetDetailRepository.InsertMultiple(BudgetDetails);
                    // insert bảng voucher detail
                    _unitOfWork.Commit();
                    return voucher;
                }
                catch (Exception ex)
                {
                    _unitOfWork.Rollback();
                    throw new MISAException(ex.Message);
                }
            }

        }

        /// <summary>
        /// Thực hiện cập nhật thông tin chứng từ
        /// </summary>
        /// <param name="voucher">thông tin chứng từ</param>
        /// <param name="FixedAssets">thông tin tài sản thuộc chứng từ</param>
        /// <param name="BudgetDetails">thông tin nguồn chi phí</param>
        /// <returns>số bản ghi bị ảnh hưởng</returns>
        /// Created By: NguyetKTB (10/07/2023)
        public int UpdateVoucher(Voucher voucher, List<FixedAsset>? FixedAssets, List<BudgetDetail>? BudgetDetails)
        {
            // update bảng voucher
            ValidateData(voucher, true);
            if (errors.Count > 0)
            {
                throw new MISAException(ResourceVN.Msg_Exception, (IDictionary?)errors);
            }
            else
            {
                _unitOfWork.GetTransaction();
                try
                {
                    // update voucher
                    voucher.total_orgprice = CaculateTotalPrice(FixedAssets);
                    int rowEffect = _voucherRepository.Update(voucher, voucher.voucher_id);

                    // xử lí dữ liệu ở bảng chi tiết chứng từ
                    foreach (var asset in FixedAssets)
                    {
                        if (asset.action != null)
                        {
                            VoucherDetail voucherDetail = new VoucherDetail();
                            voucherDetail.voucher_id = voucher.voucher_id;
                            voucherDetail.fixed_asset_id = asset.fixed_asset_id;
                            DateTime currentDate = DateTime.Now;
                            voucherDetail.modified_date = currentDate;
                            _voucherDetailService.HandleActionOfAsset(voucherDetail, voucher, (int)asset.action);
                        }
                    }
                    // kiểm tra dữ liệu danh sách nguồn chi phí
                    ValidateService(budgetDetails: BudgetDetails);
                    // mỗi tài sản phải chứa các trường chi phí độc lập nhau
                    //xử lí dữ liệu ở bảng nguồn chi phí
                    foreach (var budget in BudgetDetails)
                    {
                        if (budget.action != null)
                        {
                            BudgetDetail budgetDetail = budget;
                            _budgetDetailService.HandleBudgetDetail(budgetDetail, (int)budget.action);
                        }
                    }
                    _unitOfWork.Commit();
                    return rowEffect;
                }
                catch
                {
                    _unitOfWork.Rollback();
                    throw;
                }
            }

        }

        /// <summary>
        /// Thực hiện tính toán lại tổng giá trị cho chứng từ
        /// </summary>
        /// <param name="fixedAssets">danh sách tài sản thuộc chứng từ</param>
        /// <returns>tổng giá trị của các tài sản thuộc chứng từ</returns>
        /// Created By: NguyetKTB (10/07/2023)
        public decimal CaculateTotalPrice(List<FixedAsset> fixedAssets)
        {
            decimal totalPrice = 0;
            foreach (var asset in fixedAssets)
            {
                // nếu action = null or add or update thì mới tính tổng tiền
                if (asset.action == null || asset.action == (int)MSAction.Add || asset.action == (int)MSAction.Edit)
                {
                    totalPrice += asset.cost;
                }
            }
            return totalPrice;
        }


        /// <summary>
        /// Thực hiện kiểm tra nghiệp vụ riêng cho chi tiết nguồn chi phí
        /// </summary>
        /// <param name="budgetDetails">danh sách nguồn chi phí</param>
        /// <exception cref="MISAException">lỗi trả về nếu có</exception>
        /// Created By: NguyetKTB (10/07/2023)
        protected void ValidateService(List<BudgetDetail> budgetDetails)
        {
            //  đảm bảo rằng mỗi phần tử của list có fixed_asset_id bằng nhau hì budget_category_id phải khác nhau
            Dictionary<Guid, Guid> fixedAssetToBudgetCategory = new Dictionary<Guid, Guid>();

            foreach (var budget in budgetDetails)
            {
                List<string> messages = new List<string>();
                if (budget.action != (int)MSAction.Delete)
                {
                    if (fixedAssetToBudgetCategory.ContainsKey(budget.fixed_asset_id))
                    {
                        // Nếu fixed_asset_id đã tồn tại trong Dictionary,
                        // kiểm tra xem budget_category_id có khác với giá trị đã lưu hay không
                        if (fixedAssetToBudgetCategory[budget.fixed_asset_id] == budget.budget_category_id)
                        {
                            messages.Add("Nguồn chi phí có mã " + budget.budget_category_code + " đã bị trùng.");
                            errors.Add("Chí phí có mã " + budget.budget_detail_id, messages);
                        }
                    }
                    else
                    {
                        // Nếu fixed_asset_id chưa tồn tại trong Dictionary, thêm vào cùng với budget_category_id
                        fixedAssetToBudgetCategory.Add(budget.fixed_asset_id, budget.budget_category_id);
                    }
                }

            }
            if (errors.Count > 0)
            {
                throw new MISAException(ResourceVN.Msg_Exception, (IDictionary?)errors);
            }
        }

        /// <summary>
        /// Hàm kiểm tra dữ liệu override trong base
        /// </summary>
        /// <param name="entity">thông tin chứng từ</param>
        /// <param name="mode">trạng thái hành động</param>
        /// Created By: NguyetKTB (10/07/2023)
        protected override void ValidateService(Voucher entity, int mode)
        {
            base.ValidateService(entity, mode);
        }

    }
}
