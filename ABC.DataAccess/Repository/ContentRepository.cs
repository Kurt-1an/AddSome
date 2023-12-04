using ABC.DataAccess.Data;
using ABC.DataAccess.Repository.IRepository;
using ABC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ABC.DataAccess.Repository
{
	public class ContentRepository : Repository<Content>, IContentRepository
    {
		private AppDBContext _db;
		public ContentRepository(AppDBContext db) : base(db)
		{
			_db = db;
		}


		public void Update(Content obj)
		{
			_db.Contents.Update(obj);
		}
	}
}
