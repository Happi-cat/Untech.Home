using System;

namespace Untech.Home
{
	public class ObjectNotFoundException : Exception
	{
		public ObjectNotFoundException(Type type, object key)
			: this(type, key, null)
		{
		}

		public ObjectNotFoundException(Type type, object key, Exception innerException)
			: base($"Object {type.Name} with key {key} not found.", innerException)
		{

		}
	}
}