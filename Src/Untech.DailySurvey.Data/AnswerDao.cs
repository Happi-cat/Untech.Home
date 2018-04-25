using System;
using LinqToDB.Mapping;
using Newtonsoft.Json;
using Untech.DailySurvey.Domain.Models;
using Untech.Practices.DataStorage;

namespace Untech.DailySurvey.Data
{
	[Table("Answers")]
	public class AnswerDao : IAggregateRoot
	{
		private AnswerDao()
		{
		}

		public AnswerDao(Answer entity)
		{
			Key = entity.Key;
			QuestionKey = entity.QuestionKey;
			When = entity.When;
			Json = JsonConvert.SerializeObject(entity);
		}

		[Column, PrimaryKey, Identity] public int Key { get; set; }

		[Column] public int QuestionKey { get; set; }

		[Column] public DateTime When { get; set; }

		[Column] public string Json { get; set; }

		public static Answer ToEntity(AnswerDao dao)
		{
			var payload = JsonConvert.DeserializeObject<Answer>(dao.Json);

			return new Answer(dao.Key, payload.QuestionKey, payload.When, payload.SelectedOptions);
		}
	}
}