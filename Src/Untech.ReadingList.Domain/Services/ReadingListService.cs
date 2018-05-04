using Untech.Practices;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage;
using Untech.ReadingList.Domain.Models;
using Untech.ReadingList.Domain.Requests;

namespace Untech.ReadingList.Domain.Services
{
	public class ReadingListService :
		ICommandHandler<CreateReadingListEntry, ReadingListEntry>,
		ICommandHandler<DeleteReadingListEntry, bool>,
		ICommandHandler<StartReadingBook, Nothing>,
		ICommandHandler<CompleteReadingBook, Nothing>
	{
		private readonly IDataStorage<ReadingListEntry> _dataStorage;

		public ReadingListService(IDataStorage<ReadingListEntry> dataStorage)
		{
			_dataStorage = dataStorage;
		}

		public ReadingListEntry Handle(CreateReadingListEntry request)
		{
			var entry = new ReadingListEntry(0, request.Author, request.Title);
			return _dataStorage.Create(entry);
		}

		public bool Handle(DeleteReadingListEntry request)
		{
			var entry = _dataStorage.Find(request.Key);
			return _dataStorage.Delete(entry);
		}

		public Nothing Handle(StartReadingBook request)
		{
			var entry = _dataStorage.Find(request.ReadingListEntryKey);
			entry.StartReading();
			_dataStorage.Update(entry);

			return Nothing.AtAll;
		}

		public Nothing Handle(CompleteReadingBook request)
		{
			var entry = _dataStorage.Find(request.ReadingListEntryKey);
			entry.CompleteReading();
			_dataStorage.Update(entry);

			return Nothing.AtAll;
		}
	}
}