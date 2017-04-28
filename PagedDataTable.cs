using AutoMapper;
using Core.Data;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Helpers
{
    public class DataTable<Tview>
    {
        public DataTable()
        {
            this.data = new List<Tview>();
        }
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<Tview> data { get; set; }
    }

    public interface IPagedDataTable<Tview, Tdomain>
    {
        DataTable<Tview> retornaDataTableJS(IGenericRepository<Tdomain> _repo, int start, int size, int draw,
            string coluna,
            bool orderbydesc,
            Expression<Func<Tdomain, bool>> predicate);
    }

    public class PagedDataTable<Tview, Tdomain> : IPagedDataTable<Tview, Tdomain>
    {
        DataTable<Tview> IPagedDataTable<Tview, Tdomain>.retornaDataTableJS(IGenericRepository<Tdomain> _repo, int start, int size, int draw, 
            string coluna, bool orderbydesc, Expression<Func<Tdomain, bool>> predicate)
        {
            var list = _repo.SearchFor(coluna, orderbydesc, predicate).ToPagedList((start == 0 ? 1 : (start / size) * 1), size);
            var listView = Mapper.Map<List<Tdomain>, List<Tview>>(list.ToList());

            DataTable<Tview> dt = new DataTable<Tview>();
            dt.draw = draw;
            dt.recordsFiltered = list.TotalItemCount;
            dt.recordsTotal = list.TotalItemCount;
            dt.data = listView;

            return dt;
        }      
    }
}
