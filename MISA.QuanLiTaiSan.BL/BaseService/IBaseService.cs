﻿using MISA.QuanLiTaiSan.Common.Model;
using MISA.QuanLiTaiSan.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QuanLiTaiSan.BL.BaseBL
{
    public interface IBaseService<T>
    {
        #region GET 
        /// <summary>
        /// Lấy ra danh sách dữ liệu
        /// </summary>
        /// <returns>Danh sách dữ liệu </returns>
        /// Created By: NguyetKTB (15/05/2023)
        public IEnumerable<T> GetList();

        /// <summary>
        /// Lấy ra dữ liệu theo id
        /// </summary>
        /// <param name="id">Id cần lấy ra</param>
        /// <returns>Dữ liệu theo id truyền vào</returns>
        /// Created By: NguyetKTB (15/05/2023)
        public T GetEntityById(Guid id);

        /// <summary>
        /// Lấy ra danh sách bản ghi có phân trang theo điều kiện
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// Created By: NguyetKTB (15/05/2023)
        public PagingModel<T> GetByPaging(FilterParam filter);

        #endregion

        #region UPDATE
        /// <summary>
        /// Update dữ liệu
        /// </summary>
        /// <param name="entity">Dữ liệu cần update</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created By: NguyetKTB (15/05/2023)
        public int Update(T entity, Guid id);
        #endregion

        #region INSERT
        /// <summary>
        /// Thêm mới dữ liệu
        /// </summary>
        /// <param name="entity">Thông tin dữ liệu cần thêm mới</param>
        /// <returns>Id entity mới được thêm vào</returns>
        /// Created By: NguyetKTB (15/05/2023)
        public Guid Insert(T entity);

        #endregion

        #region DELETE
        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        /// <param name="guids">danh sách id dữ liệu cần xóa</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created By: NguyetKTB (15/05/2023)
        public int Delete(string[] guids);


        #endregion

        /// <summary>
        /// Hàm thực hiện lấy mã mới nhất
        /// </summary>
        /// <returns>mã mới nhất</returns>
        /// Created By: NguyetKTB (25/05/2023)
        public string GetNewCode();

        /// <summary>
        /// Hàm thực hiện lấy ra entity trùng lặp mã code trong hệ thống
        /// </summary>
        /// <param name="code">mã code cần kiểm tra trùng</param>
        /// <param name="id">id của entity</param>
        /// <returns>
        /// nếu trùng sẽ trả về dữ liệu trùng
        /// nếu không thì trả về null
        /// </returns>
        /// Created By: NguyetKTB (25/05/2023)
        public T DuplicateCode(string code, string? id);
    }
}
