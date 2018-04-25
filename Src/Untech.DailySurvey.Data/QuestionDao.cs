using LinqToDB.Mapping;
using Newtonsoft.Json;
using Untech.DailySurvey.Domain.Models;
using Untech.Practices.DataStorage;

namespace Untech.DailySurvey.Data
{
	[Table("Questions")]
	public class QuestionDao : IAggregateRoot
	{
		private QuestionDao()
		{
		}

		public QuestionDao(Question entity)
		{
			Key = entity.Key;
			Periodicity = entity.Periodicity;
			IsObsolete = entity.IsObsolete;
			Json = JsonConvert.SerializeObject(entity);
		}

		[Column, PrimaryKey, Identity] public int Key { get; set; }

		[Column] public QuestionPeriodicity Periodicity { get; set; }

		[Column] public bool IsObsolete { get; set; }

		[Column] public string Json { get; set; }

		public static Question ToEntity(QuestionDao dao)
		{
			var payload = JsonConvert.DeserializeObject<Question>(dao.Json);

			return new Question(dao.Key, payload.Text)
			{
				Periodicity = payload.Periodicity,
				Type = payload.Type,
				IsObsolete = payload.IsObsolete,
				Options = payload.Options
			};
		}
	}
}