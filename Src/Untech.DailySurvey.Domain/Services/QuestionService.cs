using System;
using Untech.DailySurvey.Domain.Models;
using Untech.DailySurvey.Domain.Requests;
using Untech.Practices;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage;

namespace Untech.DailySurvey.Domain.Services
{
	public class QuestionService : ICommandHandler<AnswerOnQuestion, Nothing>,
		ICommandHandler<CreateQuestion, Question>,
		ICommandHandler<MarkQuestionAsObsolete, Question>
	{
		private readonly IDataStorage<Question> _questionDataStorage;
		private readonly IDataStorage<Answer> _answerDataStorage;

		public QuestionService(IDataStorage<Question> questionDataStorage, IDataStorage<Answer> answerDataStorage)
		{
			_questionDataStorage = questionDataStorage;
			_answerDataStorage = answerDataStorage;
		}

		public Nothing Handle(AnswerOnQuestion request)
		{
			_answerDataStorage.Create(new Answer(0, request.QuestionKey, DateTime.UtcNow, request.SelectedOptions));

			return Nothing.AtAll;
		}

		public Question Handle(CreateQuestion request)
		{
			return _questionDataStorage.Create(new Question(0, request.Text)
			{
				Periodicity = request.Periodicity,
				Type = request.Type,
				Options = request.Options
			});
		}

		public Question Handle(MarkQuestionAsObsolete request)
		{
			var entity = _questionDataStorage.Find(request.Key);
			entity.IsObsolete = true;
			return _questionDataStorage.Update(entity);
		}
	}
}