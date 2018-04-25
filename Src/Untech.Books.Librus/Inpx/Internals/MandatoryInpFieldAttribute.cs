using System;
using System.ComponentModel.DataAnnotations;

namespace Untech.Books.Librus.Inpx.Internals
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	internal class MandatoryInpFieldAttribute : ValidationAttribute
	{
		public MandatoryInpFieldAttribute()
			: base("Mandatory field is not present in inpx structure")
		{
		}

		public override bool IsValid(object value)
		{
			switch (value)
			{
				case null:
					return true;
				case int index:
					return index > -1;
			}

			return false;
		}
	}
}