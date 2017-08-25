using System.Linq;
using Untech.Home.ActivityPlanner.Domain.Commands;
using Untech.Home.ActivityPlanner.Domain.Models;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.Repos.Queryable;

namespace Untech.Home.ActivityPlanner.Business
{
	public class CategoryService :
		ICommandHandler<CreateCategory, Category>,
		ICommandHandler<UpdateCategory, Category>
	{
		private readonly IRepository<Category> _categories;

		public CategoryService(IRepository<Category> activityGroups)
		{
			_categories = activityGroups;
		}

		public Category Handle(UpdateCategory request)
		{
			var group = _categories.GetAll()
				.Single(n => n.Id == request.Id);

			group.Name = request.Name;
			group.Remarks = request.Remarks;

			_categories.Update(group);

			return group;
		}

		public Category Handle(CreateCategory request)
		{
			return _categories.Create(new Category
			{
				Name = request.Name,
				Remarks = request.Remarks
			});
		}
	}
}
